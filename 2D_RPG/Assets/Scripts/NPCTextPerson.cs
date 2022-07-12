using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{
    public string[] messages;
    public float cooldown = 4.0f;
    public Color textColor;
    private float lastSpoken;
    public Sprite dialogueSprite;

    protected override void Start()
    {
        base.Start();
        lastSpoken = -cooldown;
        //hasSpokenIntroMessage = false;
    }

    protected override void OnCollide(Collider2D coll)
    {
        Debug.Log("Collided");
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
        GameManager.instance.ShowDialogue(messages, dialogueSprite);
    }
}

//Color.white
