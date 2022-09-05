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
        playerName.text = GameManager.instance.photonManager.phPlayerName;

        serverName.text = GameManager.instance.photonManager.phServerName;
    }

    private void Update()
    {
        ping.text = GameManager.instance.photonManager.phPing;
    }

}
