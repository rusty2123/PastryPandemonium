using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{

    public static bool isSinglePlayer = false;
    public static bool playerGoFirst = true;
    public static bool firstPlayer = true;

    public static string[] characterSelection = { "berryMuffin", "chipMuffin", "lemonMuffin", "chocolateCupcake", "redCupcake", "whiteCupcake" }; //change for names of characters later
    public static string characterLocalPlayer; 
    public static string characterOpponentPlayer;
    public GameObject[] Pieces;
    private int[] aiLocations = new int[24];
    private static int y;


    //AI levelset on single player menu
    public static string difficultyLevel = "easy"; 


    void Awake()
    {
    
        if (difficultyLevel == "easy")
        {
            //aIScript = gameObject.AddComponent<AIScript>();

        }
        else if (difficultyLevel == "hard")
        {
            //aIScript = gameObject.AddComponent<AIScript>();
        }

    }


    public int[] getAIMove(BitArray computer, BitArray player)
    {
        int[] move = new int[3];
        move = AI.move(player, computer, App.phase, difficultyLevel);
        return move;
    }



}

