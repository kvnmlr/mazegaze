using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : Singleton<PowerUpManager> {
    public PowerUp[] powerUps;
    public PowerUp target;
    public enum PowerUpTypes
    {
        Enlightenment = 1,
        ShowTarget = 2,
        Endurance = 3,
        Target = 4
    }



    public PowerUp spawnPowerUps(PowerUpTypes type)
    {
        int cell = Random.Range(1,(int)(MazeGenerator.Instance.xSize* MazeGenerator.Instance.ySize)-1);
       
        return spawnPowerUp(type, MazeGenerator.Instance.cells[cell].GetComponent<Cell>());
    }

    public PowerUp spawnPowerUp (PowerUpTypes type, Cell cell)
    {
        if (type.Equals(PowerUpTypes.Target))
        {
            target.transform.parent = cell.gameObject.transform;
            target.transform.localPosition = new Vector3(0, 0, 0);
            target.gameObject.SetActive(true);
            cell.powerUps.Add(target);
            target.cell = cell;

            // let target light up
            GameObject g = new GameObject();
            g.AddComponent<ShowTarget>();
            g.GetComponent<ShowTarget>().type = PowerUpTypes.ShowTarget;
            g.GetComponent<ShowTarget>().activate(GameController.Instance.players[0]);
            Destroy(g.GetComponent<ShowTarget>());
            return target.GetComponent<Target>();
        }

        foreach (PowerUp p in powerUps)
        {
            if (p.type.Equals(type))
            {
                GameObject powerUp = Instantiate(p.gameObject, cell.transform, true);
                powerUp.transform.localPosition = new Vector3(0, 0, 0);
                powerUp.SetActive(true);
                cell.powerUps.Add(powerUp.GetComponent<PowerUp>());
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
