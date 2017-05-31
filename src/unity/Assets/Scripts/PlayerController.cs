using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]

public class PlayerController : MonoBehaviour {

    public float speed;
    private Rigidbody rb;
    private Vector3 offset;
    private Vector3 screenPoint;
    private Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    [SerializeField] float treshhold = 0.5f;


    void Start () {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        Vector3 mouseIntoWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(mouseIntoWorldPos.x > gameObject.transform.position.x + treshhold)
        {
            transform.Translate(1, 0, 0);
        }else if(mouseIntoWorldPos.x > gameObject.transform.position.x - treshhold)
        {
            transform.Translate(-1, 0, 0);
        }else if(mouseIntoWorldPos.z > gameObject.transform.position.z + treshhold)
        {
            transform.Translate(0, 0, 1);
        }else if(mouseIntoWorldPos.z > gameObject.transform.position.z - treshhold)
        {
            transform.Translate(0, 0, -1);
        }
        
        

    }
    
    
    /*
    void FixedUpdate () {


        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }
    
    private void FixedUpdate()
    {

        float horizontal = Input.mousePosition.x;
        float vertical = Input.mousePosition.z;

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
