using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calibration : Singleton<Calibration> {

    public Canvas calibrationScreen;
    public Image[] markers;

    void Start()
    {
    }

    public void StartCalibration()
    {
        StartCoroutine(Example());
    }

    IEnumerator Example()
    {
        //Debug.Log("Calibration Started");
        calibrationScreen.gameObject.SetActive(true);
        for (int currentMarker = 0; currentMarker < markers.Length; currentMarker++)
        {
            foreach (Image i in markers)
            {
                i.gameObject.SetActive(false);
            }
            markers[currentMarker].gameObject.SetActive(true);
            markers[currentMarker].transform.GetChild(1).gameObject.SetActive(true);
            markers[currentMarker].transform.GetChild(0).gameObject.SetActive(false);

            yield return new WaitForSeconds(2f);
            markers[currentMarker].transform.GetChild(0).gameObject.SetActive(true);
            markers[currentMarker].transform.GetChild(1).gameObject.SetActive(false);
            yield return new WaitForSeconds(1.0f);

        }
        //Debug.Log("Calibration Done");
        calibrationScreen.gameObject.SetActive(false);
    }
}
