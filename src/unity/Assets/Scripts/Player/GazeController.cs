using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : MonoBehaviour
{
    public GameObject cursor;
    private Rigidbody rb;

    private Vector3 offset;
    private Vector3 screenPoint;
    float depth;
    float radius = 10;

    private Cell currentCell;
    private Cell targetCell;
    private Cell lastMouseCell;
    private Cell lastCell;
    private Boolean currentCellReached;

    private float gazeX;
    private float gazeY;

    public float goodGazeX { get; set; }
    public float goodGazeY { get; set; }

    public Boolean gazeOnSurface { get; set; }

    public void listenerReady()
    {
        Debug.Log("Listener for gaze controller " + gameObject.name + " is ready");
    }

    public void move(Pupil.SurfaceData3D data, string surface = "")
    {
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

        if (!gaze.on_srf || !data.name.Equals(surface))
        {
            gazeOnSurface = false;
            return;
        }

        gazeOnSurface = true;
        gazeX = (float)gaze.norm_pos[0];
        gazeY = (float)gaze.norm_pos[1];
    }

    private void protectArea(int[] pos)
    {
        GameObject[][] board = MazeGenerator.Instance.toMatrix();

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
                            c.lights[i].GetComponent<CellLight>().SaveArea(GameController.Instance.players[0]);

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

        List<Cell> possibleCells = new List<Cell>();

        for (int deltaRangeX = rangeX * (-1); deltaRangeX <= rangeX; ++deltaRangeX)
        {
            for (int deltaRangeY = rangeY * (-1); deltaRangeY <= rangeY; ++deltaRangeY)
            {
                if (deltaRangeX * deltaRangeY != 0)
                {
                    continue;
                }
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
                if ((!gazeCell.Equals(lastMouseCell) ||
                    (!currentCell.Equals(lastCell)))
                    && currentCellReached)
                {
                    lastMouseCell = gazeCell;
                    lastCell = currentCell;
                    currentCellReached = false;
                    if (PathFinding.Instance.getManhattanDistance(currentCell, gazeCell) < 5)
                    {
                        List<PathFinding.AStarNode> path = PathFinding.Instance.AStar(currentCell, gazeCell);
                        if (path.Count > 0)
                        {
                            if (path.Count < 5)
                            {
                                if (Math.Abs(deltaRangeX) + Math.Abs(deltaRangeY) == 0)
                                {
                                    targetCell = path[0].c;
                                    return;
                                }
                                possibleCells.Add(path[0].c);
                            }
                        }
                    }
                }
            }
        }

        foreach (Cell c in possibleCells)
        {
            targetCell = c;
            return;
        }
    }

    void Update()
    {
        if (!Menu.Instance.canvas.enabled)
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

            gazeX = gazeX * Screen.width;
            gazeY = gazeY * Screen.height;
            Vector3 gazePos = new Vector3();

            if (!(gazeX > 10000 || gazeY > 1000 || gazeX < -10000 || gazeY < -1000))
            {
                goodGazeX = gazeX;
                goodGazeY = gazeY;
            }

            gazePos = Camera.main.ScreenToWorldPoint(new Vector3(goodGazeX, goodGazeY, depth));
            cursor.gameObject.transform.position = gazePos;

            float x = MazeGenerator.Instance.xSize;
            float y = MazeGenerator.Instance.ySize;

            float mx = gazePos.x + x / 2 - 1.0f;
            float my = gazePos.z + y / 2;
            GameObject[][] board = MazeGenerator.Instance.toMatrix();

            int[] pos = new int[2];

            pos[0] = (int)my;
            pos[1] = (int)System.Math.Round((double)mx);

            findMostProbableCell(pos);

            if (targetCell == null)
            {
                targetCell = currentCell;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetCell.transform.position, gameObject.GetComponent<Player>().speed * Time.deltaTime);         
        }
    }
}
