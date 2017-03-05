using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkGameManager : Photon.PunBehaviour
{

    public Game game = Game.gameInstance;
    public static Player localPlayer, opponentPlayer;
    public static int placeIndex = 0, removeIndex = 0, moveFromIndex = 0, moveToIndex = 0, flyFromIndex = 0, flyToIndex = 0;

    #region Photon Messages

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnJoinedRoom()
    {
        LoadNetworkGame();
    }

    #endregion


    #region Public Methods

    public bool isMasterClient()
    {
        if(PhotonNetwork.isMasterClient)
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
        //code 0 for placing piece
        byte code = 1;
        //data must be sent in byte array
        byte[] content = new byte[] { (byte)i };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void movePiece(int from, int to)
    {
        //code 0 for placing piece
        byte code = 2;
        //data must be sent in byte array
        byte[] content = new byte[] { (byte)from, (byte)to };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void flyPiece(int from, int to)
    {
        //code 0 for placing piece
        byte code = 3;
        //data must be sent in byte array
        byte[] content = new byte[] { (byte)from, (byte)to };
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


    #endregion
}

