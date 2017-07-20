using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //PlayerA
    public float speed;
    private Rigidbody rb;
    private Vector3 offset;
    private Vector3 screenPoint;
    float depth;
    float radius = 2;
    

    void Start () {
        rb = GetComponent<Rigidbody>();
        depth = MazeGenerator.Instance.xSize * 4 - 0.5f;
        //TODO: Radius mit Eyetracker anpassen;
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
        if (!Menu.Instance.canvas.enabled)
        {
            depth = MazeGenerator.Instance.xSize * 4 - 0.5f;
            int size = MazeGenerator.Instance.xSize;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth));

            ProtectArea();
            
            Vector3 temp = transform.position;

            if (System.Math.Abs(transform.position.x - mousePos.x) < radius && System.Math.Abs(transform.position.z - mousePos.z) < radius)
            {
                transform.position = Vector3.MoveTowards(transform.position, mousePos, speed * Time.deltaTime);

            }
            else if (System.Math.Abs(transform.position.x - mousePos.x) >= radius && System.Math.Abs(transform.position.z - mousePos.z) >= radius)
            {

                rb.position = transform.position;
                
            }
            //TODO schaue nach cell von mitspielern
            if (transform.position.y > 0.5)
            {
                transform.position = new Vector3(temp.x, 0.5f, temp.z);
            }

        }

    }

    void ProtectArea()
    {
        float x = MazeGenerator.Instance.xSize;
        float y = MazeGenerator.Instance.ySize;
        int rangeX = MazeGenerator.Instance.xSize / 7;
        int rangeY = MazeGenerator.Instance.ySize / 7;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth));
        float mx = mousePos.x + x / 2 - 1.0f;
        float my = mousePos.z + y / 2;

        int[] pos = new int[2];

        pos[0] = (int)my;
        pos[1] = (int)System.Math.Round((double)mx);


        GameObject[][] board = MazeGenerator.Instance.toMatrix();

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
}
