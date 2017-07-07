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
    private bool optionRound = false;

    public GameObject mainButtonScreen;
    public GameObject playerScreen;
    public GameObject levelScreen;
    public GameObject roundScreen;
    public GameObject gameOverScreen;
    public Canvas canvas;
    public Text winText;

    void Start () {
    }
	
	void Update () {
        CheckPlayerScreen();
        CheckLevelScreen();
        CheckMainButtonScreen();
        //CheckGameOverScreen();
        CheckRoundScreen();
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

    void CheckRoundScreen()
    {
        if(optionRound == true)
        {
            roundScreen.SetActive(true);
        }
        else
        {
            roundScreen.SetActive(false);
        }
    }
   
    void CheckMainButtonScreen() {

       if (optionMainButton == true)
        {
            mainButtonScreen.SetActive(true);
        }
        else
        {
            mainButtonScreen.SetActive(false);
        }
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
        optionMainButton = true;
    }

    public void CloseLevelScreen() {
        optionLevel = false;
        optionPlayer = true;
    }

    public void CloseRoundScreen()
    {
        optionRound = false;
        optionLevel = true;
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
        optionLevel = false;
        optionRound = true;
        //StartGame();
    }

    public void MiddleLevel() {

        MazeGenerator.Instance.xSize = 15;
        MazeGenerator.Instance.ySize = 15;
        optionLevel = false;
        optionRound = true;
        //StartGame();
    }

    public void HardLevel() {

        MazeGenerator.Instance.xSize = 21;
        MazeGenerator.Instance.ySize = 21;
        optionLevel = false;
        optionRound = true;
        //StartGame();
    }

    //Round Button Control

    public void OneRound()
    {
        GameController.Instance.setNumGames(1);
        StartGame();
    }

    public void ThreeRound()
    {
        GameController.Instance.setNumGames(3);
        StartGame();
    }

    public void FiveRound()
    {
        GameController.Instance.setNumGames(5);
        StartGame();
    }

    public void SevenRound()
    {
        GameController.Instance.setNumGames(7);
        StartGame();
    }

    //GameOverScreen Button Control

    public void BackMainMenu() {
        optionGameOver = false;
        optionPlayer = false;
        optionLevel = false;
        optionRound = false;
        optionMainButton = true;
       
    }


    //General Start/Quit Functions

    public void StartGame() {

        winText.gameObject.SetActive(false);
        GameController.Instance.startNewRound();
        canvas.enabled = false;
       
    }

    public void GameOver() {
        SetCountText();
        canvas.enabled = true;
        optionPlayer = false;
        optionLevel = false;
        optionMainButton = true;
        optionRound = false;
        optionGameOver = true;//TODO implement this
        
    }

    //Main Buttons

    public void QuitGame() {
        Application.Quit();
    }

    public void Play() {
        optionMainButton = false;
        optionPlayer = true;
        optionLevel = false;
    }

    public void Settings() {

    }

    public void Help () {

    }

    //WinText

    public IEnumerator GetWinText()
    {
  
        SetCountText();
        canvas.enabled = true;
        optionPlayer = false;
        optionLevel = false;
        optionMainButton = false;
        optionRound = false;
        optionGameOver = false;
        yield return new WaitForSeconds(2);
        winText.gameObject.SetActive(false);
        canvas.enabled = false;
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
