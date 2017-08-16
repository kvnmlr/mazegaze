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
    private bool optionBreakButton = false;
    private bool optionBreakScreen = false;
    private bool optionWinText = false;

    public GameObject mainButtonScreen;
    public GameObject settingsScreen;
    public GameObject playerScreen;
    public GameObject levelScreen;
    public GameObject roundScreen;
    public GameObject gameOverScreen;
    public GameObject breakButton;
    public GameObject breakScreen;
    public Canvas canvas;
    public Text winText;

    void Start () {
    }
	
	void Update () {
        CheckPlayerScreen();
        CheckLevelScreen();
        CheckMainButtonScreen();
        CheckGameOverScreen();
        CheckRoundScreen();
        CheckBreakScreen();
        CheckBreakButton();
        CheckWinText();
	}

    //Check Screens

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


    void CheckRoundScreen() {
        if(optionRound == true) {
            roundScreen.SetActive(true);
        } else {
            roundScreen.SetActive(false);
        }
    }

    void CheckBreakButton() {
        if (optionBreakButton == true) {
            breakButton.SetActive(true);
        } else {
            breakButton.SetActive(false);
        }

    }

    void CheckBreakScreen() {
        if (optionBreakScreen == true) {
            breakScreen.SetActive(true);
        } else {
            breakScreen.SetActive(false);
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

    void CheckWinText() {
        if (optionWinText == true) {
            winText.gameObject.SetActive(true);
        } else {
            winText.gameObject.SetActive(false);
        }
    }

    //Close Options

    public void ClosePlayerScreen() {
        optionPlayer = false;
        optionMainButton = true;
    }

    public void CloseLevelScreen() {
        optionLevel = false;
        optionPlayer = true;
    }

    public void CloseRoundScreen() {
        optionRound = false;
        optionLevel = true;
    }

    public void CloseBreakButton() {
        optionBreakButton = false;
        optionBreakScreen = true;
    }

    public void CloseBreakScreen() {
        optionBreakScreen = false;
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
        MazeGenerator.Instance.xSize = 7;
        MazeGenerator.Instance.ySize = 7;
        optionLevel = false;
        optionRound = true;
    }

    public void MiddleLevel() {
        MazeGenerator.Instance.xSize = 11;
        MazeGenerator.Instance.ySize = 11;
        optionLevel = false;
        optionRound = true;
    }

    public void HardLevel() {
        MazeGenerator.Instance.xSize = 15;
        MazeGenerator.Instance.ySize = 15;
        optionLevel = false;
        optionRound = true;
    }

    //Round Button Control

    public void OneRound() {
        GameController.Instance.setNumGames(1);
        optionRound = false;
        StartGame();
    }

    public void ThreeRound() {
        GameController.Instance.setNumGames(3);
        optionRound = false;
        StartGame();
    }

    public void FiveRound() {
        GameController.Instance.setNumGames(5);
        optionRound = false;
        StartGame();
    }

    public void SevenRound() {
        GameController.Instance.setNumGames(7);
        optionRound = false;
        StartGame();
    }

    //GameOverScreen Button Control

    public void BackMainMenu() {
        optionGameOver = false;
        optionPlayer = false;
        optionLevel = false;
        optionRound = false;
        optionMainButton = true;
        optionWinText = false;
    }
    
    //Break Button Control
    
    public void GoOnButton()
    {
        CloseBreakScreen();
        canvas.enabled = false;
    }

    public void LeaveGame()
    {
        if(MazeGenerator.Instance.numPlayers == 1)
        {
            Debug.Log("Press Quite to leave the game");
        }
        else
        {
            MazeGenerator.Instance.LeaveGame();
            //TODO übergebe spieler, welcher das gedrückt hat
        }
    }

    public void AddPlayer()
    {
        if(MazeGenerator.Instance.numPlayers == 4)
        {
            Debug.Log("Nicht mehr als 4 Spieler");
        }
        else
        {
            MazeGenerator.Instance.AddPlayer();
            canvas.enabled = false;
        }

    }


    public void PressBreak() {
        CloseBreakButton();
        optionBreakScreen = true;
    }

    //General Start/Quit Functions

    public void StartGame() {
        optionWinText = false;
        optionBreakButton = true;
        GameController.Instance.startNewRound();
        canvas.enabled = false;
       
    }

    public void GameOver(Player p) {
        SetCountText(p);
        canvas.enabled = true;
        optionPlayer = false;
        optionLevel = false;
        optionMainButton = false;
        optionRound = false;
        optionBreakButton = false;
        optionBreakScreen = false;
        optionGameOver = true;
        //Reset points
        GameController.Instance.players[0].points = 0;
        GameController.Instance.players[1].points = 0;
        GameController.Instance.players[2].points = 0;
        GameController.Instance.players[3].points = 0;
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
        settingsScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void Help () {

    }


    //WinText

    public IEnumerator GetWinText(Player p) {
        SetCountText(p);
        canvas.enabled = true;
        optionPlayer = false;
        optionLevel = false;
        optionMainButton = false;
        optionRound = false;
        optionGameOver = false;
        yield return new WaitForSeconds(3);
        optionWinText = false;
        canvas.enabled = false;
    }

    void SetCountText(Player p) {
        optionWinText = true;

        string win = "Player " + p.name + " wins round " + GameController.Instance.getPlayedGames();

        string round = "";
        if (GameController.Instance.getNumGames() == GameController.Instance.getPlayedGames()) {
            string gameover = "GAME OVER - Round " + GameController.Instance.getPlayedGames() + "/" + GameController.Instance.getNumGames();
            string winner = "";
            if (isWinner() == true) {
                winner = "Winner of the match: " + Getwinner().name;
            } else {
                winner = "Draw";
            }
            round = gameover + "\n" + winner;
        } else {
            round = "Round " + GameController.Instance.getPlayedGames() + "/" + GameController.Instance.getNumGames(); 
        }

        string text = "";  
        switch (MazeGenerator.Instance.numPlayers)
        {
            case 1:
                text = "Player A: " + GameController.Instance.players[0].points;
                break;
            case 2:
                text = "Player A: "  + GameController.Instance.players[0].points + " Player B: " + GameController.Instance.players[1].points;
                break;
            case 3:
                text = "Player A: " + GameController.Instance.players[0].points +
                    " Player B: " + GameController.Instance.players[1].points +
                    " Player C: " + GameController.Instance.players[2].points;
                break;
            case 4:
                text = "Player A: " + GameController.Instance.players[0].points +
                    " Player B: " + GameController.Instance.players[1].points +
                    " Player C: " + GameController.Instance.players[2].points +
                    " Player D: " + GameController.Instance.players[3].points;
                break;

        }
       
        winText.text =  win + "\n" + round + "\n" + "Scores:    "  + text;
    }



    Player Getwinner() {
        switch (MazeGenerator.Instance.numPlayers) {
            case 1:
                return GameController.Instance.players[0];
            case 2:
                if (GameController.Instance.players[0].points > GameController.Instance.players[1].points) {
                    return GameController.Instance.players[0];
                } else  {
                    return GameController.Instance.players[1];
                }
            case 3:
                if (GameController.Instance.players[0].points > GameController.Instance.players[1].points && GameController.Instance.players[0].points > GameController.Instance.players[2].points)
                {
                    return GameController.Instance.players[0];
                }
                else if (GameController.Instance.players[1].points > GameController.Instance.players[0].points && GameController.Instance.players[1].points > GameController.Instance.players[2].points)
                {
                    return GameController.Instance.players[1];
                } else
                {
                    return GameController.Instance.players[2];
                }
            case 4:
                if (GameController.Instance.players[0].points > GameController.Instance.players[1].points && GameController.Instance.players[0].points > GameController.Instance.players[2].points && GameController.Instance.players[0].points > GameController.Instance.players[3].points)
                {
                    return GameController.Instance.players[0];
                }
                else if (GameController.Instance.players[1].points > GameController.Instance.players[0].points && GameController.Instance.players[1].points > GameController.Instance.players[2].points && GameController.Instance.players[1].points > GameController.Instance.players[3].points)
                {
                    return GameController.Instance.players[1];
                }
                else if (GameController.Instance.players[2].points > GameController.Instance.players[0].points && GameController.Instance.players[2].points > GameController.Instance.players[1].points && GameController.Instance.players[2].points > GameController.Instance.players[3].points)
                {
                    return GameController.Instance.players[2];
                } else
                {
                    return GameController.Instance.players[3];
                }
        }
        return GameController.Instance.players[1];
    }

    bool isWinner()  {
        switch (MazeGenerator.Instance.numPlayers) {
            case 1:
                return true;
            case 2:
                if (GameController.Instance.players[0].points != GameController.Instance.players[1].points){
                    return true;
                } else  {
                    return false;
                }
            case 3:
                if (GameController.Instance.players[0].points != GameController.Instance.players[1].points && GameController.Instance.players[0].points != GameController.Instance.players[2].points && GameController.Instance.players[1].points != GameController.Instance.players[2].points) {
                    return true;
                } else {
                    return false;
                }
            case 4:
                if (GameController.Instance.players[0].points != GameController.Instance.players[1].points && GameController.Instance.players[0].points != GameController.Instance.players[2].points && GameController.Instance.players[0].points != GameController.Instance.players[3].points && GameController.Instance.players[1].points != GameController.Instance.players[2].points && GameController.Instance.players[1].points != GameController.Instance.players[3].points && GameController.Instance.players[2].points != GameController.Instance.players[3].points)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return true;
    }
}
