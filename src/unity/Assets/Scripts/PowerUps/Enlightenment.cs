using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlightenment : PowerUp {

    public override IEnumerator performPowerUp(Player p)
    {
        yield return new WaitForSeconds(0);

    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.Enlightenment;
    }

    void Update()
    {

    }
}
