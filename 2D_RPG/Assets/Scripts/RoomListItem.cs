using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class RoomListItem : MonoBehaviour
{
    public Text roomNameText;
    public Text playerAmountText;

    public void SetListInfo(RoomInfo info)
    {
        roomNameText.text = info.Name;
        playerAmountText.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void JoinToListRoom()
    {
        PhotonNetwork.JoinRoom(roomNameText.text);
    }
}
