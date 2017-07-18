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

        double gazeX = gaze.norm_pos[0];
        double gazeY = gaze.norm_pos[1];

        Debug.Log("X: " + gazeX);
        Debug.Log("Y: " + gazeY);

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
