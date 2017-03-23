using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class pieceClick : MonoBehaviour
{


    private GameObject gameObj;
    private Board gameBoard = Board.boardInstance;
    public AudioSource audioSource; //piece click audio
    private float range;

    void Start()
    {
        //range = Random.Range(0f, 1f);

        //if (!(gameObject.GetComponent<Animation>() == null))
        //    StartCoroutine(startAnimation());
        //else
        //    StartCoroutine(startAnimation());

    }

    public void OnMouseDown()
    {
        gameObj = GameObject.FindWithTag("gameBoard");
        //audioSource.Play();
        //Debug.Log("down");
       switch (App.phase)
        {
            case 1:
                if (App.isLocalPlayerTurn && App.placePiece)
                {
                    if ((gameObject.name.Contains("local")) || (gameObject.name.Contains("opponent") || App.piecesPositions[Convert.ToInt32(gameObject.name)-1] != null))
                    {
                        App.validMove = false;
                    }
                    else
                    {
                       
                        App.placeIndex = Convert.ToInt32(gameObject.name);
                        App.placePiece = false;
                        App.validMove = true;
                    }
                        
                }
                                
                break;
            case 2:
                if (App.isLocalPlayerTurn && App.moveToPiece && !App.moveFromPiece)
                {

                    if ((gameObject.name.Contains("local")) || (gameObject.name.Contains("opponent") || App.piecesPositions[Convert.ToInt32(gameObject.name) - 1] != null))
                    {
                        App.validMove = false;
                    }
                    else
                    {
                        App.moveToIndex = Convert.ToInt32(gameObject.name);
                        App.moveToPiece = false;
                        Debug.Log("moving to location: " + App.moveToIndex);
                        App.validMove = true;

                    }
                }

                break;
            case 3:
                if (App.isLocalPlayerTurn && App.flyToPiece && !App.flyFromPiece)
                {
                    if ((gameObject.name.Contains("local")) || (gameObject.name.Contains("opponent") || App.piecesPositions[Convert.ToInt32(gameObject.name) - 1] != null))
                    {
                        App.validMove = false;
                    }
                    else {
                        App.flyToIndex = Convert.ToInt32(gameObject.name);
                        App.flyToPiece = false;
                        Debug.Log("flying to location: " + App.flyToIndex);

                    }
                   
                }
                break;
        }

    }


}
