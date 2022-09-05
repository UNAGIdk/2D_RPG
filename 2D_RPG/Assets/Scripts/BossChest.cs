using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChest : Chest
{
    public Sprite openChest;
    public bool bossDead = false;
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
            if (moneyAmount == 0) // ���� ��������� 0, �� ���-�� ����� ����� ����� 5-10
                moneyAmount = Random.Range(5, 10);
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.money += moneyAmount;
            //��� �������� ������� �������� ����������� ����� �� ���������� �����������
            GameManager.instance.ShowText("+" + moneyAmount + " ���", 25, Color.yellow, transform.position, Vector3.up * 30, 2.0f);
            bossChestAudioSource.PlayOneShot(bossChestSounds[Random.Range(0, bossChestSounds.Length)]);
        }
    }
}
