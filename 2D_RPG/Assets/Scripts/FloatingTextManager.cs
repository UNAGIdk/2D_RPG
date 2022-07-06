using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer; //���� � ������� ����� ���������� �����
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>(); //������ ����������� ������ floating text

    private void Update()
    {
        foreach (FloatingText text in floatingTexts)
            text.UpdateFloatingText();
    }

    public void Show(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.text.text = message; //���������� text � ���������� text � ������� floatingText
        floatingText.text.fontSize = fontSize;
        floatingText.text.color = color;
        //���� ��������� ����� = ����� ��� ���� ����� ���������� ������ �� position ������� ���������, ��� ��� � ���������� (����� ����������� � ����������) ���� ������� ���������
        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position); //������ ������� Tag "MainCamera"
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();

    }

    private FloatingText GetFloatingText()
    {
        //���� ��� ������� ��� ����� �� ������ floatingTexts FloatingText � �������� active ����� �� false
        //������ � for � if �������� � ��������� ��� ��� �� ��� ����� �������

        /*for (int i = 0; i < floatingTexts.Count; i++)
        {
            if (!floatingTexts[i].active)
                text = floatingTexts[i];
        }*/

        FloatingText text = floatingTexts.Find(t => !t.active);

        if (text == null)
        {
            text = new FloatingText();
            text.go = Instantiate(textPrefab); //������� ����� game object �� ������ textPrefab
            text.go.transform.SetParent(textContainer.transform); //����������� ����� � ���������� transform ������������� ������� SetParent
            text.text = text.go.GetComponent<Text>(); //��������� text � ������� text ��������� �������� ������ �� ���������� Text � gameObject

            floatingTexts.Add(text);
        }

        return text;
    }
}
