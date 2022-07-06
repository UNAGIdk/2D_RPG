using UnityEngine;
using UnityEngine.UI;

public class FloatingText  //��� ������ ������������ �� MonoBehavior
{
    public bool active; //������� ������ ����� ��� ���
    public GameObject go; //go stands for gameObject
    public Text text; //����������
    public Vector3 motion; //����������� ��������
    public float duration; //������������ ���������� �� ������
    public float lastShown; //����� ��������� ��� ��������


    public void Show() //�������� �����
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }


    public void Hide() //������ �����
    {
        active = false;
        go.SetActive(active);
    }


    public void UpdateFloatingText()
    {
        if (!active)
            return;

        if (Time.time - lastShown > duration) //���� duration ����� ��� ������, ����� ������ �����
            Hide();

        go.transform.position += motion * Time.deltaTime;
    }
}
