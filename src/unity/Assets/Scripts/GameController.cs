using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public GameObject cursor;
    public bool useMenu = false;
    public Dictionary<Player, int> joinedPlayersToPosition = new Dictionary<Player, int>();
    public Player[] players;

    private MazeGenerator mazeGenerator;
    private PowerUpManager powerUpManager;
    private Menu menu;

    public bool gameover { get; set; }
    private bool restart;

    private bool firstround;

    public int playedGames { get; set; }
    public int numGames { get; set; }

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
        if (!useMenu)
        {
            setUpPlayers();
            mazeGenerator.numPlayers = 4;
            mazeGenerator.xSize = 7;
            mazeGenerator.ySize = 7;
            menu.StartGame();
        }
    }

    private void startCalibration(PupilConfiguration.PupilClient client, bool startRound = false)
    {
        PupilListener.Instance.StartCalibration(client);
        client.is_calibrated = true;        // TODO replace with correct calibration
        if (startRound)
        {
            setUpNewRound();
        }
    }

    public void startPlayerAssignment()
    {
        foreach (Player p in players)
        {
            p.gameObject.SetActive(true);
        }
        joinedPlayersToPosition = new Dictionary<Player, int>();
        Menu.Instance.joinScreen.SetActive(true);
    }

    public void assignMousePlayer()
    {
        for (int i = 1; i <= 4; ++i)
        {
            if(!joinedPlayersToPosition.ContainsValue(i))
            {
                assignPlayer(players[0], i);
            }
        }
    }
    public void assignKeyboardPlayer()
    {
        for (int i = 1; i <= 4; ++i)
        {
            if(!joinedPlayersToPosition.ContainsValue(i))
            {
                assignPlayer(players[1], i);
            }
        }
    }

    public void assignPlayer(Player player, int position)
    {
        if (!joinedPlayersToPosition.ContainsKey(player) && !joinedPlayersToPosition.ContainsValue(position))
        {
            joinedPlayersToPosition.Add(player, position);
            if (joinedPlayersToPosition.Keys.Count == MazeGenerator.Instance.numPlayers)
            {
                playersAssigned();
            }
        } 
    }

    public void playersAssigned()
    {
        Menu.Instance.joinScreen.SetActive(false);
        startNewRound();
    }

    public void setUpNewRound()
    {
        restart = false;
        playedGames = 0;
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
                    client.player = p;
                    client.is_calibrated = true;
                    if (!client.is_calibrated)
                    {
                        //Debug.Log(client.name + " is not calibrated. Starting calibration procedure");
                        startCalibration(client, true);
                        return;
                    } else
                    {
                        //Debug.Log(client.name + " is calibrated and ready to play");
                    }
                }
            }
        }

        startPlayerAssignment();
    }

    public void startNewRound()
    {

        if (playedGames == 0 && !restart /*|| mazeBuildAttempts < 20*/)     // TODO not sure why this is good
        {
            mazeBuildAttempts++;

            mazeGenerator.DestroyMaze();
            setUpPlayers();

            mazeGenerator.target = powerUpManager.target.gameObject;
            mazeGenerator.BuildMaze();

            // adjust camera height
            Camera.main.transform.position = new Vector3(0, MazeGenerator.Instance.xSize * 4, 0);

            if (powerUpManager.spawnPowerUp(PowerUpManager.PowerUpTypes.Target) == null)
            {
                Debug.Log("Target not spawned");
                startNewRound();
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
        foreach(Player p in joinedPlayersToPosition.Keys)
        {
            p.gameObject.SetActive(true);
        }

        //playedGames++;
        gameover = false;
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
        double mitte = System.Math.Floor((double)(mazeGenerator.xSize / 2));
        double wRand = System.Math.Floor((mazeGenerator.xSize - mitte) / 2);
        double sRand = wRand;
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
        foreach(Player p in players)
        {
            p.gameObject.SetActive(false);
        }
        mazeGenerator.numPlayers = 0;
        mazeGenerator.playerA = null;
        mazeGenerator.playerB = null;
        mazeGenerator.playerC = null;
        mazeGenerator.playerD = null;

        foreach (Player p in joinedPlayersToPosition.Keys)
        {
            MazeGenerator.Instance.numPlayers++;
            int pos;
            joinedPlayersToPosition.TryGetValue(p, out pos);
            switch (pos) {
                case 1:
                    mazeGenerator.playerA = p.gameObject;
                    break;
                case 2:
                    mazeGenerator.playerB = p.gameObject;
                    break;
                case 3:
                    mazeGenerator.playerC = p.gameObject;
                    break;
                case 4:
                    mazeGenerator.playerD = p.gameObject;
                    break;
                default:
                    Debug.Log("Invalid position value for player " + p.name + "(" + pos + ")");
                    break;
            }
        }

        foreach(Player p1 in joinedPlayersToPosition.Keys)
        {
            foreach (Player p2 in joinedPlayersToPosition.Keys)
            {
                if (p1.Equals(p2))
                {
                    continue;
                }
                Physics.IgnoreCollision(p1.gameObject.GetComponent<Collider>(), p2.gameObject.GetComponent<Collider>());
            }
        }
    }

}
