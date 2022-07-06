using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Transform lookAt; //������, �� ������� ������� (�����), � ��� ���� ����� � ���������� �������� ������ � ����������
    public float boundX = 0.15f;
    public float boundY = 0.05f;

    private void Start() //����� ������ � ������ player � ����������� � ����
    {
        lookAt = GameObject.Find("Player").transform;
    }

    //LateUpdate ������ ��� �� ����� �������� ���� ����� ��� ����� ���� ��� ����� ������������, ����� �� ���� ���������� ����� ���������� ����� ��� ����������� ������
    private void LateUpdate()
    {
        Vector3 delta = new Vector3(0, 0, 0);

        //��������, ��������� �� �� ������ ������� �� ��� �
        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX) //���� ����� �� ��� x ���� �� boundX ������ ��� �����
        {
            if(transform.position.x < lookAt.position.x)
            {
                delta.x = deltaX - boundX;
            }
            else
            {
                delta.x = deltaX + boundX;
            }
        }

        //��������, ��������� �� �� ������ ������� �� ��� y
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY) //���� ����� �� ��� y ���� �� boundY ����� ��� ����
        {
            if (transform.position.y < lookAt.position.y)
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
