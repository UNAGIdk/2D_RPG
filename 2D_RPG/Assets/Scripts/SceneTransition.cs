using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private Animator sceneTransitionAnimator;
    public static SceneTransition instance;
    [HideInInspector] public string sceneToGo;
    private PhotonManager photonManager;
    [HideInInspector] public bool player2Joined = false;
    void Start()
    {
        sceneTransitionAnimator = GetComponent<Animator>(); //this.gameObject.
        Debug.Log("sceneTransitionAnimator has found animator on " + sceneTransitionAnimator.gameObject.name);
        instance = this;
        photonManager = FindObjectOfType<PhotonManager>();
        Debug.Log("photonManager has found PhotonManager on " + photonManager.gameObject.name);
        if(SceneManager.GetActiveScene().name == "MainMenu")
            sceneToGo = "Entrance";
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            GameManager.instance.sceneName = sceneToGo;
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
        {
            Debug.Log(sceneToGo);
            SceneManager.LoadScene(sceneToGo, LoadSceneMode.Single);
        }
        else
        {
            Debug.Log(sceneToGo);
            photonManager.PhotonLoadScene(sceneToGo);
        }
    }
}
