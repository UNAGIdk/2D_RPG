using UnityEngine;

public abstract class Mover : Fighter //abstract ������ ��� �� ����� ������ ����������� �� ����� ������ ������, � �� ��������� ���������� ������ Mover
{

    private Vector3 originalSize;

    //��� ��� ���� ���� protected, � ����� ������� �� private, ����� ������ Boss.cs �� �����
    protected BoxCollider2D boxCollider; //���� ������-�� ��� private �� �������, � ��� protected ������� ���������
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;



    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        originalSize = transform.localScale;
    }

    private void FixedUpdate()
    {
        //���������� ��� ��������� ������ � ����������
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
    }

    protected virtual void UpdateMotor(Vector3 input, float xSpeed, float ySpeed)
    {
        //�������� �����������
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0); //����������� xSpeed � ySpeed �������������� ��������

        //���������� ������ � ���� ������� ��� ������� ����� (x = 1), � ������ ��� ������� ������ (x = -1)
        if (moveDelta.x > 0)
            transform.localScale = originalSize;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);

        //��������� ������ ������������ �� �����, ���� ������� ����
        moveDelta += pushDirection;

        //�������� ���� ������������ � ������ ������, ��������� ���������� recoverySpeed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed); //.Lerp ���, ��������� � �����, ������ ������������� � �����-�� �����, �����

        //����, ����� �������� ���� Blocking � Actor �� �� ��� �����, � ����� ������ ��� ���� �� ����� ����������
        //���������� ��� � ��������� ����������� �� ��� Y ����� ���������, �������� � ���� ����������� ����� box, ���� �� ������ null, ����� ��������� �����������
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Blocking", "Actor"));
        if (hit.collider == null)
        {
            //�����������
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        //���������� ��� � ��������� ����������� �� ��� X ����� ���������, �������� � ���� ����������� ����� box, ���� �� ������ null, ����� ��������� �����������
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Blocking", "Actor"));
        if (hit.collider == null)
        {
            //�����������
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
