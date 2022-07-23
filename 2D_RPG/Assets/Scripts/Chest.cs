using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int moneyAmount = 0;
    public AudioSource collectSound;


    protected override void OnCollect()
    {

        if (!collected)
        {
            if (moneyAmount == 0) // ���� ��������� 0, �� ���-�� ����� ����� ����� 5-10
                moneyAmount = Random.Range(5, 10);
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.money += moneyAmount;
            //��� �������� ������� �������� ����������� ����� �� ���������� �����������
            GameManager.instance.ShowText("+" + moneyAmount + " GOLD", 25, Color.yellow, transform.position, Vector3.up * 30, 2.0f);
            collectSound.Play();
        }
    }
}
