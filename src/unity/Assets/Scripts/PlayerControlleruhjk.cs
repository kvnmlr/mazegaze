using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlleruhjk : MonoBehaviour {

    public float speed;
    private Rigidbody rb;
    Vector3 pos;
	// Use this for initialization

	void Start () {
        rb = GetComponent<Rigidbody>();
        pos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.K))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.H))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.U)){ 
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.J))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }


    }
}
