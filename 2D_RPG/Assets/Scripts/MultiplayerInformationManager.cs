using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MultiplayerInformationManager : MonoBehaviour
{
    public Text roomName;
    public Text playerName;
    public Text secondPlayerName;
    public Text serverName;
    public Text ping;

    private void Start()
    {
        roomName.text = GameManager.instance.photonManager.phRoomName;
        serverName.text = GameManager.instance.photonManager.phServerName;
    }

    private void Update()
    {
        ping.text = GameManager.instance.photonManager.phPing;
        if(GameManager.instance.photonManager.playingMultiplayer == true && GameManager.instance.photonManager.isFirstPlayer == true)
        {
            /*playerName.text = GameManager.instance.photonManager.phSecondPlayerName;
            secondPlayerName.text = GameManager.instance.photonManager.phPlayerName;*/ //было для player2

            secondPlayerName.text = GameManager.instance.photonManager.phSecondPlayerName;
            playerName.text = GameManager.instance.photonManager.phPlayerName;
        }

        if (GameManager.instance.photonManager.playingMultiplayer == true && GameManager.instance.photonManager.isFirstPlayer == false)
        {
            secondPlayerName.text = GameManager.instance.photonManager.phSecondPlayerName;
            playerName.text = GameManager.instance.photonManager.phPlayerName;
        }
    }
}