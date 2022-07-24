using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{
    public string[] messages;
    private int pagesCounter = 0;
    private bool hasSpokenMessages;
    
    protected override void Start()
    {
        base.Start();
        hasSpokenMessages = false;
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

    protected override void OnCollide(Collider2D coll)
    {
        Debug.Log("Collided");
        if(hasSpokenMessages == false)
            GameManager.instance.ShowDialoguePage(messages[pagesCounter], gameObject.GetComponent<SpriteRenderer>().sprite); //�������� ������ �������� ��� �������
    }
}


/*if(!hasSpokenIntroMessage) //������ �� ��� ������ ���������?
        {
            if (Time.time - lastSpoken > cooldown)
            {
                lastSpoken = Time.time;
                GameManager.instance.ShowText(messages[0], 20, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, cooldown); //������ �������� ��� ���� ����� �������� ����� ����
            }
            hasSpokenIntroMessage = true;
        }
        else
        {
            if (Time.time - lastSpoken > cooldown)
            {
                lastSpoken = Time.time;
                GameManager.instance.ShowText(messages[Random.Range(1, messages.Length)], 20, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, cooldown); //������ �������� ��� ���� ����� �������� ����� ����
            }
        }*/
//Color.white
