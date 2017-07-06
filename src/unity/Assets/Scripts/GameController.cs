using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public bool useMenu = false;
    public Player[] players;

    private MazeGenerator mazeGenerator;
    private PowerUpManager powerUpManager;
    private Menu menu;
    //public GameObject target;
    //public GameObject targetLight;

    private bool gameover;
    private bool restart;

    private bool firstround;

    public void setRestart(bool b)
    {
        restart = b;
    }

    public bool getRestart()
    {
        return restart;
    }

    void Start () {
        gameover = false;
        restart = false;

        mazeGenerator = MazeGenerator.Instance;
        powerUpManager = PowerUpManager.Instance;
        menu = Menu.Instance;

        mazeGenerator.xSize = 9;
        mazeGenerator.ySize = 9;

        setUpPlayers();

        if (!useMenu)
        {
            mazeGenerator.numPlayers = 4;
            mazeGenerator.xSize = 7;
            mazeGenerator.ySize = 7;
            menu.StartGame();
        }
    }

    public void startNewRound()
    {

        if (!restart)
        {
            mazeGenerator.target = powerUpManager.target.gameObject;


            mazeGenerator.BuildMaze();

            // adjust camera height
            Camera.main.transform.position = new Vector3(0, MazeGenerator.Instance.xSize * 4, 0);


            //int rd = getRandomCellTarget();
            //Cell celltarget = mazeGenerator.cells[rd].GetComponent<Cell>();


            // spawnen nun beide Zufaellig Target im Mittlernen Quardant und Enlightenment absolut zufaellig
            powerUpManager.spawnPowerUp(PowerUpManager.PowerUpTypes.Target);
        }
        else
        {


            mazeGenerator.BuildMaze();
            Camera.main.transform.position = new Vector3(0, MazeGenerator.Instance.xSize * 4, 0);

            //TODO: @Kevin Hier neue Funktion fuer Target
            int rd = getRandomCellTarget();
            Cell celltarget = mazeGenerator.cells[rd].GetComponent<Cell>();
            powerUpManager.spawnPowerUp(PowerUpManager.PowerUpTypes.Target, celltarget);
        }
       
        
        
    }

    
    private void Update()
    {
        
    }

    public void GameOver()
    {
        gameover = true;
    }

    private int getRandomCellTarget()
    {
        double mitteungerundet = (mazeGenerator.xSize / 2);
        double mitte = System.Math.Floor((double)(mazeGenerator.xSize / 2));
        double wRand = System.Math.Floor((mazeGenerator.xSize - mitte) / 2);
        double eRand = System.Math.Ceiling((mazeGenerator.xSize - mitte) / 2);
        double sRand = wRand;
        double nRand = eRand;
        double[] zufall = new double[(int)mitte];
        float RechteGrenze = (float)(sRand * mazeGenerator.xSize + wRand);
        float LinkeGrenze = (float)(sRand * mazeGenerator.xSize + wRand + mitte);
        for (int i=1; i <= mitte; i++)
        {
            
           
            float rd = Random.Range(RechteGrenze + (float)mazeGenerator.xSize* (i-1),
           LinkeGrenze + mazeGenerator.xSize * (i-1));
            
            zufall[i - 1] = rd;
        }

        int j = Random.Range(0, (int)mitte-1);
        Debug.Log(j);
        j = (int)zufall[j];
        //Debug.Log("Zufallszelle ist: " + j);


        return j;
    }

    private void setUpPlayers()
    {
        if (players.Length > 4)
        {
            Debug.Log("The maximum number of players is 4");
        }

        if (players.Length > 0)
        {
            //mazeGenerator.targetLight = targetLight;
            //mazeGenerator.target = target;
            mazeGenerator.playerA = players[0].gameObject;
            mazeGenerator.numPlayers = 1;
        }
        if (players.Length > 1)
        {
            //mazeGenerator.targetLight = targetLight;
            //mazeGenerator.target = target;
            mazeGenerator.playerB = players[1].gameObject;
            mazeGenerator.numPlayers = 2;
            Physics.IgnoreCollision(players[0].gameObject.GetComponent<Collider>(), players[1].gameObject.GetComponent<Collider>());
        }
        if (players.Length > 2)
        {
            //mazeGenerator.targetLight = targetLight;
            //mazeGenerator.target = target;
            mazeGenerator.playerC = players[2].gameObject;
            mazeGenerator.numPlayers = 3;
            Physics.IgnoreCollision(players[0].gameObject.GetComponent<Collider>(), players[2].gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(players[1].gameObject.GetComponent<Collider>(), players[2].gameObject.GetComponent<Collider>());

        }
        if (players.Length > 3)
        {
            //mazeGenerator.targetLight = targetLight;
            //mazeGenerator.target = target;
            mazeGenerator.playerD = players[3].gameObject;
            mazeGenerator.numPlayers = 4;
            Physics.IgnoreCollision(players[0].gameObject.GetComponent<Collider>(), players[3].gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(players[1].gameObject.GetComponent<Collider>(), players[3].gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(players[2].gameObject.GetComponent<Collider>(), players[3].gameObject.GetComponent<Collider>());

        }
    }

}
