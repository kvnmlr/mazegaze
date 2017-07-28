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
        public double[] axes = new double[] {0,0};
        public double angle;
        public double[] center = new double[] {0,0};
    }
    [Serializable]
    public class Sphere
    {
        public double radius;
        public double[] center = new double[] {0,0,0};
    }
    [Serializable]
    public class Circle3d
    {
        public double radius;
        public double[] center = new double[] {0,0,0};
        public double[] normal = new double[] {0,0,0};
    }
    [Serializable]
    public class Ellipse
    {
        public double[] axes = new double[] {0,0};
        public double angle;
        public double[] center = new double[] {0,0};
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
    public string IP = "127.0.0.1";
    public string PORT = "64509";
    public string ID = "surface";
    public GazeController gazeController;
    public bool detectPupils = true;
    public bool detectSurfaces = true;
    private bool newData = false;
    private String name;

    Pupil.PupilData3D pupilData = new  Pupil.PupilData3D();
    Pupil.SurfaceData3D surfaceData = new Pupil.SurfaceData3D();

    public void get_transform(ref Vector3 pos, ref Quaternion q)
    {
        lock (thisLock_)
        {   
            pos = new Vector3(
                        (float)(pupilData.sphere.center[0]),
                        (float)(pupilData.sphere.center[1]),
                        (float)(pupilData.sphere.center[2])
                        )*0.001f;// in [m]
            q = Quaternion.LookRotation(new Vector3(
            (float)(pupilData.circle_3d.normal[0]),
            (float)(pupilData.circle_3d.normal[1]),
            (float)(pupilData.circle_3d.normal[2])
            ));
        }
    }

    void Start()
    {
        if (gazeController != null)
        {
            client_thread_ = new Thread(NetMQClient);
            client_thread_.Start();
            gazeController.listenerReady();
            this.name = gazeController.gameObject.name;
        }

    }

    void NetMQClient()
    {
        string IPHeader = ">tcp://" + IP + ":";
        var timeout = new System.TimeSpan(0, 0, 1);

        AsyncIO.ForceDotNet.Force();
        NetMQConfig.ManualTerminationTakeOver();
        NetMQConfig.ContextCreate(true);
        
        string subport="";
        Debug.Log(name + ": Connect to the server: "+ IPHeader + PORT + ".");
        var requestSocket = new RequestSocket(IPHeader + PORT);

        double t = 0;
        const int N = 1000;
        bool is_connected =false;
        for (int k = 0; k < N; k++)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            requestSocket.SendFrame("SUB_PORT");
            is_connected = requestSocket.TryReceiveFrameString(timeout, out subport);
            sw.Stop();
            t = t+ sw.Elapsed.Milliseconds;
            if (is_connected == false) break;
        }
        requestSocket.Close();

        if (is_connected)
        {
            var subscriberSocket = new SubscriberSocket( IPHeader + subport);
            if (detectSurfaces)
            {
                subscriberSocket.Subscribe("surface");
            }
            if (detectPupils)
            {
                subscriberSocket.Subscribe("pupil.");
            } else
            {
                subscriberSocket.Unsubscribe("pupil.");
            }

            var msg = new NetMQMessage();
            while (is_connected && stop_thread_ == false)
            {
                is_connected = subscriberSocket.TryReceiveMultipartMessage(timeout,ref(msg));
                if (is_connected)
                {
                    try
                    {
                        string msgType = msg[0].ConvertToString();

                        var message = MsgPack.Unpacking.UnpackObject(msg[1].ToByteArray());

                        MsgPack.MessagePackObject mmap = message.Value;
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
                                //Debug.Log(message);
                                //Debug.Log(surfaceData.gaze_on_srf[0].confidence);
                                newData = true;
                            }
                        }
                    }
                    catch
                    {
                        Debug.Log(name + ": Failed to unpack.");
                    }
                }
                else
                {
                    Debug.Log(name + ": Failed to receive a message.");
                    Thread.Sleep(1000);
                }
            }
            subscriberSocket.Close();
        }
        else
        {
            Debug.Log(name + ": Failed to connect the server.");
        }
        Debug.Log(name + ": ContextTerminate.");
        NetMQConfig.ContextTerminate();
    }

    void OnApplicationQuit()
    {
        lock (thisLock_)stop_thread_ = true;
        client_thread_.Join();
        Debug.Log(name + ": Quit the thread.");
    }

    void Update()
    {
        lock (thisLock_)
        {
            if (newData)
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
}