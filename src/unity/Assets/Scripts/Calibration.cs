using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calibration : Singleton<Calibration> {
    public Text text;

    public Canvas calibrationScreen;
    public Image[] markers;

    private float calibrationStartTime;

    void Start()
    {
        //Wo kommen die hier hin, hab ich leider nicht gefunden..
        //AudioManager.Instance.play(AudioManager.SOUNDS.FAILEDCALIBRATION);
        //AudioManager.Instance.play(AudioManager.SOUNDS.SUCCESSCALIBRATION);

    }

    public void CalibrationDone(PupilConfiguration.PupilClient client)
    {
        Debug.Log(client.name + " calibration done");
        string t = text.text;
        t = t.Replace(client.name + " is calibrating ...", client.name + " is ready");
        Debug.Log(t);

        text.text = t;

    }

    public void StartCalibration(PupilConfiguration.PupilClient client)
    {
        calibrationStartTime = Time.time;
        calibrationScreen.gameObject.SetActive(true);
        if (!text.text.Contains(client.name))
        {
            text.text += ("\n" + client.name + " is calibrating ...");
        }
        //StartCoroutine(Example());
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

    void Update()
    {
        if (!text.text.Contains("calibrating") || Time.time - calibrationStartTime > 40)
        {
            calibrationScreen.gameObject.SetActive(false);
        }
    }
}
