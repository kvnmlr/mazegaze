using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Player[] players;
    public MazeGenerator mazeGenerator;

	// Use this for initialization
	void Start () {
        Debug.Log("MazeGaze");
		foreach (Player p in players)
        {
            Debug.Log("Player " + p.name + " joined the game");
        }
        mazeGenerator.BuildMaze();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
