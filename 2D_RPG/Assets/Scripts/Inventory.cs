using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public DataBase data;

    public List<ItemInventory> items = new List<ItemInventory>();

    public GameObject gameObjectShow; //объект ...

    public GameObject InventoryMainObject; //объект самого инвентаря
    public int maxItemCount; //максимальное кол-во предметов в ячейке

    public Camera cam; //камера, к которой приклеемся
    public EventSystem eventSystem;

    public int currentId;
    public ItemInventory currentItem;

    public RectTransform movingObject;
    public Vector3 offset; //смещение предмета относительно курсора

    public GameObject background;

    public void Start()
    {
        if (items.Count == 0)
            AddGraphics();

        for (int i = 0; i < maxItemCount; i++) //тестовое заполнение рандомных ячеек
        {
            AddItem(i, data.items[Random.Range(0, data.items.Count)], Random.Range(1, 99));
        }

        UpdateInventory();
    }

    public void Update()
    {
        if (currentId != -1)
            MoveObject();

        if(Input.GetKeyDown(KeyCode.Tab)) //если жмем tab тогда сделать инвентарь active
        {
            background.SetActive(!background.activeSelf);
            if(background.activeSelf)//данный блок нужен для того чтобы инвентарь мог обновляться даже тогда когда он закрыт
            {
                UpdateInventory();
            }
        }
    }

    public void SearchForSameItem(Item item, int itemCount)
    {
        for (int i = 0; i < maxItemCount; i++) //пройтись по всем ячейкам инвентаря
        {
            if(items[i].id == item.id)
            {
                if(items[0].itemCount < 64)
                {
                    items[i].itemCount += itemCount;

                    if(items[i].itemCount > 64)
                    {
                        itemCount = items[i].itemCount - 64;
                        items[i].itemCount = 32;
                    }
                    else
                    {
                        itemCount = 0;
                        i = maxItemCount;
                    }
                }
            }
        }

        if(maxItemCount > 0)
        {
            for (int i = 0; i < maxItemCount; i++)
            {
                if (items[i].id == 0) //если ячейка пустая (в ней лежит объект с id 0) тогда в нее можно положить что-то
                {
                    AddItem(i, item, itemCount);
                    i = maxItemCount;
                }    
            }
        }
    }

    public void AddItem(int id, Item item, int itemCount)
    {
        //установить соответствие информации у элемента в списке и у переданного item
        items[id].id = item.id; 
        items[id].itemCount = itemCount;
        items[id].itemGameObject.GetComponent<Image>().sprite = item.image;

        if (itemCount > 1 && item.id != 0)
            items[id].itemGameObject.GetComponentInChildren<Text>().text = itemCount.ToString(); //что-то типа вывода количества элементов в text приклеенный по дефолту в button
        else
            items[id].itemGameObject.GetComponentInChildren<Text>().text = ""; //тот же вывод, но пустого текста если предмет один или 0
    }

    public void AddInventoryItem(int id, ItemInventory invItem)
    {
        //установить соответствие информации у элемента в списке и у переданного item
        items[id].id = invItem.id;
        items[id].itemCount = invItem.itemCount;
        items[id].itemGameObject.GetComponent<Image>().sprite = data.items[invItem.id].image;

        if (invItem.itemCount > 1 && invItem.id != 0)
            items[id].itemGameObject.GetComponentInChildren<Text>().text = invItem.itemCount.ToString(); //что-то типа вывода количества элементов в text приклеенный по дефолту в button
        else
            items[id].itemGameObject.GetComponentInChildren<Text>().text = ""; //тот же вывод, но пустого текста если предмет один или 0
    }

    public void AddGraphics() //отвечает за отображение спрайта в каждой ячейке
    {
        for (int i = 0; i < maxItemCount; i++)
        {
            //скопировать отображаемый объект, поместить его по transform копируемого объекта
            GameObject newItem = Instantiate(gameObjectShow, InventoryMainObject.transform); //в конце было еще as GameObject

            newItem.name = i.ToString(); //установить предмету имя по его номеру в стаке предметов

            ItemInventory ii = new ItemInventory(); //создание экземпляра класса ItemInventory\
            ii.itemGameObject = newItem;

            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, 0, 0); //у каждого отображаемого сбрасываем координату, так как у всех них нулевая координата разная
            rt.localScale = new Vector3(1, 1, 1); //сбрасываем размер предмета
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1); //когда добавляем элементы, или держим элемент мышью, размер оставался таким же

            Button tempButton = newItem.GetComponent<Button>(); //каждый предмет по сути это кнопка, потому что нам необходимо взаимодействовать с предметами в инвентаре

            tempButton.onClick.AddListener(delegate { SelectObject(); }); //привязываем вызов метода SelectObject к нажатию "кнопки" предмета, т.е. нажатию на сам предмет

            items.Add(ii); //добавить предмет в экземпляр ItemInventory "ii"
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < maxItemCount; i++)
        {
            if(items[i].id != 0 && items[i].itemCount > 1)
                items[i].itemGameObject.GetComponentInChildren<Text>().text = items[i].itemCount.ToString();
            else
                items[i].itemGameObject.GetComponentInChildren<Text>().text = "";

            items[i].itemGameObject.GetComponent<Image>().sprite = data.items[items[i].id].image;
        }
    }

    public void SelectObject()
    {
        if(currentId == -1) //если ячейка пустая
        {
            currentId = int.Parse(eventSystem.currentSelectedGameObject.name); //считать имя объекта и записать его в currentId в виде int
            currentItem = CopyInventoryItem(items[currentId]);
            movingObject.gameObject.SetActive(true); //в данный момент происходит какое-то действие с gameObject
            movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].image;

            AddItem(currentId, data.items[0], 0); //добавить в такую-то ячейку 0 пустых предметов 
        }
        else
        {
            ItemInventory ii = items[int.Parse(eventSystem.currentSelectedGameObject.name)];

            if(currentItem.id != ii.id)
            {
                AddInventoryItem(currentId, ii);
                AddInventoryItem(int.Parse(eventSystem.currentSelectedGameObject.name), currentItem);
            }
            else 
            {
                if (ii.itemCount + currentItem.itemCount <= 64)
                    ii.itemCount += currentItem.itemCount;
                else
                {
                    AddItem(currentId, data.items[ii.id], ii.itemCount + currentItem.itemCount - 64);
                    ii.itemCount = 64;
                }

                ii.itemGameObject.GetComponentInChildren<Text>().text = ii.itemCount.ToString();

            }

            currentId = -1;

            movingObject.gameObject.SetActive(false); //закончили перенос предмета
        }
    }

    public void MoveObject()
    {
        Vector3 pos = Input.mousePosition + offset; //положение предмета = положение мыши + offset 
        pos.z = InventoryMainObject.GetComponent<RectTransform>().position.z;
        movingObject.position = cam.ScreenToWorldPoint(pos); //как-то позиция двигаемого предмета привязана к камере
    }

    public ItemInventory CopyInventoryItem(ItemInventory oldI) //что-то нужное для того чтобы когда мы брали мышкой ячейку, все из нее копировалось
    {
        ItemInventory newI = new ItemInventory();

        newI.id = oldI.id;
        newI.itemGameObject = oldI.itemGameObject;
        newI.itemCount = oldI.itemCount;

        return newI;
    }
}

[System.Serializable]

public class ItemInventory
{
    public int id; //номер ...
    public GameObject itemGameObject; //поле для GameObject

    public int itemCount; //переменная нужна чтобы показывать сколько в одной ячейке определенных предметов
}