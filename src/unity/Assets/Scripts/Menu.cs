using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour {

    private bool option_player = false;
    private bool option_level = false;

    public GameObject player_screen;
    public GameObject level_screen;
    public GameController gameController;
    public Canvas canvas;


    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        CheckPlayerScreen();
        CheckLevelScreen();
	}

    void CheckPlayerScreen() {
        if (option_player == true) {
            player_screen.SetActive(true);
        } else {
            player_screen.SetActive(false);
        }
    }

    void CheckLevelScreen() {
        if (option_level == true) {
            level_screen.SetActive(true);
        } else {
            level_screen.SetActive(false);
        }
    }

    public void ClosePlayerScreen() {
        option_player = false;
    }

    public void CloseLevelScreen() {
        option_level = false;
        option_player = true;
    }


    //Player Button Control

    public void OnePlayer() {

        gameController.mazeGenerator.NumPlayer = 1;

        option_player = false;
        option_level = true;     
    }

    public void TwoPlayer() {

        gameController.mazeGenerator.NumPlayer = 2;

        option_player = false;
        option_level = true;
    }

    public void ThreePlayer() {

        gameController.mazeGenerator.NumPlayer = 3;

        option_player = false;
        option_level = true;
    }

    public void FourPlayer() {

        gameController.mazeGenerator.NumPlayer = 4;

        option_player = false;
        option_level = true;
    }


    //Level Button Control

    public void SimpleLevel() {

        gameController.mazeGenerator.x_Size = 9;
        gameController.mazeGenerator.y_Size = 9;

        StartGame();
    }

    public void MiddleLevel() {

        gameController.mazeGenerator.x_Size = 15;
        gameController.mazeGenerator.y_Size = 15;

        StartGame();
    }

    public void HardLevel() {

        gameController.mazeGenerator.x_Size = 21;
        gameController.mazeGenerator.y_Size = 21;

        StartGame();
    }


    //General Start/Quit Functions

    public void StartGame() {
        //gameController.mazeGenerator.BuildMaze();
        canvas.enabled = false;
    }

    //Main Buttons

    public void QuitGame() {
        Application.Quit();
    }

    public void Play() {
        option_player = true;
        option_level = false;
    }

    public void Settings() {

    }

    public void Help () {

    }
}
