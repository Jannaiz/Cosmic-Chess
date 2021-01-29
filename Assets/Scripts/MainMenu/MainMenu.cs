using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject playOnline;
    [SerializeField] private GameObject hostGame;

    [SerializeField] private Text code;
    [SerializeField] private Text errorText;

    [SerializeField] private int localScene;
    [SerializeField] private int hostScene;
    [SerializeField] private int lobbyScene;
    [SerializeField] private int joinScene;

    [SerializeField] private Toggle isPublicToggle;

    private PlayerInformation info;
    private Network network;

    [SerializeField] private int map;
    [SerializeField] private int playerAmount;
    [SerializeField] private int dimensionAmount;

    [SerializeField] private List<GameObject> mapSelections;
    [SerializeField] private List<GameObject> playerSelections;
    [SerializeField] private List<GameObject> dimensionSelections;

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
        hostGame.SetActive(true);
        //network.HostGame(isPublicToggle.isOn);
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

    public void SelectMap(int map)
    {
        this.map = map;
        for(int i = 0; i<mapSelections.Count; i++)
        {
            mapSelections[i].SetActive(i == map);
        }
    }

    public void SelectPlayers(int amount)
    {
        this.playerAmount = amount;
        for (int i = 0; i < playerSelections.Count; i++)
        {
            playerSelections[i].SetActive(i == amount);
        }
    }

    public void SelectDimensions(int amount)
    {
        this.dimensionAmount = amount;
        for (int i = 0; i < dimensionSelections.Count; i++)
        {
            dimensionSelections[i].SetActive(i == amount);
        }
    }

    public void StartHost()
    {
        network.HostGame(isPublicToggle.isOn, map, playerAmount, dimensionAmount);
    }

    public void Back()
    {
        playOnline.SetActive(false);
        settings.SetActive(false);
    }

    public void HostBack()
    {
        hostGame.SetActive(false);
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
