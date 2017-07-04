using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //PlayerA
    public float speed;
    private Rigidbody rb;
    private Vector3 offset;
    private Vector3 screenPoint;
    float depth;
    float radius = 2;
    

    void Start () {
        rb = GetComponent<Rigidbody>();
        depth = MazeGenerator.Instance.xSize * 4 - 0.5f;
        //TODO: Radius mit Eyetracker anpassen;
        if (MazeGenerator.Instance.xSize == 9)
        {
            radius = 2;
        }else if(MazeGenerator.Instance.xSize == 15)
        {
            radius = 3;
        }
        else
        {
            radius = 5;
        }
    }

    void Update()
    {
        if (!Menu.Instance.canvas.enabled)
        {
            int size = MazeGenerator.Instance.xSize;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth));

            Vector3 temp = transform.position;

            if (System.Math.Abs(transform.position.x - mousePos.x) < radius && System.Math.Abs(transform.position.z - mousePos.z) < radius)
            {
                transform.position = Vector3.MoveTowards(transform.position, mousePos, speed * Time.deltaTime);

            }
            else if (System.Math.Abs(transform.position.x - mousePos.x) >= radius && System.Math.Abs(transform.position.z - mousePos.z) >= radius)
            {

                rb.position = transform.position;
            }

            if (transform.position.y > 0.5)
            {
                transform.position = new Vector3(temp.x, 0.5f, temp.z);
            }

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
