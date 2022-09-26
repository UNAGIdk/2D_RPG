using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NPCTextPerson : Collidable
{
    public BoxCollider2D NPCDialogueTrigger;

    public string[] messages;
    [HideInInspector] public bool hasSpokenMessages;
    private int pagesCounter = 0;

    public string[] askNameMessage;
    [HideInInspector] public bool hasAskedName;
    private string playerNameResponse;

    public Animator NPCMessageInputAnimator;
    public InputField NPCMessageInputField;
    
    
    protected override void Start()
    {
        base.Start();
        hasSpokenMessages = false;
        if (GameManager.instance.photonManager.playingMultiplayer == false)
        {
            if (PlayerPrefs.GetString("PlayerNameResponse") == "" || PlayerPrefs.HasKey("PlayerNameResponse") == false)
                hasAskedName = false;
            else
            {
                hasAskedName = true;
                messages[0] = "�� ������, " + PlayerPrefs.GetString("PlayerNameResponse") + "!";
                messages[messages.Length - 1] = "�� ���� ��� ������ �������������. �����, " + PlayerPrefs.GetString("PlayerNameResponse") + "!";
            }
        }
        else
        {
            messages[0] = "�� ������, " + PhotonNetwork.NickName + "!";
            messages[messages.Length - 1] = "�� ���� ��� ������ �������������. �����, " + PhotonNetwork.NickName + "!";
        }       
    }

    protected override void Update()
    {
        base.Update();
            if (GameManager.instance.dialogueManager.dialogueRunning == true)
            {
                GameManager.instance.ProhibitPlayerMoving();
                GameManager.instance.ProhibitPlayerSwing();
                if (Input.GetKeyDown(KeyCode.Mouse0) == true || Input.GetKeyDown(KeyCode.Space) == true)
                {
                    if (hasAskedName == true || GameManager.instance.photonManager.playingMultiplayer == true)
                        if (pagesCounter < messages.Length - 1) //���� ������ -1 �� ��������� ������ �� ������� �������
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

    public void OnCollisionWithTrigger()
    {
        if(GameManager.instance.photonManager.playingMultiplayer == false)
        {
            if (hasAskedName == false)
            {
                GameManager.instance.ShowDialoguePage(askNameMessage[0], gameObject.GetComponent<SpriteRenderer>().sprite);
                NPCMessageInputAnimator.SetTrigger("show");
                NPCMessageInputAnimator.ResetTrigger("hide");
            }
            else if (hasSpokenMessages == false)
                GameManager.instance.ShowDialoguePage(messages[pagesCounter], gameObject.GetComponent<SpriteRenderer>().sprite); //�������� ������ �������� ��� �������
        }
        else if (hasSpokenMessages == false)
            GameManager.instance.ShowDialoguePage(messages[pagesCounter], gameObject.GetComponent<SpriteRenderer>().sprite); //�������� ������ �������� ��� �������
    }

    public void ApproveNameButton()
    {
        playerNameResponse = NPCMessageInputField.text;
        PlayerPrefs.SetString("PlayerNameResponse", playerNameResponse);
        Debug.Log("Player name set to " + PlayerPrefs.GetString("PlayerNameResponse"));
        hasAskedName = true;
        NPCMessageInputAnimator.SetTrigger("hide");
        NPCMessageInputAnimator.ResetTrigger("show");
        messages[0] = "� �� ���� ��� �� ������, �� �����, ������, " + PlayerPrefs.GetString("PlayerNameResponse") + "!";
        messages[messages.Length - 1] = "�� ���� ��� ������ �������������. ����� �� ������ ������, " + PlayerPrefs.GetString("PlayerNameResponse") + "!";
        //if (GameManager.instance.playingMultiplayer == true)
        //PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerNameResponse");
    }

}