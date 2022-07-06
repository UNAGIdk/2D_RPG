using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider2D))] //накинуть на объект boxCollider2D если на объекте еще нет такого компонента
//комменчу так как такое лучше не оставлять, потому что boxcollider нужно еще настроить, а чтобы этого не забыть лучше его накидывать на объект вручную
public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter; //фильтр, который будет определять с чем именно мы ходим be collidable with
    public BoxCollider2D boxCollider;
    protected Collider2D[] hits = new Collider2D[10]; //массив объектов с которыми мы collided в текущем frame

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

            //очищаю массив
            hits[i] = null;
        }
    }
    
    protected virtual void OnCollide (Collider2D coll)
    {
        Debug.Log("OnCollide was not implemented in" + this.name);
    }
}
