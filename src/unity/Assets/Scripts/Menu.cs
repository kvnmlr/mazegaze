using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : Singleton<Menu> {

    private bool optionPlayer = false;
    private bool optionLevel = false;
    private bool optionMainButton = true;
    private bool optionGameOver = false;

    public GameObject mainButtonScreen;
    public GameObject playerScreen;
    public GameObject levelScreen;
    public GameObject gameOverScreen;
    public Canvas canvas;

    void Start () {
    }
	
	void Update () {
        CheckPlayerScreen();
        CheckLevelScreen();
        CheckMainButtonScreen();
        CheckGameOverScreen();
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

    void CheckMainButtonScreen() {
        if (optionMainButton == true) {
            mainButtonScreen.SetActive(true);
        } else {
            mainButtonScreen.SetActive(false);
        }
    }

    void CheckGameOverScreen() {
        if (optionGameOver == true) {
            gameOverScreen.SetActive(true);
        } else {
            gameOverScreen.SetActive(false);
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

    //GameOverScreen Button Control

    public void BackMainMenu() {
        optionGameOver = false;
        optionPlayer = false;
        optionLevel = false;
        optionMainButton = true;
       
    }


    //General Start/Quit Functions

    public void StartGame() {
        GameController.Instance.startNewRound();
        canvas.enabled = false;
    }

    public void GameOver() {
        canvas.enabled = true;
        optionPlayer = false;
        optionLevel = false;
        optionMainButton = false;
        optionGameOver = true;
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
