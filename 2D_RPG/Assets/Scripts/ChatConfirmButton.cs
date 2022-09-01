using UnityEngine;

public class ChatConfirmButton : MonoBehaviour
{
    private PhotonManager photonManager;
    void Start()
    {
        photonManager = FindObjectOfType<PhotonManager>();
        Debug.Log("Find PhotonManager on game object " + photonManager.gameObject.name);
    }

    public void ChatConfirmButtonClick()
    {
        photonManager.SendButton();
    }
}
