using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    //damage section
    public int damagePoint = 1;
    public float pushForce = 5.0f;

    protected override void OnCollide(Collider2D coll)
    {
        if(coll.tag == "Fighter" && coll.name == "Player")
        {
            //������� ����� ������ ������ damage ��� ���� ����� ������� ��� � fighter'y �������� ������ ��� �� ��� ���� ������ (���� ����� � ������)
            Damage damage = new Damage
            {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("RecieveDamage", damage);
        }    
    }
}
