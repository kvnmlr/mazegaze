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

    public GameObject target { get; set; }
    //public GameObject targetLight { get; set; }   targetLight is im target drin, wir brauchen dafuer keine extra variable hier.

    private float wallLength = 1.0f;
    public int xSize { get; set; }
    public int ySize { get; set; }

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

    public GameObject[][] toMatrix(GameObject[] cells)
    {
        GameObject[][] board = new GameObject[ySize][];
        int index = 0;
        for (int row = 0; row <ySize; ++row)
        {
            board[row] = new GameObject[xSize];
            for(int column = 0; column < xSize; ++column)
            {
                board[row][column] = cells[index];
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
        Maze = new GameObject("Maze");

        WallHolder = new GameObject();
        WallHolder.name = "Walls";
        WallHolder.transform.parent = Maze.transform;


        CreatFloor();

        CreatWalls();

        CreatPlayer();

        Destroy(WallHolder);

    }

   
    //TODO: Werte anpasse fuer jedes Maze
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
        GameObject tempFloor;
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
        Cells = new GameObject("Cells");
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
                start = cellProperties[currentCell]; //Startcell
                cellProperties[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
                //TODO: Von dieser Zelle Koordinaten rausbekommen -> Zielobjekt hierhin plotten
            }

        }
    }

    void BreakWall()
    {
        switch (wallToBreak)
        {
            case 1: Destroy(cellProperties[currentCell].north); break;
            case 2: Destroy(cellProperties[currentCell].east); break;
            case 3: Destroy(cellProperties[currentCell].west); break;
            case 4: Destroy(cellProperties[currentCell].south); break;
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
    
    public void StartNewGame()
    {

        Debug.Log("Geghen rhie rein");

        int m = 0;
        Vector2 player1 = new Vector2();
        Vector2 player2 = new Vector2();
        Vector2 player3 = new Vector2();
        Vector2 player4 = new Vector2();
        float x, y;
        switch (numPlayers)
        {
            case 1:
                x = playerA.transform.position.x;
                y = playerA.transform.position.z;
                player1 = new Vector2(x, y);
                m = 1;
                break;
            case 2:
                x = playerA.transform.position.x;
                y = playerA.transform.position.z;
                player1 = new Vector2(x, y);

                x = playerB.transform.position.x;
                y = playerB.transform.position.z;
                player2 = new Vector2(x, y);
                m = 2;
                break;
            case 3:
                x = playerA.transform.position.x;
                y = playerA.transform.position.z;
                player1 = new Vector2(x, y);

                x = playerB.transform.position.x;
                y = playerB.transform.position.z;
                player2 = new Vector2(x, y);

                x = playerC.transform.position.x;
                y = playerC.transform.position.z;
                player3 = new Vector2(x, y);
                m = 3;
                break;
            case 4:
                x = playerA.transform.position.x;
                y = playerA.transform.position.z;
                player1 = new Vector2(x, y);

                x = playerB.transform.position.x;
                y = playerB.transform.position.z;
                player2 = new Vector2(x, y);

                x = playerC.transform.position.x;
                y = playerC.transform.position.z;
                player3 = new Vector2(x, y);

                x = playerD.transform.position.x;
                y = playerD.transform.position.z;
                player4 = new Vector2(x, y);
                m = 4;
                break;
        }

        //eventuell ueberall noch +.5f fuer x 
        Vector3 newPos = new Vector3();
        switch (m)
        {
            case 1:
                newPos = target.gameObject.GetComponent<Target>().TransformpositionRandom();
                break;
            case 2:
                Vector2 v = player1 - player2;
                Vector2 mitte = new Vector2(0.5f * (player1.x + player2.x), 0.5f * (player1.y + player2.y));
                Vector2 lamda = new Vector2(1 / v.x, -1 / v.x);
                //Vector2 zufall = Random.insideUnitCircle;
                //TODO: eventuell noch zufall einbauen in die funktion, um auch 
                //tatsaechlich einen zufall zu erzeugen und nicht immer ene voraussehbare Position zu haben
                newPos = new Vector3(mitte.x + lamda.x, 0.5f, mitte.y + lamda.y);
                break;
            case 3:
                //Loesung durch umkreismittelpunkt
                float d = 2 * (player1.x * (player2.y - player3.y) + player2.x * (player3.y - player1.y) +
                    player3.x * (player1.y - player2.y));
                float mx = ((player1.x * player1.x + player1.y * player1.y) * (player2.y - player3.y) + 
                    (player2.x * player2.x + player2.y * player2.y) * (player3.y - player1.y) + 
                    (player3.x * player3.x + player3.y * player3.y) * (player3.y - player3.y)) / d;

                float my = ((player1.x * player1.x + player1.y * player1.y) * (player2.x - player3.x) +
                    (player2.x * player2.x + player2.y * player2.y) * (player3.x - player1.x) +
                    (player3.x * player3.x + player3.y * player3.y) * (player3.x - player3.x)) / d;
                newPos = new Vector3(mx, 0.5f, my);
                break;
            case 4:
                //noch keine optimale loesung, aber glaube gibt auch keine, da man bei 4 punkten nicht immer einen Umkreismittelpunkt findet
                newPos = new Vector3((player1.x + player2.x + player3.x + player4.x) / 4, 0.5f, (player2.y + player1.y + player3.y + player4.y) / 4);

                break;

        }

        
        float xtest = newPos.x + ((xSize/2));
        float ytest = newPos.z + (xSize/2);

        GameObject[][] checking = toMatrix(cells);
        int xwert = (int)System.Math.Floor(xtest);
        int ywert = (int)System.Math.Floor(ytest);
        Debug.Log("x " + xwert + " y " + ywert);
        //TODO dieser x und y wert muss jetzt in eine Cell umgewandelt werden und da muss das neue target spawnen


    }
    // Update is called once per frame
    void Update()
    {

    }

}
