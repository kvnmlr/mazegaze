using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerwasd : MonoBehaviour {

    //PlayerC
    public float speed;
    private Rigidbody rb;
    Vector3 pos;
    // Use this for initialization

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!Menu.Instance.canvas.enabled)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            other.gameObject.SetActive(false);
            GameController.Instance.setRestart(true);
            Menu.Instance.GameOver();
           
        }
    }

}
