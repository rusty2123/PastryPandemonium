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
        if (App.removePiece && owner == "opponent")
        {
            App.pieceToRemove = location;
            Destroy(gamePiece);
            //game.removePiece(location);
            App.removePiece = false;
        }
    }
}
