using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class Network : MonoBehaviour
{
    [SerializeField] private string serverIp;

    [SerializeField] private int stayAliveTimer;

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

        ws.OnMessage += (sender, e) => Receive(sender, e);
        ws.OnClose += (sender, e) => OnDisconnect(sender, e);

        ClientHandshake handshake = new ClientHandshake();
        HandshakeHeader header = new HandshakeHeader();
        header.packetType = (int)GameClientPackets.Handshake;
        header.username = info.username;
        handshake.header = header;
        string json = JsonUtility.ToJson(handshake);
        ws.Send(json);

        StartCoroutine(StayAlive());
    }

    private void Receive(object sender, MessageEventArgs e)
    {
        HeaderChecker packet = JsonUtility.FromJson<HeaderChecker>(e.Data);

        if (packet == null)
        {
            return;
        }

        if (packet.header.packetType == null)
        {
            return;
        }

        switch(packet.header.packetType)
        {
            case (int)GameServerPackets.Handshake:
                ServerHandshake shake = JsonUtility.FromJson<ServerHandshake>(e.Data);
                info.sessionId = shake.header.sessionId;
                Debug.Log(shake.welcomeMessage);
                break;
            case (int)GameServerPackets.GetLobbyCode:
                CreateLobby lobby = JsonUtility.FromJson<CreateLobby>(e.Data);
                info.currentGame = lobby.lobbyCode;
                actionsToRun.Add(() => SceneManager.LoadScene(joinScene));
                break;
            case (int)GameServerPackets.ErrorMessage:
                Disconnect();
                Application.Quit();
                break;
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
        Header header = new Header();
        header.packetType = (int)GameClientPackets.CreateLobby;
        header.username = info.username;
        header.sessionId = info.sessionId;
        request.header = header;
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

    private IEnumerator StayAlive()
    {
        yield return new WaitForSeconds(stayAliveTimer);
        StayAlive message = new StayAlive();
        Header header = new Header();
        header.packetType = (int)GameClientPackets.StayAlive;
        header.username = info.username;
        header.sessionId = info.sessionId;
        message.header = header;
        string json = JsonUtility.ToJson(message);
        ws.Send(json);
        StartCoroutine(StayAlive());
    }
}
