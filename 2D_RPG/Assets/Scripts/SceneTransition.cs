using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private Animator sceneTransitionAnimator;
    public static SceneTransition instance;
    private string sceneToGo;
    private PhotonManager photonManager;
    void Start()
    {
        sceneTransitionAnimator = GetComponent<Animator>(); //this.gameObject.
        Debug.Log("sceneTransitionAnimator has found animator on " + sceneTransitionAnimator.gameObject.name);
        instance = this;
        photonManager = FindObjectOfType<PhotonManager>();
        Debug.Log("photonManager has found PhotonManager on " + photonManager.gameObject.name);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            sceneToGo = GameManager.instance.sceneName;
        else
            sceneToGo = "Entrance";
    }

    public void SceneSwitch()
    {
        instance.sceneTransitionAnimator.SetTrigger("sceneEnd");
        //в конце анимации автоматически вызывается метод OnAnimationOver()
    }

    public void SceneTransitionOnSceneLoaded()
    {
        instance.sceneTransitionAnimator.SetTrigger("sceneStart");
    }

    public void OnAnimationOver()
    {
        if (photonManager.playingMultiplayer == false)
            SceneManager.LoadScene(sceneToGo, LoadSceneMode.Single);
        else
            photonManager.PhotonLoadScene(sceneToGo);
    }
}
