using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using WebSocketSharp;

public class TCPJoin : MonoBehaviour
{
    [SerializeField] private int port = 1591;
    [SerializeField] private string serverIp = "127.0.0.1";

    //private TcpClient socket;
    //private Thread tcpThread;

    WebSocket ws;

    private void Start()
    {
        //tcpThread = new Thread(new ThreadStart(Connect));
        //tcpThread.IsBackground = true;
        //tcpThread.Start();

        ws = new WebSocket("ws://" + serverIp + ":" + port);
        Debug.Log("Connecting to server - " + serverIp + ":" + port);
        ws.Connect();
        Debug.Log("Connected to server - " + serverIp + ":" + port);

        ws.OnMessage += (sender, e) => Receive(sender, e);
    }

    private void Receive(object sender, MessageEventArgs e)
    {
        Debug.Log("Received : " + Encoding.ASCII.GetString(e.RawData));
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
