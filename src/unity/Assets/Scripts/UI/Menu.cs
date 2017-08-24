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
    private bool optionRanking = false;
    private bool optionRoundText = false;

    private bool breakgameover;

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
    public Canvas canvasranking;
    public Text rankingText;
    public Canvas canvasround;
    public Text roundText;

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
        CheckRoundText();
        CheckRanking();
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
        if (optionGameOver == true && optionMainButton == false) {
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

    void CheckRoundText()
    {
        if (optionRoundText == true)
        {
            roundText.gameObject.SetActive(true);
        }
        else
        {
            roundText.gameObject.SetActive(false);
        }
        roundText.text = "Round " + GameController.Instance.getPlayedGames() + "/" + GameController.Instance.getNumGames();
    }

    void CheckRanking()
    {
        if (optionRanking == true)
        {
            rankingText.gameObject.SetActive(true);
        }
        else
        {
            rankingText.gameObject.SetActive(false);
        }
        computeRanking();
    }

    //Close Options
    public void CloseSettingsScreen()
    {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        settingsScreen.SetActive(false);
        optionPlayer = false;
        optionMainButton = true;
    }

    public void ClosePlayerScreen() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        optionPlayer = false;
        optionMainButton = true;
    }

    public void CloseLevelScreen() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        optionLevel = false;
        optionPlayer = true;
    }

    public void CloseRoundScreen() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        optionRound = false;
        optionLevel = true;
    }

    public void CloseBreakButton() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        optionBreakButton = false;
        optionBreakScreen = true;
    }

    public void CloseBreakScreen() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        optionBreakScreen = false;
    }

    //Player Button Control

    public void OnePlayer() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        MazeGenerator.Instance.numPlayers = 1;
        optionPlayer = false;
        optionLevel = true;
    }

    public void TwoPlayer() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        MazeGenerator.Instance.numPlayers = 2;
        optionPlayer = false;
        optionLevel = true;
    }

    public void ThreePlayer() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        MazeGenerator.Instance.numPlayers = 3;
        optionPlayer = false;
        optionLevel = true;
    }

    public void FourPlayer() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        MazeGenerator.Instance.numPlayers = 4;
        optionPlayer = false;
        optionLevel = true;
    }

    //Level Button Control

    public void SimpleLevel() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        MazeGenerator.Instance.xSize = 7;
        MazeGenerator.Instance.ySize = 7;
        optionLevel = false;
        optionRound = true;
    }

    public void MiddleLevel() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        MazeGenerator.Instance.xSize = 11;
        MazeGenerator.Instance.ySize = 11;
        optionLevel = false;
        optionRound = true;
    }

    public void HardLevel() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        MazeGenerator.Instance.xSize = 15;
        MazeGenerator.Instance.ySize = 15;
        optionLevel = false;
        optionRound = true;
    }

    //Round Button Control

    public void OneRound() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        GameController.Instance.setNumGames(1);
        optionRound = false;
        StartGame();
    }

    public void ThreeRound() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        GameController.Instance.setNumGames(3);
        optionRound = false;
        StartGame();
    }

    public void FiveRound() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        GameController.Instance.setNumGames(5);
        optionRound = false;
        StartGame();
    }

    public void SevenRound() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        GameController.Instance.setNumGames(7);
        optionRound = false;
        StartGame();
    }

    //GameOverScreen Button Control

    public void BackMainMenu() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        optionGameOver = false;
        optionPlayer = false;
        optionLevel = false;
        optionRound = false;
        optionMainButton = true;
        optionWinText = false;
        optionBreakButton = false;
        optionBreakScreen = false;
        optionRanking = false;
        optionRoundText = false;

        while (MazeGenerator.Instance.numPlayers > 0)
        {
            MazeGenerator.Instance.LeaveGame();
        }

        //Wenn vom BreakScreen Button getätigt wurde
        if (breakgameover == true)
        {
            //Punkte zurücksetzen
            GameController.Instance.players[0].points = 0;
            GameController.Instance.players[1].points = 0;
            GameController.Instance.players[2].points = 0;
            GameController.Instance.players[3].points = 0;
            //andere Musik
            AudioManager.Instance.stop();
            AudioManager.Instance.play(AudioManager.SOUNDS.MENU);
            //
            GameController.Instance.setRestart(true);
        }

        breakgameover = false;
    }

    public void RestartGame() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        optionGameOver = false;
        int numPlayers = MazeGenerator.Instance.numPlayers;
        while (MazeGenerator.Instance.numPlayers > 0)
        {
            MazeGenerator.Instance.LeaveGame();
        }
        MazeGenerator.Instance.numPlayers = numPlayers;
        GameController.Instance.setNumGames(GameController.Instance.getNumGames());
        StartGame();
    }
    
    //Break Button Control
    
    public void GoOnButton()
    {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        CloseBreakScreen();
        canvas.enabled = false;
        optionBreakButton = true;
        breakgameover = false;
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
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        CloseBreakButton();
        optionBreakScreen = true;
        breakgameover = true;
    }

    //General Start/Quit Functions

    public void StartGame() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        optionWinText = false;
        optionBreakButton = true;
        GameController.Instance.startNewRound();
        canvas.enabled = false;
        optionRoundText = true;
        optionRanking = true;
        AudioManager.Instance.stop();
        AudioManager.Instance.play(AudioManager.SOUNDS.BACKTRACK1);
       
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
        optionRoundText = false;
        optionRanking = false;
        optionWinText = true;
        optionGameOver = true;
        //Reset points
        GameController.Instance.players[0].points = 0;
        GameController.Instance.players[1].points = 0;
        GameController.Instance.players[2].points = 0;
        GameController.Instance.players[3].points = 0;
    }

    //Main Buttons

    public void QuitGame() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        Application.Quit();
    }

    public void Play()
    {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        optionMainButton = false;
        optionPlayer = true;
        optionLevel = false;
        optionGameOver = false;
    }

    public void Settings() {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
        settingsScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void Help () {
        AudioManager.Instance.play(AudioManager.SOUNDS.BUTTON_CLICK);
    }

    //Ranking

     public void computeRanking()
    {
        int numPlayers = MazeGenerator.Instance.numPlayers;
        Player[] a = new Player[numPlayers];

        for (int i = 0; i < numPlayers; i++)
        {
            a[i] = GameController.Instance.players[i];
        }

       

        string text = "RANKING:";

        for (int i = 0; i < numPlayers; i++)
        {
            text = text + "\n" + a[i].name + "  " + a[i].points;
        }

        rankingText.text = text;


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

    void SetCountText(Player p)
    {
        optionWinText = true;

        string win = p.name + " collected target!";

        string round = "";
        string text = "";

        if (GameController.Instance.getNumGames() == GameController.Instance.getPlayedGames())
        {
            string winner = "";
            if (isWinner() == true)
            {
                winner = "Winner of the match: " + Getwinner().name;
            }
            else
            {
                winner = "Draw";
            }
            round = "GAME OVER  -   " + winner;

            switch (MazeGenerator.Instance.numPlayers)
            {
                case 1:
                    text = "Scores:    " + GameController.Instance.players[0].name + ": " + GameController.Instance.players[0].points;
                    break;
                case 2:
                    text = "Scores:    " + GameController.Instance.players[0].name + ": " + GameController.Instance.players[0].points + "  " + GameController.Instance.players[1].name + ": " + GameController.Instance.players[1].points;
                    break;
                case 3:
                    text = "Scores:    " + GameController.Instance.players[0].name + ": " + GameController.Instance.players[0].points +
                        "  " + GameController.Instance.players[1].name + ": " + GameController.Instance.players[1].points +
                        "  " + GameController.Instance.players[2].name + ": " + GameController.Instance.players[2].points;
                    break;
                case 4:
                    text = "Scores:    " + GameController.Instance.players[0].name + ": " + GameController.Instance.players[0].points +
                        "  " + GameController.Instance.players[1].name + ": " + GameController.Instance.players[1].points +
                        "  " + GameController.Instance.players[2].name + ": " + GameController.Instance.players[2].points +
                        "  " + GameController.Instance.players[3].name + ": " + GameController.Instance.players[3].points;
                    break;
            }
        }

        winText.text = win + "\n" + round + "\n" + text;
        computeRanking();
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
