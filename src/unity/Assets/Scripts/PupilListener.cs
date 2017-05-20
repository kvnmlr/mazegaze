using UnityEngine;
using System;
using System.Threading;
using NetMQ; // for NetMQConfig
using NetMQ.Sockets;

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
}

public class PupilListener : MonoBehaviour
{

    Thread client_thread_;
    private System.Object thisLock_ = new System.Object();
    bool stop_thread_ = false;
    public string IP = "127.0.0.1";
    public string PORT = "64509";
    public string ID = "surface";
    public bool detectPupils = true;
    public bool detectSurfaces = true;

    Pupil.PupilData3D data_ = new  Pupil.PupilData3D();

    public void get_transform(ref Vector3 pos, ref Quaternion q)
    {
        lock (thisLock_)
        {   
            pos = new Vector3(
                        (float)(data_.sphere.center[0]),
                        (float)(data_.sphere.center[1]),
                        (float)(data_.sphere.center[2])
                        )*0.001f;// in [m]
            q = Quaternion.LookRotation(new Vector3(
            (float)(data_.circle_3d.normal[0]),
            (float)(data_.circle_3d.normal[1]),
            (float)(data_.circle_3d.normal[2])
            ));
        }
    }

    void Start()
    {
        client_thread_ = new Thread(NetMQClient);
        client_thread_.Start();
    }

    void NetMQClient()
    {
        string IPHeader = ">tcp://" + IP + ":";
        var timeout = new System.TimeSpan(0, 0, 1);

        AsyncIO.ForceDotNet.Force();
        NetMQConfig.ManualTerminationTakeOver();
        NetMQConfig.ContextCreate(true);
        
        string subport="";
        Debug.Log("Connect to the server: "+ IPHeader + PORT + ".");
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
                        //Debug.Log(mmap.ToString());
                        if (msgType.Contains("pupil"))
                        {
                            Debug.Log("PUPIL DETECTED");
                            lock (thisLock_)
                            {
                                data_ = JsonUtility.FromJson<Pupil.PupilData3D>(mmap.ToString());
                            }
                        }
                        else if (msgType == "surfaces")
                        {
                            Debug.Log("SURFACE DETECTED");
                        }
                    }
                    catch
                    {
                        Debug.Log("Failed to unpack.");
                    }
                }
                else
                {
                    Debug.Log("Failed to receive a message.");
                    Thread.Sleep(1000);
                }
            }
            subscriberSocket.Close();
        }
        else
        {
            Debug.Log("Failed to connect the server.");
        }
        Debug.Log("ContextTerminate.");
        NetMQConfig.ContextTerminate();
    }

    void OnApplicationQuit()
    {
        lock (thisLock_)stop_thread_ = true;
        client_thread_.Join();
        Debug.Log("Quit the thread.");
    }
}