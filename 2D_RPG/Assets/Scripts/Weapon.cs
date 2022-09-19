using UnityEngine;
using Photon.Pun;

public class Weapon : Collidable
{
    //damage
    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7 }; // превращаем это в массив потому что есть несколько типов оружи€
    public float[] pushForce = { 2.0f, 2.2f, 2.6f, 2.9f, 3.2f, 3.5f, 4f }; // превращаем это в массив потому что есть несколько типов оружи€

    //upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    //удар
    private float cooldown = 0.5f;
    private float lastSwing; //когда последний раз наносили удар
    [HideInInspector] public bool swingPermission = true;

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
    }

    protected override void Update()
    {
        base.Update(); //в колайдабл в update провер€етс€ коллизи€

        if (Input.GetKeyDown(KeyCode.Space) && GetComponentInParent<PhotonView>().IsMine)
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
            if (coll.name == "Player1" || coll.name == "Player2(Clone)") //“»ћƒ≈ћ≈ƒ∆ј Ќ≈“, Ќј—“–ј»¬ј“№ ќ“ƒ≈Ћ№Ќќ
                return;

            //создать новый объект damage и отправить его геймќбъекту с тэгом Fighter которого мы ударили
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

    private void Swing() //удар
    {
        GetComponent<Animator>().SetTrigger("Swing");
        //GameManager.instance.photonManager.SynchronizeSwing(GetComponentInParent<Player>().gameObject.name);
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
