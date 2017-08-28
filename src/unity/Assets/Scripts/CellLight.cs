using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellLight : MonoBehaviour {
    // the player who this light belongs to
    public HashSet<Player> players = new HashSet<Player>();
    public bool neutral { get; set; }

    private Player lastVisitedPlayer;

    // parameters for the light
    public float initialLightIntesity = 10f;
    public float timeToDarkness = 10f;
    public float decreaseRate = 0.2f;

    private float timePassed;
    public float currentIntensity { get; set; }
    private bool breakIntensity = false;

    public void setPlayer(Player player)
    {
        if (player != null)
        {
            neutral = false;
            gameObject.GetComponent<Light>().color = player.cellLightColor;
            timeToDarkness = player.timeToDarkness;
            lastVisitedPlayer = player;
            this.players.Add(player);
        }          
    }
    public void setNeutral(float timeToDarkness = 1)
    {
            neutral = true;
            gameObject.GetComponent<Light>().color = new Color(1,1,1,1);
            this.timeToDarkness = timeToDarkness;
    }

    public HashSet<Player> getPlayers()
    {
        return players;
    }

    public void restart()
    {
        currentIntensity = initialLightIntesity;
        timePassed = 0;
    }

    public void mix(Player.PlayerColor other)
    {
        if (lastVisitedPlayer  == null)
        {
            return;
        }
        Player.PlayerColor color = lastVisitedPlayer.color;
        int mix = (int)color + (int)other;
        Player.MixColor mixColor = (Player.MixColor)mix;
        gameObject.GetComponent<Light>().color = lastVisitedPlayer.getColor(mixColor);
        breakIntensity = false;
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
            if (currentIntensity < 0.001f)
            {
                currentIntensity = 0;
                lastVisitedPlayer = null;
                players = new HashSet<Player>();
            }
            if (breakIntensity)
            {
                gameObject.GetComponent<Light>().intensity = 0.002f;
                breakIntensity = false;
                yield return new WaitForSeconds(0.2f); // loeschen wenn man moechte dass das licht direkt wieder angeht
            }
            else
            {
                gameObject.GetComponent<Light>().intensity = currentIntensity;
            }
        }                
    }


    public void SaveArea (Player p)
    {  
        if (!players.Contains(p))
        {
            breakIntensity = true;
        }
        else
        {
            breakIntensity = false;
        }
    }
}
