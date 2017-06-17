using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    // players who discovered this cell
    private List<GameObject> lights = new List<GameObject>();
    public bool hasLight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < lights.Count; ++i)
        {
            if(lights[i].GetComponent<Light>().range <= 0)
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
            Debug.Log("Player " + p.name + " entered cell " + gameObject.name);



            if (p==null || !hasLight)
            {
                return;
            }

            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].GetComponent<CellLight>().getPlayer().name == p.name) { }
                {
                    lights[i].GetComponent<CellLight>().Restart();
                    return;
                }
            }
            
            GameObject g;
            g = Instantiate(p.cellLight.gameObject, gameObject.transform, true);
            g.name = "CellLightInstance";
            g.transform.position = transform.position - new Vector3(0, 0, 0);
            g.GetComponent<CellLight>().setPlayer(p);
            lights.Add(g);
            g.SetActive(true);

        }
    }
}
