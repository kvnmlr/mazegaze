using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    // lights that are currently active in this cell
    public List<GameObject> lights = new List<GameObject>();

    // players that are currently in this cell (note that players can be in two cells at once)
    public List<Player> players = new List<Player>();

    // determines whether this cell has a light source (to optimize performance)
    public bool hasLight;
	
	void Update () {
		for(int i = 0; i < lights.Count; ++i)
        {
            // Remove lights that are not shining anymore
            if(lights[i].GetComponent<Light>().intensity <= 0)
            {
                DestroyImmediate(lights[i]);
                lights.RemoveAt(i);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            Player p = other.gameObject.GetComponent<Player>();

            if (p==null)
            {
                return;
            }
            if (!players.Contains(p))
            {
                players.Add(p);
            }

            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].GetComponent<CellLight>().getPlayer().name.Equals(p.name))
                {
                    // If the player already has a light in this cell, reactivate it
                    lights[i].GetComponent<CellLight>().restart();
                    return;
                } else
                {
                    // Mix the new color with the other one currently active in this cell
                    lights[i].GetComponent<CellLight>().mix(p.color);
                    lights[i].GetComponent<CellLight>().restart();
                    return;
                }
            }

            if (hasLight)
            {
                // if there is no light yet shining, create a new one
                GameObject g;
                g = Instantiate(p.cellLight.gameObject, gameObject.transform, true);
                g.name = "CellLightInstance";
                g.transform.localPosition = new Vector3(0, 0.25f, 0);
                g.GetComponent<CellLight>().setPlayer(p);
                lights.Add(g);
                g.SetActive(true);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            Player p = other.gameObject.GetComponent<Player>();

            if (p == null)
            {
                return;
            }
            players.Remove(p);
        }
    }
}
