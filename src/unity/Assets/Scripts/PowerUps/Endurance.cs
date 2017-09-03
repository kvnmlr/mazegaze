using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endurance : PowerUp
{
    public float factor = 2;
    public float duration = 5;
    public ParticleSystem goodEffect;

    public override IEnumerator performPowerUp(Player p)
    {
        AudioManager.Instance.play(AudioManager.SOUNDS.COLLECT_POSITIVE_POWERUP);

        goodEffect.transform.position = gameObject.transform.position;
        goodEffect.Play(true);
        p.timeToDarkness = p.timeToDarkness * factor;
        foreach (GameObject c in MazeGenerator.Instance.cells)
        {
            foreach (GameObject cl in c.GetComponent<Cell>().lights) {
                if (cl.GetComponent<CellLight>().players.Contains(p))
                {
                    cl.GetComponent<CellLight>().timeToDarkness = p.timeToDarkness;
                    cl.GetComponent<CellLight>().decreaseRate = cl.GetComponent<CellLight>().decreaseRate * factor;
                }
            }
        }

        yield return new WaitForSeconds(duration);

        p.timeToDarkness = p.timeToDarkness / factor;
        foreach (GameObject c in MazeGenerator.Instance.cells)
        {
            foreach (GameObject cl in c.GetComponent<Cell>().lights)
            {
                if (cl.GetComponent<CellLight>().players.Contains(p))
                {
                    cl.GetComponent<CellLight>().timeToDarkness = p.timeToDarkness;
                    cl.GetComponent<CellLight>().decreaseRate = cl.GetComponent<CellLight>().decreaseRate / factor;
                    float i = cl.GetComponent<CellLight>().currentIntensity;
                    cl.GetComponent<CellLight>().restart();
                    cl.GetComponent<CellLight>().currentIntensity = i;
                }
            }
        }
        gameObject.SetActive(false);

    }

    void Start () {
        this.type = PowerUpManager.PowerUpTypes.Endurance;
	}
}
