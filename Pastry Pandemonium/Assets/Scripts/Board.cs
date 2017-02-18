using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eppy;

public class Board : MonoBehaviour
{

    private BitArray playerOne = new BitArray(24);
    private BitArray playerTwo = new BitArray(24);
    private static Board boardInstance = null;

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
        return playerOne.Xor(playerTwo)[index];
    }

    public Board getInstance
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
            return playerOne[index];
        }
        else
        {
            return playerTwo[index];
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

    public void removePiece(bool isLocalPlayer, int index)
    {
        if (isLocalPlayer)
        {
            playerOne[index - 1] = false;
        }
        else
        {
            playerTwo[index - 1] = false;
        }
    }

    public void placePiece(int index)
    {
        if (App.isLocalPlayerTurn)
        {
            playerOne[index - 1] = true;
        }
        else
        {
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