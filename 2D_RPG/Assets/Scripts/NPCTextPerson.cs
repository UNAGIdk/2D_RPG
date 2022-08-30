using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTextPerson : Collidable
{
    public string[] messages;
    private bool hasSpokenMessages;
    private int pagesCounter = 0;

    public string[] askNameMessage;
    private bool hasAskedName;
    private string playerNameResponse;

    public Animator NPCMessageInputAnimator;
    public InputField NPCMessageInputField;
    
    
    protected override void Start()
    {
        base.Start();

       hasSpokenMessages = false;
        if (PlayerPrefs.GetString("PlayerNameResponse") != null)
            hasAskedName = false;
        else
        {
            hasAskedName = true;
            messages[0] = "Welcome, " + PlayerPrefs.GetString("PlayerNameResponse") + "!";
        }
        Debug.Log(PlayerPrefs.GetString("PlayerNameResponse"));
    }

    protected override void Update()
    {
        base.Update();
        if(GameManager.instance.dialogueManager.dialogueRunning == true)
        {
            GameManager.instance.ProhibitPlayerMoving();
            GameManager.instance.ProhibitPlayerSwing();
            if(Input.GetKeyDown(KeyCode.Mouse0) == true || Input.GetKeyDown(KeyCode.Space) == true)
            {
                if(hasAskedName == true)
                    if (pagesCounter < messages.Length - 1) //если убрать -1 то постоянно выхожу за пределы массива
                    {
                        pagesCounter++;
                        GameManager.instance.ShowDialoguePage(messages[pagesCounter], gameObject.GetComponent<SpriteRenderer>().sprite);
                    }
                    else
                    {
                        GameManager.instance.dialogueManager.ClearDialogue();
                        pagesCounter = 0;
                        hasSpokenMessages = true;
                        GameManager.instance.AllowPlayerMoving();
                        GameManager.instance.AllowPlayerSwing();
                    }
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        Debug.Log("Collided");
        Debug.Log("hasAskedName is " + hasAskedName);
        if(hasAskedName == false)
        {
            GameManager.instance.ShowDialoguePage(askNameMessage[0], gameObject.GetComponent<SpriteRenderer>().sprite);
            NPCMessageInputAnimator.SetTrigger("show");
            NPCMessageInputAnimator.ResetTrigger("hide");
        }
        else if (hasSpokenMessages == false)
            GameManager.instance.ShowDialoguePage(messages[pagesCounter], gameObject.GetComponent<SpriteRenderer>().sprite); //показать первую страницу при касании
    }
    public void ApproveNameButton()
    {
        playerNameResponse = NPCMessageInputField.text;
        PlayerPrefs.SetString("PlayerNameResponse", playerNameResponse);
        Debug.Log("Player name set to " + PlayerPrefs.GetString("PlayerNameResponse"));
        hasAskedName = true;
        NPCMessageInputAnimator.SetTrigger("hide");
        NPCMessageInputAnimator.ResetTrigger("show");
        messages[0] = "Welcome, " + PlayerPrefs.GetString("PlayerNameResponse") + "!";
    }

}