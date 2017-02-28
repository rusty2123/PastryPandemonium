using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : Photon.PunBehaviour
{
    #region Public Variables

    public GameObject roomList;
    public Button roomPrefab;
    public NetworkGameManager gameManager;


    #endregion


    #region Private Variables

    private RoomInfo[] roomsList;
    private string _gameVersion = "1";
    private string roomSelection = "";

    #endregion


    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
        PhotonNetwork.autoJoinLobby = true;
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {

    }

    private void Update()
    {
       
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Start the connection process. 
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        if (PhotonNetwork.connected)
        {
            Debug.Log("Already Connected");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings("1");
        }
    }

    public void Host()
    {
        //need to decide room naming convention
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
        string roomName = PhotonNetwork.playerName + "'s game";

        if (PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default))
        {
            gameManager.LoadNetworkGame();
            Debug.Log(roomName + " created");
        }
        else
        {
            Debug.Log("room creation failed");
        }
    }

    public void Join()
    {
        if (PhotonNetwork.JoinRoom(roomSelection))
        {
            gameManager.LoadNetworkGame();
            Debug.Log("joined " + roomSelection);
        }
        else
        {
            Debug.Log("failed to join room");
        }    
    }

    public void populateRoomList()
    { 
        if (roomsList != null)
        {
            for (int i = 0; i < roomsList.Length; ++i)
            {
                Button newRoom = Instantiate(roomPrefab) as Button;
                newRoom.transform.SetParent(roomList.transform, false);
                newRoom.GetComponentInChildren<Text>().text = roomsList[i].Name;
                newRoom.GetComponent<Button>().onClick.AddListener(() => setRoomSelection(ref newRoom));
                Debug.Log("room button instantiated");
            }
        }
    }

    public void setRoomSelection(ref Button room)
    {
        //every time a new button is clicked, all other buttons' text must change back
        Button[] roomButtons = roomList.GetComponentsInChildren<Button>();

        for(int i = 0; i < roomButtons.Length; ++i)
        {
            string buttonText = roomButtons[i].GetComponentInChildren<Text>().text;
            roomButtons[i].GetComponentInChildren<Text>().text = removeBrackets(buttonText);
        }

        this.roomSelection = room.GetComponentInChildren<Text>().text;
        room.GetComponentInChildren<Text>().text = "[" + room.GetComponentInChildren<Text>().text + "]";
        Debug.Log("you clicked me");
    }

    #endregion

    #region Photon.PunBehaviour CallBacks


    public override void OnConnectedToMaster()
    {
        Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
    }


    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    public override void OnReceivedRoomListUpdate()
    {
        roomsList = PhotonNetwork.GetRoomList();
        populateRoomList();
    }

    #endregion

    #region Public Methods

    private string removeBrackets(string s)
    {
        if(s[0] == '[' && s[s.Length] == ']')
        {
            return s.Substring(1, s.Length - 1);
        }
        else
        {
            return s;
        }
    } 

    #endregion
}
