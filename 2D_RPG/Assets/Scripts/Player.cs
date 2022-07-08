using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    //�������� ������������ player-a
    public float playerYSpeed = 0.75f;
    public float playerXSpeed = 1f;

    private SpriteRenderer spriteRenderer;
    public bool canMove = true;
    public Animator weaponAnimator;

    public float x;
    public float y;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void RecieveDamage(Damage damage)
    {
        if (canMove == false) //���� ���� �� ��� �������� ��� �� �����
            return;

        base.RecieveDamage(damage);
        GameManager.instance.OnHitpointChange();
    }


    private void FixedUpdate()
    {
        //���������� ��� ��������� ������ � ����������
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if(canMove == true)
            UpdateMotor(new Vector3(x, y, 0), playerXSpeed, playerYSpeed);

        if(x != 0 || y != 0)
        {
            weaponAnimator.SetTrigger("Walk");
        }

        if (x == 0 && y == 0)
            weaponAnimator.SetTrigger("Stay");

    }

    public void SwapSprite(int skinId)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }

    public void OnLevelUp()
    {
        maxHitpoint += 2;
        hitpoint = maxHitpoint;
    }

    public void SetLevel(int level) //��� �������� ������� lvlup ������� ���, ������� ������� ����� ������
    {
        for (int i = 0; i < level; i++)
        {
            OnLevelUp();
        }
    }

    public void Heal(int healingAmount)
    {
        if (hitpoint == maxHitpoint)
            return;

        hitpoint += healingAmount;

        if (hitpoint > maxHitpoint)
            hitpoint = maxHitpoint;
        // � ����� ������ ������� ����� � ��� ��� ��������
        GameManager.instance.ShowText("+" + healingAmount.ToString() + " HP", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHitpointChange();
    }

    protected override void Death()
    {
        canMove = false;
        //Time.timeScale = 0;
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
    }

    public void Respawn()
    {
        hitpoint = maxHitpoint;
        canMove = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }
}
