using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

public class TCPJoin : MonoBehaviour
{
    [SerializeField] private int port = 1591;
    [SerializeField] private string serverIp = "127.0.0.1";

    //private TcpClient socket;
    //private Thread tcpThread;

    WebSocket ws;
    private PieceMovement movement;

    private List<Action> actionsToRun = new List<Action>();

    private void Start()
    {
        //tcpThread = new Thread(new ThreadStart(Connect));
        //tcpThread.IsBackground = true;
        //tcpThread.Start();

        movement = FindObjectOfType<PieceMovement>();

        ws = new WebSocket("ws://" + serverIp);
        Debug.Log("Connecting to server - " + serverIp + ":" + port);
        ws.Connect();
        Debug.Log("Connected to server - " + serverIp + ":" + port);

        ws.OnMessage += (sender, e) => Receive(sender, e);
        ws.OnClose += (sender, e) => OnDisconnect(sender, e);
    }

    private void Receive(object sender, MessageEventArgs e)
    {
        Debug.Log("Received : " + e.Data);

        PacketChecker packet = JsonUtility.FromJson<PacketChecker>(e.Data);

        if(packet == null)
        {
            return;
        }

        if(packet.packetType == null)
        {
            return;
        }

        if(packet.packetType == 1)
        {
            Location loc = JsonUtility.FromJson<Location>(e.Data);
            int[] startMathPos = { loc.startPos.x, loc.startPos.y, 0 };
            int[] endMathPos = { loc.endPos.x, loc.endPos.y, 0 };

            actionsToRun.Add(() => movement.requestedMove(startMathPos, endMathPos));
        }
    }

    private void OnDisconnect(object sender, CloseEventArgs e)
    {
        Debug.Log("Disconnected from server; Status: " + e.Code + "; Reason: " + e.Reason + "; WasClean: " + e.WasClean);
    }

    private void FixedUpdate()
    {
        if(actionsToRun.Count > 0)
        {
            Action actionToRun = actionsToRun[0];
            actionToRun();
            actionsToRun.RemoveAt(0);
        }
    }

    public void SendMove(int[] startPos, int[] endPos)
    {
        Location loc = new Location { packetType = (int)GameClientPackets.Movement, startPos = new Position { x = startPos[0], y = startPos[1] }, endPos = new Position { x = endPos[0], y = endPos[1] } };
        string json = JsonUtility.ToJson(loc);
        Debug.Log(json);
        ws.Send(json);
    }

    private void OnApplicationQuit()
    {
        ws.Close();
    }

    //private void Connect()
    //{
    //    try
    //    {
    //        socket = new TcpClient(serverIp, port);
    //        Debug.Log("Connected to server - " + serverIp + ":" + port);
    //        while (true)
    //        {
    //            NetworkStream stream = socket.GetStream();

    //            byte[] bytesToRead = new byte[socket.ReceiveBufferSize];
    //            int bytesRead = stream.Read(bytesToRead, 0, socket.ReceiveBufferSize);
    //            Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
    //            Console.ReadLine();
    //        }
    //    }
    //    catch (SocketException socketException)
    //    {
    //        Debug.Log("Socket exception: " + socketException);
    //    }
    //}
}
