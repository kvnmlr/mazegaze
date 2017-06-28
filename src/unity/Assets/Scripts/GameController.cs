using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public bool useMenu = false;
    public Player[] players;
    public MazeGenerator mazeGenerator;
    public PowerUpManager powerUpManager;
    public Menu menu;
    //public GameObject target;
    //public GameObject targetLight;

    void Start () {
        mazeGenerator.xSize = 9;
        mazeGenerator.ySize = 9;
        Debug.Log("MazeGaze");

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
        mazeGenerator.target = powerUpManager.spawnPowerUp(PowerUpManager.PowerUpTypes.Target, gameObject).gameObject;


        mazeGenerator.BuildMaze();

        // Just for testing
        powerUpManager.spawnPowerUp(PowerUpManager.PowerUpTypes.Endurance, mazeGenerator.cells[3].GetComponent<Cell>().gameObject);
        powerUpManager.spawnPowerUp(PowerUpManager.PowerUpTypes.Enlightenment, mazeGenerator.cells[4].GetComponent<Cell>().gameObject);
        powerUpManager.spawnPowerUp(PowerUpManager.PowerUpTypes.ShowTarget, mazeGenerator.cells[5].GetComponent<Cell>().gameObject);
    }

    private void setUpPlayers()
    {
        foreach (Player p in players)
        {
            Debug.Log("Player " + p.name + " joined the game");
        }
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
