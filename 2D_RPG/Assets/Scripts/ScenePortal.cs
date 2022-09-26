using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ScenePortal : Collidable
{
    public string portalSceneName; //название сцен, на которую мы хотим переключаться
    private Animator portalHintAnimator;

    [HideInInspector] public bool teleportHintShowing;
    public bool isEndingPortal;
    public int endingLevelNumber;

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

            SceneTransition.instance.sceneToGo = portalSceneName;

            if (Input.GetKeyDown(KeyCode.F) == true)
            {
                teleportHintShowing = false;
                if (GameManager.instance.photonManager.playingMultiplayer == false)
                    PlayerPrefs.SetString("LevelsPassed", endingLevelNumber.ToString());

                if(GameManager.instance.photonManager.playingMultiplayer == true)
                    GameManager.instance.photonManager.UpdateLevelsPassedRpcTrigger(endingLevelNumber);

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
