using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

}

[System.Serializable] //позволяет иметь доступ со всего юнити к любому пункту этого скрипта

public class Item
{
    public int id; //номер предмета
    public string name; //имя предмета
    public Sprite image; //спрайт предмета
}
