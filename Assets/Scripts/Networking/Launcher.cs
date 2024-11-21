using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [Header("UI Elements")]
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    private const string LOADING_MENU = "loading";
    private const string TITLE_MENU = "title";
    private const string ROOM_MENU = "room";
    private const string ERROR_MENU = "error";
    private const string FIND_ROOM_MENU = "find room";


    private void Awake()
    {
        Instance = this;
    }


    /// <summary>
    /// Initializes the connection to the Photon master server on startup.
    /// </summary>
    private void Start()
    {
        ConnectToMasterServer();
    }

    private void ConnectToMasterServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Callback when the client successfully connects to the Photon master server.
    /// Joins the lobby automatically upon connection.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /// <summary>
    /// Callback when the client successfully joins a Photon lobby.
    /// Opens the title menu upon joining.
    /// </summary>
    public override void OnJoinedLobby()
    {
        OpenMenu(TITLE_MENU);
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    /// <summary>
    /// Creates a room with the name specified in the input field.
    /// Opens a loading menu while waiting for room creation.
    /// </summary>
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

    /// <summary>
    /// Callback when the client successfully joins a room.
    /// Opens the room menu upon joining.
    /// </summary>
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

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    private void DisplayRoomName()
    {
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    /// <summary>
    /// Callback when the creation of a room fails.
    /// Opens an error menu and displays an error message.
    /// </summary>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ShowErrorMessage(message);
    }

    private void ShowErrorMessage(string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        OpenMenu(ERROR_MENU);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    /// <summary>
    /// Leaves the current room and shows the loading menu.
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        OpenMenu(LOADING_MENU);
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        OpenMenu(LOADING_MENU);
    }

    /// <summary>
    /// Callback when the client successfully leaves a room.
    /// Opens the title menu upon leaving.
    /// </summary>
    public override void OnLeftRoom()
    {
        OpenMenu(TITLE_MENU);
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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    private void OpenMenu(string menuName)
    {
        Menu_Manager.Instance.OpenMenu(menuName); // Open specified menu
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


