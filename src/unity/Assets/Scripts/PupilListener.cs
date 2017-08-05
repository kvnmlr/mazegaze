using UnityEngine;
using System;
using System.Threading;
using NetMQ; // for NetMQConfig
using NetMQ.Sockets;
using System.Collections.Generic;

namespace Pupil
{
    // Pupil data typea
    [Serializable]
    public class ProjectedSphere
    {
        public double[] axes = new double[] { 0, 0 };
        public double angle;
        public double[] center = new double[] { 0, 0 };
    }
    [Serializable]
    public class Sphere
    {
        public double radius;
        public double[] center = new double[] { 0, 0, 0 };
    }
    [Serializable]
    public class Circle3d
    {
        public double radius;
        public double[] center = new double[] { 0, 0, 0 };
        public double[] normal = new double[] { 0, 0, 0 };
    }
    [Serializable]
    public class Ellipse
    {
        public double[] axes = new double[] { 0, 0 };
        public double angle;
        public double[] center = new double[] { 0, 0 };
    }
    [Serializable]
    public class PupilData3D
    {
        public double diameter;
        public double confidence;
        public ProjectedSphere projected_sphere = new ProjectedSphere();
        public double theta;
        public int model_id;
        public double timestamp;
        public double model_confidence;
        public string method;
        public double phi;
        public Sphere sphere = new Sphere();
        public double diameter_3d;
        public double[] norm_pos = new double[] { 0, 0, 0 };
        public int id;
        public double model_birth_timestamp;
        public Circle3d circle_3d = new Circle3d();
        public Ellipse ellipese = new Ellipse();
    }
    [Serializable]
    public class GazeOnSurface
    {
        public bool on_srf;
        public string topic;
        public double confidence;
        public double[] norm_pos = new double[] { 0, 0, 0 };
    }

    [Serializable]
    public class SurfaceData3D
    {
        public double timestamp;
        public double uid;
        public String name;
        public List<GazeOnSurface> gaze_on_srf = new List<GazeOnSurface>();
        // TODO m_from_screen
        // TODO m_to_screen
    }
}

public class PupilListener : MonoBehaviour
{

    Thread client_thread_;

    private System.Object thisLock_ = new System.Object();
    bool stop_thread_ = false;

    public List<SettingsManager.PupilClient> clients = new List<SettingsManager.PupilClient>();
    public List<bool> connected = new List<bool>();
    private List<RequestSocket> requestSockets = new List<RequestSocket>();
    private List<String> IPHeaders = new List<string>();
    private List<SubscriberSocket> subscriberSockets = new List<SubscriberSocket>();

    public GazeController gazeController;
    public bool detectPupils = true;
    public bool detectSurfaces = true;
    private bool newData = false;
    private bool isConnected = true;


    Pupil.PupilData3D pupilData = new Pupil.PupilData3D();
    Pupil.SurfaceData3D surfaceData = new Pupil.SurfaceData3D();

    public void get_transform(ref Vector3 pos, ref Quaternion q)
    {
        lock (thisLock_)
        {
            pos = new Vector3(
                        (float)(pupilData.sphere.center[0]),
                        (float)(pupilData.sphere.center[1]),
                        (float)(pupilData.sphere.center[2])
                        ) * 0.001f;// in [m]
            q = Quaternion.LookRotation(new Vector3(
            (float)(pupilData.circle_3d.normal[0]),
            (float)(pupilData.circle_3d.normal[1]),
            (float)(pupilData.circle_3d.normal[2])
            ));
        }
    }

    public void Listen()
    {
        Debug.Log("PupilListener");
        //this.name = gazeController.gameObject.name;

        client_thread_ = new Thread(NetMQClient);

        client_thread_.Start();

        //gazeController.listenerReady();
    }


    void NetMQClient()
    {
        var timeout = new System.TimeSpan(0, 0, 1);
        AsyncIO.ForceDotNet.Force();
        NetMQConfig.ManualTerminationTakeOver();
        NetMQConfig.ContextCreate(true);
        string subport;
        List<string> subports = new List<string>();

        int index = 0;
        foreach (SettingsManager.PupilClient c in clients)
        {
            string IPHeader = ">tcp://" + c.ip + ":";
            RequestSocket requestSocket = new RequestSocket(IPHeader + c.port);
            Debug.Log("Requesting socket for " + IPHeader + ":" + c.port + " (" + c.name + ")");

            IPHeaders.Add(IPHeader);
            connected.Add(new bool());

            double t = 0;
            const int N = 5;
            for (int k = 0; k < N; k++)
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                requestSocket.SendFrame("SUB_PORT");
                timeout = new System.TimeSpan(0, 0, 1);
                bool frameReceived = requestSocket.TryReceiveFrameString(timeout, out subport);
                sw.Stop();
                t = t + sw.Elapsed.Milliseconds;

                if (!frameReceived)
                {
                    Debug.Log("Could not connect to client " + IPHeaders[index] + ":" + clients[index].port + " (" + clients[index].name + ")");
                    IPHeaders.Remove(IPHeader);
                    break;
                }
                subports.Add(subport);
            }
            requestSocket.Close();
            index++;
        }

        isConnected = IPHeaders.Count > 0;

        if (isConnected)
        {
            Debug.Log("Connected to " + IPHeaders.Count + " sockets");
            foreach (String header in IPHeaders)
            {
                SubscriberSocket subscriberSocket = new SubscriberSocket(header + subports[IPHeaders.IndexOf(header)]);
                if (clients[IPHeaders.IndexOf(header)].detect_surface)
                {
                    subscriberSocket.Subscribe("surface");
                }
                subscriberSocket.Subscribe("pupil.");
                subscriberSockets.Add(subscriberSocket);
            }

            var msg = new NetMQMessage();
            int turn = 0;
            while (isConnected && stop_thread_ == false)
            {
                turn = ++turn % IPHeaders.Count;
                timeout = new System.TimeSpan(0, 0, 0, 0, 200);

                bool stillAlive = subscriberSockets[turn].TryReceiveMultipartMessage(timeout, ref (msg));

                if (stillAlive && isConnected)
                {
                    try
                    {
                        string msgType = msg[0].ConvertToString();

                        var message = MsgPack.Unpacking.UnpackObject(msg[1].ToByteArray());

                        MsgPack.MessagePackObject mmap = message.Value;
                        Debug.Log(mmap.ToString());
                        if (msgType.Contains("pupil"))
                        {
                            // pupil detected
                            lock (thisLock_)
                            {
                                pupilData = JsonUtility.FromJson<Pupil.PupilData3D>(mmap.ToString());
                            }
                        }

                        if (msgType == "surfaces")
                        {
                            // surface detected
                            lock (thisLock_)
                            {
                                Debug.Log(mmap.ToString());

                                surfaceData = JsonUtility.FromJson<Pupil.SurfaceData3D>(mmap.ToString());
                                //Debug.Log(message);
                                //Debug.Log(surfaceData.gaze_on_srf[0].confidence);
                                newData = true;
                            }
                        }
                    }
                    catch
                    {
                        Debug.Log(clients[turn].name + ": Failed to unpack.");
                    }
                }
                else
                {
                    Debug.Log(clients[turn].name + ": Failed to receive a message.");
                }
            }
            foreach (SubscriberSocket s in subscriberSockets)
            {
                s.Close();
            }
            subscriberSockets.Clear();
        }
        else
        {
            Debug.Log("Failed to connect to all clients.");
        }
        Debug.Log("ContextTerminate.");
        NetMQConfig.ContextTerminate();
    }

    void OnApplicationQuit()
    {
        //lock (thisLock_)stop_thread_ = true;
        foreach (SubscriberSocket s in subscriberSockets)
        {
            s.Close();
        }
        foreach (RequestSocket rs in requestSockets)
        {
            rs.Close();
        }
        Debug.Log("ContextTerminate.");
        NetMQConfig.ContextTerminate();
    }

    void Update()
    {
        if (newData && isConnected)
        {
            if (gazeController != null)
            {
                if (surfaceData == null)
                {
                    return;
                }
                foreach (Pupil.GazeOnSurface gos in surfaceData.gaze_on_srf)
                {
                    if (gos == null)
                    {
                        return;
                    }
                    if (gos.norm_pos == null)
                    {
                        return;
                    }
                    gazeController.move(surfaceData);
                }

            }
            newData = false;
        }
    }
}