using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : Singleton<MazeGenerator> {

    [System.Serializable]
    public class CellProperties
    {
        public bool visited;
        public GameObject north;//1
        public GameObject east;//2
        public GameObject west;//3
        public GameObject south;//4

    }

    public GameObject wall;
    public GameObject floor;

    public GameObject playerA { get; set; }
    public GameObject playerB { get; set; }
    public GameObject playerC { get; set; }
    public GameObject playerD { get; set; }
    public int numPlayers { get; set; }
    private int oldNumPlayers;

    public GameObject target { get; set; }
    //public GameObject targetLight { get; set; }   targetLight is im target drin, wir brauchen dafuer keine extra variable hier.

    private float wallLength = 1.0f;
    public int xSize { get; set; }
    public int ySize { get; set; }
    private int oldXSize;
    private int oldYSize;

    private Vector3 initialPos;
    private GameObject WallHolder;
    private GameObject Maze;
    private GameObject Cells;
    public CellProperties[] cellProperties { get; set; }
    public GameObject[] cells { get; set; }
    private int currentCell;
    private int totalCells;
    private int visitedCells = 0;
    private bool startedBuilding = false;
    private int currentNeighbour;
    private List<int> lastCells;
    private int backingUp = 0;
    private int wallToBreak = 0;
    private CellProperties start;
    private GameObject tempFloor;

    public GameObject[][] toMatrix()
    {
        GameObject[][] board = new GameObject[ySize][];
        int index = 0;
        for (int row = 0; row <ySize; ++row)
        {
            board[row] = new GameObject[xSize];
            for(int column = 0; column < xSize; ++column)
            {
                board[row][column] = cells[index];
                cells[index].GetComponent<Cell>().posY = row;
                cells[index].GetComponent<Cell>().posX = column;
                index++;
                if (index > cells.Length)
                {
                    throw new System.Exception("tried to convert too many cells to matrix");
                }
            }
        }
        return board;
    }

    public void BuildMaze()
    {
        
        if (!GameController.Instance.getRestart())
        {
            Maze = new GameObject("Maze");

            WallHolder = new GameObject();
            WallHolder.name = "Walls";
            WallHolder.transform.parent = Maze.transform;

            CreatFloor();
            CreatWalls();
            CreatPlayer();

            WallHolder.SetActive(false);
            
        }
        else
        {
            if (oldXSize == xSize && oldYSize == ySize)
            {
                if (oldNumPlayers < numPlayers)
                {
                    switch (numPlayers - oldNumPlayers)
                    {
                        case 1:
                            //erzeuge einen neuen B oder C oder D
                            if (oldNumPlayers == 1)
                            {
                                GeneratePlayerPfeil();
                            }
                            else if (oldNumPlayers == 2)
                            {
                                GeneratePlayerwasd();
                            }
                            else
                            {
                                GeneratePlayeruhjk();
                            }
                            break;
                        case 2:
                            //erzeuge 2 neue B && C oder C && D
                            if (oldNumPlayers == 1 && numPlayers == 3)
                            {
                                GeneratePlayerPfeil();
                                GeneratePlayerwasd();
                            }
                            else
                            {
                                GeneratePlayerwasd();
                                GeneratePlayeruhjk();
                            }                            
                            break;
                        case 3:
                            //erzeuge 3 neue B && C && D
                            GeneratePlayerPfeil();
                            GeneratePlayerwasd();
                            GeneratePlayeruhjk();
                            break;
                    }
                }
                else if(oldNumPlayers > numPlayers) //oldNumPlayers > numPlayers
                {
                    switch (oldNumPlayers - numPlayers)
                    {
                        case 1:
                            if(oldNumPlayers == 4)
                            {
                                playerD.SetActive(false);
                            }
                            else if(oldNumPlayers == 3)
                            {
                                playerC.SetActive(false);
                            }
                            else
                            {
                                playerB.SetActive(false);
                            }
                            //loesche 1 B oder C oder D
                            break;
                        case 2:
                            //loesche 2 B && C(oldSize = 3) oder C && D (oldsize = 4)
                            if(oldNumPlayers == 3 && numPlayers == 1)
                            {
                                playerB.SetActive(false);
                                playerC.SetActive(false);
                            }else 
                            {
                                playerC.SetActive(false);
                                playerC.SetActive(false);
                            }
                            break;
                        case 3:
                            //loesche B, C und D
                            playerB.SetActive(false);
                            playerC.SetActive(false);
                            playerD.SetActive(false);
                            break;
                    }

                }


                WallHolder.SetActive(true);
                ActivateMaze();
                CreatMaze();
                WallHolder.SetActive(false);
            }
            else
            {
                //mach alles neu
                WallHolder.SetActive(true);
                DestroyMaze();

                CreatFloor();
                CreatWalls();
                CreatPlayer();
                WallHolder.SetActive(false);

            }
           
        }

        oldXSize = xSize;
        oldYSize = ySize;
        oldNumPlayers = numPlayers;

    }

   
    void CreatPlayer()
    {

        switch (numPlayers)
        {
            case 1:
                GeneratePlayerMouse(); 
                break;
            case 2:
                GeneratePlayerMouse();
                GeneratePlayerPfeil();
                break;
            case 3:
                GeneratePlayerMouse();
                GeneratePlayerPfeil();
                GeneratePlayerwasd();
                break;
            case 4:
                GeneratePlayerMouse();
                GeneratePlayerPfeil();
                GeneratePlayerwasd();
                GeneratePlayeruhjk();
                break;
        }
    }

    void GeneratePlayerMouse()
    {
        float nxSize = (float)xSize / 2.0f;
        float nySize = (float)ySize / 2.0f;
        playerA.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        playerA.transform.position = cells[0].transform.position;
        playerA.gameObject.GetComponent<Player>().cell = cells[0].GetComponent<Cell>();
        cells[0].GetComponent<Cell>().players.Add(playerA.gameObject.GetComponent<Player>());
        playerA.SetActive(true);

        return;

        // TODO Rewrite for cell instead of vector
        Vector3 myPosp = new Vector3(nxSize - 0.5f, 0, nySize - 0.5f);
        Vector3 myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 0.5f);


        if (xSize % 2 == 0 && ySize % 2 == 1)
        {
            myPosp = new Vector3(nxSize - 0.5f,0, nySize - 0.5f);
            myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 0.5f);
        }
        else if (xSize % 2 == 0 && ySize % 2 == 0)
        {
            myPosp = new Vector3(nxSize - 0.5f, 0, nySize - 1.0f);
            myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 1.0f);

        }
        else if (xSize % 2 == 1 && ySize % 2 == 1)
        {
            myPosp = new Vector3(nxSize, 0.5f, nySize - 0.5f);
            myPos = new Vector3(nxSize, 3.0f, nySize - 0.5f);

        }
        else if (xSize % 2 == 1 && ySize % 2 == 0)
        {
            myPosp = new Vector3(nxSize, 0.5f, nySize - 1.0f);
            myPos = new Vector3(nxSize, 3.0f, nySize - 1.0f);

        }
        playerA.transform.position = myPosp;
        playerA.SetActive(true);
    }

    void GeneratePlayerPfeil()
    {
        float nxSize = (float)xSize / 2.0f;
        float nySize = (float)ySize / 2.0f;
        playerB.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        playerB.transform.position = cells[cells.Length-1].transform.position;
        playerB.gameObject.GetComponent<Player>().cell = cells[cells.Length-1].GetComponent<Cell>();
        cells[cells.Length - 1].GetComponent<Cell>().players.Add(playerB.gameObject.GetComponent<Player>());
        playerB.SetActive(true);

        return;


        // TODO Rewrite for cell instead of vector

        Vector3 myPosp = new Vector3(-nxSize + 0.5f, 0.5f, -nySize + 0.5f);
        Vector3 myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 0.5f);


        if (xSize % 2 == 0 && ySize % 2 == 1)
        {
            myPosp = new Vector3(-nxSize + 0.5f, 0.5f, -nySize + 0.5f);
            //myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 0.5f);
        }
        else if (xSize % 2 == 0 && ySize % 2 == 0)
        {
            myPosp = new Vector3(-nxSize + 0.5f, 0.5f, -nySize + 1.0f);
            //myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 1.0f);

        }
        else if (xSize % 2 == 1 && ySize % 2 == 1)
        {
            myPosp = new Vector3(-nxSize + 1.0f, 0.5f, -nySize + 0.5f);
            myPos = new Vector3(-nxSize + 1.0f, 3.0f, -nySize + 0.5f);

        }
        else if (xSize % 2 == 1 && ySize % 2 == 0)
        {
            myPosp = new Vector3(-nxSize, 0.5f, -nySize + 1.0f);
            //myPos = new Vector3(nxSize, 3.0f, nySize - 1.0f);

        }
        playerB.transform.position = myPosp;
        playerB.SetActive(true);
    }

    void GeneratePlayerwasd()
    {
        float nxSize = (float)xSize / 2.0f;
        float nySize = (float)ySize / 2.0f;
        playerC.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        Vector3 myPosp = new Vector3(-nxSize + 0.5f, 0.5f, -nySize + 0.5f);
        Vector3 myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 0.5f);


        if (xSize % 2 == 0 && ySize % 2 == 1)
        {
            myPosp = new Vector3(-nxSize + 0.5f, 0.5f, -nySize + 0.5f);
            //myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 0.5f);
        }
        else if (xSize % 2 == 0 && ySize % 2 == 0)
        {
            myPosp = new Vector3(-nxSize + 0.5f, 0.5f, -nySize + 1.0f);
            //myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 1.0f);

        }
        else if (xSize % 2 == 1 && ySize % 2 == 1)
        {
            myPosp = new Vector3(-nxSize + 1.0f, 0.5f, nySize - 0.5f);
            myPos = new Vector3(-nxSize+1.0f, 3.0f, nySize - 0.5f);

        }
        else if (xSize % 2 == 1 && ySize % 2 == 0)
        {
            myPosp = new Vector3(-nxSize, 0.5f, -nySize + 1.0f);
            //myPos = new Vector3(nxSize, 3.0f, nySize - 1.0f);

        }
        playerC.transform.position = myPosp;
        playerC.SetActive(true);
    }

    void GeneratePlayeruhjk()
    {
        float nxSize = (float)xSize / 2.0f;
        float nySize = (float)ySize / 2.0f;
        playerD.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        Vector3 myPosp = new Vector3(-nxSize + 0.5f, 0.5f, -nySize + 0.5f);
        Vector3 myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 0.5f);


        if (xSize % 2 == 0 && ySize % 2 == 1)
        {
            myPosp = new Vector3(-nxSize + 0.5f, 0.5f, -nySize + 0.5f);
            //myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 0.5f);
        }
        else if (xSize % 2 == 0 && ySize % 2 == 0)
        {
            myPosp = new Vector3(-nxSize + 0.5f, 0.5f, -nySize + 1.0f);
            //myPos = new Vector3(nxSize - 0.5f, 3.0f, nySize - 1.0f);

        }
        //Nur das ist wichtig!!
        else if (xSize % 2 == 1 && ySize % 2 == 1)
        {
            myPosp = new Vector3(nxSize, 0.5f, -nySize + 0.5f);
            myPos = new Vector3(nxSize, 3.0f, -nySize + 0.5f);

        }
        else if (xSize % 2 == 1 && ySize % 2 == 0)
        {
            myPosp = new Vector3(-nxSize, 0.5f, -nySize + 1.0f);
            //myPos = new Vector3(nxSize, 3.0f, nySize - 1.0f);

        }
        playerD.transform.position = myPosp;
        playerD.SetActive(true);
    }

    void CreatFloor()
    {
        
        float nxSize = (float)xSize;
        float nySize = (float)ySize;
        floor.transform.localScale = new Vector3(nxSize / 10, 0.5f, nySize / 10);
        Vector3 myPos = new Vector3(0, 0, 0);
        if(xSize % 2 == 0 && ySize % 2 == 1)
        {
            myPos = new Vector3(0, 0, 0);
        }else if(xSize % 2 == 0 && ySize%2 == 0)
        {
            myPos = new Vector3(0, 0, -0.5f);
        }else if(xSize % 2 == 1 && ySize %2 == 1){
            myPos = new Vector3(0.5f, 0, 0);
        }else if (xSize %2 == 1 && ySize % 2 == 0)
        {
            myPos = new Vector3(0.5f, 0, -0.5f);
        }
        
        tempFloor = Instantiate(floor, myPos, Quaternion.identity) as GameObject;

    }

    void CreatWalls()
    {
        
        initialPos = new Vector3((-xSize / 2) + wallLength / 2, 0.0f, (-ySize / 2) + wallLength / 2);
        Vector3 myPos = initialPos;
        GameObject tempWall;

        //For xAxis
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength) - wallLength / 2,
                    wallLength / 2, initialPos.z + (i * wallLength) - wallLength / 2);
                tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                tempWall.transform.parent = WallHolder.transform;
            }
        }

        //For yAxis
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength),
                    wallLength / 2, initialPos.z + (i * wallLength) - wallLength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;
                tempWall.transform.parent = WallHolder.transform;
            }
        }
        creatCells();

    }

    void creatCells()
    {

        Cells = new GameObject("Cells" + xSize);

        Cells.transform.localPosition = new Vector3(-xSize * wallLength / 2, 0, ySize * wallLength / 2);
        Cells.transform.parent = Maze.transform;

        lastCells = new List<int>();
        lastCells.Clear();
        totalCells = xSize * ySize;
        int children = WallHolder.transform.childCount;
        GameObject[] allWalls = new GameObject[children];
        cellProperties = new CellProperties[xSize * ySize];
        cells = new GameObject[xSize * ySize];
        int eastWestProcess = 0;
        int childprocess = 0;
        int termCount = 0;

        //Get All The Children
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = WallHolder.transform.GetChild(i).gameObject;
        }

        //Assigns Walls to the Cells
        bool switcher = true;
        for (int i = 0; i < cellProperties.Length; i++)
        {

            GameObject c = new GameObject("Cell_" + i);
            c.transform.parent = Cells.transform;
            c.AddComponent<BoxCollider>().size = new Vector3(wallLength, wallLength, wallLength);
            c.GetComponent<BoxCollider>().isTrigger = true;
            c.AddComponent<Cell>().hasLight = switcher;
            cells[i] = c.gameObject;
            switcher = !switcher;

            if (termCount == xSize)
            {
                eastWestProcess++;
                termCount = 0;
            }

            cellProperties[i] = new CellProperties();
            cellProperties[i].east = allWalls[eastWestProcess];
            cellProperties[i].south = allWalls[childprocess + (xSize + 1) * ySize];

            eastWestProcess++;
            termCount++;

            childprocess++;
            cellProperties[i].west = allWalls[eastWestProcess];
            cellProperties[i].north = allWalls[(childprocess + (xSize + 1) * ySize) + xSize - 1];

            c.transform.position = cellProperties[i].north.transform.position - new Vector3(0, 0, wallLength / 2);

            cellProperties[i].east.transform.parent = c.transform;
            cellProperties[i].south.transform.parent = c.transform;
            cellProperties[i].west.transform.parent = c.transform;
            cellProperties[i].north.transform.parent = c.transform;

        }
        CreatMaze();

    }

    void CreatMaze()
    {

        while (visitedCells < totalCells)
        {
            if (startedBuilding)
            {
                GiveMeNeighbour();
                if (cellProperties[currentNeighbour].visited == false && cellProperties[currentCell].visited)
                {
                    BreakWall();
                    cellProperties[currentNeighbour].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;
                    if (lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currentCell = Random.Range(0, totalCells); 
                cellProperties[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }

        }
    }

    void BreakWall()
    {
        switch (wallToBreak)
        {
            case 1: cellProperties[currentCell].north.SetActive(false); break;
            case 2: cellProperties[currentCell].east.SetActive(false); break;
            case 3: cellProperties[currentCell].west.SetActive(false); break;
            case 4: cellProperties[currentCell].south.SetActive(false); break;
        }
    }

    void GiveMeNeighbour()
    {

        int length = 0;
        int[] neighbour = new int[4];
        int[] connectingWalls = new int[4];
        int check = 0;
        check = (currentCell + 1) / xSize;
        check -= 1;
        check *= xSize;
        check += xSize;

        //West
        if (currentCell + 1 < totalCells && currentCell + 1 != check)
        {
            if (cellProperties[currentCell + 1].visited == false)
            {
                neighbour[length] = currentCell + 1;
                connectingWalls[length] = 3;
                length++;
            }
        }

        //East
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (cellProperties[currentCell - 1].visited == false)
            {
                neighbour[length] = currentCell - 1;
                connectingWalls[length] = 2;
                length++;
            }
        }

        //North
        if (currentCell + xSize < totalCells)
        {
            if (cellProperties[currentCell + xSize].visited == false)
            {
                neighbour[length] = currentCell + xSize;
                connectingWalls[length] = 1;
                length++;
            }
        }

        //South
        if (currentCell - xSize >= 0)
        {
            if (cellProperties[currentCell - xSize].visited == false)
            {
                neighbour[length] = currentCell - xSize;
                connectingWalls[length] = 4;
                length++;
            }
        }

        if (length > 0)
        {
            int theChosenOne = Random.Range(0, length);
            currentNeighbour = neighbour[theChosenOne];
            wallToBreak = connectingWalls[theChosenOne];
        }
        else
        {
            if (backingUp >= 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }
    }
    
    void ActivateMaze()
    {
        
        for (int i = 0; i < oldXSize * oldYSize; i++)
        {
            cellProperties[i].north.SetActive(true);
            cellProperties[i].east.SetActive(true); 
            cellProperties[i].west.SetActive(true); 
            cellProperties[i].south.SetActive(true);
            cellProperties[i].visited = false;
            startedBuilding = false;
            visitedCells = 0;
            backingUp = 0;
            wallToBreak = 0;
        }

    }

    void DestroyMaze()
    {
        
        DestroyObject(Cells);
        DestroyObject(tempFloor);

        startedBuilding = false;
        visitedCells = 0;
        backingUp = 0;
        wallToBreak = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
