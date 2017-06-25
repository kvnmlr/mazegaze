using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public string name;
    
    public enum PlayerColor
    {
        red = 1,
        blue = 2,
        yellow = 4,
        pink = 7,
    };

    public enum MixColor
    {
        violet = 3,
        orange = 5,
        green = 6,
        magenta = 8,
        purple = 9,
        cyan = 11
    }

    public PlayerColor color;
    public Color cellLightColor { get; set; }

    public Light playerLight;
    public CellLight cellLight;


    public Color getColor(PlayerColor c)
    {
        switch (c)
        {
            case PlayerColor.red:
                return new Color(1, 0, 0, 1);
                break;
            case PlayerColor.blue:
                return new Color(0, 0, 1, 1);
                break;
            case PlayerColor.yellow:
                return new Color(1, 1, 0, 1);
                break;
            case PlayerColor.pink:
                return new Color(1, 0, 1, 1);
                break;
            default:
                return new Color(1, 1, 1, 1);
        }
    }
    public Color getColor(MixColor c)
    {
        switch (c)
        {
            case MixColor.violet:
                return new Color(1, 0, 0.7f, 1);
                break;
            case MixColor.orange:
                return new Color(1, 0.5f, 0, 1);
                break;
            case MixColor.green:
                return new Color(0, 1, 0, 1);
                break;
            case MixColor.magenta:
                return new Color(1, 0, 0.7f, 1);
                break;
            case MixColor.purple:
                return new Color(0.7f, 0, 1, 1);
                break;
            case MixColor.cyan:
                return new Color(0, 1, 0.5f, 1);
                break;
            default:
                return new Color(1, 1, 1, 1);
        }
    }

    // Use this for initialization
    void Start () {
        playerLight.color = getColor(color);
        cellLightColor = playerLight.color;

        GameObject playerLightObj = Instantiate(playerLight.gameObject, gameObject.transform, false);

        playerLightObj.name = "PlayerLight";
        playerLightObj.transform.parent = transform;
        playerLightObj.transform.position = new Vector3(0, 0, 0);
        playerLightObj.GetComponent<Light>().color = playerLight.color;
        playerLightObj.GetComponent<LightController>().player = gameObject;
        playerLightObj.SetActive(true);

        cellLight.setPlayer(this);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            other.gameObject.SetActive(false);
            Debug.Log("Player " + name +" Gewinnt");
        }
    }
}
