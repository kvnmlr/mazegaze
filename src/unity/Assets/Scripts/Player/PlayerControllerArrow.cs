using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerArrow : PlayerControl
{

    private bool move;
    private Vector3 movement;
    private float currentSpeed;
    private float speedingUp = 1.0000001f;

    void FixedUpdate()
    {
        if (!Menu.Instance.canvas.enabled)
        {
            movement = transform.position;
            move = false;
            LeaveGame();

            if (Input.GetKey(KeyCode.RightArrow))
            {
                movement += Vector3.right * gameObject.GetComponent<Player>().speed * Time.deltaTime;
                move = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                movement += Vector3.left * gameObject.GetComponent<Player>().speed * Time.deltaTime;
                move = true;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                movement += Vector3.forward * gameObject.GetComponent<Player>().speed * Time.deltaTime;
                move = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                movement += Vector3.back * gameObject.GetComponent<Player>().speed * Time.deltaTime;
                move = true;
            }
            if (move)
            {
                if (currentSpeed < gameObject.GetComponent<Player>().speed)
                {
                    currentSpeed += speedingUp * Time.deltaTime;
                }
            }
            else
            {
                currentSpeed = 0;
            }
            transform.position = movement;
        }
    }
    void Update()
    {
        Vector3 temp = transform.position;
        if (transform.position.y > 0.5)
        {
            transform.position = new Vector3(temp.x, 0.5f, temp.z);

        }
    }
}

