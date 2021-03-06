﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlleruhjk : MonoBehaviour
{
    public float speed;

    void FixedUpdate()
    {
        if (!Menu.Instance.canvas.enabled)
        {
            if (Input.GetKey(KeyCode.K))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.H))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.U))
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.J))
            {
                transform.position += Vector3.back * speed * Time.deltaTime;
            }
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