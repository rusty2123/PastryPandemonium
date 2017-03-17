using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    public GameObject gamePiece;
    public Game game = Game.gameInstance;

    public int location = 0;

    public string owner = "";

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
        if (App.removePiece && owner == "opponent" && !game.piecePartOfMill(location) && !App.placePiece && !App.moveToPiece && !App.moveFromPiece)
        {
            App.pieceToRemove = location;
            Destroy(gamePiece);
            App.removePiece = false;
        }
        else if(App.moveFromPiece && owner == "local")
        {
            App.moveFromIndex = location;
            App.moveFromPiece = false;
            Debug.Log("moving from location: " + App.moveFromIndex);
        }
        else if (App.flyFromPiece && owner == "local")
        {
            App.flyFromIndex = location;
            App.flyFromPiece = false;
            Debug.Log("flying from location: " + App.flyFromIndex);
        }
    }
}
