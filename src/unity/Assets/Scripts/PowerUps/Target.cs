using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : PowerUp {

    public override void activate(Player p)
    {
        Debug.Log("Player " + p.name + " Gewinnt");
    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.Target;
    }

    void Update()
    {

    }
}
