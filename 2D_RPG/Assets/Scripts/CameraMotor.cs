using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Transform lookAt; //ќбъект, за которым следить (игрок), в это поле нужно в дальнейшем засунуть игрока в инспекторе
    public float boundX = 0.15f;
    public float boundY = 0.05f;

    private void Start() //найти объект с именем player, вз€ть с него параметр transform и присвоить этот параметр в поле lookAt
    {
        lookAt = GameObject.Find("Player").transform;
    }

    //LateUpdate потому что мы хотим вызывать этот метод уже после того как игрок переместилс€, чтобы не было необходимо ждать следующего кадра дл€ перемещени€ камеры
    private void LateUpdate()
    {
        Vector3 delta = new Vector3(0, 0, 0);

        //ѕроверка, находимс€ ли мы внутри границы по оси х
        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX) //если игрок по оси x ушел за boundX вправо или влево
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

        //ѕроверка, находимс€ ли мы внутри границы по оси y
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY) //если игрок по оси y ушел за boundY вверх или вниз
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
