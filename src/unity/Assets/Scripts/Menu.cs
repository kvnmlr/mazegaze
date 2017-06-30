using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : Singleton<Menu> {

    private bool optionPlayer = false;
    private bool optionLevel = false;

    public GameObject playerScreen;
    public GameObject levelScreen;
    public Canvas canvas;

    void Start () {
    }
	
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

        MazeGenerator.Instance.numPlayers = 1;

        optionPlayer = false;
        optionLevel = true;     
    }

    public void TwoPlayer() {

        MazeGenerator.Instance.numPlayers = 2;

        optionPlayer = false;
        optionLevel = true;
    }

    public void ThreePlayer() {

        MazeGenerator.Instance.numPlayers = 3;

        optionPlayer = false;
        optionLevel = true;
    }

    public void FourPlayer() {

        MazeGenerator.Instance.numPlayers = 4;

        optionPlayer = false;
        optionLevel = true;
    }


    //Level Button Control

    public void SimpleLevel() {

        MazeGenerator.Instance.xSize = 9;
        MazeGenerator.Instance.ySize = 9;

        StartGame();
    }

    public void MiddleLevel() {

        MazeGenerator.Instance.xSize = 15;
        MazeGenerator.Instance.ySize = 15;

        StartGame();
    }

    public void HardLevel() {

        MazeGenerator.Instance.xSize = 21;
        MazeGenerator.Instance.ySize = 21;

        StartGame();
    }


    //General Start/Quit Functions

    public void StartGame() {
        GameController.Instance.startNewRound();
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
