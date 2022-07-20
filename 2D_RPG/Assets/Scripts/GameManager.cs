using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            //������� ����������, ��� ��� ��� �������� ����� ������� ��������� �� �����
            Destroy(eventSystem);
            Destroy(mainCamera.gameObject);
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(dialogueManager.gameObject);
            return;
        }

        instance = this; //��-�� ���� �������� ��� ��� ������ ��� gamemanager 
        SceneManager.sceneLoaded += LoadState; //��������� LoadState ������ ��� ��� �������� �����, sceneLoaded ��� event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


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
    

    //������
    public int money;
    public int experience;

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
        instance.ShowText(scene.name, 35, Color.green, GameObject.Find("Main Camera").transform.position + new Vector3(0, 0.48f, 0), Vector3.zero, 3.0f); //������� ����� � ��������� �����
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
}


//PlayerPrefs.DeleteAll(); ��� ����� ������� ��� ������ � Player Preferences