using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTarget : PowerUp {

    public override void activate(Player p)
    {
        Debug.Log(p.name + " collected Target PowerUp");
    }

    void Start()
    {
        this.type = PowerUpManager.PowerUpTypes.Target;

    }

    void Update()
    {

    }
}
