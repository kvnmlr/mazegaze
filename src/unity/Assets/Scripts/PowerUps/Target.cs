using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : PowerUp {

    int numGame;

    public override IEnumerator performPowerUp(Player p)
    {
        p.points++;
        //other.gameObject.SetActive(false);
        GameController.Instance.setRestart(true);
        if (GameController.Instance.getNumGames() == GameController.Instance.getPlayedGames())
        {
            Menu.Instance.GameOver();
        }
        else
        {
            StartCoroutine(Menu.Instance.GetWinText());

            GameController.Instance.startNewRound();

        }
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    void Start()
    {  
        numGame = 0;
        this.type = PowerUpManager.PowerUpTypes.Target;
    }

    void Update()
    {
        
    }

    public Vector3 TransformpositionRandom()
    {
        float xSize = MazeGenerator.Instance.xSize;
        float ySize = MazeGenerator.Instance.ySize;
        float rdx = Random.Range(-(xSize - 1) / 2, (xSize + 1) / 2);
        float rdz = Random.Range(-(ySize - 1) / 2, (ySize + 1) / 2);
        return new Vector3(rdx + 0.5f, 0.5f, rdz);
    }

   
    
}
