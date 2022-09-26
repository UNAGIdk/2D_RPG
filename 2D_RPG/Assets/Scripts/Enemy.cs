using UnityEngine;
using Photon.Pun;

public class Enemy : Mover
{
    //experience
    public int xpValue = 1;

    //logic
    public float triggerLength = 1.0f; // если подойти на triggerLength к мобу то он начнет преследовать игрока
    public float chaseLength = 5.0f; // и будет преследовать игрока на таком макс. расстоянии
    private bool chasingPlayer1 = false; // идет ли погоня
    private bool chasingPlayer2 = false;
    private bool collidingWithPlayer; // касаемся ли игрока
    public float EnemyXSpeed;
    private float EnemyYSpeed;
    public float EnemyYSpeedMultiplayer = 0.79f;

    private Transform player1Transform; //
    private Transform player2Transform;
    private Vector3 startingPosition; // стартовая позиция моба
    private int hitpontCompare;

    private Material defaultMaterial;
    public Material getDamageMaterial;
    public Material destroyMaterial;
    public float resetMaterialTime = 0.1f;

    //ParticleSystem 
    public Object enemyExplosionPrefabGO;
    public ParticleSystemRenderer enemyExplosionPrefabPSR;

    //хитбокс
    public ContactFilter2D filter; // фильтр для коллайдера, нгужен будет для понимания коллайдимся ли мы с игроком
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10]; // пишем это сюда потому что не можем наследовать от Collidable, а там это уже прописано

    private bool player2Linked = false;
    public bool isGarr;

    protected override void Start()
    {
        base.Start();
        player1Transform = GameObject.Find("Player1").transform;
            
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>(); //получить коллайдер первого дочернего объекта от того на ком лежит скрипт
        hitpontCompare = hitpoint;

        defaultMaterial = this.GetComponent<SpriteRenderer>().material;
        //enemyExplosionPrefab.material = destroyMaterial;

        EnemyYSpeed = EnemyXSpeed * 0.79f;
    }


    protected virtual void FixedUpdate() //раньше было private void, но в boss я захотел переписать эту функцию, поэтому объявляю так
    {
        if (GameManager.instance.photonManager.playingMultiplayer == false)
        {
            //игрок на нужном расстоянии?
            if (Vector3.Distance(player1Transform.position, startingPosition) < chaseLength)
            {
                if (player1Transform != null && Vector3.Distance(player1Transform.position, startingPosition) < triggerLength)
                    chasingPlayer1 = true;

                if (chasingPlayer1) //если уже преследуем, т.е. chasing == true
                {
                    if (!collidingWithPlayer)
                    {
                        UpdateMotor((player1Transform.position - transform.position).normalized, EnemyXSpeed, EnemyYSpeed);
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
                chasingPlayer1 = false;
            }
        }
        //КОНЕЦ СИНГЛПЛЕЕР ЛОГИКИ
        



        if(GameManager.instance.photonManager.playingMultiplayer == true)
        {
            if(PhotonNetwork.PlayerList.Length == 2 && player2Linked == false) // && GameObject.Find("Player2(Clone)") == null
            {
                player2Transform = GameObject.Find("Player2(Clone)").transform;
            }

            try
            {
                if(player2Transform.gameObject.name == "Player2(Clone)")
                    player2Linked = true;
            }
            catch (System.Exception)
            {
            }

            //ЛОГИКА ПРЕСЛЕДОВАНИЯ ИГРОКА 2
            if (player2Linked == true)
            {
                if (chasingPlayer1 == false && Vector3.Distance(player2Transform.position, startingPosition) < chaseLength) // && chasingPlayer1 == false
                {
                    if (Vector3.Distance(player2Transform.position, startingPosition) < triggerLength)
                        chasingPlayer2 = true;

                    if (chasingPlayer2) //если уже преследуем, т.е. chasing == true
                    {
                        if (!collidingWithPlayer)
                        {
                            UpdateMotor((player2Transform.position - transform.position).normalized, EnemyXSpeed, EnemyYSpeed);
                        }
                    }
                    else //chasing == false
                    {
                        UpdateMotor(startingPosition - transform.position, EnemyXSpeed, EnemyYSpeed);
                    }
                }
                else
                {
                    UpdateMotor(startingPosition - transform.position, EnemyXSpeed, EnemyYSpeed);
                    chasingPlayer2 = false;
                }
            }

            //ЛОГИКА ПРЕСЛЕДОВАНИЯ ИГРОКА 1
            if (chasingPlayer2 == false && Vector3.Distance(player1Transform.position, startingPosition) < chaseLength) // && chasingPlayer2 == false
            {
                if (Vector3.Distance(player1Transform.position, startingPosition) < triggerLength)
                    chasingPlayer1 = true;

                if (chasingPlayer1) //если уже преследуем, т.е. chasing == true
                {
                    if (!collidingWithPlayer)
                    {
                        UpdateMotor((player1Transform.position - transform.position).normalized, EnemyXSpeed, EnemyYSpeed);
                    }
                }
                else //chasing == false
                {
                    UpdateMotor(startingPosition - transform.position, EnemyXSpeed, EnemyYSpeed);
                }
            }
            else
            {
                UpdateMotor(startingPosition - transform.position, EnemyXSpeed, EnemyYSpeed);
                chasingPlayer1 = false;
            }
        }
        


        //colliding ли мы с игроком (ИЗ COLLIDABLE)
        collidingWithPlayer = false; // по дефолту не colliding
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            if(hits[i].name == "Player1" || hits[i].name == "Player2(Clone)") //установить true если коллайдимся с именем Player1 или Player2(Clone)
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
        enemyExplosionPrefabPSR.material = destroyMaterial;
        GameObject enemyExplosion = (GameObject)Instantiate(enemyExplosionPrefabGO);
        enemyExplosion.transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " ХР", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
        Destroy(gameObject);
    }
}
