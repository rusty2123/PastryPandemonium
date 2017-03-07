using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    public GameObject gamePiece;

    //public static int location = 0;

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
        if (App.removePiece && gamePiece.tag == "opponent")
        {
            Destroy(gamePiece);
            App.removePiece = false;
            //now update gameboard where the piece was selected
        }

        App.selectedGamePiece = gamePiece.name[gamePiece.name.Length - 1];
    }
}
