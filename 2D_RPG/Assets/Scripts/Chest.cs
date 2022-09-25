using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    [HideInInspector]public int moneyAmount;
    public int randomLowerBorder;
    public int randomUpperBorder;
    public AudioSource chestAudioSource;
    public AudioClip[] chestSounds;


    protected override void OnCollect()
    {

        if (!collected)
        {
            moneyAmount = Random.Range(randomLowerBorder, randomUpperBorder);
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.money += moneyAmount;
            //при поднятии сундука вызываем всплывающий текст со следующими параметрами
            GameManager.instance.ShowText("+" + moneyAmount + " ЗЛТ", 25, Color.yellow, transform.position, Vector3.up * 30, 2.0f);
            chestAudioSource.PlayOneShot(chestSounds[Random.Range(0, chestSounds.Length)]);
        }
    }
}
