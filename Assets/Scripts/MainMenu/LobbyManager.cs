using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private string serverIp;

    private WebSocket ws;

    private PlayerInformation info;

    [SerializeField] private int joinScene;

    [SerializeField] private Transform buttonParent;
    [SerializeField] private GameObject joinButton;

    private List<Action> actionsToRun = new List<Action>();

    private void Start()
    {
        info = FindObjectOfType<PlayerInformation>();

        ws = FindObjectOfType<Network>().GetWebSocket();

        ws.OnMessage += (sender, e) => Receive(sender, e);

        GetPublicLobbys request = new GetPublicLobbys();
        request.packetType = (int)GameClientPackets.GetPublicLobbys;
        request.username = info.username;
        string json = JsonUtility.ToJson(request);
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

        Debug.Log("Lobby man " + packet.packetType);

        if(packet.packetType == (int)GameServerPackets.ReceivePublicLobbys)
        {
            LobbyAnswer lobbys = JsonUtility.FromJson<LobbyAnswer>(e.Data);
            foreach(string code in lobbys.lobbyCodes)
            {
                actionsToRun.Add(() => AddButton(code));
            }
        }
    }

    private void FixedUpdate()
    {
        if (actionsToRun.Count > 0)
        {
            actionsToRun[0]();
            actionsToRun.RemoveAt(0);
        }
    }

    private void AddButton(string code)
    {
        GameObject clone = Instantiate(joinButton, buttonParent);
        clone.GetComponent<Button>().onClick.AddListener(() => Join(code));
        clone.GetComponentInChildren<Text>().text = code;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void Join(string code)
    {
        Debug.Log(code);
        info.currentGame = code;
        SceneManager.LoadScene(joinScene);
    }
}
