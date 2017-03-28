
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eppy;

public class Game : Photon.MonoBehaviour
{

    App appInstance = null;

    private int input, pieceCount, moveToIndex;
    private Board gameBoard = Board.getInstance;
    private MoveLookup gameBoardMoves = new MoveLookup();

    public static Player localPlayer;
    public static Player opponentPlayer;

    App a = new App();




    private List<Tuple<int, int>> moves;
    private List<Tuple<int, int, int>> mills;


    public static Game gameInstance;

    void Awake()
    {
        if (gameInstance != null)
        {
            Destroy(gameInstance);
        }
        else
        {
            gameInstance = this;
        }

        DontDestroyOnLoad(this);

        moves = new List<Tuple<int, int>>(gameInstance.gameBoardMoves.Moves);
        mills = new List<Tuple<int, int, int>>(gameInstance.gameBoardMoves.Mills);

        localPlayer = App.localPlayer;
        opponentPlayer = App.opponentPlayer;
    }

    public void placePiece(int index, bool isLocalPiece)
    {
        Player.movesSinceLastMillFormed++;
        Board.boardInstance.placePiece(index, isLocalPiece);
    }


    public bool isDraw()
    {
        return (Player.movesSinceLastMillFormed >= 10);
    }

    public void movePiece(int from, int to)
    {
        Debug.Log("move piece");

        Player.movesSinceLastMillFormed++;

        if (validMove(from, to))
        {
            Board.boardInstance.movePiece(from, to);
        }
    }

    public void moveLocalPiece(int from, int to)
    {
        Player.movesSinceLastMillFormed++;
        Board.boardInstance.moveLocalPiece(from, to);
    }

    public void moveOpponentPiece(int from, int to)
    {
        Player.movesSinceLastMillFormed++;
        Board.boardInstance.moveOpponentPiece(from, to);
    }

    public void flyPiece(int from, int to)
    {
        Debug.Log("move piece");

        Player.movesSinceLastMillFormed++;

        if (validFly(from, to))
        {
            Board.boardInstance.movePiece(from, to);

            appInstance.changePlayer();
        }
        else
        {
            Debug.Log("Invalid Fly");
        }
    }

    public bool playerCanMove()
    {
        BitArray boardConfig = Board.boardInstance.findEmptySpots();

        BitArray playerConfig = Board.boardInstance.getPlayerBoard();

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
        if (Board.boardInstance.isEmptySpotAt(index))
        {
            return true;
        }
        return false;
    }

    public void removeLocalPiece(int index)
    {
        Player.movesSinceLastMillFormed = 0;
        Board.boardInstance.removeLocalPiece(index);
    }

    public void removeOpponentPiece(int index)
    {
        Player.movesSinceLastMillFormed = 0;
        Board.boardInstance.removeOpponentPiece(index);
    }

    public bool createdMill(int to)
    {
        foreach (Tuple<int, int, int> entry in gameBoardMoves.Mills)
        {
            if (!App.isLocalPlayerTurn)
            {
                if (entry.Item1 == to+1 &&
                (Board.boardInstance.isOpponentPlayerPieceAt(entry.Item2) &&
                 Board.boardInstance.isOpponentPlayerPieceAt(entry.Item3)))
                {
                    Debug.Log(entry.Item1 + " " + entry.Item2 + " " + entry.Item3);
                    StartCoroutine(waitForMillAnimation(entry.Item1, entry.Item2, entry.Item3));
                   // a.animationCreatedMill(entry.Item1, entry.Item2, entry.Item3);
                    return true;
                }

            }
            else
            {
                if (entry.Item1 == to &&
                (Board.boardInstance.isLocalPlayerPieceAt(entry.Item2) &&
                 Board.boardInstance.isLocalPlayerPieceAt(entry.Item3)))
                {
                    Debug.Log(entry.Item1 + " " + entry.Item2 + " " + entry.Item3);
                    StartCoroutine(waitForMillAnimation(entry.Item1, entry.Item2, entry.Item3));
                   // a.animationCreatedMill(entry.Item1, entry.Item2, entry.Item3);
                    return true;
                }
            }
           
        }
        return false;
    }

    
    IEnumerator waitForMillAnimation(int item1, int item2, int item3)
    {
        yield return new WaitForSeconds(3);
        a.animationCreatedMill(item1, item2, item3);

    }
    
    public bool validMove(int from, int to)
    {
        if (Board.boardInstance.isLocalPlayerPieceAt(from) == true)
        {
            foreach (Tuple<int, int> entry in gameBoardMoves.Moves)
            {
                if (from == entry.Item1 && to == entry.Item2 && Board.boardInstance.isEmptySpotAt(to))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool validFly(int from, int to)
    {
        if ((Board.boardInstance.isLocalPlayerPieceAt(from) == true) &&
             Board.boardInstance.isEmptySpotAt(to) == true)
        {
            return true;
        }
        return false;
    }

    public bool piecePartOfMill(int index)
    {
        foreach (Tuple<int, int, int> entry in gameBoardMoves.Mills)
        {
            if (entry.Item1 == index &&
              (Board.boardInstance.isOpponentPlayerPieceAt(entry.Item2) &&
               Board.boardInstance.isOpponentPlayerPieceAt(entry.Item3)))
            {
                return true;
            }
        }
        return false;
    }

    public bool allPiecesPartOfMill()
    {
        int pieceCount = 0;
        int millPieceCount = 0;
        for (int i = 1; i <= 24; i++)
        {
            if (Board.boardInstance.isOpponentPlayerPieceAt(i))
            {
                pieceCount++;
                if (piecePartOfMill(i))
                {
                    millPieceCount++;
                }
            }
        }
        return (pieceCount == millPieceCount);
    }


}