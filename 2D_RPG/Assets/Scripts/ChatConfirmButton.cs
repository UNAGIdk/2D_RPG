using UnityEngine;
using UnityEngine.UI;

public class ChatConfirmButton : MonoBehaviour
{
    private PhotonManager photonManager;
    public Text chatMessageText;
    void Start()
    {
        photonManager = FindObjectOfType<PhotonManager>();
        Debug.Log("Chat confirm button found PhotonManager on game object " + photonManager.gameObject.name);
    }

    public void ChatConfirmButtonClick()
    {
        photonManager.SendButton(chatMessageText.text);
    }
}
