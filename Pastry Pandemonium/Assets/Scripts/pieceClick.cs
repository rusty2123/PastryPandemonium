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

    //IEnumerator startAnimation()
    //{
    //    yield return new WaitForSeconds(range);
    //    gameObject.GetComponent<Animation>().Play();
    //}

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
                    App.placeIndex = Convert.ToInt32(gameObject.name);
                    App.placePiece = false;
                    //gameObj.GetComponent<App>().piecePlacementPhase(Convert.ToInt32(gameObject.name));
                    //if (App.removePiece && !gameBoard.isLocalPlayerPieceAt(Convert.ToInt32(gameObject.name)))
                    //{
                    //    Debug.Log("remove piece index: " + gameObject.name);
                    //}
                }
                break;
            case 2:
                break;
            case 3:
                break;
        }

    }


}
