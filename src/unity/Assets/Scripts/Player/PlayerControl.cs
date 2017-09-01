using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControl : MonoBehaviour {

    public Vector3 oldPosition = new Vector3(0.0f, 0.0f, 0.0f);

    private float playerLeftGame;
    private int timeToLeave = 10;

    // Use this for initialization
    void Start () {

		
	}

    public void LeaveGame()
    {
        Vector3 currentPosition = transform.position;

        if (oldPosition == currentPosition)
        {
            if (Time.time - playerLeftGame > timeToLeave)
            {
                // Player did not look at maze for more than 5 consequtive seconds, kick him out of the game
                Player player = gameObject.GetComponent<Player>();
                //Debug.Log("Kicking player " + player.name + " out of the game (inactive)");
                GameController.Instance.joinedPlayersToPosition.Remove(player);
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
