using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public string name;
    public Color playerLightColor;
    public Color cellLightColor;

    public Light playerLight;
    public CellLight cellLight;


	// Use this for initialization
	void Start () {
        playerLight.gameObject.SetActive(true);
        playerLight.transform.position = transform.position;
        playerLight.color = playerLightColor;
        cellLight.setPlayer(this);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
