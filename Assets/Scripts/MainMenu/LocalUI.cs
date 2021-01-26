using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalUI : MonoBehaviour
{
    public void Leave()
    {
        if (FindObjectOfType<PlayerInformation>().name.Equals("")) {
            SceneManager.LoadScene(0);
        } else
        {
            SceneManager.LoadScene(1);
        }
    }
}
