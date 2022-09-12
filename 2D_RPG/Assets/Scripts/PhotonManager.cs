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
    public RoomListItem itemPrefab;
    public Transform content;
    private GameObject player2;
    public GameObject player2Prefab;

    private bool chatLastMessageTextReferenceSet;
    private Text chatLastMessageText;
    private Text chatLastMessageText1;
    private Text chatLastMessageText2;
    private Text chatLastMessageText3;
    private Text chatLastMessageText4;

    [HideInInspector] public string phRoomName;
    [HideInInspector] public string phPlayerName;
    [HideInInspector] public string phSecondPlayerName;
    [HideInInspector] public string phServerName;
    [HideInInspector] public string phPing;



    //List<Text> chatLastMessageTexts = new List<Text>();

    public Animator askNamePanelAnimator;
    public Text nicknameResponseText;

    public Button createRoomButton;
    public Button joinRoomButton;

    [HideInInspector] public bool playingMultiplayer;
    [HideInInspector] public bool isFirstPlayer;

    List<RoomInfo> allRoomsInfo = new List<RoomInfo>();


    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
        createRoomButton.GetComponent<Button>().interactable = false;
        joinRoomButton.GetComponent<Button>().interactable = false;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != "MainMenu" && chatLastMessageTextReferenceSet == false)
        {
            //condition in PhotonManager works now
            chatLastMessageText = FindObjectOfType<ChatText>().GetComponent<Text>();
            chatLastMessageText1 = FindObjectOfType<ChatText1>().GetComponent<Text>();
            chatLastMessageText2 = FindObjectOfType<ChatText2>().GetComponent<Text>();
            chatLastMessageText3 = FindObjectOfType<ChatText3>().GetComponent<Text>();
            chatLastMessageText4 = FindObjectOfType<ChatText4>().GetComponent<Text>();
            Debug.Log("PhotonManager has found chatLastMessageText on " + chatLastMessageText.gameObject.name);
            chatLastMessageTextReferenceSet = true;
        }
        phPing = PhotonNetwork.GetPing().ToString();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to " + PhotonNetwork.CloudRegion);
        Debug.Log("Current ping is " + PhotonNetwork.GetPing());
        phServerName = PhotonNetwork.CloudRegion;

        if (!PhotonNetwork.InLobby)
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
        chatLastMessageText4.text = chatLastMessageText3.text;
        chatLastMessageText3.text = chatLastMessageText2.text;
        chatLastMessageText2.text = chatLastMessageText1.text;
        chatLastMessageText1.text = chatLastMessageText.text;
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

        if (roomNameText.text != "")
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.IsVisible = true;
            PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default);
            phRoomName = roomName.text;
            SetPlayerAsFirst();
        }
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
        Debug.Log("OnRoomListUpdate has been called");
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
        SceneTransition.instance.SceneSwitch();
        Debug.Log("Joined room with name " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        try
        {
            OnPlayerConnectedToRoom(newPlayer.NickName);
        }
        catch (System.Exception)
        {
        }       
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("MainMenu");
    }

    public void JoinButton()
    {
        PhotonNetwork.JoinRoom(roomName.text);
        SetPlayerAsSecond();
    }

    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void ConfirmNicknameButtonClick()
    {
        PhotonNetwork.NickName = nicknameResponseText.text;
        phPlayerName = nicknameResponseText.text;
        askNamePanelAnimator.ResetTrigger("show");
        askNamePanelAnimator.SetTrigger("hide");
        Debug.Log("Photon NickName is now " + PhotonNetwork.NickName);
        createRoomButton.GetComponent<Button>().interactable = true;
        joinRoomButton.GetComponent<Button>().interactable = true;
    }

    public void PlayingMultiplayerToFalse()
    {
        playingMultiplayer = false;
    }

    public void PlayingMultiplayerToTrue()
    {
        playingMultiplayer = true;
    }

    public void OnPlayerConnectedToRoom(string newPlayerNickname)
    {
        photonView.RPC("Player_Connect_Rpc", RpcTarget.All, newPlayerNickname);
    }

    [PunRPC]
    private void Player_Connect_Rpc(string playerNickname)
    {
        GameManager.instance.ShowText(playerNickname + " зашел в комнату", 24, Color.white, GameObject.Find("Main Camera").transform.position + new Vector3(-0.54f, 0.94f, 0), Vector3.up * 4, 5.0f);
    }

    public void PhotonLoadScene()
    {
        PhotonNetwork.LoadLevel("Entrance");
    }

    public void SetPlayerAsFirst()
    {
        isFirstPlayer = true;
    }

    public void SetPlayerAsSecond()
    {
        isFirstPlayer = false;
    }

    public void CreatePlayer2()
    {
        Debug.Log("CreatePlayer2 in PhotonManager was called");
        player2 = PhotonNetwork.Instantiate(player2Prefab.name, GameObject.Find("Player2SpawnPoint").transform.position, Quaternion.identity);
        if (GameObject.Find("Player2(Clone)").GetComponent<PhotonView>().IsMine && isFirstPlayer == true)
            Destroy(GameObject.Find("Player2(Clone)"));

        if (GameObject.Find("Player2(Clone)").GetComponent<PhotonView>().IsMine && isFirstPlayer == true)
            Destroy(GameObject.Find("Player2(Clone)"));

        if (isFirstPlayer == true)
            GameObject.Find("Player2(Clone)").GetComponent<AudioListener>().enabled = false;

        if (isFirstPlayer == false)
            GameObject.Find("Player1").GetComponent<AudioListener>().enabled = false;
    }
}
