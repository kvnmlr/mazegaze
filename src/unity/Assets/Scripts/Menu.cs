using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour {

    private bool optionPlayer = false;
    private bool optionLevel = false;

    public GameObject playerScreen;
    public GameObject levelScreen;
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
        if (optionPlayer == true) {
            playerScreen.SetActive(true);
        } else {
            playerScreen.SetActive(false);
        }
    }

    void CheckLevelScreen() {
        if (optionLevel == true) {
            levelScreen.SetActive(true);
        } else {
            levelScreen.SetActive(false);
        }
    }

    public void ClosePlayerScreen() {
        optionPlayer = false;
    }

    public void CloseLevelScreen() {
        optionLevel = false;
        optionPlayer = true;
    }


    //Player Button Control

    public void OnePlayer() {

        gameController.mazeGenerator.numPlayers = 1;

        optionPlayer = false;
        optionLevel = true;     
    }

    public void TwoPlayer() {

        gameController.mazeGenerator.numPlayers = 2;

        optionPlayer = false;
        optionLevel = true;
    }

    public void ThreePlayer() {

        gameController.mazeGenerator.numPlayers = 3;

        optionPlayer = false;
        optionLevel = true;
    }

    public void FourPlayer() {

        gameController.mazeGenerator.numPlayers = 4;

        optionPlayer = false;
        optionLevel = true;
    }


    //Level Button Control

    public void SimpleLevel() {

        gameController.mazeGenerator.xSize = 9;
        gameController.mazeGenerator.ySize = 9;

        StartGame();
    }

    public void MiddleLevel() {

        gameController.mazeGenerator.xSize = 15;
        gameController.mazeGenerator.ySize = 15;

        StartGame();
    }

    public void HardLevel() {

        gameController.mazeGenerator.xSize = 21;
        gameController.mazeGenerator.ySize = 21;

        StartGame();
    }


    //General Start/Quit Functions

    public void StartGame() {
        gameController.startNewRound();
        canvas.enabled = false;
    }

    //Main Buttons

    public void QuitGame() {
        Application.Quit();
    }

    public void Play() {
        optionPlayer = true;
        optionLevel = false;
    }

    public void Settings() {

    }

    public void Help () {

    }
}
