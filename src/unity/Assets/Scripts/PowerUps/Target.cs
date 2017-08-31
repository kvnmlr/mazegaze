using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : PowerUp
{
    public override IEnumerator performPowerUp(Player p)
    {
        PowerUpManager.Instance.activePowerUps++;

        p.points++;
        GameController.Instance.setRestart(true);

        AudioManager.Instance.play(AudioManager.SOUNDS.COLLECT_TARGET);
        GameController.Instance.playedGames++;

        if (GameController.Instance.getNumGames() == GameController.Instance.getPlayedGames())
        {
            Menu.Instance.GameOver(p);
            AudioManager.Instance.stop();
            AudioManager.Instance.play(AudioManager.SOUNDS.MENU);
            GameController.Instance.playedGames = 0;
        }
        else
        {
            StartCoroutine(Menu.Instance.GetWinText(p));
            GameController.Instance.startNewRound();
            yield return new WaitForSeconds(3);
            if (GameController.Instance.getPlayedGames() == 4)
            {
                AudioManager.Instance.stop();
                AudioManager.Instance.play(AudioManager.SOUNDS.BACKTRACK2);
            }
            else if (GameController.Instance.getPlayedGames() == 6)
            {
                AudioManager.Instance.stop();
                AudioManager.Instance.play(AudioManager.SOUNDS.BACKTRACK3);
            }
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().enabled = true;

        }
        yield return new WaitForSeconds(1);
    }

    void Start()
    {  
        this.type = PowerUpManager.PowerUpTypes.Target;
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
