using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    private Rigidbody rb;
    private Vector3 offset;
    private Vector3 screenPoint;
    float depth = 44.5f;

    void Start () {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth));
        float radius = 2;
        if (System.Math.Abs(transform.position.x - mousePos.x) < radius && System.Math.Abs(transform.position.y - mousePos.y) < radius)
        {
            transform.position = Vector3.MoveTowards(transform.position, mousePos, speed * Time.deltaTime);

        }
    }

}
