using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    public GameObject gamePiece;
    public Game game = Game.gameInstance;
    public static bool validPiece;
    public int location = 0;

    public string owner = "";

    private void Awake()
    {
        location = 0;
        owner = "";
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        if (App.removePiece && owner == "opponent"
            && (!game.piecePartOfMill(location) || game.allPiecesPartOfMill())
            && !App.placePiece && !App.moveToPiece && !App.moveFromPiece)
        {
            App.pieceToRemove = location;
            if (!App.isSinglePlayer)
            {
                Destroy(gamePiece);
            }

            validPiece = true;
            App.removePiece = false;
        }
        else if (App.removePiece && owner == "opponent"
            && (game.piecePartOfMill(location) && game.allPiecesPartOfMill()))
        {

            validPiece = false;
            App.removePiece = false;
        }
        else if (App.moveFromPiece && owner == "local")
        {
            selectPiece();
        }
        else if (App.flyFromPiece && owner == "local")
        {
            App.flyFromIndex = location;
            App.flyFromPiece = false;
            Debug.Log("flying from location: " + App.flyFromIndex);
        }
    }


    public void selectPiece()
    {
        //highlight gamePiece
        App.moveToPiece = true;
        App.moveFromIndex = location;
        App.moveFromPiece = false;
        Debug.Log("moving from location: " + App.moveFromIndex);
    }

}
