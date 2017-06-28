using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endurance : PowerUp
{
    public override void activate(Player p)
    {
        Debug.Log(p.name + " collected Endurance");
    }

    void Start () {
        this.type = PowerUpManager.PowerUpTypes.Endurance;
	}
	
	void Update () {
		
	}
}
