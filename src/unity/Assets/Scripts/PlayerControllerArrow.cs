using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerArrow : MonoBehaviour {

    //PlayerB
    public float speed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!Menu.Instance.canvas.enabled)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
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
            if (GameController.Instance.getNumGames() == GameController.Instance.getPlayedGames())
            {
                Menu.Instance.GameOver();
            }
            else
            {
                StartCoroutine(Menu.Instance.GetWinText());

                GameController.Instance.startNewRound();

            }
        }
    }

}

