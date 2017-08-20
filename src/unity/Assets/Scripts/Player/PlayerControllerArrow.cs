using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerArrow : MonoBehaviour
{
    void FixedUpdate()
    {
        if (!Menu.Instance.canvas.enabled)
        {           
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * gameObject.GetComponent<Player>().speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * gameObject.GetComponent<Player>().speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += Vector3.forward * gameObject.GetComponent<Player>().speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += Vector3.back * gameObject.GetComponent<Player>().speed * Time.deltaTime;
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

