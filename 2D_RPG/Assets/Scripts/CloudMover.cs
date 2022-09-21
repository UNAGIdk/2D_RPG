using UnityEngine;
using UnityEngine.UI;

public class CloudMover : MonoBehaviour
{
    public Sprite cloudSprite;
    public float instantiateCooldown;
    private float lastInstantiated;
    private Transform parentTransform;
    private Vector3 moveDelta;

    void Start()
    {
        moveDelta = new Vector3(1.0f, 0f, 0f);
        parentTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if(Time.time - lastInstantiated > instantiateCooldown)
        {
            Instantiate(cloudSprite, parentTransform);
            lastInstantiated = Time.time;
        }
        transform.Translate(moveDelta.x * Time.time, 0, 0);
    }
}
