using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
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
    [SerializeField] GameObject roomListItemPrefab;

    private const string loadingMenu = "loading";
    private const string titleMenu = "title";
    private const string roomMenu = "room";
    private const string errorMenu = "error";


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Callback when the client successfully connects to the Photon master server.
    /// Joins the lobby automatically upon connection.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// Callback when the client successfully joins a Photon lobby.
    /// Opens the title menu upon joining.
    /// </summary>
    public override void OnJoinedLobby()
    {
        OpenMenu(titleMenu);
        Debug.Log("Joined lobby");
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
            OpenMenu(loadingMenu); // Show loading menu
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
        OpenMenu(roomMenu);
        DisplayRoomName();
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
        OpenMenu(errorMenu);
    }

    /// <summary>
    /// Leaves the current room and shows the loading menu.
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        OpenMenu(loadingMenu);
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        OpenMenu(loadingMenu);
    }

    /// <summary>
    /// Callback when the client successfully leaves a room.
    /// Opens the title menu upon leaving.
    /// </summary>
    public override void OnLeftRoom()
    {
        OpenMenu(titleMenu);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<Room_List_Item>().SetUp(roomList[i]);
        }
    }

    private void OpenMenu(string menuName)
    {
        Menu_Manager.Instance.OpenMenu(menuName); // Open specified menu
    }
}
