using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    //������ ���������� ��������
    protected bool collected; //protected ��� ��� private, �� ������ ������ ����� ����������� protected ����������, � ������� �� private

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player1" || coll.name == "Player2(Clone)")
            OnCollect();
    }

    protected virtual void OnCollect()
    {
        collected = true;
    }
}
