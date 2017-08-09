﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsManager : Singleton<SettingsManager> {

    [Serializable]
    private class Settings
    {
        public List<PupilClient> pupil_clients = new List<PupilClient>();
        public string setup_id = "";
    }

    [Serializable]
    public class PupilClient
    {
        public string name;
        public string ip;
        public string port;
        public bool detect_surface = true;
        public GazeController gaze_controller;
    }

    private Settings settings;
    private List<PupilListener> listeners = new List<PupilListener>();

    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "settings.json");
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            settings = JsonUtility.FromJson<Settings>(dataAsJson);
            Debug.Log(settings.setup_id + " have been loaded");

            StartListen();

        } else
        {
            Debug.Log("Settings file does not exist");
        }
    }

    public void StopL()
    {
        StartCoroutine(StopListen());
    }
    IEnumerator StopListen()
    {
        Debug.Log("Terminating " + listeners.Count + " listeners");
        foreach (PupilListener p in listeners)
        {
            Debug.Log("Shutting down client " + p.name);
            //p.StopListen();
            yield return new WaitForSeconds(3);
        }
    }

    private void StartListen()
    {
        GameObject listener = new GameObject("PupilListener");
        listener.AddComponent<PupilListener>();
        PupilListener p = listener.GetComponent<PupilListener>();
        p.detectPupils = true;
        p.clients = settings.pupil_clients;

        int count = 0;
        foreach (PupilClient c in settings.pupil_clients)
        {
            GameObject player = GameController.Instance.players[count + 2].gameObject;        // todo +2 because A and B are non-gaze players
            player.AddComponent<GazeController>();
            player.GetComponent<Player>().name = c.name;
            c.gaze_controller = player.GetComponent<GazeController>();
            ++count;
        }

        p.Listen();
    }
}