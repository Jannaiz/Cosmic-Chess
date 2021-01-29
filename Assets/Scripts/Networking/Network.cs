using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class Network : MonoBehaviour
{
    [SerializeField] private string serverIp;

    private WebSocket ws;

    private PlayerInformation info;

    [SerializeField] private int joinScene;

    private List<Action> actionsToRun = new List<Action>();

    private void Awake()
    {
        if(FindObjectsOfType<Network>().Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

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
        PacketChecker packet = JsonUtility.FromJson<PacketChecker>(e.Data);

        if (packet == null)
        {
            return;
        }

        if (packet.packetType == null)
        {
            return;
        }
        Debug.Log("main network : " + packet.packetType);

        if (packet.packetType == (int)GameServerPackets.Handshake)
        {
            ServerHandshake shake = JsonUtility.FromJson<ServerHandshake>(e.Data);
            Debug.Log(shake.welcomeMessage);
        } else if(packet.packetType == (int)GameServerPackets.GetLobbyCode)
        {
            CreateLobby lobby = JsonUtility.FromJson<CreateLobby>(e.Data);
            info.currentGame = lobby.lobbyCode;
            actionsToRun.Add(() => SceneManager.LoadScene(joinScene));
        } else if(packet.packetType == (int)GameServerPackets.ErrorMessage)
        {
            Disconnect();
            Application.Quit();
        }
    }

    private void OnDisconnect(object sender, CloseEventArgs e)
    {
        Debug.Log("Disconnected from server; Status: " + e.Code + "; Reason: " + e.Reason + "; WasClean: " + e.WasClean);
    }

    private void Disconnect()
    {
        ws.Close();
    }

    private void FixedUpdate()
    {
        if(actionsToRun.Count > 0)
        {
            actionsToRun[0]();
            actionsToRun.RemoveAt(0);
        }
    }

    public WebSocket GetWebSocket()
    {
        return ws;
    }

    public void HostGame(bool isPublic, int map, int playerAmount, int dimensionAmount)
    {
        LobbyRequest request = new LobbyRequest();
        request.packetType = (int)GameClientPackets.CreateLobby;
        request.username = info.username;
        if (isPublic)
        {
            request.isPublic = 1;
        } else
        {
            request.isPublic = 0;
        }
        request.map = map;
        request.playerAmount = playerAmount;
        request.dimensionAmount = dimensionAmount;
        string json = JsonUtility.ToJson(request);
        ws.Send(json);
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}
