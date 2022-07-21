using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    //damage
    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7 }; // превращаем это в массив потому что есть несколько типов оружи€
    public float[] pushForce = { 2.0f, 2.2f, 2.6f, 2.9f, 3.2f, 3.5f, 4f }; // превращаем это в массив потому что есть несколько типов оружи€

    //upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    //удар
    private Animator anim;
    public AudioSource swingSound;
    private float cooldown = 0.5f;
    private float lastSwing; //когда последний раз наносили удар
    public bool swingPermission = true;

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
        base.Update(); //в колайдабл в update провер€етс€ коллизи€

        if (Input.GetKeyDown(KeyCode.Space)) //true когда нажимаетс€ пробел
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
            if (coll.name == "Player")
                return;

            //создать новый объект damage и отправить его геймќбъекту с тэгом Fighter которого мы ударили
            Damage damage = new Damage
            {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };

            coll.SendMessage("RecieveDamage", damage);
        }
    }

    private void Swing() //удар
    {
        anim.SetTrigger("Swing"); //в аниматоре включить триггер под названием "Swing"
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
