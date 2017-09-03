using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : PlayerControl
{
    public GameObject cursor;
    private Rigidbody rb;

    private Vector3 offset;
    private Vector3 screenPoint;
    float depth;
    float radius = 10;
    private float currentSpeed;
    private float speedingUp = 1.1f;

    private Cell currentCell;
    private Cell targetCell;
    private Cell lastMouseCell;
    private Cell lastCell;
    private Boolean currentCellReached;

    public float gazeX { get; set; }
    public float gazeY { get; set; }

    public float goodGazeX { get; set; }
    public float goodGazeY { get; set; }

    public Boolean gazeOnSurface { get; set; }
    private float gazeLeftSurface;

    public void listenerReady()
    {
        Debug.Log("Listener for gaze controller " + gameObject.name + " is ready");
    }

    public void move(Pupil.SurfaceData3D data, string surface = "screen")
    {
        //Debug.Log("move");
        Pupil.GazeOnSurface gaze = new Pupil.GazeOnSurface();
        double maxConfidence = 0;
        foreach (Pupil.GazeOnSurface gos in data.gaze_on_srf)
        {
            if (gos.confidence > maxConfidence)
            {
                maxConfidence = gos.confidence;
                gaze = gos;
            }
        }

        //gaze.on_srf = !gaze.on_srf;
        //Debug.Log(gaze.on_srf);

        if (!gaze.on_srf || !data.name.Equals(surface))
        {
            if (gazeOnSurface == true)
            {
                gazeLeftSurface = Time.time;
            }
            gazeOnSurface = false;
            return;
        } else
        {
            if (GameController.Instance.joinedPlayersToPosition.Keys.Count < MazeGenerator.Instance.numPlayers)
            {
                processGaze();
            }
            gazeOnSurface = true;
            gazeX = (float)gaze.norm_pos[0];
            gazeY = (float)gaze.norm_pos[1];

            //Debug.Log(gazeX);
        }

        

    }

    private void protectArea(int[] pos)
    {
        GameObject[][] board = MazeGenerator.Instance.toMatrix();

        if (gameObject.GetComponent<Player>().cell == null)
        {
            return;
        }

        if (Math.Abs(gameObject.GetComponent<Player>().cell.posX - pos[1]) + Math.Abs(gameObject.GetComponent<Player>().cell.posY - pos[0]) < 3)
        {
            return;
        }
        int rangeX = MazeGenerator.Instance.xSize / 7;
        int rangeY = MazeGenerator.Instance.ySize / 7;
        for (int deltaRangeX = rangeX * (-1); deltaRangeX <= rangeX; ++deltaRangeX)
        {
            for (int deltaRangeY = rangeY * (-1); deltaRangeY <= rangeY; ++deltaRangeY)
            {
                if (deltaRangeX + pos[1] < 0 || deltaRangeY + pos[0] < 0 || deltaRangeX + pos[1] >= board[0].Length || deltaRangeY + pos[0] >= board.Length)
                {
                    continue;
                }

                Cell c = board[pos[0] + deltaRangeY][pos[1] + deltaRangeX].GetComponent<Cell>();
                if (c == null)
                {
                    continue;
                }
                if (c.lights != null)
                {
                    if (c.lights.Count > 0)
                    {
                        for (int i = 0; i < c.lights.Count; i++)
                        {
                            c.lights[i].GetComponent<CellLight>().SaveArea(gameObject.GetComponent<Player>());

                        }
                    }
                }
            }
        }
    }

    private void findMostProbableCell(int[] pos)
    {
        GameObject[][] board = MazeGenerator.Instance.toMatrix();

        int rangeX = 1;
        int rangeY = 1;

        List<List<PathFinding.AStarNode>> possibleCells = new List<List<PathFinding.AStarNode>>();

        for (int deltaRangeX = rangeX * (-1); deltaRangeX <= rangeX; ++deltaRangeX)
        {
            for (int deltaRangeY = rangeY * (-1); deltaRangeY <= rangeY; ++deltaRangeY)
            {
                
                if (deltaRangeX + pos[1] < 0 || deltaRangeY + pos[0] < 0 || deltaRangeX + pos[1] >= board[0].Length || deltaRangeY + pos[0] >= board.Length)
                {
                    continue;
                }

                Cell gazeCell = board[pos[0] + deltaRangeY][pos[1] + deltaRangeX].GetComponent<Cell>();
                if (gazeCell == null)
                {
                    continue;
                }
                

                protectArea(new int[] { pos[0] + deltaRangeY, pos[1] + deltaRangeX });
                if (currentCellReached)
                {
                    if (PathFinding.Instance.getManhattanDistance(currentCell, gazeCell) < 5)
                    {
                        List<PathFinding.AStarNode> path = PathFinding.Instance.AStar(currentCell, gazeCell);
                        if (path.Count >= 0)
                        {
                            if (path.Count < 5)
                            {
                                if (Math.Abs(deltaRangeX) + Math.Abs(deltaRangeY) == 0) // if this is the cell we are actually looking at
                                {
                                    if (path.Count == 0)
                                    {
                                        targetCell = gazeCell;
                                    } else
                                    {
                                        targetCell = path[0].c;
                                    }
                                    lastMouseCell = gazeCell;
                                    lastCell = currentCell;
                                    currentCellReached = false;
                                    return;
                                } else
                                {
                                    if (deltaRangeX != 0 && deltaRangeY != 0 && path.Count > 0)
                                    {
                                        path.Add(new PathFinding.AStarNode());
                                    }
                                    possibleCells.Add(path);
                                }
                            }
                        }
                    }
                }
            }
        }

        Cell bestCell = null;
        int bestCellPathLength = int.MaxValue;

        foreach(List<PathFinding.AStarNode> path in possibleCells)
        {
            if (path.Count < bestCellPathLength)
            {
                bestCellPathLength = path.Count;
                if (path.Count == 0)
                {
                    bestCell = currentCell;
                    continue;
                }
                bestCell = path[0].c;
            }
        }
        

        lastCell = currentCell;
        currentCellReached = false;

        targetCell = bestCell;

        /*
        if (possibleCells.Count > 0)
        {
            possibleCells.Sort();
            Debug.Log("Cell " + possibleCells[0].posX + ", " + possibleCells[0].posY + " is best");
            targetCell = possibleCells[0];
        } else
        {
            targetCell = null;
        }

        foreach (Cell c in possibleCells)
        {
            targetCell = c;
            return;
        }*/
    }

    void Update()
    {
        processGaze();
    }

    void processGaze()
    {
        //Debug.Log("gazeX " + gazeX);
        //Debug.Log("gazeY " + gazeY);

        //return;
        /*if (!gazeOnSurface)
        {
            if (Time.time - gazeLeftSurface > 5)
            {
                // Player did not look at maze for more than 5 consequtive seconds, kick him out of the game
                Player player = gameObject.GetComponent<Player>();
                //Debug.Log("Kicking player " + player.name + " out of the game (inactive)");
                //GameController.Instance.joinedPlayersToPosition.Remove(player);
                //gameObject.SetActive(false);
            }
        }*/
        //Or code above with leaving screen

        if (!Menu.Instance.joinScreen.gameObject.activeSelf)
        {
            LeaveGame();
        }

        if (!Menu.Instance.canvas.enabled || Menu.Instance.joinScreen.gameObject.activeSelf)
        {

            depth = MazeGenerator.Instance.xSize * 4 - 0.5f;
            int size = MazeGenerator.Instance.xSize;

            // Move player
            currentCell = gameObject.GetComponent<Player>().cell;
            if (!currentCellReached && currentCell != null)
            {
                if (Vector3.Distance(transform.position, currentCell.transform.position) < 0.01f)
                {
                    currentCellReached = true;
                }
            }

            Vector3 gazePos = new Vector3();


            //Debug.Log(Screen.width);
            //Debug.Log(Screen.height);

            if (!(gazeX > 10000 || gazeY > 1000 || gazeX < -10000 || gazeY < -1000))
            {
                //goodGazeX = ((gazeX * Screen.width)) * ((Screen.width + 25) / Screen.width);
                //goodGazeY = ((gazeY * Screen.height)) * ((Screen.height + 25) / Screen.height);

                goodGazeX = (gazeX * (Screen.width - 50)) + 25;
                goodGazeY = (gazeY * (Screen.height - 50)) + 25;


            }

            //Debug.Log("goodGazeX " + goodGazeX);
           // Debug.Log("goodGazeY " + goodGazeY);


            gazePos = Camera.main.ScreenToWorldPoint(new Vector3(goodGazeX, goodGazeY, depth));
            cursor.gameObject.transform.position = gazePos;

            if (MazeGenerator.Instance.cells == null)
            {
                return;
            }

            float x = MazeGenerator.Instance.xSize;
            float y = MazeGenerator.Instance.ySize;

            float mx = gazePos.x + x / 2 - 1.0f;
            float my = gazePos.z + y / 2;
            GameObject[][] board = MazeGenerator.Instance.toMatrix();

            int[] pos = new int[2];

            pos[0] = (int)my;
            pos[1] = (int)System.Math.Round((double)mx);

            if (currentCellReached)
            {
                findMostProbableCell(pos);
                if (targetCell == null)
                {
                    //Debug.Log("target cell is null");
                    //targetCell = currentCell;
                } else
                {
                    //Debug.Log("target cell is NOT null");
                    //Debug.Log(targetCell.posX + " " + targetCell.posY);

                }
            }

            if (transform.position != targetCell.transform.position)
            {
                if (currentSpeed < gameObject.GetComponent<Player>().speed)
                {
                    currentSpeed += speedingUp;
                }

            }
            else
            {
                currentSpeed = 0;
            }

            if (targetCell != null)
            {
                //transform.position = Vector3.MoveTowards(transform.position, targetCell.transform.position, gameObject.GetComponent<Player>().speed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, targetCell.transform.position, currentSpeed * Time.deltaTime);

            }
        }
        /*else
        {
            currentSpeed = 0;
        }*/
    }
}
