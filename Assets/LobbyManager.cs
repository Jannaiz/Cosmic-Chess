using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private string serverIp;

    WebSocket ws;

    PlayerInformation info;

    [SerializeField] private int joinScene;

    private void Start()
    {
        info = FindObjectOfType<PlayerInformation>();

        ws = new WebSocket("ws://" + serverIp);
        Debug.Log("Connecting to server - " + serverIp);
        ws.Connect();
        Debug.Log("Connected to server - " + serverIp);

        ws.OnMessage += (sender, e) => Receive(sender, e);
        ws.OnClose += (sender, e) => OnDisconnect(sender, e);

        ClientHandshake handshake = new ClientHandshake();
        handshake.packetType = (int)GameClientPackets.Handshake;
        handshake.username = info.username;
        string json = JsonUtility.ToJson(handshake);
        ws.Send(json);
    }

    private void Receive(object sender, MessageEventArgs e)
    {
        Debug.Log("Received : " + e.Data);

        PacketChecker packet = JsonUtility.FromJson<PacketChecker>(e.Data);

        if (packet == null)
        {
            return;
        }

        if (packet.packetType == null)
        {
            return;
        }
        
        if(packet.packetType == (int)GameServerPackets.Handshake)
        {
            ServerHandshake shake = new ServerHandshake();
            Debug.Log(shake.welcomeMessage);

            LobbyRequest request = new LobbyRequest();
            request.packetType = (int)GameClientPackets.LobbyRequest;
            request.username = info.username;
            request.isPublic = 1;
            string json = JsonUtility.ToJson(request);
            ws.Send(json);
        }
    }

    private void OnDisconnect(object sender, CloseEventArgs e)
    {
        Debug.Log("Disconnected from server; Status: " + e.Code + "; Reason: " + e.Reason + "; WasClean: " + e.WasClean);
    }

    private void OnApplicationQuit()
    {
        ws.Close();
    }

    public void BackToMainMenu()
    {
        ws.Close();
        SceneManager.LoadScene(0);
    }

    public void Join(string code)
    {
        info.currentGame = code;
        SceneManager.LoadScene(joinScene);
    }
}
