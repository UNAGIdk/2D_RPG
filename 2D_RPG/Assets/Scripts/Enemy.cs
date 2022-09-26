using UnityEngine;
using Photon.Pun;

public class Enemy : Mover
{
    //experience
    public int xpValue = 1;

    //logic
    public float triggerLength = 1.0f; // ���� ������� �� triggerLength � ���� �� �� ������ ������������ ������
    public float chaseLength = 5.0f; // � ����� ������������ ������ �� ����� ����. ����������
    private bool chasingPlayer1 = false; // ���� �� ������
    private bool chasingPlayer2 = false;
    private bool collidingWithPlayer; // �������� �� ������
    public float EnemyXSpeed;
    private float EnemyYSpeed;
    public float EnemyYSpeedMultiplayer = 0.79f;

    private Transform player1Transform; //
    private Transform player2Transform;
    private Vector3 startingPosition; // ��������� ������� ����
    private int hitpontCompare;

    private Material defaultMaterial;
    public Material getDamageMaterial;
    public Material destroyMaterial;
    public float resetMaterialTime = 0.1f;

    //ParticleSystem 
    public Object enemyExplosionPrefabGO;
    public ParticleSystemRenderer enemyExplosionPrefabPSR;

    //�������
    public ContactFilter2D filter; // ������ ��� ����������, ������ ����� ��� ��������� ����������� �� �� � �������
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10]; // ����� ��� ���� ������ ��� �� ����� ����������� �� Collidable, � ��� ��� ��� ���������

    private bool player2Linked = false;
    public bool isGarr;

    protected override void Start()
    {
        base.Start();
        player1Transform = GameObject.Find("Player1").transform;
            
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>(); //�������� ��������� ������� ��������� ������� �� ���� �� ��� ����� ������
        hitpontCompare = hitpoint;

        defaultMaterial = this.GetComponent<SpriteRenderer>().material;
        //enemyExplosionPrefab.material = destroyMaterial;

        EnemyYSpeed = EnemyXSpeed * 0.79f;
    }


    protected virtual void FixedUpdate() //������ ���� private void, �� � boss � ������� ���������� ��� �������, ������� �������� ���
    {
        if (GameManager.instance.photonManager.playingMultiplayer == false)
        {
            //����� �� ������ ����������?
            if (Vector3.Distance(player1Transform.position, startingPosition) < chaseLength)
            {
                if (player1Transform != null && Vector3.Distance(player1Transform.position, startingPosition) < triggerLength)
                    chasingPlayer1 = true;

                if (chasingPlayer1) //���� ��� ����������, �.�. chasing == true
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
            else //chaseLength ������ ��� ���������� ��� ������ �������������
            {
                UpdateMotor(startingPosition - transform.position, EnemyXSpeed, EnemyYSpeed);
                chasingPlayer1 = false;
            }
        }
        //����� ���������� ������
        



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

            //������ ������������� ������ 2
            if (player2Linked == true)
            {
                if (chasingPlayer1 == false && Vector3.Distance(player2Transform.position, startingPosition) < chaseLength) // && chasingPlayer1 == false
                {
                    if (Vector3.Distance(player2Transform.position, startingPosition) < triggerLength)
                        chasingPlayer2 = true;

                    if (chasingPlayer2) //���� ��� ����������, �.�. chasing == true
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

            //������ ������������� ������ 1
            if (chasingPlayer2 == false && Vector3.Distance(player1Transform.position, startingPosition) < chaseLength) // && chasingPlayer2 == false
            {
                if (Vector3.Distance(player1Transform.position, startingPosition) < triggerLength)
                    chasingPlayer1 = true;

                if (chasingPlayer1) //���� ��� ����������, �.�. chasing == true
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
        


        //colliding �� �� � ������� (�� COLLIDABLE)
        collidingWithPlayer = false; // �� ������� �� colliding
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            if(hits[i].name == "Player1" || hits[i].name == "Player2(Clone)") //���������� true ���� ����������� � ������ Player1 ��� Player2(Clone)
            {
                collidingWithPlayer = true;
            }

            //������ ������
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
        GameManager.instance.ShowText("+" + xpValue + " ��", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
        Destroy(gameObject);
    }
}
