using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {
    public PowerUp[] powerUps;
    public enum PowerUpTypes
    {
        Enlightenment = 1,
        Target = 2,
        Endurance = 3
    }

    public void spawnPowerUp (PowerUpTypes type, Cell cell)
    {
        Debug.Log("spawning powerup " + type.ToString());
        foreach (PowerUp p in powerUps)
        {
            if (p.type.Equals(type))
            {
                GameObject powerUp = Instantiate(p.gameObject, cell.gameObject.transform, true);
                powerUp.transform.localPosition = new Vector3(0, 0, 0);
                powerUp.SetActive(true);
            }
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
