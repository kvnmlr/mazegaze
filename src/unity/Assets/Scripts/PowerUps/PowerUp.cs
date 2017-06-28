using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public PowerUpManager.PowerUpTypes type { get; set; }
    public abstract void activate(Player p);
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            Player p = other.gameObject.GetComponent<Player>();

            if (p == null)
            {
                return;
            }
            activate(p);
            gameObject.SetActive(false);
        }
    }
}
