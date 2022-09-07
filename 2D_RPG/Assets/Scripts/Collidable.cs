using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider2D))] //�������� �� ������ boxCollider2D ���� �� ������� ��� ��� ������ ����������
//�������� ��� ��� ����� ����� �� ���������, ������ ��� boxcollider ����� ��� ���������, � ����� ����� �� ������ ����� ��� ���������� �� ������ �������
public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter; //������, ������� ����� ���������� � ��� ������ �� ����� be collidable with
    private BoxCollider2D boxCollider;
    protected Collider2D[] hits = new Collider2D[10]; //������ �������� � �������� �� collided � ������� frame

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    protected virtual void Update()
    {
        //collision work
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            OnCollide(hits[i]);

            //������ ������
            hits[i] = null;
        }
    }
    
    protected virtual void OnCollide (Collider2D coll)
    {
        Debug.Log("OnCollide was not implemented in" + this.name);
    }
}
