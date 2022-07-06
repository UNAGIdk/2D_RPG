using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer; //ПОЛЕ В КОТОРОЕ БУДЕТ ВЫВОДИТЬСЯ ТЕКСТ
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>(); //список экземпляров класса floating text

    private void Update()
    {
        foreach (FloatingText text in floatingTexts)
            text.UpdateFloatingText();
    }

    public void Show(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.text.text = message; //переменная text в компоненте text у объекта floatingText
        floatingText.text.fontSize = fontSize;
        floatingText.text.color = color;
        //ниже структура после = нужна для того чтобы показатели взятые из position взялись корректно, так как у интерфейса (текст принадлежит к интерфейсу) своя система координат
        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position); //камера имеющая Tag "MainCamera"
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();

    }

    private FloatingText GetFloatingText()
    {
        //ниже два способа как взять из списка floatingTexts FloatingText у которого active стоит на false
        //способ с for и if оставляю в комментах так как он мне более понятен

        /*for (int i = 0; i < floatingTexts.Count; i++)
        {
            if (!floatingTexts[i].active)
                text = floatingTexts[i];
        }*/

        FloatingText text = floatingTexts.Find(t => !t.active);

        if (text == null)
        {
            text = new FloatingText();
            text.go = Instantiate(textPrefab); //создаем новый game object на основе textPrefab
            text.go.transform.SetParent(textContainer.transform); //привязываем текст к компоненту transform родительского объекта SetParent
            text.text = text.go.GetComponent<Text>(); //переменой text в объекте text присвоить значение взятое из компонента Text у gameObject

            floatingTexts.Add(text);
        }

        return text;
    }
}
