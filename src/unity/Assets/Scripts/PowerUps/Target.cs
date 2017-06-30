using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : PowerUp {

    public override IEnumerator performPowerUp(Player p)
    {
        Debug.Log(p + " wins");
        yield return new WaitForSeconds(0);
    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.Target;
    }

    void Update()
    {

    }
}
