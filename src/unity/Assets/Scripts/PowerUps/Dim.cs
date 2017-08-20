using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dim : PowerUp {

    private float duration = 0;
    public override IEnumerator performPowerUp(Player p)
    {
        Debug.Log("perform on " + p.name);
        foreach (Player player in GameController.Instance.players)
        {
            if (!player.Equals(p))
            {
                player.timeToDarkness /= 5.0f;
            }
        }
       
        yield return new WaitForSeconds(duration);

    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.Dim;
    }
}
