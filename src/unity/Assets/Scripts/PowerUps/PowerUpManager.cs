using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {
    public PowerUp[] powerUps;
    public enum PowerUpTypes
    {
        Enlightenment = 1,
        ShowTarget = 2,
        Endurance = 3,
        Target = 4
    }

    public PowerUp spawnPowerUp (PowerUpTypes type, GameObject cell)
    {
        Debug.Log("spawning powerup " + type.ToString());
        foreach (PowerUp p in powerUps)
        {
            if (p.type.ToString().Equals(type.ToString()))
            {
                GameObject powerUp = Instantiate(p.gameObject, cell.transform, true);
                powerUp.transform.localPosition = new Vector3(0, 0, 0);
                powerUp.SetActive(true);

                return powerUp.GetComponent<PowerUp>();
            }
        }
        return null;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
