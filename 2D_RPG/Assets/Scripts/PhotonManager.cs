using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public string region;
    public InputField roomName;
    public Text roomNameText;
    //public RoomListItem itemPrefab;
    public RoomListItem itemPrefab;
    public Transform content;

    List<RoomInfo> allRoomsInfo = new List<RoomInfo>();


    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to " + PhotonNetwork.CloudRegion);
        Debug.Log("Current ping is " + PhotonNetwork.GetPing());

        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
        Debug.Log("Joined lobby with name " + PhotonNetwork.CurrentLobby);
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
        //JoinOrCreateRoom - зайти в комнату, и если комнаты с таким названием еще нет, тогда создать
        PhotonNetwork.LoadLevel("Entrance");
        //PhotonNetwork.JoinLobby();
        //Debug.Log("Joined lobby with name " + PhotonNetwork.CurrentLobby);
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

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Entrance");
        Debug.Log("Joined room with name " + PhotonNetwork.CurrentRoom.Name);
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
}
