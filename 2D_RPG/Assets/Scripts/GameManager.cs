using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //ресурсы
    public List<Sprite> playerSprites; //список спрайтов для игрока
    public List<Sprite> weaponSprites; //список спрайтов для оружия
    public List<int> weaponPrices; //список спрайтов для цены оружия
    public List<int> xpTable; //список спрайтов для необходимого опыта

    //references для заполнения в редакторе
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public DialogueManager dialogueManager;
    public RectTransform hitpointBar;
    public GameObject hud; //это и следующее поле нужно для того чтобы при переходе между сценами не создавалось новых меню и hud-a
    public GameObject menu;
    public Animator deathMenuAnim; // поле аниматора меню смерти
    public GameObject eventSystem;
    public Camera mainCamera;
    public AudioManager audioManager;
    public GameObject backgroundMusicObject;
    public GameObject multiplayerInformationCoverage;
    [HideInInspector]public SceneTransition sceneTranition;

    private Animator entranceLevel1GateAnimator;
    private Animator entranceLevel2GateAnimator;
    private Animator entranceLevel3GateAnimator;

    //логика
    public int money;
    public int experience;


    public Animator chatWindowAnimator;
    private bool chatWindowShowing = false;

    [HideInInspector] public string sceneName;
    [HideInInspector] public string ruSceneName;


    //photon
    public Text textLastMessage;
    public InputField textMessageField;
    [HideInInspector] public PhotonView photonView;
    [HideInInspector] public PhotonManager photonManager;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            //удалить компоненты, так как при переходе между сценами создаются их копии
            //Destroy(eventSystem);
            Destroy(mainCamera.gameObject);
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(dialogueManager.gameObject);
            Destroy(audioManager.gameObject);
            Destroy(backgroundMusicObject);
            return;
        }

        instance = this; //че-то типа проверки что это нужный нам gamemanager 
        SceneManager.sceneLoaded += LoadState; //запускает LoadState каждый раз при загрузке сцены, sceneLoaded это event
        SceneManager.sceneLoaded += OnSceneLoaded;

        photonView = FindObjectOfType<PhotonView>(); //GetComponent<PhotonView>();
        Debug.Log("GameManager has found photonView on game object " + photonView.name);
        photonManager = FindObjectOfType<PhotonManager>();
        Debug.Log("GameManager has found photonManager on game object " + photonManager.name);
        sceneTranition = FindObjectOfType<SceneTransition>();
        Debug.Log("GameManager has found sceneTranition on game object " + sceneTranition.name);

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
            instance.ShowText("Вход", 35, Color.green, GameObject.Find("Main Camera").transform.position + new Vector3(0, 0.48f, 0), Vector3.zero, 3.0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) == true) //МЭЙБИ GetKey будет лучше
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
                ruSceneName = "Вход";
                break;
            case "Dungeon 1":
                ruSceneName = "Уровень 1";
                break;
            case "Dungeon 2":
                ruSceneName = "Уровень 2";
                break;
            case "Dungeon 3":
                ruSceneName = "Уровень 3";
                break;
            case "Dungeon 4":
                ruSceneName = "Уровень 4";
                break;
            case "Dungeon 5":
                ruSceneName = "Уровень 5";
                break;
            default:
                ruSceneName = "Вход";
                break;
        }
    }


    public void ShowText(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) //пишем это тут для того чтоб можно было
    {                                                                                                                 //откуда угодно заставить появиться floatingText
        floatingTextManager.Show(message, fontSize, color, position, motion, duration);
    }

    public void ShowDialoguePage(string textPage, Sprite characterSprite)
    {
        dialogueManager.ShowPage(textPage, characterSprite);
    }

    //UpgradeWeapon
    public bool TryUpgradeWeapon() //bool потому что возвращаем значение, можно ли заапгрейдить оружие
    {
        //оружие уже максимального уровня?
        if (weaponPrices.Count <= weapon.weaponLevel) //мой вариант weapon.weaponLevel >= weaponPrices.Count
            return false;

        if(money >= weaponPrices[weapon.weaponLevel])
        {
            money -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false; //если наше оружие не макс лвл но и денег на апгрейд не хватает то офк просто не апгрейдим
    }

    // полоска хп
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }


    //Опыт
    public int GetCurrentLevel()
    {
        int returnValue = 0;
        int add = 0;

        while(experience >= add)
        {
            add += xpTable[returnValue];
            returnValue++;

            if (returnValue == xpTable.Count) //проверка на max lvl
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
    public void GrantXp(int xp) //вместо простого прибавления к переменной experience теперь будем вызывать функцию GrantXp
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel()) //это условие значит что произошел лвл ап
            OnLevelUp();
    }
    public void OnLevelUp()
    {
        Debug.Log("Level Up!");
        player.OnLevelUp();
        OnHitpointChange();
    }

    //при загрузке сцены нужно игрока телепортировать к SpawnPoint
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position; //телепортировать игрока к SpawnPoint
        instance.ShowText(ruSceneName, 35, Color.green, GameObject.Find("Main Camera").transform.position + new Vector3(0, 0.48f, 0), Vector3.zero, 3.0f); //вывести текст с названием сцены
        sceneTranition.SceneTransitionOnSceneLoaded();
    }

    public void Respawn()
    {
        deathMenuAnim.SetTrigger("Hide");
        SceneManager.LoadScene("Entrance");
        player.Respawn();
        PlayerPrefs.DeleteAll();
        instance.money = 0;
        instance.experience = 0;
        instance.player.SetLevel(0);
        instance.weapon.SetWeaponLevel(0);
        player.hitpoint = 5;
        player.maxHitpoint = 5;
        Debug.Log("Cleared Player Prefs");
    }

    public void SaveState()
    {
        string s = ""; //типа s stands for SaveState

        // знак "|" разделяет показатели между друг другом, в итоге в s получается строка со всеми показателями которые надо сохранить
        s += "0" + "|"; //скин игрока, у нас пока его нету
        s += money.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString(); //уровень оружия

        PlayerPrefs.SetString("SaveState", s); //установить в ключ SaveState строку s

        Debug.Log("Saved State");
    }

    public void LoadState(Scene scene, LoadSceneMode mode) //загрузить прогресс, //сохранить прогресс, параметры добавлены чтобы строчка 13 могла нормально отработать
    {
        SceneManager.sceneLoaded -= LoadState;
        //PlayerPrefs.DeleteAll(); разкомментить если хочу обнулить сохранение
        if (!PlayerPrefs.HasKey("SaveState")) //если такового ключа нет то return
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|'); // взять из PlayerPrefs по ключу SaveState строку, которая хранится в соответствии этому ключу
        // выше уже '|' пишется в одинарных кавычках, в отличие от когда мы сверху вписывали этот символ в строку, потому что это символ а не строка и он нормально распознается
        // в итоге из s загрузиться что-то типа 0|10|50|2 и оно разделится методом Split на несколько строк, в каждой по одному параметру

        //взять количество денег и опыта
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


//PlayerPrefs.DeleteAll(); так можно удалить все записи о Player Preferences