using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellLight : MonoBehaviour {
    private Player player;
    private float initialLightIntesity = 5f;
    private float timeToDarkness = 15f;

    private float decreaseRate = 0.2f;
    private float timePassed;
    private float currentIntensity;

    public void setPlayer(Player player)
    {
        gameObject.GetComponent<Light>().color = player.playerLightColor;
        this.player = player;
    }
    public Player getPlayer()
    {
        return player;
    }

    public void Restart()
    {
        currentIntensity = initialLightIntesity;
        timePassed = 0;
    }

    void Start()
    {
        currentIntensity = initialLightIntesity;
        timePassed = 0;

        gameObject.GetComponent<Light>().range = currentIntensity;

        StartCoroutine(lowerLightIntensity());
    }

    IEnumerator lowerLightIntensity()
    {
        float intensityInterval = initialLightIntesity / (timeToDarkness/decreaseRate);

        Debug.Log("Lowering intensity");
        while (timePassed < timeToDarkness && currentIntensity > 0)
        {
            yield return new WaitForSeconds(decreaseRate);
            timePassed += decreaseRate;
            currentIntensity -= intensityInterval;

            gameObject.GetComponent<Light>().range = currentIntensity;
        }
    }
}
