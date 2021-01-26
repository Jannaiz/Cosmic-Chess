using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresetChatMessages : MonoBehaviour
{
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private List<string> preGameMessages;
    [SerializeField] private List<string> midGameMessages;
    [SerializeField] private List<string> endGameMessages;

    private TCPJoin network;

    private int phase = 0;

    private bool visible = false;

    private void Start()
    {
        network = FindObjectOfType<TCPJoin>();
        UpdateMessages();
    }

    public void UpdateMessages()
    {
        for (int i = transform.childCount-1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        switch(phase)
        {
            case 0:
                foreach(string message in preGameMessages)
                {
                    GameObject clone = Instantiate(messagePrefab, transform);
                    clone.GetComponentInChildren<Text>().text = message;
                    clone.GetComponent<Button>().onClick.AddListener(() => Send(message));
                }
                break;
            case 1:
                foreach (string message in midGameMessages)
                {
                    GameObject clone = Instantiate(messagePrefab, transform);
                    clone.GetComponentInChildren<Text>().text = message;
                    clone.GetComponent<Button>().onClick.AddListener(() => Send(message));
                }
                break;
            case 2:
                foreach (string message in endGameMessages)
                {
                    GameObject clone = Instantiate(messagePrefab, transform);
                    clone.GetComponentInChildren<Text>().text = message;
                    clone.GetComponent<Button>().onClick.AddListener(() => Send(message));
                }
                break;
        }
        if (visible)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private void Send(string message)
    {
        network.SendChat(message);
    }

    public void UpdateMessages(int newPhase)
    {
        phase = newPhase;
        UpdateMessages();
    }

    public void ToggleVisibility()
    {
        visible = !visible;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(visible);
        }
    }
}
