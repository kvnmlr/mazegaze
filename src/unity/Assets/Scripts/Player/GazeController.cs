using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : Singleton<GazeController> {
    public float speed;

    private Rigidbody rb;

    private Vector3 offset;
    private Vector3 screenPoint;
    float depth;
    float radius = 10;

    private float gazeX;
    private float gazeY;


    void Start()
    {
    }

    public void move(Pupil.SurfaceData3D data)
    {
        Pupil.GazeOnSurface gaze = new Pupil.GazeOnSurface();
        double maxConfidence = 0;
        foreach(Pupil.GazeOnSurface gos in data.gaze_on_srf)
        {
            if (gos.confidence > maxConfidence)
            {
                maxConfidence = gos.confidence;
                gaze = gos;
            }
        }

        if (!gaze.on_srf)
        {
            // gaze is not on the surface, don't do anything
            //Debug.Log("Not on surface");
            return;
        }

        if (!data.name.Equals("game_screen"))
        {
            // gaze is not on the right surface, don't do anything
            //Debug.Log("On wrong surface");
            return;
        }

        gazeX = (float)gaze.norm_pos[0];
        gazeY = (float)gaze.norm_pos[1];

        Debug.Log("X: " + gazeX);
        Debug.Log("Y: " + gazeY);
        move();

    }

    void move()
    {
        rb = GetComponent<Rigidbody>();

        Debug.Log("Update");

            depth = MazeGenerator.Instance.xSize * 4 - 0.5f;
            int size = MazeGenerator.Instance.xSize;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * gazeX, Screen.height * gazeY, depth));

            //CheckArea();

            Debug.Log("Pos: " + mousePos);

            Vector3 temp = transform.position;

            if (System.Math.Abs(transform.position.x - mousePos.x) < radius && System.Math.Abs(transform.position.z - mousePos.z) < radius)
            {
                transform.position = Vector3.MoveTowards(transform.position, mousePos, speed * Time.deltaTime);

            }
            else if (System.Math.Abs(transform.position.x - mousePos.x) >= radius && System.Math.Abs(transform.position.z - mousePos.z) >= radius)
            {

                rb.position = transform.position;

            }
            //TODO schaue nach cell von mitspielern
            if (transform.position.y > 0.5)
            {
                transform.position = new Vector3(temp.x, 0.5f, temp.z);
            }

        }

}
