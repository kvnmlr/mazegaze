using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTarget : PowerUp {
    public override IEnumerator performPowerUp(Player p)
    {
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
