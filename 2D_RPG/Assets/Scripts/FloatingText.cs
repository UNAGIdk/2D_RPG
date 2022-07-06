using UnityEngine;
using UnityEngine.UI;

public class FloatingText  //нет смысла заимствовать от MonoBehavior
{
    public bool active; //активен сейчас текст или нет
    public GameObject go; //go stands for gameObject
    public Text text; //содержание
    public Vector3 motion; //направление движения
    public float duration; //длительность нахождения на экране
    public float lastShown; //когда последний раз всплывал


    public void Show() //показать текст
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }


    public void Hide() //скрыть текст
    {
        active = false;
        go.SetActive(active);
    }


    public void UpdateFloatingText()
    {
        if (!active)
            return;

        if (Time.time - lastShown > duration) //если duration время уже прошло, тогда прячем текст
            Hide();

        go.transform.position += motion * Time.deltaTime;
    }
}
