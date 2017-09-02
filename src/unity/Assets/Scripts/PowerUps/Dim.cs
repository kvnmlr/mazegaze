using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dim : PowerUp {

    private float duration = 0;
    public ParticleSystem badEffect;

    public override IEnumerator performPowerUp(Player p)
    {
        badEffect.transform.position = gameObject.transform.position;
        badEffect.Play(true);
        AudioManager.Instance.play(AudioManager.SOUNDS.COLLECT_NEGATIVE_POWERUP);
        Debug.Log("perform on " + p.name);
        foreach (Player player in GameController.Instance.joinedPlayersToPosition.Keys)
        {
            if (!player.name.Equals(p.name))
            {
                player.timeToDarkness *= 0.7f;
            }
        }
       
        yield return new WaitForSeconds(duration);

    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.Dim;
    }
}
