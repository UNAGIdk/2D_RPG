using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Transform lookAt; //Объект, за которым следить (игрок), в это поле нужно в дальнейшем засунуть игрока в инспекторе
    public float boundX = 0.15f;
    public float boundY = 0.05f;
    public float motorXMultiplier = 0.003f;
    public float motorYMultiplier = 0.003f;

    private void Start() //найти объект с именем player, взять с него параметр transform и присвоить этот параметр в поле lookAt
    {
        lookAt = GameObject.Find("Player").transform;
        transform.position = lookAt.position + new Vector3(0, 0, -10);
    }

    //LateUpdate потому что мы хотим вызывать этот метод уже после того как игрок переместился, чтобы не было необходимо ждать следующего кадра для перемещения камеры
    private void LateUpdate()
    {
        Vector3 delta = new Vector3(0, 0, 0);

        //Проверка, находимся ли мы внутри границы по оси х
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

        //Проверка, находимся ли мы внутри границы по оси y
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

        //структура ниже позволяет изменять коэффициенты motorX и Y, которые делают движение камеры плавнее
        
        /*if (lookAt.GetComponent<Player>().x != 0 && motorXMultiplier < 0.01f)
            motorXMultiplier += 0.0001f;
        else if (motorXMultiplier > 0.0002f)
            motorXMultiplier -= 0.0001f;

        if (lookAt.GetComponent<Player>().y != 0 && motorYMultiplier < 0.01f)
            motorYMultiplier += 0.0001f;
        else if(motorYMultiplier > 0.0002f)
            motorYMultiplier -= 0.0001f;*/
        
        //строчку ниже можно раскомментить чтобы использовать коэффициенты motorX и motorY при вычислении вектора движения камеры
        transform.position += new Vector3(delta.x * motorXMultiplier, delta.y * motorYMultiplier, 0);
        //transform.position += new Vector3(delta.x, delta.y, 0);
    }
}