using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject playOnline;

    [SerializeField] private Text code;
    [SerializeField] private Text errorText;

    [SerializeField] private int localScene;
    [SerializeField] private int hostScene;
    [SerializeField] private int lobbyScene;
    [SerializeField] private int joinScene;

    [SerializeField] private Toggle isPublicToggle;

    private PlayerInformation info;
    private Network network;

    private void Start()
    {
        network = FindObjectOfType<Network>();
        info = FindObjectOfType<PlayerInformation>();
    }

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
        network.HostGame(isPublicToggle.isOn);
    }

    public void ShowLobbys()
    {
        SceneManager.LoadScene(lobbyScene);
    }

    public void Join()
    {
        if (CheckCode())
        {
            info.currentGame = code.text;
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

    private bool CheckCode()
    {
        if (code.text.Equals(""))
        {
            errorText.text = "Invalid code";
            return false;
        }
        else
        {
            return true;
        }
    }
}
