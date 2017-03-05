using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkGameManager : Photon.PunBehaviour
{

    public Game game = Game.gameInstance;
    public static Player localPlayer, opponentPlayer;
    private static int placeIndex = 0, removeIndex = 0, moveFromIndex = 0, moveToIndex = 0, flyFromIndex = 0, flyToIndex = 0;

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

    #region Getters and Setters

    public static void setPlaceIndex(int i)
    {
        placeIndex = i;
    }

    public static int getPlaceIndex()
    {
        return placeIndex;
    }

    public static void setRemoveIndex(int i)
    {
        removeIndex = i;
    }

    public static int getRemoveIndex()
    {
        return removeIndex;
    }

    public static void setMoveToIndex(int i)
    {
        moveToIndex = i;
    }

    public static int getMoveToIndex()
    {
        return moveToIndex;
    }

    public static void setMoveFromIndex(int i)
    {
        moveFromIndex = i;
    }

    public static int getMoveFromIndex()
    {
        return moveFromIndex;
    }

    public static void setFlyToIndex(int i)
    {
        flyToIndex = i;
    }

    public static int getFlyToIndex()
    {
        return flyToIndex;
    }

    public static void setFlyFromIndex(int i)
    {
        flyFromIndex = i;
    }

    public static int getFlyFromIndex()
    {
        return flyFromIndex;
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

