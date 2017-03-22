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
                        //Debug.Log("local at " + aiLocations[y]);
                        y++;
                    }

                }
            }

            if (y >= 2)
            {
                move[2] = aiLocations[UnityEngine.Random.Range(0, y)];

            }


        }
        else if (App.phase == 2 || App.phase == 3)
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
                        y++;
                    }

                }
            }

            move[2] = aiLocations[UnityEngine.Random.Range(0, y)];
        }
        else
        {

        }

        return move;
    }



}

