using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    //experience
    public int xpValue = 1;

    //logic
    public float triggerLength = 1.0f; // если подойти на triggerLength к мобу то он начнет преследовать игрока
    public float chaseLength = 5.0f; // и будет преследовать игрока на таком макс. расстоянии
    private bool chasing; // идет ли погоня
    private bool collidingWithPlayer; // касаемся ли игрока
    public float EnemyXSpeed;
    private float EnemyYSpeed;
    public float EnemyYSpeedMultiplayer = 0.79f;

    private Transform playerTransform; //
    private Vector3 startingPosition; // стартовая позиция моба
    private int hitpontCompare;

    private Material defaultMaterial;
    public Material getDamageMaterial;
    public float resetMaterialTime = 0.1f;

    //хитбокс
    public ContactFilter2D filter; // фильтр для коллайдера, нгужен будет для понимания коллайдимся ли мы с игроком
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10]; // пишем это сюда потому что не можем наследовать от Collidable, а там это уже прописано


    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>(); //получить коллайдер первого дочернего объекта от того на ком лежит скрипт
        hitpontCompare = hitpoint;

        defaultMaterial = this.GetComponent<SpriteRenderer>().material;

        EnemyYSpeed = EnemyXSpeed * 0.79f;
    }


    protected virtual void FixedUpdate() //раньше было private void, но в boss я захотел переписать эту функцию, поэтому объявляю так
    {
        //игрок на нужном расстоянии?
        if(Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            if(Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                chasing = true;

            if (chasing) //если уже преследуем, т.е. chasing == true
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
        else //chaseLength больше чем необходимо для начала преследования
        {
            UpdateMotor(startingPosition - transform.position, EnemyXSpeed, EnemyYSpeed);
            chasing = false;
        }

        //colliding ли мы с игроком (ИЗ COLLIDABLE)
        collidingWithPlayer = false; // по дефолту не colliding
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            if(hits[i].tag == "Fighter" && hits[i].name == "Player") //установить true если коллайдимся с тэгом Fighter и именем Player
            {
                collidingWithPlayer = true;
            }

            //очищаю массив
            hits[i] = null;
        }

        if(hitpontCompare != hitpoint)
        {
            GetDamage();
        }
        hitpontCompare = hitpoint;
    }

    public void GetDamage()
    {
        this.GetComponent<SpriteRenderer>().material = getDamageMaterial;
        Invoke("ResetMaterial", resetMaterialTime);
    }

    public void ResetMaterial()
    {
        this.GetComponent<SpriteRenderer>().material = defaultMaterial;
    }


    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " ХР", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }
}
