using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Transform lookAt; //������, �� ������� ������� (�����), � ��� ���� ����� � ���������� �������� ������ � ����������
    public float boundX = 0.15f;
    public float boundY = 0.05f;
    public float motorXMultiplier = 0.003f;
    public float motorYMultiplier = 0.003f;

    private void Start() //����� ������ � ������ player, ����� � ���� �������� transform � ��������� ���� �������� � ���� lookAt
    {
        lookAt = GameObject.Find("Player").transform;
        transform.position = lookAt.position;
        transform.position += new Vector3(0, 0, -10);
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
            if (motorXMultiplier < 0.98)
                motorXMultiplier += 0.001f;
        }
        else
        {
            if (motorXMultiplier > 0.02)
                motorXMultiplier -= 0.001f;
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
            if (motorYMultiplier < 0.98)
                motorYMultiplier += 0.001f;
        }
        else
        {
            if (motorYMultiplier > 0.02)
                motorYMultiplier -= 0.001f;
        }

        //������� ���� ����� ������������� ���������� ������������ ������������ motorX � motorY, ������� ���� ��� �������� �����������
        //transform.position += new Vector3(delta.x * motorXMultiplier, delta.y * motorYMultiplier, 0);
        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}


