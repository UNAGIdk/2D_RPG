using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPortal : Collidable
{
    public Transform portalToTeleport;
    private Animator portalHintAnimator;

    [HideInInspector] public bool teleportHintShowing;

    protected override void Start()
    {
        base.Start();
        portalHintAnimator = FindObjectOfType<PortalHint>().GetComponent<Animator>();
        Debug.Log("LocalPortal has found portal hint animator on " + portalHintAnimator.gameObject.name);
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
        if (coll.name == "Player")
        {
            Debug.Log("Player collided local portal");
            if (teleportHintShowing == false)
            {
                teleportHintShowing = true;
            }

            if (Input.GetKeyDown(KeyCode.F) == true)
            {
                teleportHintShowing = false;
                Teleport();
            }
        }
    }

    private void Teleport()
    {
        GameManager.instance.player.transform.localPosition = portalToTeleport.localPosition;
    }
}
