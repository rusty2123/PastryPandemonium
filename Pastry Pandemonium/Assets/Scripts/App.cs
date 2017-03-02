using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class App : MonoBehaviour
{
    //we need these documented, so we know what all of these variables do. some are obvious, but many are not

    #region variables

    public static GameObject gameInstance, boardInstance;

    private GameObject startPosition, endPosition, piecePosition, board, characterLocalPlayer, characterOpponentPlayer, piece;

    public GameObject shadow, chipMuffin, berryMuffin, lemonMuffin, chocolateCupcake, redCupcake, whiteCupcake;

    public NetworkGameManager networkManager;

    public Game game = Game.gameInstance;

    public Board gameBoard = Board.boardInstance;

    private static int localIndex = 0, opponentIndex = 0, remainingLocal = 9, remainingOpponent = 9;

    private GameObject[] opponentPieces = new GameObject[9];
    private GameObject[] localPieces = new GameObject[9];
    public GameObject[] outOfBoardSpaces = new GameObject[18];
    public GameObject[] boardSpaces = new GameObject[24];

    public static bool isSinglePlayer, gameOver = false, isLocalPlayerTurn, localPlayerWon;
    public static Player localPlayer, opponentPlayer;
    public static int phase = 1;

    private GameObject clickedFirst = null;
    private GameObject clickedSecond = null;

    public static int from;
    public static int to;

    #endregion

    void Awake()
    {
        gameBoard.initializeBoard();

        //why do we have two variables for the same game object?
        gameInstance = GameObject.FindGameObjectWithTag("gameBoard");
        boardInstance = GameObject.FindGameObjectWithTag("gameBoard");

        //why do we need this variable? why can't we just use Player.isSinglePlayer?
        isSinglePlayer = Player.isSinglePlayer;

        localPlayer = gameObject.AddComponent<Player>();
        opponentPlayer = gameObject.AddComponent<Player>();

        if (isSinglePlayer)
        {
            if (Player.playerGoFirst)
            {
                isLocalPlayerTurn = true;
                //change UI turn indicator
            }
            else
            {
                isLocalPlayerTurn = false;
                //change UI turn indicator
            }
            setUpPlayerPieces();
        }
        //if multiplayer
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
            setUpMultiPlayerPieces();
        }
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

    public void setUpMultiPlayerPieces()
    {
        if (networkManager.isMasterClient())
        {
            Debug.Log("starting game");
            //for now host will always go second, and will play as red cupcakes
            isLocalPlayerTurn = false;
            characterLocalPlayer = redCupcake;
            setUpPiecesLocal(characterLocalPlayer);

            characterOpponentPlayer = berryMuffin;
            setUpPiecesOpponent(characterOpponentPlayer);
        }
        else
        {
            //for now client will always go first, and will play as berry muffins
            isLocalPlayerTurn = true;
            characterLocalPlayer = berryMuffin;
            setUpPiecesLocal(characterLocalPlayer);

            characterOpponentPlayer = redCupcake;
            setUpPiecesOpponent(characterOpponentPlayer);
        }

        localPlayer.Pieces = localPieces;
        opponentPlayer.Pieces = opponentPieces;
    }

    void setUpPiecesLocal(GameObject localCharacter)
    {
        for (int i = 1; i < 10; i++)
        {

            piece = Instantiate(localCharacter) as GameObject;
            piecePosition = GameObject.Find("L-" + i);
            piece.transform.position = piecePosition.transform.position;
            piece.SetActive(true);
            piece.tag = "local";
            piece.name = "local" + (i).ToString();
            localPieces[i - 1] = piece;

        }

    }

    void setUpPiecesOpponent(GameObject opponentCharacter)
    {
        for (int i = 1; i < 10; i++)
        {

            piece = Instantiate(opponentCharacter) as GameObject;
            piecePosition = GameObject.Find("O-" + i);
            piece.transform.position = piecePosition.transform.position;
            piece.SetActive(true);
            piece.name = "opponent" + (i).ToString();
            piece.tag = "opponent";
            opponentPieces[i - 1] = piece;

        }
    }

    private void animationPhaseOne(GameObject gamePiece,GameObject startPosition, GameObject endPosition)
    {

        gamePiece.transform.position = startPosition.transform.position;
        shadow.transform.position = startPosition.transform.position;

        //scale piece
        LeanTween.scale(gamePiece, new Vector3(.7f, .7f, .7f), .6f).setDelay(.3f);
        LeanTween.scale(gamePiece, new Vector3(0.5f, 0.5f, 0.5f), 1.3f).setDelay(2.9f);

        //move piece up and then to the endPosition
        LeanTween.moveY(gamePiece, startPosition.transform.position.y + 22f, .6f).setDelay(.3f);
        LeanTween.move(gamePiece, endPosition.transform.position, 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(1.1f);

        //Move shadow
        LeanTween.move(shadow, endPosition.transform.position, 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(1.15f);

    }

    IEnumerator executeAIMove()
    {
        yield return new WaitForSeconds(3);
        if (remainingOpponent > 0)
            {
                int[] move = opponentPlayer.getAIMove();

                to = move[1];
                startPosition = opponentPieces[opponentIndex];
                endPosition = GameObject.Find(to.ToString());
                animationPhaseOne(startPosition, startPosition, endPosition);
                opponentIndex++;
                remainingOpponent--;
                changePlayer();
            }
        

       
        print("delay");
    }

    public void piecePlacementPhase(GameObject selected)
    {
        int index = Convert.ToInt32(selected.name);

        //need to verify the moves and update the game board
        if (isLocalPlayerTurn && Player.isSinglePlayer)
        {
            //check to make sure there are still pieces to play
            if (remainingLocal > 0)
            {
                //check to make sure the player is allowed to move there
                if (game.validPlace(index))
                {
                    Debug.Log("valid move");

                    //play move animation
                    startPosition = localPieces[localIndex];
                    endPosition = GameObject.Find(selected.name);
                    animationPhaseOne(startPosition, startPosition, endPosition);
                    localIndex++;
                    remainingLocal--;

                    //check if created mill
                    if(game.createdMill(index))
                    {
                        //remove piece
                    }
                    //opponent's turn
                    changePlayer();
                }
            }
        }
        //get move from AI
        if (!isLocalPlayerTurn && Player.isSinglePlayer)
        {
            Debug.Log("ai move");
            StartCoroutine(executeAIMove());
        }
        //send move over Network
        if(isLocalPlayerTurn && !Player.isSinglePlayer)
        {
            //check to make sure there are still pieces to play
            if (remainingLocal > 0)
            {
                if (game.validPlace(index))
                {
                    //place the piece and send it to the network
                    networkManager.placePiece(index);

                    //check if it created a mill
                    if(game.createdMill(index))
                    {
                        //remove a piece
                    }

                    //play the animation
                    startPosition = localPieces[localIndex];
                    endPosition = GameObject.Find(selected.name);
                    animationPhaseOne(startPosition, startPosition, endPosition);
                    localIndex++;
                    remainingLocal--;

                    if (game.createdMill(Convert.ToInt32(selected.name)))
                    {
                        //now get remove index from click
                    }
                    changePlayer();
                }
            }
        }
        //get move from network
        if (!isLocalPlayerTurn && !Player.isSinglePlayer)
        {

        }

        if (remainingOpponent == 0 && remainingLocal == 0)
        {
            phase = 2;
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


    /// <summary>
    /// UPDATE!!!!!!!!!!!!
    /// </summary>
    private void Update()
    {
        if (gameOver)
        {
            //change turn text to game over text

            if (!gameInstance.GetComponent<Game>().isDraw())
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
                if ((gameInstance.GetComponent<Game>().validMove(from, to)))
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




