using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTarget : PowerUp {
    public override IEnumerator performPowerUp(Player p)
    {
        PowerUpManager.Instance.target.cell.spawnLight(p, true);
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
