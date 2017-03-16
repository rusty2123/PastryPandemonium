using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkGameManager : Photon.PunBehaviour
{

    public Game game = Game.gameInstance;
    public App app;
    public static Player localPlayer, opponentPlayer;
    public static int placeIndex = 0, removeIndex = 0, moveFromIndex = 0, moveToIndex = 0, flyFromIndex = 0, flyToIndex = 0;

    #region Photon Messages

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Room");
    }

    #endregion


    #region Public Methods

    public bool isMasterClient()
    {
        if (PhotonNetwork.isMasterClient)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void changePlayer()
    {
        //code 4 for changing player
        byte code = 4;
        //data must be sent in byte array
        byte[] content = new byte[] { };
        PhotonNetwork.RaiseEvent(code, content, true, null);
        //PhotonView photonView = PhotonView.Get(this);
        //photonView.RPC("changePlayerRPC", PhotonTargets.All);
    }

    public void placePiece(int i)
    {
        //code 0 for placing piece
        byte code = 0;
        //data must be sent in byte array
        byte[] content = new byte[] { (byte)i };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void removePiece(int i)
    {
        //code 1 for removing piece
        byte code = 1;
        //data must be sent in byte array
        byte[] content = new byte[] { (byte)i };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void movePiece(int from, int to)
    {
        //code 2 for moving piece
        byte code = 2;
        //data must be sent in byte array
        byte[] content = new byte[] { (byte)from, (byte)to };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void flyPiece(int from, int to)
    {
        //code 3 for flying piece
        byte code = 3;
        //data must be sent in byte array
        byte[] content = new byte[] { (byte)from, (byte)to };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void chooseCharacter(int character)
    {
        //code 4 for your selected character
        byte code = 4;
        //data must be sent in byte array
        byte[] content = new byte[] { (byte)character };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void ready(int ready)
    {
        //code 5 for readying up
        byte code = 5;
        //data must be sent in byte array
        byte[] content = new byte[] { (byte)ready };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void LoadNetworkGame()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load game, but we are not the master Client");
        }
        else
        {
            PhotonNetwork.LoadLevel("GameBoard");
        }

        localPlayer = gameObject.AddComponent<Player>();
        opponentPlayer = gameObject.AddComponent<Player>();
    }

    #endregion

    #region Private Methods

    #endregion

    #region RPCs

    [PunRPC]
    void changePlayerRPC()
    {
        if (App.isLocalPlayerTurn) // also check if it a draw ?
        {
            App.isLocalPlayerTurn = false;
        }
        else if (!App.isLocalPlayerTurn)
        {
            App.isLocalPlayerTurn = true;
        }
        Debug.Log("changed player");
    }

    #endregion
}

