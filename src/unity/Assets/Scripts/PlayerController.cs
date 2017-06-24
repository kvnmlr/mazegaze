﻿using System.Collections;
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
        Vector3 temp = transform.position;
        if (System.Math.Abs(transform.position.x - mousePos.x) < radius && System.Math.Abs(transform.position.z - mousePos.z) < radius)
        {
            transform.position = Vector3.MoveTowards(transform.position, mousePos, speed * Time.deltaTime);

        }else if(System.Math.Abs(transform.position.x - mousePos.x) >= radius && System.Math.Abs(transform.position.z - mousePos.z) >= radius)
        {
            
            rb.position = transform.position;
        }
        if(transform.position.y > 0.5)
        {
            transform.position = new Vector3(temp.x, 0.5f, temp.z);
        }

        

    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("nophysics"))
        {
            
            other.gameObject.SetActive(false);
            /*rb.isKinematic = true;
            int i = 0;
            while(i< 1000)
            {
                i++;
            }
            rb.isKinematic = false;
        }
    }*/

}