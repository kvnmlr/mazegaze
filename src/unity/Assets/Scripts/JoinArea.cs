using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinArea : MonoBehaviour
{
    Dictionary<PupilConfiguration.PupilClient, int> clientToPosition = new Dictionary<PupilConfiguration.PupilClient, int>();
    Dictionary<PupilConfiguration.PupilClient, float> clientToFirstGazeTime = new Dictionary<PupilConfiguration.PupilClient, float>();
    private List<PupilConfiguration.PupilClient> joined = new List<PupilConfiguration.PupilClient>();
    public List<int> positions = new List<int>();
    public Image[] backgroundPosition;

    public Text[] textPosition;

    public Color emptyColor;
    public string emptyText = "Look here to join";

    void OnEnable()
    {
        clientToPosition = new Dictionary<PupilConfiguration.PupilClient, int>();
        clientToFirstGazeTime = new Dictionary<PupilConfiguration.PupilClient, float>();
        joined = new List<PupilConfiguration.PupilClient>();
        foreach (Image i in backgroundPosition)
        {
            i.color = emptyColor;
        }
        foreach(Text t in textPosition)
        {
            t.text = emptyText;
        }

        
    }

    public void addPossiblePosition(int position)
    {
        if (positions.Contains(position))
        {
            return;
        }
        positions.Add(position);
        for (int i = 0; i < backgroundPosition.Length; ++i)
        {
            if (!positions.Contains(i+1))
            {
                backgroundPosition[i].gameObject.SetActive(false);
                textPosition[i].gameObject.SetActive(false);
            } else
            {
                backgroundPosition[i].gameObject.SetActive(true);
                textPosition[i].gameObject.SetActive(true);
            }
        }
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
            foreach (Player p in GameController.Instance.joinedPlayersToPosition.Keys)
            {
                if (p.Equals(client.player))
                {
                    //Debug.Log(0);

                    joined.Add(client);
                }
            }

            if (joined.Contains(client))
            {
                continue;
            }
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
                if (!positions.Contains(joinPosition))
                {
                    
                    int lastPosition = -1;
                    clientToPosition.TryGetValue(client, out lastPosition);
                    if (lastPosition > 0)
                    {
                        backgroundPosition[lastPosition - 1].color = emptyColor;
                        textPosition[lastPosition - 1].text = emptyText;
                        
                    }
                    clientToFirstGazeTime.Remove(client);
                    clientToPosition.Remove(client);
                    return;
                }

                
                //Debug.Log(joinPosition);

                if (clientToPosition.ContainsValue(joinPosition))
                {
                    foreach (PupilConfiguration.PupilClient c in PupilListener.Instance.clients)
                    {
                        int pos = 0;
                        clientToPosition.TryGetValue(c, out pos);
                        if (pos == joinPosition && !c.Equals(client))
                        {
                            //Debug.Log("This position is already taken by " + c.name);
                            return;
                        }
                    }
                }

                if (!clientToPosition.ContainsKey(client) && !clientToFirstGazeTime.ContainsKey(client))
                {
                    clientToPosition.Add(client, joinPosition);
                    clientToFirstGazeTime.Add(client, Time.time);
                    backgroundPosition[joinPosition - 1].color = client.player.getColor(client.player.color);

                } else
                {
                    //Debug.Log(client.name + " asd ");
                    int lastPosition = 0;
                    clientToPosition.TryGetValue(client, out lastPosition);
                    if (lastPosition != joinPosition)
                    {
                        // user lookes at another join position
                        backgroundPosition[lastPosition - 1].color = emptyColor;
                        textPosition[lastPosition - 1].text = emptyText;

                        clientToFirstGazeTime.Remove(client);
                        clientToPosition.Remove(client);
                    }  else
                    {
                        textPosition[joinPosition - 1].text = client.name + " joining here in " + ((int)(4 - (Time.time - clientToFirstGazeTime[client])) + " ... ");

                        if (Time.time - clientToFirstGazeTime[client] > 3)
                        {
                            textPosition[joinPosition - 1].text = "Welcome " + client.name + ". Get ready!";
                            joined.Add(client);
                            GameController.Instance.assignPlayer(client.player, joinPosition, positions.Count == 4, positions.Count != 4);
                        }
                    }
                }
            }
        }
    }
}
