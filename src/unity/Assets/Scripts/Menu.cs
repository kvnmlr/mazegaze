using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public Text winText;
    private int aenderung;

    void Start () {
        aenderung = 0;
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
    //TODO: @Lena: Wenn unkommentiert einen Fehler, weil in Unity kein Object uebergeben
    void CheckMainButtonScreen() {
        /*if (optionMainButton == true) {
            mainButtonScreen.SetActive(true);
        } else {
            mainButtonScreen.SetActive(false);
        {*/
    }
    //TODO: @Lena: Wenn unkommentiert einen Fehler, weil in Unity kein Object uebergeben
    void CheckGameOverScreen() {
       /* if (optionGameOver == true) {
            gameOverScreen.SetActive(true);
        } else {
            gameOverScreen.SetActive(false);
        }*/
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
        if (!GameController.Instance.getRestart())     
        {
            MazeGenerator.Instance.numPlayers = 1;

            
        }
        else
        {
            if(MazeGenerator.Instance.numPlayers != 1)
            {
                //Alle Spieler bis auf einen löschen
                
            }
        }
        optionPlayer = false;
        optionLevel = true;

    }

    public void TwoPlayer() {
        if (!GameController.Instance.getRestart())
        {

            MazeGenerator.Instance.numPlayers = 2;
        }
        else
        {
            if(MazeGenerator.Instance.numPlayers < 2)
            {
                //mache einen dazu
            }else if(MazeGenerator.Instance.numPlayers > 2)
            {
                //loesch einen oder zwei
            }
        }
        optionPlayer = false;
        optionLevel = true;
    }

    public void ThreePlayer() {

        if (!GameController.Instance.getRestart())
        {

            MazeGenerator.Instance.numPlayers = 3;
        }
        else
        {
            if (MazeGenerator.Instance.numPlayers < 3)
            {
                //mache einen oder zwei dazu
            }
            else if (MazeGenerator.Instance.numPlayers > 3)
            {
                //loesch einen
            }
        }

        optionPlayer = false;
        optionLevel = true;
    }

    public void FourPlayer() {


        if (!GameController.Instance.getRestart())
        {

            MazeGenerator.Instance.numPlayers = 4;
        }
        else
        {
            if (MazeGenerator.Instance.numPlayers < 4)
            {
                //mache einen, zwei oder drei dazu
            }
            
        }

        optionPlayer = false;
        optionLevel = true;
    }


    //Level Button Control

    public void SimpleLevel() {

        if (!GameController.Instance.getRestart())
        {
            MazeGenerator.Instance.xSize = 9;
            MazeGenerator.Instance.ySize = 9;

        }
        else { 
        
            if(MazeGenerator.Instance.xSize != 9)
            {
                //mache groesser
            }
            
        }
        

        StartGame();
    }

    public void MiddleLevel() {

        if (!GameController.Instance.getRestart())
        {
            MazeGenerator.Instance.xSize = 15;
            MazeGenerator.Instance.ySize = 15;

        }
        else
        {

            if (MazeGenerator.Instance.xSize != 15)
            {
                //mache auf 15
            }

        }

        StartGame();
    }

    public void HardLevel() {

        if (!GameController.Instance.getRestart())
        {
            MazeGenerator.Instance.xSize = 21;
            MazeGenerator.Instance.ySize = 21;

        }
        else
        {

            if (MazeGenerator.Instance.xSize != 21)
            {
                //mache auf 21
            }

        }
       

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
        winText.gameObject.SetActive(false);
        if(aenderung  == 0)
        {
            
        }
        else
        {

        }

        switch (aenderung)
        {
            case 0:
                GameController.Instance.startNewRound();
                canvas.enabled = false;
                break;
            case 1:
                //TODO: Neues SPielfeld jeder wird auf startpunkt gespawnd
                break;
            case 2:
                //TODO: +1 Spieler naechster Spieler
                break;
            case 3:
                //TODO: -1 Spieler niedrigester Spiler
                break;
            case 4:
                //TODO: +2 Spieler
                break;
            case 5:
                //TODO: -2 Spieler
                break;
            case 6:
                //TODO: +3 Spieler
                break;
            case 7:
                //TODO: -3 Spieler
                break;

        }
       
    }

    public void GameOver() {
        SetCountText();
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

    void SetCountText()
    {
        winText.gameObject.SetActive(true);
        string text = "";
       
        switch (MazeGenerator.Instance.numPlayers)
        {
            case 1:
                text = "PlayerA: " + GameController.Instance.players[0].points;
                break;
            case 2:
                Debug.Log("Gespielr");
                text = "PlayerA: "  + GameController.Instance.players[0].points + " PlayerB: " + GameController.Instance.players[1].points;
                break;
            case 3:
                text = "PlayerA: " + GameController.Instance.players[0].points +
                    " PlayerB: " + GameController.Instance.players[1].points +
                    " PlayerC: " + GameController.Instance.players[2].points;
                break;
            case 4:
                text = "PlayerA: " + GameController.Instance.players[0].points +
                    " PlayerB: " + GameController.Instance.players[1].points +
                    " PlayerC: " + GameController.Instance.players[2].points +
                    " PlayerD: " + GameController.Instance.players[3].points;
                break;

        }
       
        winText.text = text;
    }
}
