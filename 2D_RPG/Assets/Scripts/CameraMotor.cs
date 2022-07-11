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
        transform.position = lookAt.position + new Vector3(0, 0, -10);
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

        //��������� ���� ��������� �������� ������������ motorX � Y, ������� ������ �������� ������ �������
        
        /*if (lookAt.GetComponent<Player>().x != 0 && motorXMultiplier < 0.01f)
            motorXMultiplier += 0.0001f;
        else if (motorXMultiplier > 0.0002f)
            motorXMultiplier -= 0.0001f;

        if (lookAt.GetComponent<Player>().y != 0 && motorYMultiplier < 0.01f)
            motorYMultiplier += 0.0001f;
        else if(motorYMultiplier > 0.0002f)
            motorYMultiplier -= 0.0001f;*/
        
        //������� ���� ����� ������������� ����� ������������ ������������ motorX � motorY ��� ���������� ������� �������� ������
        transform.position += new Vector3(delta.x * motorXMultiplier, delta.y * motorYMultiplier, 0);
        //transform.position += new Vector3(delta.x, delta.y, 0);
    }
}