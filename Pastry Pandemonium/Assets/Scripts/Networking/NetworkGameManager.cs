using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkGameManager : Photon.PunBehaviour
{

    Game game = Game.gameInstance;
    public static Player localPlayer;
    public static Player opponentPlayer;

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

    public void placePiece(int index)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("movePieceRPC", PhotonTargets.All, index);
    }

    public void movePiece(int from, int to)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("movePieceRPC", PhotonTargets.All, from, to);
    }

    public void flyPiece(int from, int to)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("flyPieceRPC", PhotonTargets.All, from, to);
    }

    public void removePiece(int index)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("removePieceRPC", PhotonTargets.All, index);
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
    private void placePieceRPC(int index)
    {
        game.placePiece(index);
    }

    [PunRPC]
    private void movePieceRPC(int from, int to)
    {
        game.movePiece(from, to);
    }

    [PunRPC]
    private void flyPieceRPC(int from, int to)
    {
        game.flyPiece(from, to);
    }

    [PunRPC]
    private void removePieceRPC(int index)
    {
        game.removePiece(index);
    }


    #endregion
}

