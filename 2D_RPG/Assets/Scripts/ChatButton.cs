using UnityEngine;

public class ChatButton : MonoBehaviour
{
    public Animator chatAnimator;
    private bool chatOpened;

    private void Start()
    {
        chatOpened = false;
    }

    public void OnChatClickButton()
    {
        if(chatOpened == false)
        {
            chatAnimator.SetTrigger("hide");
            chatAnimator.ResetTrigger("show");
            chatOpened = true;
        }
        else
        {
            chatAnimator.SetTrigger("show");
            chatAnimator.ResetTrigger("hide");
            chatOpened = false;
        }
    }
}
