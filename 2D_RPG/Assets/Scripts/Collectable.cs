using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    //логика коллектабл предмета
    protected bool collected; //protected это как private, но другие классы могут наследовать protected переменные, в отличие от private

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
