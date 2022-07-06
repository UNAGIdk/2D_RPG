using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    //experience
    public int xpValue = 1;

    //logic
    public float triggerLength = 1.0f; // ���� ������� �� triggerLength � ���� �� �� ������ ������������ ������
    public float chaseLength = 5.0f; // � ����� ������������ ������ �� ����� ����. ����������
    private bool chasing; // ���� �� ������
    private bool collidingWithPlayer; // �������� �� ������
    public float EnemyXSpeed;
    public float EnemyYSpeed;
    private Transform playerTransform; //
    private Vector3 startingPosition; // ��������� ������� ����

    //�������
    public ContactFilter2D filter; // ������ ��� ����������, ������ ����� ��� ��������� ����������� �� �� � �������
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10]; // ����� ��� ���� ������ ��� �� ����� ����������� �� Collidable, � ��� ��� ��� ���������


    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>(); //�������� ��������� ������� ��������� ������� �� ���� �� ��� ����� ������
    }


    protected virtual void FixedUpdate() //������ ���� private void, �� � boss � ������� ���������� ��� �������, ������� �������� ���
    {
        //����� �� ������ ����������?
        if(Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            if(Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                chasing = true;

            if (chasing) //���� ��� ����������, �.�. chasing == true
            {
                if (!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized, EnemyXSpeed, EnemyYSpeed);
                }
            }
            else //chasing == false
            {
                UpdateMotor(startingPosition - transform.position, EnemyXSpeed, EnemyYSpeed);
            }
        }
        else //chaseLength ������ ��� ���������� ��� ������ �������������
        {
            UpdateMotor(startingPosition - transform.position, EnemyXSpeed, EnemyYSpeed);
            chasing = false;
        }

        //colliding �� �� � ������� (�� COLLIDABLE)
        collidingWithPlayer = false; // �� ������� �� colliding
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            if(hits[i].tag == "Fighter" && hits[i].name == "Player") //���������� true ���� ����������� � ����� Fighter � ������ Player
            {
                collidingWithPlayer = true;
            }

            //������ ������
            hits[i] = null;
        }
    }


    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " XP", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }
}
