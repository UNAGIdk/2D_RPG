using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float[] fireballSpeed = { 2.5f, -2.5f };
    public Transform[] fireballs;
    public float distance = 0.25f;
    public BossChest bossChest;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //������ �������� ��������� transform � fireball-�� ���, ����� ��� ��������� ������ transform-a �����
        //transform.position � ������ ������ ��� ��������� position �����
        for (int i = 0; i < fireballs.Length; i++)
        {
            fireballs[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * fireballSpeed[i]) * distance, Mathf.Sin(Time.time * fireballSpeed[i]) * distance, 0);
        }
    }

    protected override void Death()
    {
        base.Death();
        if(bossChest != null)
            bossChest.bossDead = true;
    }
}