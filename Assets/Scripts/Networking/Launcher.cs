using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Singleton Instance

    public static Launcher Instance;

    #endregion

    #region UI Elements

    [Header("UI Elements")]
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    #endregion

    #region Constants

    private const string LOADING_MENU = "loading";
    private const string TITLE_MENU = "title";
    private const string ROOM_MENU = "room";
    private const string ERROR_MENU = "error";
    private const string FIND_ROOM_MENU = "find room";

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ConnectToMasterServer();
    }

    #endregion

    #region Photon Connection Methods

    private void ConnectToMasterServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        OpenMenu(TITLE_MENU);
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    #endregion

    #region Room Management Methods

    public void CreateRoom()
    {
        if (IsRoomNameValid())
        {
            PhotonNetwork.CreateRoom(roomNameInputField.text); // Create a new Photon room with the specified name
            OpenMenu(LOADING_MENU); // Show loading menu
        }
    }

    private bool IsRoomNameValid()
    {
        return !string.IsNullOrEmpty(roomNameInputField.text); // Check if the room name is valid
    }

    public override void OnJoinedRoom()
    {
        OpenMenu(ROOM_MENU);
        DisplayRoomName();

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ShowErrorMessage(message);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        OpenMenu(LOADING_MENU);
    }

    public override void OnLeftRoom()
    {
        OpenMenu(TITLE_MENU);
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        OpenMenu(LOADING_MENU);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    #endregion

    #region Player Management Methods

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    #endregion

    #region UI Management Methods

    private void DisplayRoomName()
    {
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    private void ShowErrorMessage(string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        OpenMenu(ERROR_MENU);
    }

    private void OpenMenu(string menuName)
    {
        Menu_Manager.Instance.OpenMenu(menuName); // Open specified menu
    }

    #endregion

    #region Game Management Methods

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}


