using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class TCPJoin : MonoBehaviour
{
    [SerializeField] private int port = 1591;
    [SerializeField] private string serverIp = "127.0.0.1";

    [SerializeField] private GameObject readyButton;

    //private TcpClient socket;
    //private Thread tcpThread;

    private WebSocket ws;
    private PieceMovement movement;

    private List<Action> actionsToRun = new List<Action>();

    private PlayerInformation info;

    private void Start()
    {
        info = FindObjectOfType<PlayerInformation>();

        movement = FindObjectOfType<PieceMovement>();

        ws = FindObjectOfType<Network>().GetWebSocket();

        ws.OnMessage += (sender, e) => Receive(sender, e);

        JoinRequest request = new JoinRequest();
        request.packetType = (int)GameClientPackets.JoinRequest;
        request.username = info.username;
        request.lobbyCode = info.currentGame;
        Send(request);
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

        switch(packet.packetType)
        {
            case (int)GameServerPackets.ServerInfo:
                ServerData serverData = JsonUtility.FromJson<ServerData>(e.Data);
                OnConnect(serverData);
                break;
            case (int)GameServerPackets.StartGame:
                StartGame startData = JsonUtility.FromJson<StartGame>(e.Data);
                StartGame(startData);
                break;
            case (int)GameServerPackets.Movement:
                Location loc = JsonUtility.FromJson<Location>(e.Data);
                Move(loc);
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

        }
        else
        {
            SceneManager.LoadScene(0);
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
        Location loc = new Location { packetType = (int)GameClientPackets.Movement, username = info.username, startPos = new Position { x = startPos[0], y = startPos[1] }, endPos = new Position { x = endPos[0], y = endPos[1] } };
        Send(loc);
    }

    public void ReadyUp()
    {
        readyButton.SetActive(false);
        ReadyUp request = new ReadyUp();
        request.packetType = (int)GameClientPackets.ReadyUp;
        request.username = info.username;
        request.lobbyCode = info.currentGame;
        string json = JsonUtility.ToJson(request);
        ws.Send(json);
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
