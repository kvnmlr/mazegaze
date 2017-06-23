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
        GameObject playerLightObj = Instantiate(playerLight.gameObject, gameObject.transform, false);
        playerLightObj.name = "PlayerLight";
        playerLightObj.transform.parent = transform;
        playerLightObj.transform.position = new Vector3(0, 0, 0);
        playerLightObj.GetComponent<Light>().color = playerLightColor;
        playerLightObj.GetComponent<LightController>().player = gameObject;
        playerLightObj.SetActive(true);

        cellLight.setPlayer(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
