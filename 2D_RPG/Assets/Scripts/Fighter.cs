using UnityEngine;

public class Fighter : MonoBehaviour
{
    //public fields
    public int hitpoint = 10;
    public int maxHitpoint = 10;
    public float pushRecoverySpeed = 0.2f;

    //����������
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    //������������
    protected Vector3 pushDirection;

    //��� Fighter ����� �������� ���� � �������
    protected virtual void RecieveDamage(Damage damage) //virtual ������� ����� � ���������� override, ����� ��
    {
        if(Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitpoint -= damage.damageAmount;
            pushDirection = (transform.position - damage.origin).normalized * damage.pushForce;

            GameManager.instance.ShowText(damage.damageAmount.ToString(), 20, Color.red, transform.position, Vector3.zero, 0.5f); //������� ���-�� ������ ������� ������

            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Death();
            }
        }
    }

    protected virtual void Death()
    {
        Debug.Log("death was not overriden");
    }
}
