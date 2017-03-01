using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{

    public static bool isSinglePlayer;
    public static bool playerGoFirst;
    public static bool firstPlayer = true;
    public static int gamePhase;

    public static string[] characterSelection = { "berryMuffin", "chipMuffin", "lemonMuffin", "chocolateCupcake", "redCupcake", "whiteCupcake" }; //change for names of characters later
    public static string characterLocalPlayer; //enum list of characters
    public static string characterOpponentPlayer;
    public GameObject[] Pieces;

    private static int piecesToPlace = 9;
    private static int pieceCount;
    private int moveTo;
    private int moveFrom;


    public static string difficultyLevel; //by default

    public static Player instance;


    void Awake()
    {
    //    if (instance == null)
    //    {
    //        DontDestroyOnLoad(gameObject);
    //        instance = this;
    //    }
    //    else if (instance != this)
    //    {
    //        Destroy(gameObject);
    //    }

        if (difficultyLevel == "easy")
        {
            //aIScript = gameObject.AddComponent<AIScript>();

        }
        else if (difficultyLevel == "hard")
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
        int[] move = new int[2];

        if (difficultyLevel == "easy")
        {
            //call A1
        }
        else if (difficultyLevel == "difficult")
        {
            //call A1
        }

        move[1] = UnityEngine.Random.Range(1,24);
      
        

        return move;
    }


}

