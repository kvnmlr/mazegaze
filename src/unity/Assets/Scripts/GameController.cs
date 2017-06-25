﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public Player[] players;
    public MazeGenerator mazeGenerator;
    public GameObject target;
    public GameObject targetLight;

    void Start () {
        Debug.Log("MazeGaze");
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
            mazeGenerator.targetLight = targetLight;
            mazeGenerator.target = target;
            mazeGenerator.playerA = players[0].gameObject;
            mazeGenerator.NumPlayer = 1;
        }
        if (players.Length > 1)
        {
            mazeGenerator.targetLight = targetLight;
            mazeGenerator.target = target;
            mazeGenerator.playerB = players[1].gameObject;
            mazeGenerator.NumPlayer = 2;
            Physics.IgnoreCollision(players[0].gameObject.GetComponent<Collider>(), players[1].gameObject.GetComponent<Collider>());
        }
        if (players.Length > 2)
        {
            mazeGenerator.targetLight = targetLight;
            mazeGenerator.target = target;
            mazeGenerator.playerC = players[2].gameObject;
            mazeGenerator.NumPlayer = 3;
            Physics.IgnoreCollision(players[0].gameObject.GetComponent<Collider>(), players[2].gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(players[1].gameObject.GetComponent<Collider>(), players[2].gameObject.GetComponent<Collider>());

        }
        if (players.Length > 3)
        {
            mazeGenerator.targetLight = targetLight;
            mazeGenerator.target = target;
            mazeGenerator.playerD = players[3].gameObject;
            mazeGenerator.NumPlayer = 4;
            Physics.IgnoreCollision(players[0].gameObject.GetComponent<Collider>(), players[3].gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(players[1].gameObject.GetComponent<Collider>(), players[3].gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(players[2].gameObject.GetComponent<Collider>(), players[3].gameObject.GetComponent<Collider>());

        }
        mazeGenerator.BuildMaze();

	}

    public void Set_Player(int numPlayers) {

    }

    public void Set_Maze(int x, int y) {
        mazeGenerator.xSize = x;
        mazeGenerator.ySize = y;
    }
}
