using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalUI : MonoBehaviour
{
    public void Leave()
    {
        SceneManager.LoadScene(0);
    }
}
