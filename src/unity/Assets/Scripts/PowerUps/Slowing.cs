using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowing : PowerUp {

    private float duration = 0;
    public ParticleSystem badEffect;

    public override IEnumerator performPowerUp(Player p)
    {
        badEffect.transform.position = gameObject.transform.position;
        badEffect.Play(true);
        AudioManager.Instance.play(AudioManager.SOUNDS.COLLECT_NEGATIVE_POWERUP);
        foreach (Player player in GameController.Instance.players)
        {
            if (!player.name.Equals(p.name))
            {
                player.speed *= 0.8f;
            }
        }
        yield return new WaitForSeconds(duration);
    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.Slowing;
    }
}
