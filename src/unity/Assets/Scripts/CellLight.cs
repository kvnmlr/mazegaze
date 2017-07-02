using System.Collections;
using UnityEngine;

public class CellLight : MonoBehaviour {
    // the player who this light belongs to
    public Player player { get; set; }
    public bool neutral { get; set; }

    // parameters for the light
    public float initialLightIntesity = 10f;
    public float timeToDarkness = 10f;
    public float decreaseRate = 0.2f;

    private float timePassed;
    public float currentIntensity { get; set; }

    public void setPlayer(Player player)
    {
        if (player != null)
        {
            neutral = false;
            gameObject.GetComponent<Light>().color = player.cellLightColor;
            timeToDarkness = player.timeToDarkness;
            this.player = player;
        }          
    }
    public void setNeutral(float timeToDarkness = 1)
    {
            neutral = true;
            gameObject.GetComponent<Light>().color = new Color(1,1,1,1);
            this.timeToDarkness = timeToDarkness;
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
            if (currentIntensity < 0.01f)
            {
                currentIntensity = 0;
            }

            gameObject.GetComponent<Light>().intensity = currentIntensity;
        }
    }
}
