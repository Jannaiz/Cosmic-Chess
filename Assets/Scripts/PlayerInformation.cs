﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    public string username;
    public string currentGame;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(FindObjectsOfType<PlayerInformation>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
