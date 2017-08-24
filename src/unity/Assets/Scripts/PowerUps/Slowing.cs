using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowing : PowerUp {

    private float duration = 0;
    public override IEnumerator performPowerUp(Player p)
    {
        AudioManager.Instance.play(AudioManager.SOUNDS.COLLECT_NEGATIVE_POWERUP);
        foreach (Player player in GameController.Instance.players)
        {
            if (!player.Equals(p))
            {
                player.speed *= 0.9f;
            }
        }
        yield return new WaitForSeconds(duration);
    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.Slowing;
    }
}
