using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float xSpeed;
    private Vector3 moveDelta; //0.00006f = 6e-05

    private void Start()
    {
        moveDelta = new Vector3(xSpeed, 0f, 0f);
    }

    void Update()
    {
        transform.Translate(moveDelta.x, 0, 0); // * Time.deltaTime
    }
}
