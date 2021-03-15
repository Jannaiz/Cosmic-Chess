using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private PlayerInformation info;

    [SerializeField] private Text username;
    [SerializeField] private Text errorText;

    [SerializeField] private int offlineScene;

    public void StartGame()
    {
        if (CheckUsername())
        {
            info.username = username.text;
            SceneManager.LoadScene(1);
        }
    }

    public void PlayOffline()
    {
        SceneManager.LoadScene(offlineScene);
    }

    private bool CheckUsername()
    {
        if (username.text.Equals(""))
        {
            errorText.text = "Invalid username";
            return false;
        }
        else
        {
            return true;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
