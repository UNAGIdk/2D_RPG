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
        /*if(!hasSpokenIntroMessage) //сказал ли уже первое сообщение?
        {
            if (Time.time - lastSpoken > cooldown)
            {
                lastSpoken = Time.time;
                GameManager.instance.ShowText(messages[0], 20, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, cooldown); //вектор добавлен для того чтобы сдвинуть текст выше
            }
            hasSpokenIntroMessage = true;
        }
        else
        {
            if (Time.time - lastSpoken > cooldown)
            {
                lastSpoken = Time.time;
                GameManager.instance.ShowText(messages[Random.Range(1, messages.Length)], 20, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, cooldown); //вектор добавлен для того чтобы сдвинуть текст выше
            }
        }*/
        GameManager.instance.ShowDialogue(messages, dialogueSprite);
    }
}

//Color.white
