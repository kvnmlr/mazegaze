using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerArrow : PlayerControl
{

    private bool move;
    private Vector3 movement;
    private float currentSpeed=0;
    private float speedingUp = 0.4f;

    void FixedUpdate()
    {
        if (!Menu.Instance.canvas.enabled && !Menu.Instance.joinScreen.activeSelf)
        {
            movement = transform.position;
            move = false;
            LeaveGame();

            if (Input.GetKey(KeyCode.RightArrow))
            {
                movement += Vector3.right * currentSpeed * Time.deltaTime;
                move = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                movement += Vector3.left * currentSpeed * Time.deltaTime;
                move = true;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                movement += Vector3.forward * currentSpeed * Time.deltaTime;
                move = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                movement += Vector3.back * currentSpeed * Time.deltaTime;
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

