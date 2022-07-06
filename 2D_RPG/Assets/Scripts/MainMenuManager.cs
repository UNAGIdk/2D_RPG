using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnToEntranceClick()
    {
        SceneManager.LoadScene("Entrance");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
