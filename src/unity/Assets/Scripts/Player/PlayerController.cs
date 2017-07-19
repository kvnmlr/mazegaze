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

            CheckArea();
            
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

    void CheckArea()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth));
        float x = MazeGenerator.Instance.xSize;
        float y = MazeGenerator.Instance.ySize;
        float mx = mousePos.x + x/2 - 1.0f;
        float my = mousePos.z + y/2;
        float limitUntenX = -0.5f;
        float limitUntenY = 0;
        float limitObenX = x - 1;
        float limitObenY = y;
        int abstand = 1; //groesse der area
        GameObject[][] board = MazeGenerator.Instance.toMatrix();

        if (mx >= limitUntenX && my >= limitUntenY && mx <= limitObenX && my < limitObenY)
        {
            int middelY = (int)my;
            int middelX = (int)System.Math.Round((double)mx);
            Cell[] cells = new Cell[9]; //wenn abstand == 1, fuer abstand == 2 -> 25
            //TODO: mach boolean draus damit man nicht immer neu rechnen muss
            if (mx >= limitUntenX + abstand && my >= limitUntenY + abstand && mx <= limitObenX - abstand && my < limitObenY - abstand)
            {

                cells[0] = board[middelY - abstand][middelX - abstand].GetComponent<Cell>(); //-1-1
                cells[1] = board[middelY - abstand][middelX].GetComponent<Cell>(); //-1-0
                cells[2] = board[middelY - abstand][middelX + abstand].GetComponent<Cell>(); //-1+1
                cells[3] = board[middelY][middelX - abstand].GetComponent<Cell>(); //0-1
                cells[4] = board[middelY][middelX].GetComponent<Cell>(); //00
                cells[5] = board[middelY][middelX + abstand].GetComponent<Cell>(); //0+1
                cells[6] = board[middelY + abstand][middelX - abstand].GetComponent<Cell>(); //+1-1
                cells[7] = board[middelY + abstand][middelX].GetComponent<Cell>(); //+1-0
                cells[8] = board[middelY + abstand][middelX + abstand].GetComponent<Cell>(); //+1+1
            } else if (mx < limitUntenX + abstand && my >= limitUntenY + abstand && mx <= limitObenX - abstand && my < limitObenY - abstand)
            { 
                //x kleiner
                //
                //x
                //
            }else if(mx >= limitUntenX + abstand && my >= limitUntenY + abstand && mx > limitObenX - abstand && my < limitObenY - abstand)
            {
                //x groesser
                //
                //      x
                //
            }else if (mx >= limitUntenX + abstand && my < limitUntenY + abstand && mx <= limitObenX - abstand && my < limitObenY - abstand)
            {
                //y kleiner 
                //  
                //
                //  y
            }else if (mx >= limitUntenX + abstand && my >= limitUntenY + abstand && mx <= limitObenX - abstand && my >= limitObenY - abstand)
            {
                //y groesser
                //  y
                //
                //
            }else if (mx < limitUntenX + abstand && my < limitUntenY + abstand && mx <= limitObenX - abstand && my < limitObenY - abstand)
            {
                //x klein y klein
                //
                //
                //xy
            }
            else if (mx < limitUntenX + abstand && my >= limitUntenY + abstand && mx <= limitObenX - abstand && my >= limitObenY - abstand)
            {
                //xklein y gross
                //xy
                //
                //
            }
            else if (mx >= limitUntenX + abstand && my < limitUntenY + abstand && mx > limitObenX - abstand && my < limitObenY - abstand)
            {
                //xgrossyklein
                //
                //
                //      xy
            }
            else if (mx >= limitUntenX + abstand && my >= limitUntenY + abstand && mx > limitObenX - abstand && my >= limitObenY - abstand)
            {
                //xgrosygros
                //      xy
                //
                //
            }
            for (int i=0; i< cells.Length;i++)
            {
                if(cells[i] != null)
                {
                    if(cells[i].lights.Count >= 0)
                    {
                        for (int j = 0; i < cells[i].lights.Count; i++)
                        {   //Rufe methode auf die            
                            cells[i].lights[j].GetComponent<CellLight>().SaveArea(GameController.Instance.players[0]);
                        }
                    }
                    
                }
            }
           
            
        }

    }



}
