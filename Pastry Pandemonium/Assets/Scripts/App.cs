using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class App : MonoBehaviour
{

    Game game = Game.gameInstance;

    private Board boardInstance;

    private GameObject gameObj;

    public GameObject[] opponentPieces = new GameObject[9];
    public GameObject[] localPieces = new GameObject[9];
    public GameObject[] outOfBoardSpaces = new GameObject[18];

    public static bool isSinglePlayer;
    public static bool gameOver;
    public static bool isLocalPlayerTurn;
    public static bool localPlayerWon;
    public static Player localPlayer;
    public static Player opponentPlayer;

    private GameObject clickedFirst = null;
    private GameObject clickedSecond = null;

    public static int from;
    public static int to;

    private void Start()
    {
        isSinglePlayer = Player.isSinglePlayer;

        if (isSinglePlayer)
        {
            if (Player.PlayerGoFirst)
            {
                isLocalPlayerTurn = true;
                //change UI turn indicator

            }
            else
                isLocalPlayerTurn = false;
            //change UI turn indicator
        }//if multiplayer
        else
        {
            isLocalPlayerTurn = true;

            if (Player.firstPlayer)
            {
                //change UI turn indicator 
            }
            else { }
            //change UI turn indicator

        }

        localPlayer = gameObject.AddComponent<Player>();
        opponentPlayer = gameObject.AddComponent<Player>();
    }


    public void setUpPlayerPieces()
    {
        gameObj = GameObject.FindWithTag("Game");
        boardInstance.initializeBoard();

        if ((Player.characterLocalPlayer) == Player.characterSelection.name1)
        {
            //call piece set up function for local player passing the prefab name
            // localPlayerPieceSetUp(prefabName)
        }
        else if (Player.characterLocalPlayer == Player.characterSelection.name2)
        {
            //call piece set up function for local player passing the prefab name
            // localPlayerPieceSetUp(prefabName)
        }
        else if (Player.characterLocalPlayer == Player.characterSelection.name3)
        {
            //call piece set up function for local player passing the prefab name
            // localPlayerPieceSetUp(prefabName)
        }
        else
        //Add more if else statements once we know how many characters we'll have

        if (Player.isSinglePlayer)
        {
            int character = UnityEngine.Random.Range(1, 4); //depends on ho wmany chaacters
            Player.characterOpponentPlayer = (Player.characterSelection)character;
        }

        while (Player.characterOpponentPlayer == Player.characterLocalPlayer)
        {
            int character = UnityEngine.Random.Range(1, 4);
            Player.characterOpponentPlayer = (Player.characterSelection)character;

        }

        if (Player.characterOpponentPlayer == Player.characterSelection.name1)
        {
            // call piece set up function for opponent player passing the prefab name
            // opponetPlayerPieceSetUp(prefabName)
        }
        else if (Player.characterOpponentPlayer == Player.characterSelection.name2)
        {
            // call piece set up function for opponent player passing the prefab name
            // opponetPlayerPieceSetUp(prefabName)
        }
        else if (Player.characterOpponentPlayer == Player.characterSelection.name3)
        {
            // call piece set up function for opponent player passing the prefab name
            // opponetPlayerPieceSetUp(prefabName)
        }
        else
        {
            //Add more if else statements once we know how many characters we'll have
        }

        localPlayer.Pieces = localPieces;
        opponentPlayer.Pieces = opponentPieces;

    }

    void setUpPiecesLocal(GameObject localCharacter)
    {
        Vector3 vector = new Vector3(0, 0, 0); ;

        for (int i = 0; i < 9; i++)
        {
            vector.x = outOfBoardSpaces[i].transform.position.x;
            vector.z = outOfBoardSpaces[i].transform.position.z;
            // vector.y our we are settting up our pieces out of the board? 

            GameObject piece = Instantiate(localCharacter, vector, Quaternion.identity) as GameObject;
            piece.tag = null;
            piece.name = "local" + (i).ToString();
            localPieces[i] = piece;

        }

    }

    void pieceSetupOpponent(GameObject localCharacter)
    {
        Vector3 vector = new Vector3(0, 0, 0); ;

        for (int i = 0; i < 9; i++)
        {
            vector.x = outOfBoardSpaces[i].transform.position.x;
            vector.z = outOfBoardSpaces[i].transform.position.z;
            // vector.y our we are settting up our pieces out of the board? 

            GameObject piece = Instantiate(localCharacter, vector, Quaternion.identity) as GameObject;

            piece.name = "opponent" + (i).ToString();
            piece.tag = null;
            localPieces[i] = piece;

        }
    }

    public bool getTurn()
    {
        return isLocalPlayerTurn;
    }

    public void changePlayer()
    {
        if (isLocalPlayerTurn) // also check if it a draw ?
        {
            isLocalPlayerTurn = false;

            if (Player.firstPlayer)
            {
                //change turn indicator
            }
            else
            {
                //change turn indicator
            }

        }
        else if (!isLocalPlayerTurn)
        {
            isLocalPlayerTurn = true;

            if (isSinglePlayer)
            {
                //change UI turn indicator

            }
            else
            {
                if (Player.firstPlayer)
                {
                    //change UI turn indicator
                }
                else
                {
                    //change UI turn indicator
                }

            }


        }
    }



    private void Update()
    {
        if (game.gameOver())
        {
            //change turn text to game over text

            if (!game.isDraw())
            {
                if (isSinglePlayer)
                {
                    if (localPlayerWon)
                    {
                        //check if audio is enabled
                        //play win audio

                        displayWinMessage();
                    }
                }
            }
            else
            {
                //check if audio is enabled
                //play loss audio

                displayLossMessage();
            }
        }
        else
        {
            // continues 

            if (Player.firstPlayer)
            {

                if (localPlayerWon)
                {
                    //check if audio is enabled
                    //play win audio

                    displayWinMessage();
                }
                else
                {
                    //check if audio is enabled
                    //play win loss audio

                    displayLossMessage();

                }

            }
            else
            {
                if (localPlayerWon)
                {
                    //check if audio is enabled
                    //play win loss audio
                }
                else
                {
                    //check if audio is enabled
                    //play win audio

                }

            }
        }
        disableButtons();
        disablePieces();
    }

    public int setMoveFrom(GameObject obj)
    {
        if (obj.tag == null)
        {
            obj.tag = "-1";
            return Convert.ToInt32(obj.tag);

        }
        else
        {
            return Convert.ToInt32(obj.tag);
        }

    }

    public int setMoveTo(GameObject obj)
    {

        return Convert.ToInt32(obj.tag);
    }

    //works with the networking part
    public void setClickedObjects(GameObject obj)
    {


        if (clickedFirst == null)
        {
            if (obj.name.Contains("local") && isLocalPlayerTurn)
            {
                if (!isSinglePlayer)
                {
                    if (Player.firstPlayer) //both connected
                        from = setMoveFrom(obj);
                }
                else
                {
                    from = setMoveFrom(obj);

                }
            }
            else if (obj.name.Contains("opponent") && !isLocalPlayerTurn && !Player.firstPlayer) // &&check if both players are connected
            {
                from = setMoveFrom(obj);
            }
            else
            {
                //do nothing, empty space clicked
            }
        }
        else
        {

            to = setMoveTo(obj);

            if (clickedSecond.tag == "Empty")
            {
                //two valid clicks registered
                if ((game.validMove(from, to)))
                {
                    //was a valid move, go ahead and move the piece
                    if (isSinglePlayer)
                    {
                        //move(clickedFirst, clickedSecond.transform.position);
                    }
                    else//else if multiplayer TO MOVE PIECES, TRANSFORM MUST BE IN SEPARATE FUNCTION
                    {

                    }
                    clickedFirst = null;
                    clickedSecond = null;

                }
                //invalid move, setting selections to nothing.
                clickedFirst = null;
                clickedSecond = null;

            }
            else
            {
                if (clickedSecond == clickedFirst)
                {
                    //same piece clicked twice, deselect

                    clickedSecond = null;
                    clickedFirst = null;
                }
                else
                {
                    //different piece clicked, set new selected piece to the new piece

                    setMoveFrom(clickedSecond);
                    clickedSecond = null;
                }
            }
        }
    }


    public void displayWinMessage()
    {
        //disable buttons and pieces
        //set game object active
        //dislay animation 
    }

    public void displayLossMessage()
    {
        //disable buttons and pieces
        //set game object active
        //dislay animation 
    }

    public void displayTieMessage()
    {
        //disable buttons and pieces
        //set game object active
        //dislay animation 
    }

    public void muteAudio()
    {
        bool isMute = GetComponent<AudioSource>().mute;

        if (isMute)
        {
            GetComponent<AudioSource>().mute = true;
        }
        else
        {
            GetComponent<AudioSource>().mute = false;
        }
    }

    public void enablePieces()
    {
        foreach (var piece in localPieces)
        {
            //enable game object 
        }

        foreach (var piece in opponentPieces)
        {
            //enable game object 
        }
    }

    public void disablePieces()
    {
        foreach (var piece in localPieces)
        {
            //disable game object 
        }

        foreach (var piece in opponentPieces)
        {
            //disable game object 
        }
    }

    public void enableButtons()
    {
        //disable all buttons
    }

    public void disableButtons()
    {
        // enable all buttons
    }



}




//void play()
//{


//    for (short i = 0; i < 9; ++i)
//    {
//        game.placePiece();
//    }

//    while (!game.gameOver())
//    {
//        if (game.phaseThree())
//        {
//            game.flyPiece();
//        }
//        else
//        {
//            game.movePiece();
//        }
//    }
//}