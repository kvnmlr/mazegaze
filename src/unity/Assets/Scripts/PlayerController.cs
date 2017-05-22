using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]

public class PlayerController : MonoBehaviour {

    public float speed;
    private Rigidbody rb;
    private Vector3 offset;
    private Vector3 screenPoint;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        
	}

    // Update is called once per frame
    void FixedUpdate () {


        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }
    /*private void FixedUpdate()
    {

        float horizontal = Input.mousePosition.x;
        float vertical = Input.mousePosition.y;

        Vector3 movement = new Vector3(horizontal, 0.0f,vertical);

        rb.AddForce(movement * speed);
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;


    }*/

}
