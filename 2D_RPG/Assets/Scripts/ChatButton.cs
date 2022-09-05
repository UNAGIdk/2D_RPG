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
            chatAnimator.ResetTrigger("hide");
            chatAnimator.SetTrigger("show");
            GameManager.instance.ProhibitPlayerMoving();
            GameManager.instance.ProhibitPlayerSwing();
            chatOpened = true;
        }
        else
        {
            chatAnimator.ResetTrigger("show");
            chatAnimator.SetTrigger("hide");
            GameManager.instance.AllowPlayerMoving();
            GameManager.instance.AllowPlayerSwing();
            chatOpened = false;
        }
    }
}
