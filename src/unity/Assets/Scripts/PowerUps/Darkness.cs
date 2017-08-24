using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : PowerUp
{
    private float duration = 0;
    public ParticleSystem badEffect;

    public override IEnumerator performPowerUp(Player p)
    {
        badEffect.transform.position = gameObject.transform.position;
        badEffect.Play(true);
        AudioManager.Instance.play(AudioManager.SOUNDS.COLLECT_NEGATIVE_POWERUP);
        GameObject[][] board = MazeGenerator.Instance.toMatrix();
        for (int row = 0; row < MazeGenerator.Instance.ySize; ++row)
        {
            for (int column = 0; column < MazeGenerator.Instance.xSize; ++column)
            {
                Cell cell = board[row][column].GetComponent<Cell>();
                if (cell == null)
                {
                    continue;
                }
                foreach(GameObject light in cell.lights)
                {
                    CellLight cellLight = light.GetComponent<CellLight>();
                    if (!cellLight.players.Contains(p))
                    {
                        cellLight.currentIntensity = 0;
                    } else
                    {
                        cellLight.players = new HashSet<Player>();
                        cellLight.players.Add(p);
                        cellLight.gameObject.GetComponent<Light>().color = p.getColor(p.color);
                    }
                }
            }
        }

        yield return new WaitForSeconds(duration);

    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.Darkness;
    }
}
