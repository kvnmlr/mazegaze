using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlightenment : PowerUp {

    public override void activate(Player p)
    {
        Debug.Log(p.name + " collected Enlightenment");
    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.Enlightenment;

    }

    void Update()
    {

    }
}
