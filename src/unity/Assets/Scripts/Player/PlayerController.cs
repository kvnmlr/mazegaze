using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerControl
{
    private Rigidbody rb;
    private Vector3 offset;
    private Vector3 screenPoint;
    float depth;
    float radius = 2;
    private float speedingUp = 0.5f;
    private float currentSpeed = 0;
    private Vector3 oldMousePos = new Vector3(0, 0, 0);

    private Cell currentCell;
    private Cell targetCell;
    private Cell lastMouseCell;
    private Cell lastCell;
    
    //Vector3 richtung = Vector3.zero;

    private Boolean currentCellReached;

    void Start () {
        rb = GetComponent<Rigidbody>();
        depth = MazeGenerator.Instance.xSize * 4 - 0.5f;
        if (MazeGenerator.Instance.xSize == 9)
        {
            radius = 2;
        }else if(MazeGenerator.Instance.xSize == 15)
        {
            radius = 3;
        }
        else
        {
            radius = 5;
        }
    }

    void Update()
    {
        if (!Menu.Instance.canvas.enabled && MazeGenerator.Instance.cells != null && !Menu.Instance.joinScreen.activeSelf)
        {
            LeaveGame();
           
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

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth));

            float x = MazeGenerator.Instance.xSize;
            float y = MazeGenerator.Instance.ySize;
            int rangeX = MazeGenerator.Instance.xSize / 7;
            int rangeY = MazeGenerator.Instance.ySize / 7;
            float mx = mousePos.x + x / 2 - 1.0f;
            float my = mousePos.z + y / 2;

            int[] pos = new int[2];

            pos[0] = (int)my;
            pos[1] = (int)System.Math.Round((double)mx);
            GameObject[][] board = MazeGenerator.Instance.toMatrix();
            if (pos[1] < MazeGenerator.Instance.xSize && pos[0] < MazeGenerator.Instance.ySize && pos[1] >= 0 && pos[0] >= 0)
            {
                Cell mouseCell = board[pos[0]][pos[1]].GetComponent<Cell>();
                if ((!mouseCell.Equals(lastMouseCell) ||
                    (!currentCell.Equals(lastCell)))
                    && currentCellReached)
                {
                    lastMouseCell = mouseCell;
                    lastCell = currentCell;
                    currentCellReached = false;
                    if (PathFinding.Instance.getManhattanDistance(currentCell, mouseCell) < 5)
                    {
                        List<PathFinding.AStarNode> path = PathFinding.Instance.AStar(currentCell, mouseCell);
                        if (path.Count > 0)
                        {
                            if (path.Count < 5)
                            {
                                targetCell = path[0].c;
                            }
                        }
                    }
                }
                /*if (!mouseCell.Equals(lastMouseCell) ||
                   !currentCell.Equals(lastCell))
                {
                   */



            }


            if (targetCell == null)
            {
                targetCell = currentCell;
            }

            if (targetCell == null)
            {
                return;
            }
            if(transform.position != targetCell.transform.position)
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



            //transform.position = Vector3.MoveTowards(transform.position, targetCell.transform.position, GetComponent<Player>().speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetCell.transform.position, currentSpeed * Time.deltaTime);


            // Protect area
            if (Math.Abs(gameObject.GetComponent<Player>().cell.posX - pos[1]) + Math.Abs(gameObject.GetComponent<Player>().cell.posY - pos[0]) < 3)
            {
                return;
            }

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

    }
}
