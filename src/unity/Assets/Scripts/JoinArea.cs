using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinArea : MonoBehaviour
{
    Dictionary<PupilConfiguration.PupilClient, int> clientToPosition = new Dictionary<PupilConfiguration.PupilClient, int>();
    Dictionary<PupilConfiguration.PupilClient, float> clientToFirstGazeTime = new Dictionary<PupilConfiguration.PupilClient, float>();

    void OnEnable()
    {
        clientToPosition = new Dictionary<PupilConfiguration.PupilClient, int>();
        clientToFirstGazeTime = new Dictionary<PupilConfiguration.PupilClient, float>();
    }

    void OnDisable()
    {

    }

    void Update()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        foreach (PupilConfiguration.PupilClient client in PupilListener.Instance.clients)
        {
            if (client.gaze_controller.gazeOnSurface)
            {
                float y = client.gaze_controller.gazeY;
                float x = client.gaze_controller.gazeX;

                int joinPosition = 0;

                if (y > 0.5f)
                {
                    // upper half
                    if (x > 0.5f)
                    {
                        //Debug.Log(client.name + " wants to join upper right");
                        joinPosition = 4;
                    } else
                    {
                       // Debug.Log(client.name + " wants to join upper left");
                        joinPosition = 2;
                    }
                } else
                {
                    // lower half
                    if (x > 0.5f)
                    {
                        //Debug.Log(client.name + " wants to join lower right");
                        joinPosition = 3;
                    }
                    else
                    {
                        //Debug.Log(client.name + " wants to join lower left");
                        joinPosition = 1;
                    }
                }
                if (joinPosition == 0)
                {
                    // something went wrong
                    return;
                }

                //Debug.Log(joinPosition);

                if (!clientToPosition.ContainsKey(client) && !clientToFirstGazeTime.ContainsKey(client))
                {
                    clientToPosition.Add(client, joinPosition);
                    clientToFirstGazeTime.Add(client, Time.time);
                } else
                {
                    int lastPosition = 0;
                    clientToPosition.TryGetValue(client, out lastPosition);
                    if (lastPosition != joinPosition)
                    {
                        // user lookes at another join position
                        clientToFirstGazeTime[client] = Time.time;
                        clientToPosition[client] = joinPosition;
                    }  else
                    {
                        if (Time.time - clientToFirstGazeTime[client] > 1)
                        {
                            foreach (Player p in GameController.Instance.players)
                            {
                                if (p.name.Equals(client.name))
                                {
                                    Debug.Log(client.name);
                                    GameController.Instance.assignPlayer(p, joinPosition);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
