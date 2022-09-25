using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChest : Chest
{
    public Sprite openChest;
    [HideInInspector] public bool bossDead = false;
    public Boss boss;
    private bool chestOpened = false;

    public AudioSource bossChestAudioSource;
    public AudioClip[] bossChestSounds;

    protected override void Update()
    {
        base.Update();
        if (bossDead && !chestOpened)
        {
            GetComponent<SpriteRenderer>().sprite = openChest;
            chestOpened = true;
        }
    }

    protected override void OnCollect()
    {
        if (!collected && bossDead) 
        {
            moneyAmount = Random.Range(randomLowerBorder, randomUpperBorder);
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.money += moneyAmount;
            //при поднятии сундука вызываем всплывающий текст со следующими параметрами
            GameManager.instance.ShowText("+" + moneyAmount + " ЗЛТ", 25, Color.yellow, transform.position, Vector3.up * 30, 2.0f);
            bossChestAudioSource.PlayOneShot(bossChestSounds[Random.Range(0, bossChestSounds.Length)]);
        }
    }
}
