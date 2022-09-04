using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPortal : Collidable
{
    public Transform portalToTeleport;
    //private GameManager gameManager;

    public Sprite teleportButtonSprite;
    public GameObject teleportButtonPrefab;
    public Transform portalTransform;

    private GameObject teleportButton;
    [HideInInspector] public bool teleportHintShowing;

    protected override void Start()
    {
        base.Start();
        teleportHintShowing = false;
        //gameManager = FindObjectOfType<GameManager>();
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            Debug.Log("Collided local portal");
            if (teleportHintShowing == false)
            {
                teleportButton = Instantiate(teleportButtonPrefab, portalTransform);
                teleportButton.transform.localPosition = new Vector3(0, 4.53f, 0);
                //teleportText = Instantiate(teleportTextPrefab, portalTransform);

                //teleportButton.GetComponent<SpriteRenderer>().sprite = teleportButtonSprite;
                //teleportText.GetComponent<Text>().text = "to go into " + sceneName;

                teleportHintShowing = true;
            }

            if (Input.GetKeyDown(KeyCode.F) == true)
            {
                ClearPortalText();
                Teleport();
            }
        }
        else
        {
            ClearPortalText();
        }
    }

    private void Teleport()
    {
        GameManager.instance.player.transform.localPosition = portalToTeleport.localPosition;
    }

    private void ClearPortalText()
    {
        Destroy(teleportButton);
        teleportHintShowing = false;
    }
}
