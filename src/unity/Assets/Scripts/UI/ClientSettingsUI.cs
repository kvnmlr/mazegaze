﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientSettingsUI : MonoBehaviour
{
    private bool firstTimeEnable = true;
    public int clientIndex;
    public Image connectionStatus;
    public InputField clientName;
    public InputField ipAddress;
    public InputField port;
    public InputField surfaceName;
    public Button connect;
    public Button calibrate;
    public GameObject add;
    public Image cover;

    private PupilConfiguration.PupilClient client;

    void OnDisable()
    {
        if (client != null)
        {
            if ((client.name.Equals("") || client.ip.Equals("") || client.port.Equals("") || client.surface_name.Equals("")))
            {
                PupilConfiguration.Instance.settings.pupil_clients.Remove(client);
            }
            PupilConfiguration.Instance.SaveSettings();
        }
    }

    void OnEnable()
    {
        if (clientIndex < PupilListener.Instance.clients.Count && clientIndex >= 0)
        {
            gameObject.SetActive(true);
            cover.gameObject.SetActive(false);
            add.gameObject.SetActive(false);

            client = PupilListener.Instance.clients[clientIndex];
            clientName.text = client.name;
            ipAddress.text = client.ip;
            port.text = client.port;
            surfaceName.text = client.surface_name;

            if (client.is_connected)
            {
                connectionStatus.color = new Color(1, 1, 1);
                connect.GetComponentInChildren<Text>().text = "Disconnect";
                if (!firstTimeEnable) AudioManager.Instance.play(AudioManager.SOUNDS.SUCCESSCALIBRATION);
                calibrate.interactable = true;
            }
            else
            {
                connectionStatus.color = new Color(0.1f, 0.1f, 0.1f);
                connect.GetComponentInChildren<Text>().text = "Connect";
                if (!firstTimeEnable) AudioManager.Instance.play(AudioManager.SOUNDS.FAILEDCALIBRATION);
                calibrate.interactable = false;
            }

            if (client.is_calibrated)
            {
                calibrate.GetComponentInChildren<Text>().text = "Re-Calibrate";
            }
            else
            {
                calibrate.GetComponentInChildren<Text>().text = "Calibrate";
            }
        }
        else
        {
            if (clientIndex == PupilListener.Instance.clients.Count)
            {
                add.gameObject.SetActive(true);
            } else
            {
                gameObject.SetActive(false);
            }
        }

    }

    public void ClickConnect()
    {
        client.initially_active = !client.is_connected;
        cover.gameObject.SetActive(true);

        StartCoroutine(Reconnect());
    }

    public void ClickCalibration()
    {
        PupilListener.Instance.StartCalibration(client);
    }

    IEnumerator Reconnect()
    {
        PupilListener.Instance.Reconnect();
        yield return new WaitForSeconds(2);
        firstTimeEnable = false;
        OnEnable();
    }

    public void AddClient()
    {
        PupilConfiguration.PupilClient client = new PupilConfiguration.PupilClient();
        client.is_connected = false;
        client.initially_active = false;
        client.detect_surface = true;
        PupilConfiguration.Instance.settings.pupil_clients.Add(client);
        PupilListener.Instance.clients.Add(client);
        StartCoroutine(Reconnect());
        //OnEnable();
    }

    public void UpdateName()
    {
        client.name = clientName.text;
    }
    public void UpdateIP()
    {
        client.ip = ipAddress.text;
        UpdateConnectionRelevant();
    }
    public void UpdatePort()
    {
        client.port = port.text;
        UpdateConnectionRelevant();
    }
    public void UpdateSurfaceName()
    {
        client.surface_name = surfaceName.text;
    }
    private void UpdateConnectionRelevant()
    {
        connect.GetComponentInChildren<Text>().text = "Connect";
        connectionStatus.color = new Color(0.1f, 0.1f, 0.1f);

    }
}
