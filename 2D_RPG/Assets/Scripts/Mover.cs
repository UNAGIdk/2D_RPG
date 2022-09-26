using UnityEngine;

public abstract class Mover : Fighter //abstract потому что мы хотим только наследовать от этого класса методы, а не создавать экземпляры класса Mover
{

    private Vector3 originalSize;

    //ВСЕ ЭТИ ПОЛЯ БЫЛИ protected, Я РЕШИЛ СДЕЛАТЬ ИХ private, ЧТОБЫ СКРИПТ Boss.cs ИХ ВИДЕЛ
    protected BoxCollider2D boxCollider; //этот почему-то как private не видится, а как protected видится нормально
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;



    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        originalSize = transform.localScale;
    }

    private void FixedUpdate()
    {
        //переменные для получения данных с клавиатуры
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
    }

    protected virtual void UpdateMotor(Vector3 input, float xSpeed, float ySpeed)
    {
        //Обнулить перемещение
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0); //переменными xSpeed и ySpeed контролируется скорость

        //развернуть игрока в одну сторону при нажатии влево (x = 1), в другую при нажатии вправо (x = -1)
        if (moveDelta.x > 0)
            transform.localScale = originalSize;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);

        //добавляем вектор отталкивания от удара, если таковой есть
        moveDelta += pushDirection;

        //убавляем силу отталкивания с каждым кадром, используя переменную recoverySpeed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed); //.Lerp это, насколько я понял, плавно переместиться в какую-то точку, вроде

        //Ниже, можно поменять слои Blocking и Actor на те что нужны, и тогда сквозь эти слои не будет пропускать
        //Убеждаемся что в выбранном направлении по оси Y можно двигаться, запуская в этом направлении некий box, если он вернет null, тогда позволить перемещение
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Blocking", "Actor"));
        if (hit.collider == null)
        {
            //Перемещение
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        //Убеждаемся что в выбранном направлении по оси X можно двигаться, запуская в этом направлении некий box, если он вернет null, тогда позволить перемещение
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Blocking", "Actor"));
        if (hit.collider == null)
        {
            //Перемещение
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
