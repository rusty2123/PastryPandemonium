﻿using System;
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

    public void movePiece()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("movePieceRPC", PhotonTargets.All);
    }

    public void flyPiece()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("flyPieceRPC", PhotonTargets.All);
    }

    public void removePiece()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("removePieceRPC", PhotonTargets.All);
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
    private void movePieceRPC()
    {
        game.movePiece();
    }

    [PunRPC]
    private void flyPieceRPC()
    {
        game.flyPiece();
    }

    [PunRPC]
    private bool removePieceRPC()
    {
        if(game.removePiece())
        {
            return true;
        }
        return false;
    }


    #endregion
}

