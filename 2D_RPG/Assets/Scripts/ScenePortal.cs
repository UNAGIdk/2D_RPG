using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ScenePortal : Collidable
{
    public string sceneName; //название сцен, на которую мы хотим переключаться
    private Animator portalHintAnimator;

    [HideInInspector] public bool teleportHintShowing;

    protected override void Start()
    {
        base.Start();
        portalHintAnimator = FindObjectOfType<PortalHint>().GetComponent<Animator>();
        Debug.Log("ScenePortal has found portal hint animator on " + portalHintAnimator.gameObject.name);
        teleportHintShowing = false;
    }

    protected override void Update()
    {
        base.Update();
        if (teleportHintShowing == true)
        {
            portalHintAnimator.ResetTrigger("hide");
            portalHintAnimator.SetTrigger("show");
        }
        else
        {
            portalHintAnimator.ResetTrigger("show");
            portalHintAnimator.SetTrigger("hide");
        }

        teleportHintShowing = false;
        Debug.Log("teleportHintShowing is now " + teleportHintShowing);
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            Debug.Log("Player collided scene portal");
            teleportHintShowing = true;

            GameManager.instance.sceneName = sceneName;

            if (Input.GetKeyDown(KeyCode.F) == true)
            {
                teleportHintShowing = false;
                ToDungeon();
            }
        }
    }

    private void ToDungeon()
    {
        //телепортировать игрока в dungeon
        GameManager.instance.SaveState();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
