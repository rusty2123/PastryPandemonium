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
    public static int gamePhase;

    public static string[] characterSelection = { "berryMuffin", "chipMuffin", "lemonMuffin", "chocolateCupcake", "redCupcake", "whiteCupcake" }; //change for names of characters later
    public static string characterLocalPlayer; //enum list of characters
    public static string characterOpponentPlayer;
    public GameObject[] Pieces;

    private static int piecesToPlace = 9;
    private static int pieceCount;
    private int moveTo;
    private int moveFrom;
    private int[] aiLocations = new int[24];
    private static int y;

    public static string difficultyLevel = "easy"; //by default

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


    public int[] getAIMove()
    {

        int[] move = new int[3];


        if (App.phase == 1)
        {
            move[1] = UnityEngine.Random.Range(0, 24);
            y = 0;

            for (int i = 0; i < 24; i++)
            {

                if (App.piecesPositions[i] != null)
                {
                    if (App.piecesPositions[i].name.Contains("local"))
                    {
                        aiLocations[y] = i;
                        Debug.Log("local at " + aiLocations[y]);
                        y++;
                    }

                }
            }

            if (y >= 2)
            {
                move[2] = aiLocations[UnityEngine.Random.Range(0, y)];

            }


        }
        else if (App.phase == 2)
        {
            y = 0;

            for (int i = 0; i < 24; i++)
            {
                if (App.piecesPositions[i] != null)
                {
                    if (App.piecesPositions[i].name.Contains("opponent"))
                    {
                        aiLocations[y] = i;
                        y++;
                    }

                }
            }

            move[0] = aiLocations[UnityEngine.Random.Range(0, y)];

            Debug.Log("move from - phase two " + move[0]);

            for (int i = 0; i < 24; i++)
            {
                if (App.piecesPositions[i] == null)
                {
                    move[1] = i;
                }
            }

            Debug.Log("move to - phase two " + move[1]);

            y = 0;

            for (int i = 0; i < 24; i++)
            {

                if (App.piecesPositions[i] != null)
                {
                    if (App.piecesPositions[i].name.Contains("local"))
                    {
                        aiLocations[y] = i;
                        Debug.Log("local at " + aiLocations[y]);
                        y++;
                    }

                }
            }

            move[2] = aiLocations[UnityEngine.Random.Range(0, y)];
            Debug.Log("piece to remove - phase two " + move[2]);

        }
        else
        {

        }

        //if (difficultyLevel == "easy")
        //{
        //    //call A1
        //}
        //else if (difficultyLevel == "difficult")
        //{
        //    //call A1
        //}


        return move;
    }



}

