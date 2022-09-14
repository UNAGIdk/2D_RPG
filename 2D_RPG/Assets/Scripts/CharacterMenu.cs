using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CharacterMenu : MonoBehaviour
{
    public GameManager gameManager;

    //��������� ����
    public Text levelText, hitpointText, goldText, upgradeCostText, xpText;

    //���� ��� ������
    public Image weaponSprite;
    public RectTransform xpBar; //��� �� ����� ������ ��� xpBar, � ��� �� ����� ������ ������ ������ (scale)
    public NPCTextPerson NPC;

    public Text roomNameText;
    public Text playerNameText;
    public Text serverNameText;
    public Text pingText;
    
    
    //���� ������� ������� ���������� ���� �� escape
    public Animator menuAnimator;
    [HideInInspector] public bool menuIsNowHidden = true;

    public void OnMenuButtonClick()
    {
        if (menuIsNowHidden == true)
            menuIsNowHidden = false;

        if (menuIsNowHidden == false)
            menuIsNowHidden = true;
    }


    //������� ������
    public void OnUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon() == true)
            UpdateMenu();
    }


    //�������� ���������� �� ������
    public void UpdateMenu()
    {
        //weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "����"; //���� ������� ��� ����
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString(); //���� ��� �� ���� ����� ������� ��������� ��������


        //meta
        hitpointText.text = GameManager.instance.player.hitpoint.ToString() + " / " + GameManager.instance.player.maxHitpoint;
        goldText.text = GameManager.instance.money.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        //xp bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if (currLevel == GameManager.instance.xpTable.Count) //���� �� ��� ������������� ���-�
        {
            xpText.text = GameManager.instance.experience.ToString() + " ����� �����";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLvlXp = GameManager.instance.GetXpToLevel(currLevel - 1);
            int currLvlXp = GameManager.instance.GetXpToLevel(currLevel);

            int diff = currLvlXp - prevLvlXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLvlXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff; //�� ������� % ��������� �������
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;
        }
    }

    public void OnResetClick() //������ ��������� ��������� � ��������� ���� �������� ������ �� ���������
    {
        try
        {
            PlayerPrefs.DeleteAll();
            GameManager.instance.money = 0;
            GameManager.instance.experience = 0;
            GameManager.instance.player.SetLevel(0);
            GameManager.instance.weapon.SetWeaponLevel(0);
            GameManager.instance.audioManager.SetMasterVolume(1);
            GameManager.instance.audioManager.SetEffectsVolume(1);
            GameManager.instance.audioManager.SetMusicVolume(1);
            GameManager.instance.audioManager.SetUserInterfaceVolume(1);
            GameObject.Find("Player").GetComponent<Player>().hitpoint = 5;
            GameObject.Find("Player").GetComponent<Player>().maxHitpoint = 5;
            foreach (var NPCObject in FindObjectsOfType<NPCTextPerson>())
            {
                if(GameManager.instance.photonManager.playingMultiplayer == false)
                    NPCObject.hasAskedName = false;
                
                NPCObject.hasSpokenMessages = false;
            }
            UpdateMenu();
            Debug.Log("Cleared Player Prefs");
        }
        catch (System.Exception)
        {
        }
    }
}
