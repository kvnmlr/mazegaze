using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PupilConfiguration : Singleton<PupilConfiguration> {

    public PupilListener pupilListener;
    public string pupilConfigFilePath;

    [Serializable]
    public class Settings
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
        public string surface_name;
        public bool detect_surface = true;
        public bool initially_active = true;
        public GazeController gaze_controller { get; set; }
        public bool is_connected { get; set; }
        public bool is_calibrated { get; set; }
    }

    public Settings settings;
    private List<PupilListener> listeners = new List<PupilListener>();

    void Start()
    {
        pupilConfigFilePath = Path.Combine(Application.streamingAssetsPath, "pupil_config.json");
        if (File.Exists(pupilConfigFilePath))
        {
            string dataAsJson = File.ReadAllText(pupilConfigFilePath);
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

    public void SaveSettings()
    {
        String dataAsJson = JsonUtility.ToJson(settings, true);
        File.WriteAllText(pupilConfigFilePath, dataAsJson);

    }

    private void StartListen()
    {
        List<PupilClient> clients = new List<PupilClient>(settings.pupil_clients);
        //clients.RemoveAll((c) => !c.initially_active);
        pupilListener.clients = clients;

        int count = 0;
        foreach (PupilClient c in settings.pupil_clients)
        {
            if (count+1 >= GameController.Instance.players.Length)      // TODO +2 anpassen
            {
                break;
            }
            GameObject player = GameController.Instance.players[count + 1].gameObject;        // TODO +2 because A and B are non-gaze players
            player.AddComponent<GazeController>();
            player.GetComponent<Player>().name = c.name;
            c.gaze_controller = player.GetComponent<GazeController>();
            c.gaze_controller.cursor = Instantiate(GameController.Instance.cursor, null);
            ++count;
        }

        pupilListener.Listen();
    }
}
