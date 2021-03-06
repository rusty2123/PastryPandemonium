﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkGameManager : Photon.PunBehaviour
{

    public Game game = Game.gameInstance;
    public App app;
    public MultiplayerSetup multiplayerSetup;
    public static Player localPlayer, opponentPlayer;
    public static int placeIndex = 0, removeIndex = 0, moveFromIndex = 0, moveToIndex = 0, flyFromIndex = 0, flyToIndex = 0;
    public static bool localReady = false, opponentReady = false, moveEventsAdded = false, setupEventsAdded = false, readyEventsAdded = false,
                       drawEventsAdded = false, drawResponseRecieved = false, drawAccepted = false, drawRequested = false,
                       youDisconnected = false, hostDisconnected = false, opponentDisconnected = false, inRoom = false;

    #region Photon Messages

    public override void OnLeftRoom()
    {
        if (PhotonNetwork.connected && !App.gameOver)
        {
            Debug.Log("game over: " + App.gameOver);
            SceneManager.LoadScene("Lobby");
        }
        else if(PhotonNetwork.connected && App.gameOver)
        {

        }
        else if(!PhotonNetwork.connected)
        {
            Debug.Log("game over: " + App.gameOver);
            SceneManager.LoadScene("mainMenu");
        }
        Debug.Log("leaving room");
        MultiplayerSetup.selectedCharacter = "";
        Player.characterLocalPlayer = "";
        Player.characterOpponentPlayer = "";
        localReady = false;
        opponentReady = false;
        inRoom = false;
    }

    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        Debug.Log("host switched");
        //LeaveRoom();
        if (PhotonNetwork.connected && !App.gameOver)
        {
            hostDisconnected = true;
            Debug.Log("game over: " + App.gameOver);
            SceneManager.LoadScene("Lobby");
        }
        else if (!PhotonNetwork.connected && !App.gameOver)
        {
            hostDisconnected = true;
            Debug.Log("game over: " + App.gameOver);
            //SceneManager.LoadScene("mainMenu");
        }
    }

    public override void OnDisconnectedFromPhoton()
    {
        youDisconnected = true;
        LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        inRoom = true;
        SceneManager.LoadScene("Room");
        if(Player.characterOpponentPlayer.Contains("Muffin"))
        {
            Player.characterLocalPlayer = "redCupcake";
            MultiplayerSetup.selectedCharacter = "redCupcake";
        }
        else if(Player.characterOpponentPlayer.Contains("Muffin"))
        {
            Player.characterLocalPlayer = "berryMuffin";
            MultiplayerSetup.selectedCharacter = "berryMuffin";
        }
    }

    //called when another player joins the room
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("player connected");

        //let the joined player know if you are ready, and which character you have chosen
        if(localReady)
        {
            ready(1);
        }
        else if(!localReady)
        {
            ready(0);
        }

        multiplayerSetup.sendCharacterSelection(Player.characterLocalPlayer);
    }

    //called when a player disconnects
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("player disconnected");
        opponentReady = false;
        localReady = false;
        Player.characterOpponentPlayer = "";

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
        if(inRoom)
            PhotonNetwork.LeaveRoom();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
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
    public void changePlayer()
    {
        //code 4 for changing player
        byte code = 4;
        //data must be sent in byte array
        byte[] content = new byte[] { };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void chooseCharacter(int character)
    {
        //code 5 for your selected character
        byte code = 5;
        //data must be sent in byte array
        byte[] content = new byte[] { (byte)character };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void ready(int ready)
    {
        //code 6 for readying up
        byte code = 6;
        //data must be sent in byte array
        byte[] content = new byte[] { (byte)ready };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void offerDraw()
    {
        Debug.Log("offer draw");
        byte code = 7;
        byte[] content = new byte[] {};
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void acceptDraw(int decision)
    {
        Debug.Log("accept/decline draw");
        byte code = 8;
        byte[] content = new byte[] { (byte)decision };
        PhotonNetwork.RaiseEvent(code, content, true, null);
    }

    public void sendWin(int win)
    {
        byte code = 9;
        byte[] content = new byte[] { (byte)win };
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

