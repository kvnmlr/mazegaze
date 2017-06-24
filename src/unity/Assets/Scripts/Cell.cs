using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    // players who discovered this cell
    public List<GameObject> lights = new List<GameObject>();
    public bool hasLight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < lights.Count; ++i)
        {
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

            if (p==null || !hasLight)
            {
                return;
            }

            //Debug.Log(p.name + " entered " + gameObject.name);

            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].GetComponent<CellLight>().getPlayer().name.Equals(p.name))
                {
                    lights[i].GetComponent<CellLight>().restart();
                    return;
                } else
                {
                    lights[i].GetComponent<CellLight>().mix(p.color);
                    lights[i].GetComponent<CellLight>().restart();
                    return;
                }
            }

            Debug.Log("Starting light for " + p.name + " in " + gameObject.name);
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
