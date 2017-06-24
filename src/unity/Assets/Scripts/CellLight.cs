using System.Collections;
using UnityEngine;

public class CellLight : MonoBehaviour {
    private Player player;
    public float initialLightIntesity = 10f;
    public float timeToDarkness = 20f;

    public float decreaseRate = 0.2f;
    private float timePassed;
    private float currentIntensity;

    public void setPlayer(Player player)
    {
        gameObject.GetComponent<Light>().color = player.cellLightColor;
        this.player = player;
    }
    public Player getPlayer()
    {
        return player;
    }

    public void restart()
    {
        currentIntensity = initialLightIntesity;
        timePassed = 0;
    }

    public void mix(Player.PlayerColor other)
    {
        Player.PlayerColor color = player.color;
        int mix = (int)color + (int)other;
        Player.MixColor mixColor = (Player.MixColor)mix;
        gameObject.GetComponent<Light>().color = player.getColor(mixColor);
    }

    void Start()
    {
        currentIntensity = initialLightIntesity;
        timePassed = 0;

        gameObject.GetComponent<Light>().intensity = currentIntensity;
        StartCoroutine(lowerLightIntensity());
    }

    IEnumerator lowerLightIntensity()
    {
        float intensityInterval = initialLightIntesity / (timeToDarkness/decreaseRate);
        while (timePassed < timeToDarkness && currentIntensity > 0)
        {
            yield return new WaitForSeconds(decreaseRate);
            timePassed += decreaseRate;
            currentIntensity -= intensityInterval;

            gameObject.GetComponent<Light>().intensity = currentIntensity;
        }
    }
}
