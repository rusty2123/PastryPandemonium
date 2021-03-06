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
    public static Board boardInstance;

    void Awake()
    {
        if (boardInstance != null)
            Destroy(boardInstance);
        else
            boardInstance = this;

        DontDestroyOnLoad(this);
    }

    public Board()
    { }

    private Board(BitArray first, BitArray second)
    {
        playerOne = first;
        playerTwo = second;
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

    public void initializeBoard()
    {
        playerOne.SetAll(false);
        playerTwo.SetAll(false);
    }

    public BitArray findEmptySpots()
    {
        BitArray bitArray1 = new BitArray(playerOne);
        BitArray bitArray2 = new BitArray(playerTwo);
        return bitArray1.Xor(bitArray2);
    }

    public bool isEmptySpotAt(int index)
    {
        BitArray bitArray1 = new BitArray(playerOne);
        BitArray bitArray2 = new BitArray(playerTwo);

        if (Player.isSinglePlayer && !App.isLocalPlayerTurn)
        {
            if (bitArray1.Xor(bitArray2)[index] == false)
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            if (bitArray1.Xor(bitArray2)[index - 1] == false)
            {
                return true;
            }
            else
                return false;
        }
      
    }

    
    //if the local player controls the place on the board at index
    public bool isLocalPlayerPieceAt(int index)
    {
        return playerOne[index - 1];
    }

    public bool isOpponentPlayerPieceAt(int index)
    {
        return playerTwo[index - 1];
    }


    public BitArray getPlayerBoard()
    {
        if (App.isLocalPlayerTurn)
        {
            return playerOne;
        }
        return playerTwo;
    }

    // 1 for playerOne (typically local player) and 2 for playerTwo
    public BitArray getPlayerBoard(int playerNumber)
    {
        if (playerNumber == 1)
        {
            return playerOne;
        }
        return playerTwo;
    }

    public BitArray getLocalPlayerBoard()
    {
        if (App.isLocalPlayerTurn)
        {
            return playerOne;
        }
        return playerTwo;
    }

    public BitArray getOpponentPlayerBoard()
    {
        if (App.isLocalPlayerTurn)
        {
            return playerTwo;
        }
        return playerOne;
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

    public void placePiece(int index, bool isLocalPiece)
    {
        if (Player.isSinglePlayer && !App.isLocalPlayerTurn)
        {
            Debug.Log("opponent piece placed");
            playerTwo[index] = true;
        }
        else
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

    public void moveLocalPiece(int from, int to)
    {
        playerOne[from - 1] = false;
        playerOne[to - 1] = true;
    }

    public void moveOpponentPiece(int from, int to)
    {
        if (Player.isSinglePlayer && !App.isLocalPlayerTurn)
        {
            Debug.Log("placing bitarray opponent ");
            Debug.Log(from + " " + to);
            playerTwo[from] = false;
            playerTwo[to] = true;
        }
        else
        {
            playerTwo[from - 1] = false;
            playerTwo[to - 1] = true;
        }
    }


    public void removeLocalPiece(int index)
    {
        Debug.Log("removing local piece");

        if (Player.isSinglePlayer && !App.isLocalPlayerTurn)
        {
            playerOne[index] = false;
        }
        else
         playerOne[index - 1] = false;
    }

    public void removeOpponentPiece(int index)
    {
        Debug.Log("removing opponent piece");
        playerTwo[index - 1] = false;
    }

    public void printBoard()
    {
        string bitArray1 = "";
        string bitArray2 = "";

        for(int i = 0; i < 24; ++i)
        {
            bitArray1 = bitArray1 + playerOne[i] + i;
            bitArray2 = bitArray2 + playerTwo[i] + i;
        }

        Debug.Log(bitArray1);
        Debug.Log(bitArray2);
    }

    public void resetBoard()
    {
        playerOne.SetAll(false);
        playerTwo.SetAll(false);
    }

}