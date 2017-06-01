using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]

public class PlayerController : MonoBehaviour {

    public float speed;
    private Rigidbody rb;
    private Vector3 offset;
    private Vector3 screenPoint;
    // private Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //[SerializeField] float treshhold = 0.5f;
    float depth = 44.5f;

   

    


    void Start () {
        rb = GetComponent<Rigidbody>();
        //Screen.showCursor = false;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 wantedPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));
        rb.AddForce(wantedPos * speed);
    }

  
    
   
   */
}
