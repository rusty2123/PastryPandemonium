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
        App.selectedGamePiece = gamePiece.name[gamePiece.name.Length - 1];
        //Debug.Log("piece selected: " + App.selectedGamePiece);
    }
}
