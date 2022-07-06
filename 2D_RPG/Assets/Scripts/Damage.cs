using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Damage //struct потому что, насколько я понял, мы тут ниче не делаем а только переменные храним
{
    public Vector3 origin; //позиция того кто бъет вроде
    public int damageAmount;
    public float pushForce;
}
