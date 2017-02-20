using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{

    public static bool isSinglePlayer = true;
    public static bool PlayerGoFirst = true;
    public static bool firstPlayer = true;
    public static int gamePhase;

    public string [] aiLevel = { "easy", "difficult" };
    public static string[] characterSelection = { "berryMuffin", "chipMuffin", "lemonMuffin", "chocolateCupcake", "redCupcake", "whiteCupcake" }; //change for names of characters later
    public static string characterLocalPlayer; //enum list of characters
    public static string characterOpponentPlayer;
    public GameObject[] Pieces;

    private static int piecesToPlace = 9;
    private static int pieceCount;
    private int moveTo;
    private int moveFrom;


    public static string level = "easy"; //by default

    public static Player instance;


    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (level == "easy")
        {
            //aIScript = gameObject.AddComponent<AIScript>();

        }
        else if (level == "difficult")
        {
            //aIScript = gameObject.AddComponent<AIScript>();
        }

    }




    public int getMoveFrom()
    {
        return App.from;
    }

    public int getMoveTo()
    {
        return App.to;
    }

    public int getPieceToRemove()
    {
        int pieceToRemove = 0;
        //set pieceToRemove from the GUI, the network, or the AI
        return pieceToRemove;
    }

    public int getPiecesToPlace()
    {
        return piecesToPlace--;
    }


    public int getPieceCount()
    {
        return pieceCount--;
    }

    public int[] getAIMove()
    {
        print("Calling AI");
        int[] move = null;

        if (level =="easy")
        {
            //call A1
        }
        else if (level == "difficult")
        {
            //call A1
        }


        return move;
    }


}

