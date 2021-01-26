using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerMessages : MonoBehaviour
{
    //[SerializeField] private string joinMessage;
    //[SerializeField] private string readyUpMessage;
    //[SerializeField] private string moveMessage;
    //[SerializeField] private string disconnectMessage;
    //[SerializeField] private string playerMessage;

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Text visibilityStatus;

    private bool visible = true;

    public void ReceiveMessage(string message)
    {
        GameObject clone = Instantiate(messagePrefab, parent);
        clone.GetComponent<Text>().text = message;

        if (visible)
        {
            clone.SetActive(true);
        }

        if(parent.childCount > 4)
        {
            Destroy(parent.GetChild(0).gameObject);
        }
    }

    public void ToggleVisibility()
    {
        visible = !visible;
        for (int i = 0; i < parent.childCount; i++)
        {
            parent.GetChild(i).gameObject.SetActive(visible);
        }
        if(visible)
        {
            visibilityStatus.text = "/\\";
        } else
        {
            visibilityStatus.text = "\\/";
        }
    }
}
