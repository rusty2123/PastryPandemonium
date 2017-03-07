
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eppy;

public class Game : Photon.MonoBehaviour
{

    App appInstance = null;

    public static Game gameInstance = null;

    private int input, pieceCount, moveToIndex;
    private Board gameBoard = new Board();
    private MoveLookup gameBoardMoves = new MoveLookup();

    public static Player localPlayer;
    public static Player opponentPlayer;




    private List<Tuple<int, int>> moves; 
    private List<Tuple<int, int, int>> mills;


    private void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;
        }

        moves = new List<Tuple<int, int>>(gameInstance.gameBoardMoves.Moves);
        mills = new List<Tuple<int, int, int>>(gameInstance.gameBoardMoves.Mills);
        //else if (gameInstance != this)
        //    Destroy(gameInstance);

        localPlayer = App.localPlayer;
        opponentPlayer = App.opponentPlayer;


    }

    public void placePiece(int index)
    {
        gameBoard.placePiece(index);

    }

    //public void UpdatePlacePiece()
    //{
    //    if (!photonView.isMine)
    //    {
    //        return;
    //    }
    //    if (PhotonNetwork.offlineMode)
    //    {
    //        ai place piece
    //            }
    //    send move
    //            else
    //            {
    //        photonView.RPC(
    //            "placePiece",
    //            PhotonTargets.OthersBuffered,
    //            moveToIndex
    //            );
    //    }


    //public void placePiece()
    //{
    //    Debug.Log("place piece");

    //   int index = gameSpaces.getIndexClicked();

    //    Debug.Log(index);

    //    if (gameBoard.isEmptySpotAt(index))
    //    {
    //        gameBoard.placePiece(index);

    //        if (createdMill(index))
    //        {
    //            removePiece();
    //        }
    //        appInstance.changePlayer();
    //    }
    //}

    public bool gameOver()
    {
        if (localPlayer.getPieceCount() <= 2 || !playerCanMove())
            return true;
        else
        {
            return false;
        }
    }

    public bool phaseThree()
    {
        if (localPlayer.getPieceCount() == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isDraw()
    {
        return false;
    }

    public void movePiece(int from, int to)
    {
        Debug.Log("move piece");

        if (validMove(from, to))
        {
            gameBoard.movePiece(from, to);

            if (createdMill(to))
            {
                //removePiece();
            }
            appInstance.changePlayer();
        }
        else
        {
            Debug.Log("Invalid Move");
        }
    }

    public void flyPiece(int from, int to)
    {
        Debug.Log("move piece");

        if (validFly(from, to))
        {
            gameBoard.movePiece(from, to);

            if (createdMill(to))
            {
                //removePiece();
            }
            appInstance.changePlayer();
        }
        else
        {
            Debug.Log("Invalid Fly");
        }
    }

    public bool playerCanMove()
    {
        BitArray boardConfig = gameBoard.findEmptySpots();

        BitArray playerConfig = gameBoard.getPlayerBoard();

        for (int i = 0; i < 24; i++)
        {
            //  enter if player has piece at this spot
            if (playerConfig[i] == true)
            {
                for (int j = 0; i < 24; ++i)
                {
                    if (boardConfig[i] == true)
                    {
                        if (validMove(i, j))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool validPlace(int index)
    {
        if (gameBoard.isEmptySpotAt(index))
        {
            return true;
        }  
       
        return false;
    }

    public void removePiece(int index)
    {
        Debug.Log("remove piece");

        //something here is going out of range
        //this condition makes sure it's an opponent's piece, and that the piece isn't part of a mill unless all opponent's pieces are part of a mill
        if (gameBoard.isLocalPlayerPieceAt(index) == true
            && (!piecePartOfMill(index) || allPiecesPartOfMill()))
        {
            gameBoard.removePiece(!App.isLocalPlayerTurn, index);
        }
    }

    public bool createdMill(int to)
    {
        foreach (Tuple<int, int, int> entry in gameBoardMoves.Mills)
        {
            if (entry.Item1 == to &&
              (gameBoard.isLocalPlayerPieceAt(entry.Item2) &&
               gameBoard.isLocalPlayerPieceAt(entry.Item3)))
            {
                Debug.Log("created mill");
                return true;
            }
        }
        return false;
    }

    public bool validMove(int from, int to)
    {
        if (gameBoard.isLocalPlayerPieceAt(from) == true)
        {
            foreach (Tuple<int, int> entry in gameBoardMoves.Moves)
            {
                if (from == entry.Item1 && gameBoard.isEmptySpotAt(to))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool validFly(int from, int to)
    {
        if ((gameBoard.isLocalPlayerPieceAt(from) == true) &&
             gameBoard.isEmptySpotAt(to) == true)
        {
            return true;
        }
        return false;
    }

    private bool piecePartOfMill(int index)
    {
        foreach (Tuple<int, int, int> entry in gameBoardMoves.Mills)
        {
            if (entry.Item1 == index &&
              (gameBoard.isLocalPlayerPieceAt(entry.Item2) &&
               gameBoard.isLocalPlayerPieceAt(entry.Item3)))
            {
                return true;
            }
        }
        return false;
    }

    private bool allPiecesPartOfMill()
    {
        int pieceCount = 0;
        for (int i = 1; i <= 24; i++)
        {
            if (gameBoard.isLocalPlayerPieceAt(i) && piecePartOfMill(i))
            {
                pieceCount++;
            }
        }
        return (pieceCount == localPlayer.getPieceCount());
    }


}