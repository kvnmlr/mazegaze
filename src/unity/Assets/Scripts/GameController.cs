using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public bool useMenu = false;
    public Player[] players;
    public Player[] joined;

    private MazeGenerator mazeGenerator;
    private PowerUpManager powerUpManager;
    private Menu menu;

    private bool gameover;
    private bool restart;

    private bool firstround;

    private int playedGames = 0;
    private int numGames;

    private int mazeBuildAttempts = 0;

    public void setRestart(bool b)
    {
        restart = b;
    }

    public bool getRestart()
    {
        return restart;
    }

    public void setNumGames(int i)
    {
        playedGames = 0;
        numGames = i;
    }

    public int getNumGames()
    {
        return numGames;
    }

    public int getPlayedGames()
    {
        return playedGames;
    }

    void Start()
    {
        AudioManager.Instance.play(AudioManager.SOUNDS.MENU);
        gameover = false;
        restart = false;

        mazeGenerator = MazeGenerator.Instance;
        powerUpManager = PowerUpManager.Instance;
        menu = Menu.Instance;

        mazeGenerator.xSize = 9;
        mazeGenerator.ySize = 9;

        joined = new Player[players.Length];
        setUpPlayers();

        if (!useMenu)
        {
            mazeGenerator.numPlayers = 4;
            mazeGenerator.xSize = 7;
            mazeGenerator.ySize = 7;
            menu.StartGame();
        }
    }

    private void startCalibration(PupilConfiguration.PupilClient client, bool startRound = false)
    {
        client.is_calibrated = true;
        if (startRound)
        {
            setUpNewRound();
        }
    }

    public void startPlayerAssignment()
    {
        Menu.Instance.joinScreen.SetActive(true);
        //playersAssigned();
    }

    public void assignMousePlayer()
    {
        assignPlayer(players[0], 0);
    }
    public void assignKeyboardPlayer()
    {
        assignPlayer(players[1], 1);
    }
    public void assignPlayer(Player player, int position)
    {
        Debug.Log(player.name + " joined the round");
        joined[position] = player;
        int numJoined = 0;
        foreach (Player p in joined)
        {
            if (p != null)
            {
                numJoined++;
            }
        }
        if (numJoined == MazeGenerator.Instance.numPlayers)
        {
            playersAssigned();
        }
    }

    public void playersAssigned()
    {
        Debug.Log("All players joined");
        Menu.Instance.joinScreen.SetActive(false);
        startNewRound();
    }

    public void setUpNewRound()
    {
        // check if player need calibration
        foreach (Player p in players)
        {
            if (p.gameObject.GetComponent<GazeController>() != null)
            {
                // this is a gaze controlled player
                PupilConfiguration.PupilClient client = PupilConfiguration.Instance.settings.pupil_clients.Find((x) => x.name.Equals(p.name));
                if (client == null)
                {
                    Debug.LogWarning("Did not find client with name " + p.name + " in pupil settings");
                } else
                {
                    if (!client.is_calibrated)
                    {
                        Debug.Log(client.name + " is not calibrated. Starting calibration procedure");
                        startCalibration(client, true);
                        return;
                    } else
                    {
                        Debug.Log(client.name + " is calibrated and ready to play");
                    }
                }
            }
        }

        startPlayerAssignment();
    }

    public void startNewRound() { 

        mazeBuildAttempts++;
        if (playedGames == 0 || mazeBuildAttempts > 20)
        {
            mazeGenerator.target = powerUpManager.target.gameObject;
            mazeGenerator.BuildMaze();

            // adjust camera height
            Camera.main.transform.position = new Vector3(0, MazeGenerator.Instance.xSize * 4, 0);

            if (powerUpManager.spawnPowerUp(PowerUpManager.PowerUpTypes.Target) == null)
            {
                mazeGenerator.DestroyMaze();
                setUpNewRound();
                return;
            }
            powerUpManager.setSpawnedPowerUps(0);
            Debug.Log("Took " + mazeBuildAttempts + " attempts to build a good maze");
            mazeBuildAttempts = 0;
        }
        else
        {
            powerUpManager.spawnPowerUp(PowerUpManager.PowerUpTypes.Target, true);
            powerUpManager.setSpawnedPowerUps(0);
        }
        playedGames++;
    }


    private void Update()
    {

    }

    public void GameOver()
    {
        //gameover = true;
    }

    private int getRandomCellTarget()
    {
        //double mitteungerundet = (mazeGenerator.xSize / 2);
        double mitte = System.Math.Floor((double)(mazeGenerator.xSize / 2));
        double wRand = System.Math.Floor((mazeGenerator.xSize - mitte) / 2);
        //double eRand = System.Math.Ceiling((mazeGenerator.xSize - mitte) / 2);
        double sRand = wRand;
        //double nRand = eRand;
        double[] zufall = new double[(int)mitte];
        float RechteGrenze = (float)(sRand * mazeGenerator.xSize + wRand);
        float LinkeGrenze = (float)(sRand * mazeGenerator.xSize + wRand + mitte);
        for (int i = 1; i <= mitte; i++)
        {


            float rd = Random.Range(RechteGrenze + (float)mazeGenerator.xSize * (i - 1),
           LinkeGrenze + mazeGenerator.xSize * (i - 1));

            zufall[i - 1] = rd;
        }

        int j = Random.Range(0, (int)mitte - 1);
        j = (int)zufall[j];



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
            mazeGenerator.playerA = players[0].gameObject;
            mazeGenerator.numPlayers = 1;
        }
        if (players.Length > 1)
        {
            mazeGenerator.playerB = players[1].gameObject;
            mazeGenerator.numPlayers = 2;
            Physics.IgnoreCollision(players[0].gameObject.GetComponent<Collider>(), players[1].gameObject.GetComponent<Collider>());
        }
        if (players.Length > 2)
        {
            mazeGenerator.playerC = players[2].gameObject;
            mazeGenerator.numPlayers = 3;
            Physics.IgnoreCollision(players[0].gameObject.GetComponent<Collider>(), players[2].gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(players[1].gameObject.GetComponent<Collider>(), players[2].gameObject.GetComponent<Collider>());

        }
        if (players.Length > 3)
        {
            mazeGenerator.playerD = players[3].gameObject;
            mazeGenerator.numPlayers = 4;
            Physics.IgnoreCollision(players[0].gameObject.GetComponent<Collider>(), players[3].gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(players[1].gameObject.GetComponent<Collider>(), players[3].gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(players[2].gameObject.GetComponent<Collider>(), players[3].gameObject.GetComponent<Collider>());

        }
    }

}
