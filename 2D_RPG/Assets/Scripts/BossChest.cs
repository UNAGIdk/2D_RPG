using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChest : Chest
{
    public Sprite openChest;
    public bool bossDead = false;
    public Boss boss;
    private bool chestOpened = false;

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
            if (moneyAmount == 0) // если поставить 0, то кол-во денег будет равно 5-10
                moneyAmount = Random.Range(5, 10);
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.money += moneyAmount;
            //при поднятии сундука вызываем всплывающий текст со следующими параметрами
            GameManager.instance.ShowText("+" + moneyAmount + " GOLD", 25, Color.yellow, transform.position, Vector3.up * 30, 2.0f);
        }
    }
}
