using UnityEngine;
using UnityEngine.UI;

public class CloudParent : MonoBehaviour
{
    public GameObject cloudSprite;
    public float instantiateCooldown;
    private float lastInstantiated;
    private Transform parentTransform;

    void Start()
    {
        parentTransform = GetComponent<Transform>();
        Instantiate(cloudSprite, parentTransform);
    }

    void Update()
    {
        if(Time.time - lastInstantiated > instantiateCooldown)
        {
            Instantiate(cloudSprite, parentTransform);
            lastInstantiated = Time.time;
        }
    }
}
