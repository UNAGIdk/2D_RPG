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
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public DialogueManager dialogueManager;
    public RectTransform hitpointBar;
    public GameObject hud; //��� � ��������� ���� ����� ��� ���� ����� ��� �������� ����� ������� �� ����������� ����� ���� � hud-a
    public GameObject menu;
    public Animator deathMenuAnim; // ���� ��������� ���� ������
    public GameObject eventSystem;
    public Camera mainCamera;
    public AudioManager audioManager;
    public GameObject backgroundMusicObject;

    public bool playingMultiplayer;


    //������
    public int money;
    public int experience;


    public Animator chatWindowAnimator;
    private bool chatWindowShowing = false;

    public string sceneName;
    public string ruSceneName;


    //photon
    public Text textLastMessage;
    public InputField textMessageField;
    private PhotonView photonView;
    private PhotonManager photonManager;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            //������� ����������, ��� ��� ��� �������� ����� ������� ��������� �� �����
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

        instance = this; //��-�� ���� �������� ��� ��� ������ ��� gamemanager 
        SceneManager.sceneLoaded += LoadState; //��������� LoadState ������ ��� ��� �������� �����, sceneLoaded ��� event
        SceneManager.sceneLoaded += OnSceneLoaded;

        photonView = FindObjectOfType<PhotonView>(); //GetComponent<PhotonView>();
        Debug.Log("Found phView on game object " + photonView.name);
        photonManager = FindObjectOfType<PhotonManager>();
        Debug.Log("Found phManager on game object " + photonManager.name);

        if (PhotonNetwork.CurrentRoom != null)
            playingMultiplayer = false;
        else
            playingMultiplayer = true;
 
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
                ruSceneName = "������ �����";
                break;
            case "Dungeon 2":
                ruSceneName = "������ 2";
                break;
            case "Dungeon 3":
                ruSceneName = "������ 3";
                break;
            case "Dungeon 4":
                ruSceneName = "������ 4";
                break;
            case "Dungeon 5":
                ruSceneName = "������ 5";
                break;
            default:
                break;
        }
    }

    /*public void SendButton()
    {
        photonView.RPC("Send_Data", RpcTarget.AllBuffered, PhotonNetwork.NickName, textMessageField.text); //��������� � ��� ��� + ���������
    }

    [PunRPC] //����� RPC ������� �����������
    private void Send_Data(string nickname, string message)
    {
        textLastMessage.text = nickname + ": " + message;
    }*/


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

    //��� �������� ����� ����� ������ ��������������� � SpawnPoint
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position; //��������������� ������ � SpawnPoint
        instance.ShowText(ruSceneName, 35, Color.green, GameObject.Find("Main Camera").transform.position + new Vector3(0, 0.48f, 0), Vector3.zero, 3.0f); //������� ����� � ��������� �����
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

        //���� ������ ���� ��� ��� ����� �����

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


//PlayerPrefs.DeleteAll(); ��� ����� ������� ��� ������ � Player Preferences