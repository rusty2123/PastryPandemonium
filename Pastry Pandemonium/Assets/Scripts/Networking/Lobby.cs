using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : Photon.PunBehaviour
{
    #region Public Variables

    public GameObject roomList, disconnected;
    public Button roomPrefab;
    public NetworkGameManager gameManager;


    #endregion


    #region Private Variables

    private Collider2D[] colliders;
    private RoomInfo[] rooms;
    private string _gameVersion = "1";
    private string roomSelection = "";

    #endregion


    #region MonoBehaviour CallBacks


    void Awake()
    {
        // #Critical
        // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
        PhotonNetwork.autoJoinLobby = true;
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;
        disconnected.SetActive(false);
        clearRoomList();
        populateRoomList();
    }

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
    public bool Connect()
    {
        if (PhotonNetwork.connected)
        {
            Debug.Log("Already Connected");
            return true;
        }
        else
        {
            if(PhotonNetwork.ConnectUsingSettings("1"))
            {
                return true;
            }
            return false;
        }
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void Host()
    {
        //need to decide room naming convention
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
        string roomName = nameRoom(PhotonNetwork.playerName);

        StartCoroutine(createRoom(roomOptions, roomName));
    }

    private string nameRoom(string playerName)
    {
        bool availableName = true;
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        string roomName = playerName + "'s game";
        int i = 1;
        do
        {
            availableName = true;

            foreach (RoomInfo room in rooms)
            {
                if (room.Name == roomName)
                {
                    availableName = false;
                }
            }

            if (!availableName)
            {
                roomName = playerName + i + "'s game";
                ++i;
            }

        } while (!availableName);

        return roomName;
    }

    IEnumerator createRoom(RoomOptions roomOptions, string roomName)
    {
        while(!PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default))
        {
            yield return null;
        }
        SceneManager.LoadScene("Room");
    }

    public void Join()
    {
        if (PhotonNetwork.JoinRoom(roomSelection))
        {
            Debug.Log("joined " + roomSelection);
        }
        else
        {
            Debug.Log("failed to join room");
        }    
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void populateRoomList()
    {
        rooms = PhotonNetwork.GetRoomList();

        foreach (RoomInfo room in rooms)
        {
            if (room.PlayerCount == 1)
            {
                Button newRoom = Instantiate(roomPrefab) as Button;
                newRoom.transform.SetParent(roomList.transform, false);
                newRoom.GetComponentInChildren<Text>().text = room.Name;
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
            roomButtons[i].GetComponentInChildren<Text>().color = Color.white;
        }

        roomSelection = room.GetComponentInChildren<Text>().text;
        room.GetComponentInChildren<Text>().color = Color.magenta;
    }

    #endregion

    #region Photon.PunBehaviour CallBacks


    public override void OnConnectedToMaster()
    {
        Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
    }

    public override void OnReceivedRoomListUpdate()
    {
        clearRoomList();
        populateRoomList();
    }

    public override void OnDisconnectedFromPhoton()
    {
        disconnected.GetComponentInChildren<Text>().text = "You have disconnected";
        disableColliders();
        MultiplayerSetup.selectedCharacter = "";
        Player.characterLocalPlayer = "";
        Player.characterOpponentPlayer = "";
        NetworkGameManager.localReady = false;
        NetworkGameManager.opponentReady = false;
        disconnected.SetActive(true);
    }

    public void ReturnToLobby()
    {
        disconnected.SetActive(false);
        if (PhotonNetwork.connected)
        {
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            SceneManager.LoadScene("mainMenu");
        }
    }

    #endregion

    #region Private Methods

    private void clearRoomList()
    {
        Button[] roomButtons = roomList.GetComponentsInChildren<Button>();

        foreach (Button child in roomButtons)
        {
            Destroy(child.gameObject);
        }
    }

    private void disableColliders()
    {
        //disable all 2d colliders
        colliders = FindObjectsOfType<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    #endregion
}
