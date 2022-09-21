using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float xSpeed;
    private Vector3 moveDelta; //0.00006f = 6e-05

    private void Start()
    {
        moveDelta = new Vector3(xSpeed, 0f, 0f);
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
    }

    void Update()
    {
        transform.Translate(moveDelta.x * Time.deltaTime, 0, 0); // * Time.deltaTime
        if(transform.localPosition.x > 50.0f)
            Destroy(gameObject);
    }
}
