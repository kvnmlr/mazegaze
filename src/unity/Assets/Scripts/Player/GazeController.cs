using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : Singleton<GazeController> {
    public float speed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void move(double gazeX, double gazeY)
    {
        if (gazeX < 0.3f)
        {
            moveRight();
        } else
        {
            moveLeft();
        }
    }

    private void moveRight()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    private void moveLeft()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    private void moveUp()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }

    private void moveDown()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }

}
