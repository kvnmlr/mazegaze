using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTarget : PowerUp {

    public ParticleSystem goodEffect;

    public override IEnumerator performPowerUp(Player p)
    {
        goodEffect.transform.position = gameObject.transform.position;
        goodEffect.Play(true);
        PowerUpManager.Instance.target.cell.spawnLight(p, true);
        AudioManager.Instance.play(AudioManager.SOUNDS.COLLECT_POSITIVE_POWERUP);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0);
    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.ShowTarget;
    }

    void Update()
    {

    }
}
