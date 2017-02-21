using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class App : MonoBehaviour
{

    Game game = Game.gameInstance;

    private Board boardInstance;

	private GameObject piecePosition;

    public GameObject chipMuffin;
    public GameObject berryMuffin;
    public GameObject lemonMuffin;
    public GameObject chocolateCupcake;
    public GameObject redCupcake;
    public GameObject whiteCupcake;

    private GameObject board;
    private GameObject characterLocalPlayer;
    private GameObject characterOpponentPlayer;
    private GameObject[] opponentPieces = new GameObject[9];
    private GameObject[] localPieces = new GameObject[9];
    public GameObject[] outOfBoardSpaces = new GameObject[18];
    public GameObject[] boardSpaces = new GameObject[24];

    private GameObject piece;
    public static bool isSinglePlayer;
    public static bool gameOver = false;
    public static bool isLocalPlayerTurn;
    public static bool localPlayerWon;
    public static Player localPlayer;
    public static Player opponentPlayer;

    private GameObject clickedFirst = null;
    private GameObject clickedSecond = null;

    public static int from;
    public static int to;

    void Awake()
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
        setUpPlayerPieces();

    }


    public void setUpPlayerPieces()
    {

        // boardInstance.initializeBoard();

        Player.characterLocalPlayer = SinglePlayerMenu.selectedCharacter;
        Player.characterOpponentPlayer = "chipMuffin";

        switch (Player.characterLocalPlayer)
        {
            case "berryMuffin":
                characterLocalPlayer = berryMuffin;
                setUpPiecesLocal(characterLocalPlayer);
                break;
            case "chipMuffin":
                characterLocalPlayer = chipMuffin;
                setUpPiecesLocal(characterLocalPlayer);
                break;
            case "lemonMuffin":
                characterLocalPlayer = lemonMuffin;
                setUpPiecesLocal(characterLocalPlayer);
                break;
            case "chocolateCupcake":
                characterLocalPlayer = chocolateCupcake;
                setUpPiecesLocal(characterLocalPlayer);
                break;
            case "redCupcake":
                characterLocalPlayer = redCupcake;
                setUpPiecesLocal(characterLocalPlayer);
                break;
            case "whiteCupcake":
                characterLocalPlayer = whiteCupcake;
                setUpPiecesLocal(characterLocalPlayer);
                break;
            default:
                break;

        }

        if (Player.characterLocalPlayer.Contains("Muffin"))
        {
            int character = UnityEngine.Random.Range(3, 5);
            Player.characterOpponentPlayer = Player.characterSelection[character];
        }
        else if (Player.characterLocalPlayer.Contains("Cupcake"))
        {
            int character = UnityEngine.Random.Range(0, 2);
            Player.characterOpponentPlayer = Player.characterSelection[character];

        }
       
        switch (Player.characterOpponentPlayer)
        {
            case "berryMuffin":
                characterOpponentPlayer = berryMuffin;
                setUpPiecesOpponent(characterOpponentPlayer);
                break;
            case "chipMuffin":
                characterOpponentPlayer = chipMuffin;
                setUpPiecesOpponent(characterOpponentPlayer);
                break;
            case "lemonMuffin":
                characterOpponentPlayer = lemonMuffin;
                setUpPiecesOpponent(characterOpponentPlayer);
                break;
            case "chocolateCupcake":
                characterOpponentPlayer = chocolateCupcake;
                setUpPiecesOpponent(characterOpponentPlayer);
                break;
            case "redCupcake":
                characterOpponentPlayer = redCupcake;
                setUpPiecesOpponent(characterOpponentPlayer);
                break;
            case "whiteCupcake":
                characterOpponentPlayer = whiteCupcake;
                setUpPiecesOpponent(characterOpponentPlayer);
                break;
            default:
                break;

        }

        localPlayer.Pieces = localPieces;
        opponentPlayer.Pieces = opponentPieces;

    }

    void setUpPiecesLocal(GameObject localCharacter)
    {
        for (int i = 1; i < 10; i++)
        {
            piece = Instantiate(localCharacter) as GameObject;

			piecePosition = GameObject.Find ("L-" + i);
			piece.transform.position = piecePosition.transform.position;
            piece.SetActive(true);
            piece.tag = "local";
           
            piece.name = "local" + (i).ToString();
            localPieces[i-1] = piece;

        }

    }

    void setUpPiecesOpponent(GameObject opponentCharacter)
    {
       

        for (int i = 1; i < 10; i++)
        {
            
            piece = Instantiate(opponentCharacter) as GameObject;

            
            piece.SetActive(true);
			piecePosition = GameObject.Find ("O-" + i);
			piece.transform.position = piecePosition.transform.position;
           
            piece.name = "opponent" + (i).ToString();
            piece.tag = "opponent";
            opponentPieces[i-1] = piece;

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
        if (gameOver)
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