using UnityEngine;
using UnityEngine.UI;

public class ChatConfirmButton : MonoBehaviour
{
    private PhotonManager photonManager;
    public Text chatMessageText;
    public InputField chatInputField;
    void Start()
    {
        photonManager = FindObjectOfType<PhotonManager>();
        Debug.Log("Chat confirm button found PhotonManager on game object " + photonManager.gameObject.name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            ChatConfirmButtonClick();
    }

    public void ChatConfirmButtonClick()
    {
        photonManager.SendButton(chatMessageText.text);
        chatInputField.text = "";
    }
}
