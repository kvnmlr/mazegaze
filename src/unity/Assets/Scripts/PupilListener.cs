using UnityEngine;
using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using System.Collections.Generic;
using System.Net;
using MsgPack.Serialization;
using System.IO;

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
        public bool on_srf = false;
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

public class PupilListener : Singleton<PupilListener>
{
    Thread client_thread_;
    private System.Object thisLock_ = new System.Object();
    bool stop_thread_ = false;

    public List<PupilConfiguration.PupilClient> clients = new List<PupilConfiguration.PupilClient>();

    public List<String> IPHeaders = new List<string>();
    private List<SubscriberSocket> subscriberSockets = new List<SubscriberSocket>();
    private List<RequestSocket> requestSockets = new List<RequestSocket>();

    private bool newData = false;
    private bool isConnected = true;
    private int turn = 0;
    private TimeSpan timeout = new System.TimeSpan(0, 0, 1);


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
        IPHeaders = new List<string>();
        subscriberSockets = new List<SubscriberSocket>();
        requestSockets = new List<RequestSocket>();
        client_thread_ = new Thread(NetMQClient);
        turn = 0;

        client_thread_.Start();
    }


    void NetMQClient()
    {
        AsyncIO.ForceDotNet.Force();
        NetMQConfig.ManualTerminationTakeOver();
        NetMQConfig.ContextCreate(true);

        List<string> subports = new List<string>();     // subports for each client connection

        // loop through all clients and try to connect to them
        foreach (PupilConfiguration.PupilClient c in clients)
        {
            string subport = "";
            string IPHeader = ">tcp://" + c.ip + ":";
            bool frameReceived = false;

            Debug.LogFormat("Requesting socket for {0}:{1} ({2})", c.ip, c.port, c.name);
            RequestSocket requestSocket;
            try
            {
                // validate ip header
                if (!validateIPHeader(c.ip, c.port))
                {
                    Debug.LogErrorFormat("{0}:{1} is not a valid ip header for client {2}", c.ip, c.port, c.name);
                    continue;
                }

                requestSocket = new RequestSocket(IPHeader + c.port);
                if (requestSocket != null)
                {
                    requestSocket.SendFrame("SUB_PORT");
                    timeout = new System.TimeSpan(0, 0, 1);
                    frameReceived = requestSocket.TryReceiveFrameString(timeout, out subport);  // request subport, will be saved in var subport for this client

                    if (frameReceived)
                    {
                        if (c.initially_active)
                        {
                            subports.Add(subport);
                            IPHeaders.Add(IPHeader);
                            c.is_connected = true;

                            // set up the pupil client software
                            // sendRequest(requestSocket, new Dictionary<string, object> { { "subject", "eye_process.should_start.0" }, { "eye_id", 0 } });
                        }
                        else
                        {
                            string failHeader = "";
                            subports.Add(failHeader);
                            IPHeaders.Add(failHeader);
                            c.is_connected = false;
                            Debug.LogWarningFormat("Skipped connection to client {0}:{1} ({2})", c.ip, c.port, c.name);
                        }
                    }
                    else
                    {
                        string failHeader = "";
                        subports.Add(failHeader);
                        IPHeaders.Add(failHeader);
                        c.is_connected = false;
                        Debug.LogWarningFormat("Could not connect to client {0}:{1} ({2}). Make sure address is corect and pupil remote service is running", c.ip, c.port, c.name);
                    }
                    requestSockets.Add(requestSocket);
                    //requestSocket.Close();
                }

            }
            catch (Exception e)
            {
                Debug.LogWarningFormat("Could not reach to client {0}:{1} ({2}): {4}", c.ip, c.port, c.name, e.ToString());
            }

        }

        isConnected = true; // (IPHeaders.Count == clients.Count);   // check if all clients are connected

        if (isConnected)
        {
            Debug.LogFormat("Connected to {0} sockets", IPHeaders.Count);
            foreach (String header in IPHeaders)
            {
                if (header.Equals(""))
                {
                    subscriberSockets.Add(new SubscriberSocket());
                    continue;
                }
                SubscriberSocket subscriberSocket = new SubscriberSocket(header + subports[IPHeaders.IndexOf(header)]);
                if (clients[IPHeaders.IndexOf(header)].detect_surface)
                {
                    subscriberSocket.Subscribe("surface");
                }
                subscriberSocket.Subscribe("pupil.");
                //subscriberSocket.Subscribe("notify.");
                //subscriberSocket.Subscribe("calibration.");
                //subscriberSocket.Subscribe("logging.info");
                //subscriberSocket.Subscribe("calibration_routines.calibrate");
                //subscriberSocket.Subscribe("frame.");
                //subscriberSocket.Subscribe("gaze.");

                subscriberSockets.Add(subscriberSocket);
            }

            var msg = new NetMQMessage();
            turn = 0;   // used receive a message from each client in turn
            while (!stop_thread_)
            {

                turn = ++turn % clients.Count;
                if (IPHeaders[turn].Equals("") || clients[turn].is_connected == false)
                {
                    continue;
                }
                timeout = new System.TimeSpan(0, 0, 0, 0, 1);     // wait 200ms to receive a message

                bool stillAlive = subscriberSockets[turn].TryReceiveMultipartMessage(timeout, ref (msg));

                if (stillAlive)
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
                        if (msgType.Contains("frame"))
                        {
                        }
                        if (msgType.Contains("gaze"))
                        {
                        }

                        if (msgType.Contains("surfaces"))
                        {
                            // surface detected
                            lock (thisLock_)
                            {
                                newData = true;
                                surfaceData = JsonUtility.FromJson<Pupil.SurfaceData3D>(mmap.ToString());
                            }
                        }

                        if (msgType.Equals("notify.calibration.started"))
                        {
                            //Debug.LogFormat("Calibration for client {0} started: {1}", clients[turn].name, mmap.ToString());
                        }

                        if (msgType.Equals("notify.calibration.failed"))
                        {
                            //Debug.LogFormat("Calibration for client {0} failed", clients[turn].name);
                        }

                        if (msgType.Equals("notify.calibration.successful"))
                        {
                            //Debug.LogFormat("Calibration for client {0} successful", clients[turn].name);
                        }

                        if (msgType.Equals("notify.calibration.calibration_data"))
                        {
                           // Debug.LogFormat("New calibration data for client {0}: {1}", clients[turn].name, mmap.ToString());
                        }
                        if (msgType.Equals("logging.info"))
                        {
                            //Debug.LogFormat("logging info for client {0}: {1}", clients[turn].name, mmap.ToString());
                        }
                        if (msgType.Equals("calibration_routines.calibrate"))
                        {
                            //Debug.LogFormat("Calibration info for client {0}: {1}", clients[turn].name, mmap.ToString());
                        }
                    }
                    catch
                    {
                        Debug.LogWarningFormat("Failed to deserialize pupil data for client {0}", clients[turn].name);
                    }
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
            Debug.LogWarning("Failed to connect to all clients specified in config file");
        }
        NetMQConfig.ContextTerminate();
    }

    private Boolean validateIPHeader(string ip, string port)
    {
        if (ip.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Length == 4)
        {
            IPAddress ipAddr;
            if (IPAddress.TryParse(ip, out ipAddr))
            {
                if (int.Parse(port) <= 65535)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Reconnect()
    {
        Disconnect();
        Listen();
    }

    private void Disconnect()
    {
        lock (thisLock_) stop_thread_ = true;
        foreach (SubscriberSocket s in subscriberSockets.ToArray())
        {
            s.Close();
        }
        foreach (RequestSocket s in requestSockets)
        {
            s.Close();
        }

        NetMQConfig.ContextTerminate();
        Debug.Log("Network Threads terminated.");
    }

    public void StartCalibration(PupilConfiguration.PupilClient client)
    {
        Debug.Log("Starting Calibration for client " + client.name);
        if (!(clients.Contains(client) && client.is_connected))
        {
            Debug.LogWarningFormat("Client {0}:{1} ({2}) can't be calibrated because it is not connected", client.ip, client.port, client.name);
            return;
        }
        RequestSocket requestSocket = requestSockets[clients.IndexOf(client)];
        if (requestSocket == null)
        {
            Debug.LogError("Error trying to get request socket");
            return;
        }
        //sendRequest(requestSocket, new Dictionary<string, object> { { "subject", "eye_process.should_start.0" }, { "eye_id", 0 } });
        sendRequest(requestSocket, new Dictionary<string, object> { { "subject", "calibration.should_start" }, { "marker_size", "1.50" }, { "sample_duration", "50" } });

        Calibration.Instance.StartCalibration();
    }

    NetMQMessage sendRequest(RequestSocket socket, Dictionary<string, object> data)
    {
        NetMQMessage m = new NetMQMessage();
        m.Append("notify." + data["subject"]);

        using (var byteStream = new MemoryStream())
        {
            var ctx = new SerializationContext();
            ctx.CompatibilityOptions.PackerCompatibilityOptions = MsgPack.PackerCompatibilityOptions.None;
            var ser = MessagePackSerializer.Get<object>(ctx);
            ser.Pack(byteStream, data);
            m.Append(byteStream.ToArray());
        }

        socket.SendMultipartMessage(m);
        //timeout = new System.TimeSpan(0, 0, 0, 0, 200);
        //socket.TrySendMultipartMessage(timeout, m);

        NetMQMessage recievedMsg;
        recievedMsg = socket.ReceiveMultipartMessage();

        string msgType = recievedMsg[0].ConvertToString();
        if (recievedMsg.FrameCount > 1)
        {
            MsgPack.UnpackingResult<MsgPack.MessagePackObject> message = MsgPack.Unpacking.UnpackObject(recievedMsg[1].ToByteArray());
            MsgPack.MessagePackObject mmap = message.Value;
            Debug.Log("message: " + mmap.ToString());
        }

        return recievedMsg;
    }

    void OnApplicationQuit()
    {
        Disconnect();
    }

    void Update()
    {

        if (newData)
        {
            PupilConfiguration.PupilClient client = clients.Find((c) => IPHeaders[turn].Contains(c.ip));
            if (client == null)
            {
                return;
            }

            if (client.gaze_controller != null)
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
                    if (gos.on_srf)
                    {
                        client.gaze_controller.move(surfaceData, client.surface_name);
                    }
                }

            }
            newData = false;
        }
    }
}