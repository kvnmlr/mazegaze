using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControl : MonoBehaviour {

    public Vector3 oldPosition = new Vector3(0.0f, 0.0f, 0.0f);
    public float playerLeftGame { get; set; }
    private int timeToLeave = 10;

    // Use this for initialization
    void Start () {		
	}

    void OnEnable()
    {
        playerLeftGame = Time.time;
    }

    public void LeaveGame()
    {
        Vector3 currentPosition = transform.position;

        if (oldPosition == currentPosition)
        {
            if (Time.time - playerLeftGame > timeToLeave && GameController.Instance.joinedPlayersToPosition.Keys.Count > 1)
            {
                AudioManager.Instance.play(AudioManager.SOUNDS.PLAYERKICKED);
                // Player did not look at maze for more than 5 consequtive seconds, kick him out of the game
                Player player = gameObject.GetComponent<Player>();
                //Debug.Log("Kicking player " + player.name + " out of the game (inactive)");
                int pos;
                if (player != null)
                {
                    GameController.Instance.joinedPlayersToPosition.TryGetValue(player, out pos);
                    if (pos > 0)
                    {
                        GameController.Instance.inGameJoinArea.addPossiblePosition(pos);
                        GameController.Instance.inGameJoinArea.gameObject.SetActive(true);
                    }
                    GameController.Instance.joinedPlayersToPosition.Remove(player);

                }
                gameObject.SetActive(false);

            }
        }
        else
        {
            oldPosition = currentPosition;
            playerLeftGame = Time.time;
          
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

   
}
