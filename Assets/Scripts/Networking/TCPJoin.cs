using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class TCPJoin : MonoBehaviour
{
    [SerializeField] private int port = 1591;
    [SerializeField] private string serverIp = "127.0.0.1";

    [SerializeField] private GameObject readyButton;

    [SerializeField] private ServerMessages messages;

    [SerializeField] private Text lobbyCodeText;

    //private TcpClient socket;
    //private Thread tcpThread;

    private WebSocket ws;
    private PieceMovement movement;
    private PlayerInformation info;
    private PresetChatMessages playerMessages;

    private List<Action> actionsToRun = new List<Action>();

    private void Start()
    {
        info = FindObjectOfType<PlayerInformation>();

        lobbyCodeText.text = "Code: " + info.currentGame;

        movement = FindObjectOfType<PieceMovement>();
        playerMessages = FindObjectOfType<PresetChatMessages>();

        ws = FindObjectOfType<Network>().GetWebSocket();

        ws.OnMessage += (sender, e) => Receive(sender, e);

        if (!info.connected)
        {
            JoinRequest request = new JoinRequest();
            Header header = new Header();
            header.packetType = (int)GameClientPackets.JoinRequest;
            header.username = info.username;
            header.sessionId = info.sessionId;
            request.header = header;
            request.lobbyCode = info.currentGame;
            Send(request);
        }
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
            case (int)GameServerPackets.ServerInfo:
                ServerData serverData = JsonUtility.FromJson<ServerData>(e.Data);
                actionsToRun.Add(() => OnConnect(serverData));
                break;
            case (int)GameServerPackets.StartGame:
                StartGame startData = JsonUtility.FromJson<StartGame>(e.Data);
                actionsToRun.Add(() => StartGame(startData));
                break;
            case (int)GameServerPackets.Movement:
                Location loc = JsonUtility.FromJson<Location>(e.Data);
                Move(loc);
                break;
            case (int)GameServerPackets.ServerMessage:
                ServerMessage message = JsonUtility.FromJson<ServerMessage>(e.Data);
                actionsToRun.Add(() => messages.ReceiveMessage(message.message));
                break;
        }
    }

    private void Move(Location loc)
    {
        int[] startMathPos = { loc.startPos.x, loc.startPos.y, 0 };
        int[] endMathPos = { loc.endPos.x, loc.endPos.y, 0 };

        actionsToRun.Add(() => movement.requestedMove(startMathPos, endMathPos));
    }

    private void StartGame(StartGame data)
    {
        playerMessages.UpdateMessages(1);
        if(data.color == 0)
        {
            movement.white = true;
        } else
        {
            movement.white = false;
        }
    }

    private void OnConnect(ServerData data)
    {
        if (data.succes == 1)
        {
            messages.ReceiveMessage("You joined the game with code " + info.currentGame + "!");
        }
        else
        {
            SceneManager.LoadScene(1);
        }
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
        Location loc = new Location { header = new Header { packetType = (int)GameClientPackets.Movement, username = info.username, sessionId = info.sessionId }, startPos = new Position { x = startPos[0], y = startPos[1] }, endPos = new Position { x = endPos[0], y = endPos[1] } };
        Send(loc);
    }

    public void ReadyUp()
    {
        readyButton.SetActive(false);
        ReadyUp request = new ReadyUp();
        Header header = new Header();
        header.packetType = (int)GameClientPackets.ReadyUp;
        header.username = info.username;
        header.sessionId = info.sessionId;
        request.header = header;
        request.lobbyCode = info.currentGame;
        string json = JsonUtility.ToJson(request);
        ws.Send(json);
        messages.ReceiveMessage("You are ready!");
    }

    public void SendChat(string message)
    {
        PlayerMessage pMessage = new PlayerMessage();
        Header header = new Header();
        header.packetType = (int)GameClientPackets.PlayerMessage;
        header.username = info.username;
        header.sessionId = info.sessionId;
        pMessage.header = header;
        pMessage.message = message;
        string json = JsonUtility.ToJson(pMessage);
        ws.Send(json);
        messages.ReceiveMessage(info.username + ": " + message);
    }

    public void Disconnect()
    {
        SceneManager.LoadScene(1);
    }

    private void Send(object data)
    {
        string json = JsonUtility.ToJson(data);
        ws.Send(json);
    }
}
