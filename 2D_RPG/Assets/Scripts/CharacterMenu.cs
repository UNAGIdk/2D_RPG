using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //��������� ����
    public Text levelText, hitpointText, goldText, upgradeCostText, xpText;

    //���� ��� ������
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar; //��� �� ����� ������ ��� xpBar, � ��� �� ����� ������ ������ ������ (scale)

    //����� ���������
    public void OnArrowClick(bool right) //bool ���������� ��� ����������� ����, �� ����� ������� �� �������� (������ -> true, ����� -> false)
    {
        if(right)
        {
            currentCharacterSelection++;
            //���� ����� �� ����� ������
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;

        OnSelectionChanged();
        }
        else
        {
            currentCharacterSelection--;
            //���� ����� �� ����� ������
            if (currentCharacterSelection < GameManager.instance.playerSprites.Count)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1; //������� � ����� ���� ���� ������� �����

            OnSelectionChanged();
        }
    }

    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
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
            upgradeCostText.text = "MAX"; //���� ������� ��� ����
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
            xpText.text = GameManager.instance.experience.ToString() + " total experience points";
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
