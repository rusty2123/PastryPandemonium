using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;
using UnityEngine.UI;
using ExitGames = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : PunBehaviour
{

    public GameObject playerPrefab;

    private RoomInfo[] roomsList;
    private PunTurnManager turnManager;
    public InputField InputField;
    public string UserId;
    public string previousRoom;
    const string NickNamePlayerPrefsKey = "NickName";

    // Use this for initialization
    void Start()
    {
        connect();
    }
    void Awake()
    {
        PhotonNetwork.autoJoinLobby = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public override void OnConnectedToMaster()
    {
        // after connect 
        UserId = PhotonNetwork.player.UserId;

        // after timeout: re-join "old" room (if one is known)
        if (!string.IsNullOrEmpty(this.previousRoom))
        {
            Debug.Log("ReJoining previous room");
            PhotonNetwork.ReJoinRoom(previousRoom);
            previousRoom = null;       // we only will try to re-join once. if this fails, we will get into a random/new room
        }
    }

    public override void OnJoinedLobby()
    {
        OnConnectedToMaster(); // this way, it does not matter if we join a lobby or not
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.room.Name);
        //PhotonNetwork.Instantiate(playerPrefab.name, Vector3.up * 5, Quaternion.identity, 0);
        previousRoom = PhotonNetwork.room.Name;

    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        previousRoom = null;
    }

    public override void OnConnectionFail(DisconnectCause cause)
    {
        Debug.Log("Disconnected due to: " + cause + ". this.previousRoom: " + this.previousRoom);
    }
    private void OnGUI()
    {
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else if (PhotonNetwork.room == null)
        {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Host"))
            {
                host();
            }

            if (roomsList != null)
            {
                for (int i = 0; i < roomsList.Length; ++i)
                {
                    if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].Name))
                    {
                        join(roomsList[i].Name);
                    }
                }
            }
        }
    }

    void OnReceivedRoomListUpdate()
    {
        roomsList = PhotonNetwork.GetRoomList();
    }

    public void applyUserIdAndConnect()
    {
        string nickName = "DemoNick";
        if (this.InputField != null && !string.IsNullOrEmpty(this.InputField.text))
        {
            nickName = this.InputField.text;
            PlayerPrefs.SetString(NickNamePlayerPrefsKey, nickName);
        }
        //if (string.IsNullOrEmpty(UserId))
        //{
        //    this.UserId = nickName + "ID";
        //}
        Debug.Log("Nickname: " + nickName + " userID: " + this.UserId, this);


        if (PhotonNetwork.AuthValues == null)
        {
            PhotonNetwork.AuthValues = new AuthenticationValues();
        }
        //else
        //{
        //    Debug.Log("Re-using AuthValues. UserId: " + PhotonNetwork.AuthValues.UserId);
        //}

        PhotonNetwork.playerName = nickName;
        connect();

        // this way we can force timeouts by pausing the client (in editor)
        PhotonHandler.StopFallbackSendAckThread();
    }

    private bool connect()
    {
        if (PhotonNetwork.connected)
        {
            Debug.Log("Already Connected");
            return true;
        }
        else
        {
            return PhotonNetwork.ConnectUsingSettings("v1.0");
        }
    }

    private bool host()
    {
        //need to decide room naming convention
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
        string roomName = "my room";

        if (PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default))
        {
            Debug.Log("room created");
            return true;
        }
        return false;
    }

    private bool join(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {
            Debug.Log("room joined");
            return true;
        }
        return false;
    }

    [PunRPC]
    public void sendPlacePiece(int moveToIndex)
    {
    }
}
