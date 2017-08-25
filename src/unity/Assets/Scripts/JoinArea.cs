using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinArea : MonoBehaviour
{

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    void Update()
    {
        foreach (PupilConfiguration.PupilClient client in PupilListener.Instance.clients)
        {
            if (client.gaze_controller.gazeOnSurface)
            {
                int index = 0;
                foreach (Player p in GameController.Instance.players)
                {
                    if (p.name.Equals(client.name))
                    {
                        GameController.Instance.assignPlayer(p, index+1);
                    }
                    index++;
                }
            }
        }
    }
}
