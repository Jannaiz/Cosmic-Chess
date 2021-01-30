using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPHost : MonoBehaviour
{
    //[SerializeField] private int port = 1591;
    //[SerializeField] private string serverIp = "127.0.0.1";

    //private Thread tcpThread;

    //private TcpListener listener;

    //private void Start()
    //{
    //    tcpThread = new Thread(new ThreadStart(StartServer));
    //    tcpThread.IsBackground = true;
    //    tcpThread.Start();
    //}

    //private void StartServer()
    //{
    //    try
    //    {
    //        IPAddress localAddress = IPAddress.Parse(serverIp);
    //        listener = new TcpListener(localAddress, port);
    //        Debug.Log("Server starting - " + localAddress.ToString() + ":" + port);
    //        listener.Start();
    //        Debug.Log("Server started");

    //        while (true)
    //        {
    //            TcpClient client = listener.AcceptTcpClient();

    //            Debug.Log("New connection - " + client.Client.LocalEndPoint.ToString());

    //            NetworkStream nwStream = client.GetStream();
    //            byte[] buffer = new byte[client.ReceiveBufferSize];

    //            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

    //            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
    //            Debug.Log("Received : " + dataReceived);

    //            Debug.Log("Sending back : " + dataReceived);
    //            nwStream.Write(buffer, 0, bytesRead);
    //        }
    //    }
    //    catch (SocketException socketException)
    //    {
    //        Debug.Log("SocketException " + socketException.ToString());
    //    }
    //}

    //private void Update()
    //{
        
    //}
}
