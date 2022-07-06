using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //текстовые поля
    public Text levelText, hitpointText, goldText, upgradeCostText, xpText;

    //поля для логики
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar; //нам не нужен спрайт для xpBar, у нее мы будем менять только размер (scale)

    //выбор персонажа
    public void OnArrowClick(bool right) //bool переменная для определения того, на какую стрелку мы кликнули (правая -> true, левая -> false)
    {
        if(right)
        {
            currentCharacterSelection++;
            //если дошли до конца списка
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;

        OnSelectionChanged();
        }
        else
        {
            currentCharacterSelection--;
            //если дошли до конца списка
            if (currentCharacterSelection < GameManager.instance.playerSprites.Count)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1; //перейти в конец если ушли слишком влево

            OnSelectionChanged();
        }
    }

    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
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
            upgradeCostText.text = "MAX"; //если уровень уже макс
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
            xpText.text = GameManager.instance.experience.ToString() + " total experience points";
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
        PlayerPrefs.DeleteAll();
        GameManager.instance.money = 0;
        GameManager.instance.experience = 0;
        GameManager.instance.player.SetLevel(0);
        GameManager.instance.weapon.SetWeaponLevel(0);
        GameObject.Find("Player").GetComponent<Player>().hitpoint = 5;
        GameObject.Find("Player").GetComponent<Player>().maxHitpoint = 5;
        UpdateMenu();
        Debug.Log("Cleared Player Prefs");
    }
}
