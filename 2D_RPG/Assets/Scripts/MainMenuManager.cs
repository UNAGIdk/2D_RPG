using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnToEntranceClick()
    {
        SceneManager.LoadScene("Entrance", LoadSceneMode.Single);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnOptionsClick()
    {
        SceneManager.LoadScene("Options", LoadSceneMode.Single);
    }
}
