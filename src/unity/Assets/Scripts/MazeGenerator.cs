﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

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
    public int NumPlayer { get; set; }
    private float wallLength = 1.0f;
    public int xSize = 5;
    public int ySize = 5;
    private Vector3 initialPos;
    private GameObject WallHolder;
    private GameObject Maze;
    private GameObject Cells;
    public CellProperties[] cells { get; set; }
    private int currentCell;
    private int totalCells;
    private int visitedCells = 0;
    private bool startedBuilding = false;
    private int currentNeighbour;
    private List<int> lastCells;
    private int backingUp = 0;
    private int wallToBreak = 0;

    // Use this for initialization
    void Start()
    {

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

        switch (NumPlayer)
        {
            case 1:
                GeneratePlayerMouse();
                //playerB.SetActive(false);
                //playerC.SetActive(false);
                //playerD.SetActive(false);
                break;
            case 2:
                GeneratePlayerMouse();
                GeneratePlayerPfeil();
                //playerC.SetActive(false);
                //playerD.SetActive(false);
                break;
            case 3:
                GeneratePlayerMouse();
                GeneratePlayerPfeil();
                GeneratePlayerwasd();
                //playerD.SetActive(false);
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
        cells = new CellProperties[xSize * ySize];
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
        for (int i = 0; i < cells.Length; i++)
        {

            GameObject c = new GameObject("Cell_" + i);
            c.transform.parent = Cells.transform;
            c.AddComponent<BoxCollider>().size = new Vector3(wallLength, wallLength, wallLength);
            c.GetComponent<BoxCollider>().isTrigger = true;
            c.AddComponent<Cell>().hasLight = switcher;
            switcher = !switcher;

            if (termCount == xSize)
            {
                eastWestProcess++;
                termCount = 0;
            }

            cells[i] = new CellProperties();
            cells[i].east = allWalls[eastWestProcess];
            cells[i].south = allWalls[childprocess + (xSize + 1) * ySize];

            eastWestProcess++;
            termCount++;

            childprocess++;
            cells[i].west = allWalls[eastWestProcess];
            cells[i].north = allWalls[(childprocess + (xSize + 1) * ySize) + xSize - 1];

            c.transform.position = cells[i].north.transform.position - new Vector3(0, 0, wallLength / 2);

            cells[i].east.transform.parent = c.transform;
            cells[i].south.transform.parent = c.transform;
            cells[i].west.transform.parent = c.transform;
            cells[i].north.transform.parent = c.transform;

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
                if (cells[currentNeighbour].visited == false && cells[currentCell].visited)
                {
                    BreakWall();
                    cells[currentNeighbour].visited = true;
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
                currentCell = Random.Range(0, totalCells); //Startzelle
                cells[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
                //TODO: Von dieser Zelle Koordinaten rausbekommen -> Zielobjekt hierhin plotten
            }

        }
        Debug.Log("Finied");

    }

    void BreakWall()
    {
        switch (wallToBreak)
        {
            case 1: Destroy(cells[currentCell].north); break;
            case 2: Destroy(cells[currentCell].east); break;
            case 3: Destroy(cells[currentCell].west); break;
            case 4: Destroy(cells[currentCell].south); break;
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
            if (cells[currentCell + 1].visited == false)
            {
                neighbour[length] = currentCell + 1;
                connectingWalls[length] = 3;
                length++;
            }
        }

        //East
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (cells[currentCell - 1].visited == false)
            {
                neighbour[length] = currentCell - 1;
                connectingWalls[length] = 2;
                length++;
            }
        }

        //North
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neighbour[length] = currentCell + xSize;
                connectingWalls[length] = 1;
                length++;
            }
        }

        //South
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].visited == false)
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

    // Update is called once per frame
    void Update()
    {

    }
}
