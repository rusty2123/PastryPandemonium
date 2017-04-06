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

    public void OnMouseEnter()
    {
        if (App.phase == 1 && !((gameObject.tag.Contains("local")) || (gameObject.tag.Contains("opponent") || App.piecesPositions[Convert.ToInt32(gameObject.name) - 1] != null)))
        {
            // highlight the open board spot
        }
    }

    public void OnMouseExit()
    {
        if (App.phase == 1 && !((gameObject.tag.Contains("local")) || (gameObject.tag.Contains("opponent") || App.piecesPositions[Convert.ToInt32(gameObject.name) - 1] != null)))
        {
            // unhighlight the open board spot
        }
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
                    if ((gameObject.tag.Contains("local")) || (gameObject.tag.Contains("opponent") || App.piecesPositions[Convert.ToInt32(gameObject.name)-1] != null))
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

                    if ((gameObject.tag.Contains("local")) || (gameObject.tag.Contains("opponent") || App.piecesPositions[Convert.ToInt32(gameObject.name) - 1] != null))
                    {
                        App.validMove = false;
                        App.moveFromPiece = true;
                        App.moveToPiece = false;
                        Debug.Log("reset moveFromPiece");
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
                    if ((gameObject.tag.Contains("local")) || (gameObject.tag.Contains("opponent") || App.piecesPositions[Convert.ToInt32(gameObject.name) - 1] != null))
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
