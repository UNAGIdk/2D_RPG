using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ScenePortal : Collidable
{
    public string portalSceneName; //название сцен, на которую мы хотим переключаться
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
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player1" || coll.name == "Player2(Clone)")
        {
            Debug.Log("Player collided scene portal");
            teleportHintShowing = true;

            GameManager.instance.sceneName = portalSceneName;

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
        SceneTransition.instance.SceneSwitch();
    }
}
