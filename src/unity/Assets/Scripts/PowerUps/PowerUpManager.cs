using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : Singleton<PowerUpManager>
{
    public PowerUp[] powerUps;
    public PowerUp target;
    //public ParticleSystem goodEffect;
    //public ParticleSystem badEffect;

    public enum PowerUpTypes

    {
        Enlightenment = 1,
        ShowTarget = 2,
        Endurance = 3,
        Darkness = 4,
        Dim = 5,
        Slowing = 6,
        Target = 7,
    }

    // indicating how many powerups are currently active
    public int activePowerUps { get; set; }

    // time to wait until new powerup spawns
    public float waitTime = 4.5f;

    // elapsed time since last powerup spawned
    private float elapsedTime = 0.0f;

    public PowerUp spawnPowerUp(PowerUpTypes type, bool forceSpawn = false, float spawnDifficulty = 1.0f)
    {

        int cell = 0;
        if (type.Equals(PowerUpTypes.Target))
        {
            int bestCellY = 0, bestCellX = 0;
            while (true)
            {
                // get a fair cell for the target
                MazeGenerator.Instance.toMatrix();
                int ySize = MazeGenerator.Instance.ySize;
                int xSize = MazeGenerator.Instance.xSize;

                float[][][] board = new float[ySize][][];
                float bestCellValue = int.MaxValue;


                for (int row = 0; row < ySize; ++row)
                {
                    board[row] = new float[xSize][];
                    for (int column = 0; column < xSize; ++column)
                    {
                        board[row][column] = new float[MazeGenerator.Instance.numPlayers];

                        int index = 0;
                        foreach (Player p in GameController.Instance.joinedPlayersToPosition.Keys)
                        {
                            if (p.cell != null)
                            {
                                List<PathFinding.AStarNode> path = PathFinding.Instance.AStar(p.cell, MazeGenerator.Instance.toMatrix()[row][column].GetComponent<Cell>());
                                board[row][column][index] = (path == null) ? int.MaxValue : path.Count;
                                ++index;
                            }    
                        }

                        // average distance to each cell
                        float total = 0;
                        for (int i = 0; i < MazeGenerator.Instance.numPlayers; ++i)
                        {
                            total += board[row][column][i];
                        }
                        float average = total / (MazeGenerator.Instance.numPlayers);

                        // sum up difference to average
                        float deltaVal = 0;                 // indicating the fairness of the cell, 0 being best
                        for (int i = 0; i < MazeGenerator.Instance.numPlayers; ++i)
                        {
                            float pathLength = board[row][column][i];
                            float diffToAverage = Math.Abs(average - pathLength);

                            deltaVal += (diffToAverage - (UnityEngine.Random.Range(0, pathLength / MazeGenerator.Instance.numPlayers)) * 0.01f);    // favor long paths
                        }

                        bool possible = true;
                        if (deltaVal < bestCellValue)
                        {
                            for (int i = 0; i < MazeGenerator.Instance.numPlayers; ++i)
                            {
                                if (board[row][column][i] < MazeGenerator.Instance.xSize * 2 * spawnDifficulty)
                                {
                                    // do not use this cell if a user only has to go size * n cells to reach it
                                    possible = false;
                                }
                                if (deltaVal / average > 0.2f * MazeGenerator.Instance.numPlayers && !forceSpawn)
                                {
                                    possible = false;
                                }
                            }
                            if (possible)
                            {
                                bestCellValue = deltaVal;
                                bestCellY = row;
                                bestCellX = column;
                            }
                        }
                    }
                }
                if (bestCellValue == int.MaxValue)
                {
                    // did not find a suitable cell, do again
                    if (forceSpawn)
                    {
                        return spawnPowerUp(PowerUpTypes.Target, forceSpawn, spawnDifficulty / 0.8f);
                    }
                    else
                    {
                        return null;
                    }
                }
                Debug.Log("Target fairness: " + bestCellValue);
                Debug.Log("Target cell: " + bestCellX + ", " + bestCellY);

                if (MazeGenerator.Instance.toMatrix()[bestCellY][bestCellX].GetComponent<Cell>().lights.Count == 0 &&
                    MazeGenerator.Instance.toMatrix()[bestCellY][bestCellX].GetComponent<Cell>().powerUps.Count == 0)
                {
                    break;
                }
            }
            return spawnPowerUp(PowerUpTypes.Target, MazeGenerator.Instance.toMatrix()[bestCellY][bestCellX].GetComponent<Cell>());

        }
        else
        {
            while (true)
            {
                cell = UnityEngine.Random.Range(1, (int)(MazeGenerator.Instance.xSize * MazeGenerator.Instance.ySize) - 1);

                if (MazeGenerator.Instance.cells[cell].GetComponent<Cell>().players.Count == 0 &&
                    MazeGenerator.Instance.cells[cell].GetComponent<Cell>().powerUps.Count == 0)
                {
                    break;
                }
            }
            
            

            return spawnPowerUp(type, MazeGenerator.Instance.cells[cell].GetComponent<Cell>());
        }
    }

    public PowerUp spawnPowerUp(PowerUpTypes type, Cell cell)
    {
        if (type.Equals(PowerUpTypes.Target))
        {       target.transform.parent = cell.gameObject.transform;
                target.transform.localPosition = new Vector3(0, 0, 0);
                target.gameObject.SetActive(true);
                cell.powerUps.Add(target);
                target.cell = cell;

                target.gameObject.GetComponent<Collider>().enabled = true;
                target.gameObject.GetComponent<MeshRenderer>().enabled = true;

                // let target light up
                /*GameObject g = new GameObject();
                g.AddComponent<ShowTarget>();
                g.GetComponent<ShowTarget>().type = PowerUpTypes.ShowTarget;
                g.GetComponent<ShowTarget>().activate(GameController.Instance.players[0]);
                Destroy(g.GetComponent<ShowTarget>());*/
                return target.GetComponent<Target>();
            
        }

        foreach (PowerUp p in powerUps)
        {
            if (p.type.Equals(type))
            {
                GameObject powerUp = Instantiate(p.gameObject, cell.transform, true);
                powerUp.transform.localPosition = new Vector3(0, 0, 0);
                powerUp.SetActive(true);
                cell.powerUps.Add(powerUp.GetComponent<PowerUp>());
                powerUp.gameObject.GetComponent<Collider>().enabled = true;
                powerUp.gameObject.GetComponent<MeshRenderer>().enabled = true;
                return powerUp.GetComponent<PowerUp>();
            }
        }
        return null;
    }

    public void setSpawnedPowerUps(int id)
    {
        activePowerUps = id;
    }

    void Update()
    {
        // have max so viele powerups wie das Maze breit ist * kleiner faktor
        if (activePowerUps >= ((MazeGenerator.Instance.xSize) * 1.3)|| GameController.Instance.gameover)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        if (MazeGenerator.Instance.cells != null && elapsedTime >= waitTime)
        {
            int type = UnityEngine.Random.Range(0, 6);
            switch (type)
            {
                case 0:
                    spawnPowerUp(PowerUpManager.PowerUpTypes.Enlightenment);
                    break;
                case 1:
                    spawnPowerUp(PowerUpManager.PowerUpTypes.ShowTarget);
                    break;
                case 2:
                    spawnPowerUp(PowerUpManager.PowerUpTypes.Endurance);
                    break;
                case 3:
                    spawnPowerUp(PowerUpManager.PowerUpTypes.Darkness);
                    break;
                case 4:
                    spawnPowerUp(PowerUpManager.PowerUpTypes.Dim);
                    break;
                case 5:
                    spawnPowerUp(PowerUpManager.PowerUpTypes.Slowing);
                    break;
            }
            elapsedTime = 0.0f;
            activePowerUps++;
        }
    }
}
