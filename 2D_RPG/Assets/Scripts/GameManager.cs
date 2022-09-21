using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //�������
    public List<Sprite> playerSprites; //������ �������� ��� ������
    public List<Sprite> weaponSprites; //������ �������� ��� ������
    public List<int> weaponPrices; //������ �������� ��� ���� ������
    public List<int> xpTable; //������ �������� ��� ������������ �����

    //references ��� ���������� � ���������
    [HideInInspector] public Weapon weapon;
    [HideInInspector] public Player player;

    public FloatingTextManager floatingTextManager;
    public DialogueManager dialogueManager;
    public RectTransform hitpointBar;
    public GameObject hud; //��� � ��������� ���� ����� ��� ���� ����� ��� �������� ����� ������� �� ����������� ����� ���� � hud-a
    public GameObject menu;
    public Animator deathMenuAnim; // ���� ��������� ���� ������
    public Camera mainCamera;
    public AudioManager audioManager;
    public GameObject backgroundMusicObject;
    public GameObject multiplayerInformationCoverage;
    [HideInInspector] public SceneTransition sceneTransition;

    private Animator entranceLevel1GateAnimator;
    private Animator entranceLevel2GateAnimator;
    private Animator entranceLevel3GateAnimator;

    //������
    public int money;
    public int experience;


    public Animator chatWindowAnimator;
    private bool chatWindowShowing = false;

    [HideInInspector] public string sceneName = "Entrance";
    [HideInInspector] public string ruSceneName;

    private bool audioListenersDisabled = false;


    //photon
    public Text textLastMessage;
    public InputField textMessageField;
    [HideInInspector] public PhotonView photonView;
    [HideInInspector] public PhotonManager photonManager;

    private void Awake()
    {
        photonView = FindObjectOfType<PhotonView>(); //GetComponent<PhotonView>();
        Debug.Log("GameManager has found photonView on game object " + photonView.name);
        photonManager = FindObjectOfType<PhotonManager>();
        Debug.Log("GameManager has found photonManager on game object " + photonManager.name);
        sceneTransition = FindObjectOfType<SceneTransition>();
        Debug.Log("GameManager has found sceneTranition on game object " + sceneTransition.name);

        if (SceneManager.GetActiveScene().name == "Entrance" && photonManager.isFirstPlayer == false)
        {
            photonManager.CreatePlayer2();
        }

        if (photonManager.playingMultiplayer == false)
        {
            player = GameObject.Find("Player1").GetComponent<Player>();
            weapon = GameObject.Find("Weapon1").GetComponent<Weapon>();
        }
        else
        {
            if (photonManager.isFirstPlayer == true)
            {
                player = GameObject.Find("Player1").GetComponent<Player>();
                weapon = GameObject.Find("Weapon1").GetComponent<Weapon>();
            }
            else
            {
                player = GameObject.Find("Player2(Clone)").GetComponent<Player>();
                weapon = GameObject.Find("Weapon2").GetComponent<Weapon>();
            }
        }

        if (photonManager.playingMultiplayer == false)
        {
            player.GetComponent<PhotonView>().enabled = false;
            player.GetComponent<PhotonTransformView>().enabled = false;
        }

        if (GameManager.instance != null)
        {
            //������� ����������, ��� ��� ��� �������� ����� ������� ��������� �� �����
            Destroy(GameObject.Find("Main Camera").gameObject);
            Destroy(gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(dialogueManager.gameObject);
            Destroy(audioManager.gameObject);
            Destroy(backgroundMusicObject);
            return;
        }

        instance = this; //��-�� ���� �������� ��� ��� ������ ��� gamemanager 
        SceneManager.sceneLoaded += LoadState; //��������� LoadState ������ ��� ��� �������� �����, sceneLoaded ��� event
        SceneManager.sceneLoaded += OnSceneLoaded;

        

        entranceLevel1GateAnimator = GameObject.Find("Level1Gate").GetComponent<Animator>();
        Debug.Log("entranceLevel1GateAnimator has found sceneTranition on game object " + entranceLevel1GateAnimator.name);
        entranceLevel2GateAnimator = GameObject.Find("Level2Gate").GetComponent<Animator>();
        entranceLevel3GateAnimator = GameObject.Find("Level3Gate").GetComponent<Animator>();

        if(PlayerPrefs.HasKey("LevelsPassed"))
            switch(PlayerPrefs.GetString("LevelsPassed"))
            {
                case "0":
                    entranceLevel1GateAnimator.SetTrigger("open");
                    break;
                case "1":
                    entranceLevel2GateAnimator.SetTrigger("open");
                    break;
                case "2":
                    entranceLevel3GateAnimator.SetTrigger("open");
                    break;
                case "3":

                    break;
                case "4":

                    break;
                case "5":

                    break;

                default:
                    entranceLevel1GateAnimator.SetTrigger("open");
                    break;
            }
        else
            entranceLevel1GateAnimator.SetTrigger("open");

        if (photonManager.playingMultiplayer == false)
            multiplayerInformationCoverage.gameObject.SetActive(true);
        else
            multiplayerInformationCoverage.gameObject.SetActive(false);

        if(SceneManager.GetActiveScene().name == "Entrance" && photonManager.playingMultiplayer == false)
            instance.ShowText("����", 35, Color.green, GameObject.Find("Main Camera").transform.position + new Vector3(0, 0.48f, 0), Vector3.zero, 3.0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) == true) //����� GetKey ����� �����
        {
            if (chatWindowShowing == true)
            {
                chatWindowAnimator.SetTrigger("hide");
                chatWindowAnimator.ResetTrigger("show");
                chatWindowShowing = false;
            }
            else
            {
                chatWindowAnimator.SetTrigger("show");
                chatWindowAnimator.ResetTrigger("hide");
                chatWindowShowing = true;
            }
        }

        switch (sceneName) //SceneManager.GetActiveScene().name
        {
            case "Entrance":
                ruSceneName = "����";
                break;
            case "Dungeon 1":
                ruSceneName = "������� 1";
                break;
            case "Dungeon 2":
                ruSceneName = "������� 2";
                break;
            case "Dungeon 3":
                ruSceneName = "������� 3";
                break;
            case "Dungeon 4":
                ruSceneName = "������� 4";
                break;
            case "Dungeon 5":
                ruSceneName = "������� 5";
                break;
            default:
                ruSceneName = "����";
                break;
        }

        if(GameObject.Find("Player2(Clone)") != null && audioListenersDisabled == false)
        {
            if (photonManager.isFirstPlayer == true)
                GameObject.Find("Player2(Clone)").GetComponent<AudioListener>().enabled = false;

            if (photonManager.isFirstPlayer == false)
                GameObject.Find("Player1").GetComponent<AudioListener>().enabled = false;

            audioListenersDisabled = true;
        }
    }


    public void ShowText(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) //����� ��� ��� ��� ���� ���� ����� ����
    {                                                                                                                 //������ ������ ��������� ��������� floatingText
        floatingTextManager.Show(message, fontSize, color, position, motion, duration);
    }

    public void ShowDialoguePage(string textPage, Sprite characterSprite)
    {
        dialogueManager.ShowPage(textPage, characterSprite);
    }

    //UpgradeWeapon
    public bool TryUpgradeWeapon() //bool ������ ��� ���������� ��������, ����� �� ������������ ������
    {
        //������ ��� ������������� ������?
        if (weaponPrices.Count <= weapon.weaponLevel) //��� ������� weapon.weaponLevel >= weaponPrices.Count
            return false;

        if(money >= weaponPrices[weapon.weaponLevel])
        {
            money -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false; //���� ���� ������ �� ���� ��� �� � ����� �� ������� �� ������� �� ��� ������ �� ���������
    }

    // ������� ��
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }


    //����
    public int GetCurrentLevel()
    {
        int returnValue = 0;
        int add = 0;

        while(experience >= add)
        {
            add += xpTable[returnValue];
            returnValue++;

            if (returnValue == xpTable.Count) //�������� �� max lvl
                return returnValue;
        }

        return returnValue;
    }
    public int GetXpToLevel(int level)
    {
        int returnValue = 0;
        int xp = 0;

        while(returnValue < level)
        {
            xp += xpTable[returnValue];
            returnValue++;
        }

        return xp;
    }
    public void GrantXp(int xp) //������ �������� ����������� � ���������� experience ������ ����� �������� ������� GrantXp
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel()) //��� ������� ������ ��� ��������� ��� ��
            OnLevelUp();
    }
    public void OnLevelUp()
    {
        Debug.Log("Level Up!");
        player.OnLevelUp();
        OnHitpointChange();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoadedCalled");
        if (photonManager.playingMultiplayer == true && photonManager.isFirstPlayer == true)
        {
            player = GameObject.Find("Player1").GetComponent<Player>();
            weapon = GameObject.Find("Weapon1").GetComponent<Weapon>();
        }
        else if (photonManager.playingMultiplayer == true && photonManager.isFirstPlayer == false)
        {
            player = GameObject.Find("Player2(Clone)").GetComponent<Player>();
            weapon = GameObject.Find("Weapon2").GetComponent<Weapon>();
        }

        /*int player1Count = 0;
        List<Player> player1List = null;
        foreach (var playerObj in FindObjectsOfType<Player>())
        {
            if (playerObj.name == "Player1")
            {
                player1Count++;
                player1List.Add(playerObj);
            }
        }
        player1List.ToArray();
        Destroy(player1List[1].gameObject);*/

        sceneTransition.SceneTransitionOnSceneLoaded();

        instance.ShowText(ruSceneName, 35, Color.green, GameObject.Find("Main Camera").transform.position + new Vector3(0, 0.48f, 0), Vector3.zero, 3.0f); //GameObject.Find("Main Camera").transform.position
        player.TeleportToSpawnPoint();

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamera.transform.position = player.transform.position;
    }
    
    public void RespawnRpcTrigger()
    {
        if (photonManager.playingMultiplayer == true)
            photonManager.OnRespawnRpcTriggered();
        else
            Respawn();
    }

    public void Respawn()
    {
        deathMenuAnim.SetTrigger("Hide");
        instance.money = 0;
        instance.experience = 0;
        instance.player.SetLevel(0);
        instance.weapon.SetWeaponLevel(0);
        player.hitpoint = 5;
        player.maxHitpoint = 5;
        try
        {
            PlayerPrefs.DeleteKey("SaveState");
        }
        catch (System.Exception)
        {
        }
        Debug.Log("Cleared Player Prefs");
        player.Respawn();
        SceneTransition.instance.sceneToGo = "Entrance";
        SceneTransition.instance.SceneSwitch();
    }

    public void SaveState()
    {
        string s = ""; //���� s stands for SaveState

        // ���� "|" ��������� ���������� ����� ���� ������, � ����� � s ���������� ������ �� ����� ������������ ������� ���� ���������
        s += "0" + "|"; //���� ������, � ��� ���� ��� ����
        s += money.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString(); //������� ������

        PlayerPrefs.SetString("SaveState", s); //���������� � ���� SaveState ������ s

        Debug.Log("Saved State");
    }

    public void LoadState(Scene scene, LoadSceneMode mode) //��������� ��������, //��������� ��������, ��������� ��������� ����� ������� 13 ����� ��������� ����������
    {
        SceneManager.sceneLoaded -= LoadState;
        //PlayerPrefs.DeleteAll(); ������������� ���� ���� �������� ����������
        if (!PlayerPrefs.HasKey("SaveState")) //���� �������� ����� ��� �� return
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|'); // ����� �� PlayerPrefs �� ����� SaveState ������, ������� �������� � ������������ ����� �����
        // ���� ��� '|' ������� � ��������� ��������, � ������� �� ����� �� ������ ��������� ���� ������ � ������, ������ ��� ��� ������ � �� ������ � �� ��������� ������������
        // � ����� �� s ����������� ���-�� ���� 0|10|50|2 � ��� ���������� ������� Split �� ��������� �����, � ������ �� ������ ���������

        //����� ���������� ����� � �����
        money = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        if(GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());
        weapon.SetWeaponLevel(int.Parse(data[3]));

        Debug.Log("Loaded State");
    }

    public void ProhibitPlayerMoving()
    {
        player.canMove = false;
    }

    public void AllowPlayerMoving()
    {
        player.canMove = true;
    }

    public void ProhibitPlayerSwing()
    {
        weapon.swingPermission = false;
    }

    public void AllowPlayerSwing()
    {
        weapon.swingPermission = true;
    }
}