using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public DataBase data;

    public List<ItemInventory> items = new List<ItemInventory>();

    public GameObject gameObjectShow; //������ ...

    public GameObject InventoryMainObject; //������ ������ ���������
    public int maxItemCount; //������������ ���-�� ��������� � ������

    public Camera cam; //������, � ������� ����������
    public EventSystem eventSystem;

    public int currentId;
    public ItemInventory currentItem;

    public RectTransform movingObject;
    public Vector3 offset; //�������� �������� ������������ �������

    public GameObject background;

    public void Start()
    {
        if (items.Count == 0)
            AddGraphics();

        for (int i = 0; i < maxItemCount; i++) //�������� ���������� ��������� �����
        {
            AddItem(i, data.items[Random.Range(0, data.items.Count)], Random.Range(1, 99));
        }

        UpdateInventory();
    }

    public void Update()
    {
        if (currentId != -1)
            MoveObject();

        if(Input.GetKeyDown(KeyCode.Tab)) //���� ���� tab ����� ������� ��������� active
        {
            background.SetActive(!background.activeSelf);
            if(background.activeSelf)//������ ���� ����� ��� ���� ����� ��������� ��� ����������� ���� ����� ����� �� ������
            {
                UpdateInventory();
            }
        }
    }

    public void SearchForSameItem(Item item, int itemCount)
    {
        for (int i = 0; i < maxItemCount; i++) //�������� �� ���� ������� ���������
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
                if (items[i].id == 0) //���� ������ ������ (� ��� ����� ������ � id 0) ����� � ��� ����� �������� ���-��
                {
                    AddItem(i, item, itemCount);
                    i = maxItemCount;
                }    
            }
        }
    }

    public void AddItem(int id, Item item, int itemCount)
    {
        //���������� ������������ ���������� � �������� � ������ � � ����������� item
        items[id].id = item.id; 
        items[id].itemCount = itemCount;
        items[id].itemGameObject.GetComponent<Image>().sprite = item.image;

        if (itemCount > 1 && item.id != 0)
            items[id].itemGameObject.GetComponentInChildren<Text>().text = itemCount.ToString(); //���-�� ���� ������ ���������� ��������� � text ����������� �� ������� � button
        else
            items[id].itemGameObject.GetComponentInChildren<Text>().text = ""; //��� �� �����, �� ������� ������ ���� ������� ���� ��� 0
    }

    public void AddInventoryItem(int id, ItemInventory invItem)
    {
        //���������� ������������ ���������� � �������� � ������ � � ����������� item
        items[id].id = invItem.id;
        items[id].itemCount = invItem.itemCount;
        items[id].itemGameObject.GetComponent<Image>().sprite = data.items[invItem.id].image;

        if (invItem.itemCount > 1 && invItem.id != 0)
            items[id].itemGameObject.GetComponentInChildren<Text>().text = invItem.itemCount.ToString(); //���-�� ���� ������ ���������� ��������� � text ����������� �� ������� � button
        else
            items[id].itemGameObject.GetComponentInChildren<Text>().text = ""; //��� �� �����, �� ������� ������ ���� ������� ���� ��� 0
    }

    public void AddGraphics() //�������� �� ����������� ������� � ������ ������
    {
        for (int i = 0; i < maxItemCount; i++)
        {
            //����������� ������������ ������, ��������� ��� �� transform ����������� �������
            GameObject newItem = Instantiate(gameObjectShow, InventoryMainObject.transform); //� ����� ���� ��� as GameObject

            newItem.name = i.ToString(); //���������� �������� ��� �� ��� ������ � ����� ���������

            ItemInventory ii = new ItemInventory(); //�������� ���������� ������ ItemInventory\
            ii.itemGameObject = newItem;

            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, 0, 0); //� ������� ������������� ���������� ����������, ��� ��� � ���� ��� ������� ���������� ������
            rt.localScale = new Vector3(1, 1, 1); //���������� ������ ��������
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1); //����� ��������� ��������, ��� ������ ������� �����, ������ ��������� ����� ��

            Button tempButton = newItem.GetComponent<Button>(); //������ ������� �� ���� ��� ������, ������ ��� ��� ���������� ����������������� � ���������� � ���������

            tempButton.onClick.AddListener(delegate { SelectObject(); }); //����������� ����� ������ SelectObject � ������� "������" ��������, �.�. ������� �� ��� �������

            items.Add(ii); //�������� ������� � ��������� ItemInventory "ii"
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
        if(currentId == -1) //���� ������ ������
        {
            currentId = int.Parse(eventSystem.currentSelectedGameObject.name); //������� ��� ������� � �������� ��� � currentId � ���� int
            currentItem = CopyInventoryItem(items[currentId]);
            movingObject.gameObject.SetActive(true); //� ������ ������ ���������� �����-�� �������� � gameObject
            movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].image;

            AddItem(currentId, data.items[0], 0); //�������� � �����-�� ������ 0 ������ ��������� 
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

            movingObject.gameObject.SetActive(false); //��������� ������� ��������
        }
    }

    public void MoveObject()
    {
        Vector3 pos = Input.mousePosition + offset; //��������� �������� = ��������� ���� + offset 
        pos.z = InventoryMainObject.GetComponent<RectTransform>().position.z;
        movingObject.position = cam.ScreenToWorldPoint(pos); //���-�� ������� ���������� �������� ��������� � ������
    }

    public ItemInventory CopyInventoryItem(ItemInventory oldI) //���-�� ������ ��� ���� ����� ����� �� ����� ������ ������, ��� �� ��� ������������
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
    public int id; //����� ...
    public GameObject itemGameObject; //���� ��� GameObject

    public int itemCount; //���������� ����� ����� ���������� ������� � ����� ������ ������������ ���������
}