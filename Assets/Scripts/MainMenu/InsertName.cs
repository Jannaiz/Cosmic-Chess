using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsertName : MonoBehaviour
{
    private void Start()
    {
        string username = FindObjectOfType<PlayerInformation>().username;
        GetComponentInParent<InputField>().text = username;
        GetComponent<Text>().text = username;
    }
}
