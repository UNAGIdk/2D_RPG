using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public string region;
    public InputField roomName;
    public Text roomNameText;
    //public RoomListItem itemPrefab;
    public RoomListItem itemPrefab;
    public Transform content;

    private Text chatLastMessageText;
    private bool chatLastMessageTextReferenceSet;

    public Animator askNamePanelAnimator;
    public Text nicknameResponseText;

    public bool playingMultiplayer;

    List<RoomInfo> allRoomsInfo = new List<RoomInfo>();


    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != "MainMenu" && chatLastMessageTextReferenceSet == false)
        {
            //condition in PhotonManager works now
            chatLastMessageText = FindObjectOfType<ChatText>().GetComponent<Text>();
            Debug.Log("PhotonManager has found chatLastMessageText on " + chatLastMessageText.gameObject.name);
            chatLastMessageTextReferenceSet = true;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to " + PhotonNetwork.CloudRegion);
        Debug.Log("Current ping is " + PhotonNetwork.GetPing());

        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
        Debug.Log("Joined lobby with name " + PhotonNetwork.CurrentLobby);
    }

    public void SendButton(string message)
    {
        photonView.RPC("Send_Data", RpcTarget.All, PhotonNetwork.NickName, message); //отправить в чат ник + сообщение
    }

    [PunRPC] //перед RPC методом обязательно
    private void Send_Data(string nickname, string message)
    {
        chatLastMessageText.text = nickname + ": " + message;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server");
    }

    public void OnCreateRoomButtonClick()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        if(roomNameText.text != "")
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.IsVisible = true;
            PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default);
        }
        PhotonNetwork.LoadLevel("Entrance");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room with name " + PhotonNetwork.CurrentRoom.Name + " was created");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error on creating room!");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
        foreach (RoomInfo info in roomList)
        {
            for (int i = 0; i < allRoomsInfo.Count; i++)
            {
                if (allRoomsInfo[i].masterClientId == info.masterClientId)
                    return;
            }

            RoomListItem listitem = Instantiate(itemPrefab, content);
            if (listitem != null)
            {
                listitem.SetListInfo(info);
                allRoomsInfo.Add(info);
            }
            
        }
    }

    public override void OnJoinedRoom() //вызывается только при СОЗДАНИИ комнаты
    {
        PhotonNetwork.LoadLevel("Entrance");
        Debug.Log("Joined room with name " + PhotonNetwork.CurrentRoom.Name);

        //chatLastMessageText = FindObjectOfType<ChatText>().GetComponent<Text>();
        //Debug.Log("PhotonManager has found chatLastMessageText on " + chatLastMessageText.gameObject.name);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("MainMenu");
    }

    public void JoinButton()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void ConfirmNicknameButtonClick()
    {
        PhotonNetwork.NickName = nicknameResponseText.text;
        askNamePanelAnimator.ResetTrigger("show");
        askNamePanelAnimator.SetTrigger("hide");
        Debug.Log("Photon NickName is now " + PhotonNetwork.NickName);
    }

    public void PlayingMultiplayerToFalse()
    {
        playingMultiplayer = false;
    }

    public void PlayingMultiplayerToTrue()
    {
        playingMultiplayer = true;
    }
}
