using UnityEngine;
using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using System.Collections.Generic;

namespace Pupil
{
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
    }
}

public class PupilListener : MonoBehaviour
{
    Thread client_thread_;
    private System.Object thisLock_ = new System.Object();
    bool stop_thread_ = false;

    public List<SettingsManager.PupilClient> clients = new List<SettingsManager.PupilClient>();
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
        client_thread_ = new Thread(NetMQClient);
        client_thread_.Start();
    }


    void NetMQClient()
    {
        var timeout = new System.TimeSpan(0, 0, 1);
        AsyncIO.ForceDotNet.Force();
        NetMQConfig.ManualTerminationTakeOver();
        NetMQConfig.ContextCreate(true);

        List<string> subports = new List<string>();     // subports for each client connection

        // loop through all clients and try to connect to them
        foreach (SettingsManager.PupilClient c in clients)
        {
            string subport = "";
            string IPHeader = ">tcp://" + c.ip + ":";
            IPHeaders.Add(IPHeader);

            Debug.Log("Requesting socket for " + IPHeader + ":" + c.port + " (" + c.name + ")");
            RequestSocket requestSocket = new RequestSocket(IPHeader + c.port);

            bool frameReceived = false;
            requestSocket.SendFrame("SUB_PORT");
            timeout = new System.TimeSpan(0, 0, 1);
            frameReceived = requestSocket.TryReceiveFrameString(timeout, out subport);  // request subport, will be saved in var subport for this client
            if (frameReceived)
            {
                subports.Add(subport);
            } else
            {
                Debug.Log("Could not connect to client " + IPHeader + ":" + c.port + " (" + c.name + ")");
                IPHeaders.Remove(IPHeader);     // remove IPHeaders for unavailable clients
            }
            requestSocket.Close();
        }

        isConnected = (IPHeaders.Count == clients.Count);   // check if all clients are connected
        
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
            int turn = 0;   // used receive a message from each client in turn
            while (!stop_thread_)
            {
                turn = ++turn % IPHeaders.Count;
                timeout = new System.TimeSpan(0, 0, 0, 0, 200);     // wait 200ms to receive a message

                bool stillAlive = subscriberSockets[turn].TryReceiveMultipartMessage(timeout, ref (msg));

                if (stillAlive)
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
                                surfaceData = JsonUtility.FromJson<Pupil.SurfaceData3D>(mmap.ToString());
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
        NetMQConfig.ContextTerminate();
    }

    void OnApplicationQuit()
    {
        lock (thisLock_)stop_thread_ = true;
        foreach (SubscriberSocket s in subscriberSockets)
        {
            s.Close();
        }
        foreach (RequestSocket rs in requestSockets)
        {
            rs.Close();
        }
        NetMQConfig.ContextTerminate();
        Debug.Log("Network Threads terminated.");
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