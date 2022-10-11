using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnSingleplayerClick()
    {
        PlayerPrefs.DeleteAll();
        SceneTransition.instance.SceneSwitch();
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
