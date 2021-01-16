using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject playOnline;

    [SerializeField] private Text username;
    [SerializeField] private Text errorText;

    [SerializeField] private int localScene;
    [SerializeField] private int hostScene;
    [SerializeField] private int joinScene;

    public void LocalMultiplayer()
    {
        SceneManager.LoadScene(localScene);
    }

    public void PlayOnline()
    {
        playOnline.SetActive(true);
    }

    public void Host()
    {
        if (CheckUsername())
        {
            SceneManager.LoadScene(hostScene);
        }
    }

    public void Join()
    {
        if (CheckUsername())
        {
            SceneManager.LoadScene(joinScene);
        }
    }

    public void Settings()
    {
        settings.SetActive(true);
    }

    public void Back()
    {
        playOnline.SetActive(false);
        settings.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private bool CheckUsername()
    {
        if(username.text.Equals(""))
        {
            errorText.text = "Invalid username";
            return false;
        } else
        {
            return true;
        }
    }
}
