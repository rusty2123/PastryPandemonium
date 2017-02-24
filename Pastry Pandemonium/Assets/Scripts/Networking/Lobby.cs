using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : Photon.PunBehaviour
{
    #region Public Variables

    public GameObject roomList;
    public GameObject roomPrefab;

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
        string roomName = PhotonNetwork.playerName + "'s Game";

        if (PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default))
        {
            Debug.Log(roomName + " created");
        }
        else
        {
            Debug.Log("room creation failed");
        }
    }

<<<<<<< HEAD
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

    public void populateRoomList()
=======
    IEnumerator ListRooms()
>>>>>>> origin/master
    {
        roomText.text = "";
        if (roomsList != null)
        {
            for (int i = 0; i < roomsList.Length; ++i)
            {
<<<<<<< HEAD
                GameObject newRoom = Instantiate(roomPrefab) as GameObject;
                newRoom.transform.SetParent(roomList.transform, false);
                newRoom.GetComponentInChildren<Text>().text = roomsList[i].Name;
                newRoom.GetComponent<Button>().onClick.AddListener(() => setRoomSelection(newRoom.GetComponentInChildren<Text>().text));
                Debug.Log("room button instantiated");
=======
                roomText.text = roomText.text + roomsList[i].Name + "\n";
                yield return null;
>>>>>>> origin/master
            }
        }
    }

    private void setRoomSelection(string room)
    {
<<<<<<< HEAD
        this.roomSelection = room;
    }

=======
        Debug.Log(roomsList.Length);
        return true;
        //if (PhotonNetwork.JoinRoom(roomName))
        //{
        //    Debug.Log("room joined");
        //    return true;
        //}
        //return false;
    }

    public Button buttonPrefab;

    IEnumerator populateRoomList()
    {
        if (roomsList != null)
        {
            for (int i = 0; i < roomsList.Length; ++i)
            {
                Button newRoom = Instantiate(buttonPrefab);
                newRoom.GetComponentInChildren<Text>().text = roomsList[i].Name;
                Debug.Log("room button instantiated");
                yield return null;
            }
        }
    }

>>>>>>> origin/master
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
        StartCoroutine("populateRoomList");
    }

    #endregion
}
