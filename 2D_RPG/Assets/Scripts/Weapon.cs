using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    //damage
    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7 }; // ���������� ��� � ������ ������ ��� ���� ��������� ����� ������
    public float[] pushForce = { 2.0f, 2.2f, 2.6f, 2.9f, 3.2f, 3.5f, 4f }; // ���������� ��� � ������ ������ ��� ���� ��������� ����� ������

    //upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    //����
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing; //����� ��������� ��� �������� ����
    public bool swingPermission = true;

    public AudioSource weaponAudioSource;
    public AudioClip weaponSwingClip;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update(); //� ��������� � update ����������� ��������

        if (Input.GetKeyDown(KeyCode.Space)) //true ����� ���������� ������
        {
            if (Time.time - lastSwing > cooldown)
            {
                if(swingPermission == true)
                {
                    lastSwing = Time.time;
                    Swing();
                }
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if(coll.tag == "Fighter")
        {
            if (coll.name == "Player1" || coll.name == "Player2(Clone)") //���������� ���, ����������� ��������
                return;

            //������� ����� ������ damage � ��������� ��� ����������� � ����� Fighter �������� �� �������
            Damage damage = new Damage
            {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };

            coll.SendMessage("RecieveDamage", damage);
            Debug.Log("Weapon on" + gameObject.name + "collided with " + coll.gameObject.name);
        }
    }

    private void Swing() //����
    {
        anim.SetTrigger("Swing"); //� ��������� �������� ������� ��� ��������� "Swing"
        weaponAudioSource.pitch = Random.Range(0.8f, 1.2f);
        weaponAudioSource.PlayOneShot(weaponSwingClip);
    }



    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }


    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}
