using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : Photon.PunBehaviour
{
    #region Public Variables

    public GameObject roomPanel, roomListItemPrefab;

    #endregion


    #region Private Variables

    private RoomInfo[] roomsList;
    private string _gameVersion = "1";


    #endregion


    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
        PhotonNetwork.autoJoinLobby = false;
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

    public void ListRooms()
    {
        if (roomsList != null)
        {
            for (int i = 0; i < roomsList.Length; ++i)
            {
                GameObject newRoom = Instantiate(roomListItemPrefab) as GameObject;

                if (GUI.Button(new Rect(100, 250 + (110 * i), 160, 30), "Join " + roomsList[i].Name))
                {
                    Join(roomsList[i].Name);
                }
            }
        }
    }

    public bool Join(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {
            Debug.Log("room joined");
            return true;
        }
        return false;
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


    #endregion
}
