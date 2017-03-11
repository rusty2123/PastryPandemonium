﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eppy;

public class Board : MonoBehaviour
{

    private BitArray playerOne = new BitArray(24);
    private BitArray playerTwo = new BitArray(24);
    public static Board boardInstance = null;

    public Board()
    { }

    private Board(BitArray first, BitArray second)
    {
        playerOne = first;
        playerTwo = second;
    }

    public void initializeBoard()
    {
        playerOne.SetAll(false);
        playerTwo.SetAll(false);
    }

    public BitArray findEmptySpots()
    {
        return playerOne.Xor(playerTwo);
    }

    public bool isEmptySpotAt(int index)
    {

        if (playerOne.Xor(playerTwo)[index -1] == false)
        {
            return true;
        }
        else
            return false;
    }

    public static Board getInstance
    {
        get
        {
            if (boardInstance == null)
            {
                boardInstance = new Board();

            }
            return boardInstance;
        }
    }

    //if the local player controls the place on the board at index
    public bool isLocalPlayerPieceAt(int index)
    {
        if (App.isLocalPlayerTurn)
        {
            return playerOne[index - 1];
        }
        else
        {
            return playerTwo[index - 1];
        }
    }

    public BitArray getPlayerBoard()
    {
        if (App.isLocalPlayerTurn)
        {
            return playerOne;
        }
        return playerTwo;
    }

    public int getPlayerPieceCount()
    {
        if (App.isLocalPlayerTurn)
        {
            return (int)playerOne.Count;
        }
        else
        {
            return (int)playerTwo.Count;
        }
    }

    public void movePiece(int from, int to)
    {
        if (App.isLocalPlayerTurn)
        {
            playerOne[from - 1] = false;
            playerOne[to - 1] = true;
        }
        else
        {
            playerTwo[from - 1] = false;
            playerTwo[to - 1] = true;
        }
    }

    public void removePiece(int index, bool isLocalPiece)
    {
        if (isLocalPiece)
        {
            Debug.Log("removing local piece");
            playerOne[index - 1] = false;
        }
        else
        {
            Debug.Log("removing opponent piece");
            playerTwo[index - 1] = false;
        }
    }

    public void placePiece(int index, bool isLocalPiece)
    {
        if (isLocalPiece)
        {
            Debug.Log("local piece placed");
            playerOne[index - 1] = true;
        }
        else
        {
            Debug.Log("opponent piece placed");
            playerTwo[index - 1] = true;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

}