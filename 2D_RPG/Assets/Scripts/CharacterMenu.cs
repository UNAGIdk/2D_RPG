using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CharacterMenu : MonoBehaviour
{
    public GameManager gameManager;

    //текстовые поля
    public Text levelText, hitpointText, goldText, upgradeCostText, xpText;

    //поля для логики
    public Image weaponSprite;
    public RectTransform xpBar; //нам не нужен спрайт для xpBar, у нее мы будем менять только размер (scale)
    public NPCTextPerson NPC;

    public Text roomNameText;
    public Text playerNameText;
    public Text serverNameText;
    public Text pingText;
    
    
    //ниже пытался сделать открывание меню на escape
    public Animator menuAnimator;
    [HideInInspector] public bool menuIsNowHidden = true;

    public void OnMenuButtonClick()
    {
        if (menuIsNowHidden == true)
            menuIsNowHidden = false;

        if (menuIsNowHidden == false)
            menuIsNowHidden = true;
    }


    //апгрейд оружия
    public void OnUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon() == true)
            UpdateMenu();
    }


    //обновить информацию об игроке
    public void UpdateMenu()
    {
        //weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "МАКС"; //если уровень уже макс
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString(); //если еще не макс тогда вывести стоймость апгрейда


        //meta
        hitpointText.text = GameManager.instance.player.hitpoint.ToString() + " / " + GameManager.instance.player.maxHitpoint;
        goldText.text = GameManager.instance.money.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        //xp bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if (currLevel == GameManager.instance.xpTable.Count) //если мы уже максимального лвл-а
        {
            xpText.text = GameManager.instance.experience.ToString() + " очков опыта";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLvlXp = GameManager.instance.GetXpToLevel(currLevel - 1);
            int currLvlXp = GameManager.instance.GetXpToLevel(currLevel);

            int diff = currLvlXp - prevLvlXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLvlXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff; //на сколько % заполнить полоску
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;
        }
    }

    public void OnResetClick() //кнопка обнуления прогресса и установки всех значений игрока на стартовые
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
