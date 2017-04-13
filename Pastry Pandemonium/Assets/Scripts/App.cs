using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class App : Photon.PunBehaviour
{ 
    //we need these documented, so we know what all of these variables do. some are obvious, but many are not

    #region variables

    public static GameObject gameInstance, boardInstance;

    private GameObject startPosition, endPosition, piecePosition, board, characterLocalPlayer, characterOpponentPlayer, piece, availableSpace, availableSpaceImage;

    private GameObject turnPositionLeft, turnPositionRight, muffinTurnOff, muffinTurnOn, cupcakeTurnOff, cupcakeTurnOn;
    private string firstPlayer;
    private string createdMillHint = "You created a mill! Please remove an opponent’s piece.",
                         placementPhaseHint = "Placement phase: select an empty board space to place a piece.",
                         movementPhaseHint = "Movement phase: select the piece to move and then the space to move it to.",
                         flyingPhaseHint = "Flying phase: you can now move to any open space.",
                         falseRemoveHint = "Pieces may only be removed from a mill if ALL of the opponent’s pieces are in a mill.",
                         opponentFlyingHint = "Opponent has entered flying phase and can now move to any open space.";

    public GameObject shadow, chipMuffin, berryMuffin, lemonMuffin, chocolateCupcake, redCupcake, whiteCupcake, disconnected;
    public GameObject destroyBerry, destroyChip, destroyChocolate, destroyLemon, destroyRed, destroyWhite, destroyY2, destroy2;
    public Text hintText;
    private GameObject destroyPosition, tempDestroy1, tempDestroy2, millPosition, millPosition2, millPosition3;
	private GameObject win, lose, gameOverPosition, mainMenu, offTheBoard, draw, drawButton;
    public bool gameStarted = false;

    public NetworkGameManager networkManager;
    
    private Thread aiThread;
    private bool aiThreadRunning = false;
    public static bool aiMoveMade = false;

    public static int localIndex = 0, opponentIndex = 0, remainingLocal = 9, remainingOpponent = 9, outOfBoardOpponent = 9, outOfBoardLocal = 9;

    private static GameObject[] opponentPieces = new GameObject[9];
    private static GameObject[] localPieces = new GameObject[9];
    private List<GameObject> localPiecesList = new List<GameObject>(9);
    public GameObject[] outOfBoardSpaces = new GameObject[18];
    public GameObject[] boardSpaces = new GameObject[24];
    private static GameObject[] boardSpaces2 = new GameObject[24];
    private Collider2D[] colliders;

    private List<int> lastAvailableSpaces = new List<int>();

    public static  GameObject[] piecesPositions = new GameObject[24];

    public static bool isSinglePlayer, gameOver = false, isDraw = false, 
         isLocalPlayerTurn, localPlayerWon, removePiece = false, placePiece = false,
                        moveFromPiece = false, moveToPiece = false, flyFromPiece = false, flyToPiece = false, validMove=false,
                        showHelfulHints = true;

    public static Player localPlayer, opponentPlayer;
    public static int phase = 1;
    bool createdMill = false;

    private GameObject clickedFirst = null;
    private GameObject clickedSecond = null;

    public AudioClip removeSound;
    private AudioSource audioSource;

    //goal is to use selectedGamePiece to indicate which piece is clicked when deciding when to remove or move a piece
    public static int placeIndex, moveFromIndex, moveToIndex, flyFromIndex, flyToIndex, from, to, pieceToRemove, positionIndex;

    #endregion

    #region setup methods

    private void Start()
    {
    }

    void Awake()
    {
        disconnected.SetActive(false);
        updateHintText("");

        audioSource = GetComponent<AudioSource>();


        if (!NetworkGameManager.moveEventsAdded)
        {
            PhotonNetwork.OnEventCall += this.OnEvent;
            NetworkGameManager.moveEventsAdded = true;
        }

        resetBoard();

        //initialize event system

        Board.boardInstance.initializeBoard();

        gameInstance = GameObject.FindGameObjectWithTag("gameBoard");
        //DontDestroyOnLoad(gameInstance);
        boardInstance = GameObject.FindGameObjectWithTag("gameBoard");
        //DontDestroyOnLoad(boardInstance);
		win = GameObject.Find ("win");
		lose = GameObject.Find ("lose");
        draw = GameObject.Find("draw");
        drawButton = GameObject.Find("requestDraw");
        availableSpaceImage = GameObject.Find("availableSpace");

		win.GetComponent<Renderer> ().enabled = false;
		lose.GetComponent<Renderer> ().enabled = false;
        gameOverPosition = GameObject.Find("GameOverPosition");
        offTheBoard = GameObject.Find("offTheBoard");
        mainMenu = GameObject.Find("mainMenuButton");

      //  mainMenu.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
     //   mainMenu.GetComponent<BoxCollider2D>().enabled = false;

        isSinglePlayer = Player.isSinglePlayer;

        localPlayer = gameObject.AddComponent<Player>();
        opponentPlayer = gameObject.AddComponent<Player>();
        
        for (int i = 0; i <boardSpaces.Length; i++)
        {
            boardSpaces2[i] = boardSpaces[i];
        }
        muffinTurnOn = GameObject.Find("muffinTurn");
        cupcakeTurnOn = GameObject.Find("cupcakeTurn");
        turnPositionLeft = GameObject.Find("TurnIndicator1");
        turnPositionRight = GameObject.Find("TurnIndicator2");

        if (isSinglePlayer)
        {
			setUpTurnIndicator ();

            Debug.Log(Player.playerGoFirst);
            Debug.Log(Player.difficultyLevel);
            if (Player.playerGoFirst)
            {
                Debug.Log("player goes first");
                isLocalPlayerTurn = true;

                //change UI turn indicator
                StartCoroutine(changeTurnIndicator(true));
            }
            else
            {
                Debug.Log("ai goes first");
                isLocalPlayerTurn = false;
                //start ai by passing it an invalid move

                //change UI turn indicator
                StartCoroutine(changeTurnIndicator(false));
            }
            setUpPlayerPieces();
        }
        //if multiplayer
        else
		{
            setUpMultiplayerTurnIndicator();

            if (Player.firstPlayer)
            {
                StartCoroutine(changeTurnIndicator(true));
            }
            else
            {
                StartCoroutine(changeTurnIndicator(false));
            }
            setUpMultiPlayerPieces();
        }
        StartCoroutine(startGame());
    }

    private void setUpTurnIndicator()
    {
        muffinTurnOff = GameObject.Find("muffinTurnOff");
        cupcakeTurnOff = GameObject.Find("cupcakeTurnOff");

        if (SinglePlayerMenu.selectedCharacter.Contains("Muffin"))
        {
            muffinTurnOff.transform.position = turnPositionRight.transform.position;
            cupcakeTurnOff.transform.position = turnPositionLeft.transform.position;
            firstPlayer = "muffin";

        }
        else
        {
            cupcakeTurnOff.transform.position = turnPositionRight.transform.position;
            muffinTurnOff.transform.position = turnPositionLeft.transform.position;
            firstPlayer = "cupcake";
        }

        cupcakeTurnOn.transform.position = new Vector3(cupcakeTurnOff.transform.position.x + 2,
            cupcakeTurnOff.transform.position.y + 2,
            cupcakeTurnOff.transform.position.z - 3f);

        muffinTurnOn.transform.position = new Vector3(muffinTurnOff.transform.position.x - 10,
            muffinTurnOff.transform.position.y + 5,
            muffinTurnOff.transform.position.z - 3f);
    }

    private void setUpMultiplayerTurnIndicator()
    {
        muffinTurnOff = GameObject.Find("muffinTurnOff");
        cupcakeTurnOff = GameObject.Find("cupcakeTurnOff");

        if (Player.characterLocalPlayer.Contains("Muffin"))
        {
            muffinTurnOff.transform.position = turnPositionRight.transform.position;
            cupcakeTurnOff.transform.position = turnPositionLeft.transform.position;

            if (networkManager.isMasterClient())
            {         
                firstPlayer = "muffin";
            }
            else
            {
                firstPlayer = "cupcake";
            }

        }
        else if(Player.characterLocalPlayer.Contains("Cupcake") && networkManager.isMasterClient())
        {
            cupcakeTurnOff.transform.position = turnPositionRight.transform.position;
            muffinTurnOff.transform.position = turnPositionLeft.transform.position;

            if (networkManager.isMasterClient())
            {
                firstPlayer = "cupcake";
            }
            else
            {
                firstPlayer = "muffin";
            }
        }

        cupcakeTurnOn.transform.position = new Vector3(cupcakeTurnOff.transform.position.x + 2,
            cupcakeTurnOff.transform.position.y + 2,
            cupcakeTurnOff.transform.position.z - 3f);

        muffinTurnOn.transform.position = new Vector3(muffinTurnOff.transform.position.x - 10,
            muffinTurnOff.transform.position.y + 5,
            muffinTurnOff.transform.position.z - 3f);
    }

    IEnumerator changeTurnIndicator(bool firstPlayerTurn)
    {

        if (gameStarted)
        {
            yield return new WaitForSeconds(2.5f);
        }

        gameStarted = true;
        if (firstPlayerTurn)
        {
            if (firstPlayer == "muffin")
            {
                muffinTurnOn.GetComponent<Renderer>().enabled = true;
                cupcakeTurnOn.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                muffinTurnOn.GetComponent<Renderer>().enabled = false;
                cupcakeTurnOn.GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {

            if (firstPlayer == "muffin")
            {
                muffinTurnOn.GetComponent<Renderer>().enabled = false;
                cupcakeTurnOn.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                muffinTurnOn.GetComponent<Renderer>().enabled = true;
                cupcakeTurnOn.GetComponent<Renderer>().enabled = false;
            }

        }

    }

    IEnumerator startGame()
    {
        updateHintText(placementPhaseHint);
        //18 because piecePlacementPhase is called every time it is not your turn and when it is your turn
        for (int i = 0; i < 18; i++)
        {
            yield return StartCoroutine(piecePlacementPhase());
            updateHintText("");
        }
        while (!gameOver && !isDraw)
        {
            if (remainingLocal > 3 && remainingOpponent > 3)
            {
                phase = 2;
                Debug.Log("phase 2");
                shadow.transform.position = offTheBoard.transform.position;
                shadow.GetComponent<Renderer>().enabled = false;
                yield return StartCoroutine(pieceMovePhase());
                //Board.boardInstance.printBoard();
            }
            else if (remainingLocal == 3 || remainingOpponent == 3)
            {
                shadow.GetComponent<Renderer>().enabled = true;
                yield return StartCoroutine(pieceFlyPhase());
            }
        }
    }

    public void setUpPlayerPieces()
    {

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
            isLocalPlayerTurn = true;
        }
        else
        {
            isLocalPlayerTurn = false;
        }

        if (Player.characterLocalPlayer == "" || Player.characterOpponentPlayer == "")
        {
            if (networkManager.isMasterClient())
            {
                characterLocalPlayer = redCupcake;
                setUpPiecesLocal(characterLocalPlayer);

                characterOpponentPlayer = berryMuffin;
                setUpPiecesOpponent(characterOpponentPlayer);
            }
            else
            {
                characterLocalPlayer = berryMuffin;
                setUpPiecesLocal(characterLocalPlayer);

                characterOpponentPlayer = redCupcake;
                setUpPiecesOpponent(characterOpponentPlayer);
            }
        }

        else
        {
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
        }



		//setting up turn indicators for multiplayer
		muffinTurnOff = GameObject.Find("muffinTurnOff");
		cupcakeTurnOff = GameObject.Find("cupcakeTurnOff");

		if (Player.characterLocalPlayer.Contains("Muffin"))
		{
			muffinTurnOff.transform.position = turnPositionRight.transform.position;
			cupcakeTurnOff.transform.position = turnPositionLeft.transform.position;
			firstPlayer = "muffin";

		}
		else
		{
			cupcakeTurnOff.transform.position = turnPositionRight.transform.position;
			muffinTurnOff.transform.position = turnPositionLeft.transform.position;
			firstPlayer = "cupcake";
		}

		cupcakeTurnOn.transform.position = new Vector3(cupcakeTurnOff.transform.position.x + 2,
			cupcakeTurnOff.transform.position.y + 2,
			cupcakeTurnOff.transform.position.z - 3f);

		muffinTurnOn.transform.position = new Vector3(muffinTurnOff.transform.position.x - 10,
			muffinTurnOff.transform.position.y + 5,
			muffinTurnOff.transform.position.z - 3f);


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
            piece.GetComponent<GamePiece>().location = 0;
            piece.GetComponent<GamePiece>().owner = "local";
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
            piece.GetComponent<GamePiece>().location = 0;
            piece.GetComponent<GamePiece>().owner = "opponent";
            opponentPieces[i - 1] = piece;

        }
    }

    #endregion

    #region phase one

    IEnumerator piecePlacementPhase()
    {
        //if it's the local player's turn, and there are still pieces left to place
        if (isLocalPlayerTurn && outOfBoardLocal > 0)
        {
            enableGameObjects(true);

            yield return StartCoroutine(localPlacePiece());

            checkGameOver();
        }

        //get move from AI or network
        if (!isLocalPlayerTurn && outOfBoardOpponent > 0)
        {
            enableGameObjects(false);

            yield return StartCoroutine(opponentPlacePiece());

            checkGameOver();
        }

        Debug.Log("local= " + remainingLocal + " opponent= " + remainingOpponent);
        Debug.Log(gameOver);
    }

    IEnumerator localPlacePiece()
    {
        yield return StartCoroutine("getPlaceIndex");

        while (!validMove)
        {
            Debug.Log("not valid move.. select another place");
            yield return StartCoroutine("getPlaceIndex");
        }

        Game.gameInstance.placePiece(placeIndex, true);

        if (isSinglePlayer)
        {
            piecesPositions[placeIndex - 1] = localPieces[localIndex];
        }

        //send move over network if it's a networked game
        if (!Player.isSinglePlayer)
        {
            networkManager.placePiece(placeIndex);
            //changeTurnIndicator(true);
        }

        //play move animation
        startPosition = localPieces[localIndex];
        endPosition = GameObject.Find(placeIndex.ToString());
        animationPhaseOne(startPosition, startPosition, endPosition);
        localIndex++;
        outOfBoardLocal--;

        //check if the placement created a mill
        if (Game.gameInstance.createdMill(placeIndex))
        {
            Debug.Log("Local player a created mill!");
            updateHintText(createdMillHint);
            //allow the local player to select an opponent's piece to remove
            yield return StartCoroutine("getPieceToRemove");

            updateHintText("");
        }

        printPieceLocations();
        Debug.Log("Local turn over");
       
        changePlayer();
        networkManager.changePlayer();

        //turn indicator
        if (networkManager.isMasterClient())
            StartCoroutine(changeTurnIndicator(false));
        else
            StartCoroutine(changeTurnIndicator(false));
    }

    IEnumerator opponentPlacePiece()
    {
        Debug.Log("your turn: " + isLocalPlayerTurn);

        //get move from ai
        if (Player.isSinglePlayer)
        {
            yield return StartCoroutine(waitForSeconds(2));
            threadedAI();
            yield return StartCoroutine("waitForChangePlayer");
            yield return StartCoroutine(waitForSeconds(2));
        }
        //get move from network
        else if (!Player.isSinglePlayer)
        {
            //wait until OnEvent is triggered, get the index, run the animation, increment opponentIndex, decrement outOfBoardOpponent, change the player
            yield return StartCoroutine("waitForChangePlayer");
            yield return StartCoroutine(waitForSeconds(2));
        }
    }

    IEnumerator getPlaceIndex()
    {
        placePiece = true;

        yield return new WaitWhile(() => placePiece);
      
        yield return new WaitForSeconds(.7f);
     
    }

    IEnumerator executeAIMovePhaseOne()
    {
        Debug.Log("AI move phase one");
        if (!isDraw)
        {
            to = opponentPlayer.to;
            positionIndex = to + 1;

            Game.gameInstance.placePiece(to, false);
            piecesPositions[to] = opponentPieces[opponentIndex];

            startPosition = opponentPieces[opponentIndex];
            endPosition = GameObject.Find(positionIndex.ToString());
            animationPhaseOne(startPosition, startPosition, endPosition);

            opponentIndex++;
            outOfBoardOpponent--;

            printPieceLocations();

            //check if the placement created a mill
            if (Game.gameInstance.createdMill(to))
            {
                Debug.Log("AI a created mill");
                
                pieceToRemove = opponentPlayer.remove;
                yield return StartCoroutine("removeAIPiece");
            }

            changePlayer();
        }
        else
        {
            yield return new WaitWhile(() => isDraw);
            yield return StartCoroutine("executeAIMovePhaseOne");
        }
       
    }

    #endregion

    #region phase two

    IEnumerator pieceMovePhase()
    {
        //if it's the local player's turn, and there are still pieces left to place
        if (isLocalPlayerTurn)
        {
            enableGameObjects(true);

            yield return StartCoroutine(localMovePiece());
            checkGameOver();
        }
       
        //get move from AI or network
        if (!isLocalPlayerTurn && remainingOpponent > 3)
        {
            enableGameObjects(false);

            yield return StartCoroutine(opponentMovePiece());
            checkGameOver();
        }
        
    }

    IEnumerator localMovePiece()
    {
        moveToIndex = 0; moveFromIndex = 0;

        updateHintText(movementPhaseHint);

        yield return StartCoroutine(getMoveFromIndex());

        foreach (GameObject piece in localPieces)
        {
            if (piece != null && piece.GetComponent<GamePiece>().location == moveFromIndex)
            {      
                selectAnimation(piece);
                showAvailableMoves(Game.gameInstance.getValidMoves(moveFromIndex));         
            }
        }

        do
        {
            yield return StartCoroutine(getMoveToIndex());

            if (!validMove)
            {

                Debug.Log("Changing from location.");
                yield return StartCoroutine(getMoveFromIndex());
                foreach (GameObject piece in localPieces)
                {
                    if (piece != null && piece.GetComponent<GamePiece>().location == moveFromIndex)
                    {

                        selectAnimation(piece);
                        showAvailableMoves(Game.gameInstance.getValidMoves(moveFromIndex));

                    }
                }
            }

        } while (!Game.gameInstance.validMove(moveFromIndex, moveToIndex));

        updateHintText("");

        //place the piece and update the gameboard
        Game.gameInstance.moveLocalPiece(moveFromIndex, moveToIndex);

        //send move over network if it's a networked game
        if (!Player.isSinglePlayer)
        {
            networkManager.movePiece(moveFromIndex, moveToIndex);
        }

        //play move animation
        foreach (GameObject piece in localPieces)
        {
            if (piece != null && piece.GetComponent<GamePiece>().location == moveFromIndex)
            {
                if (isSinglePlayer)
                {
                    piecesPositions[moveToIndex - 1] = piece;
                    piecesPositions[moveFromIndex - 1] = null;
                }
                destroyAvailableMoves();
                //start position needs to be the gamepiece with location at moveFromIndex
                startPosition = piece;
                endPosition = GameObject.Find(moveToIndex.ToString());
                animationPhaseTwo(startPosition, startPosition, endPosition);
                piece.GetComponent<GamePiece>().location = moveToIndex;
                break;
            }
        }

        printPieceLocations();

        //check if the placement created a mill
        if (Game.gameInstance.createdMill(moveToIndex))
        {
            Debug.Log("Local player created a mill");
            updateHintText(createdMillHint);
            //allow the local player to select an opponent's piece to remove
            yield return StartCoroutine("getPieceToRemove");

            updateHintText("");
        }

        Debug.Log("Local turn over");
        printPieceLocations();
        changePlayer();
        networkManager.changePlayer();
        //turn indicator
        if (networkManager.isMasterClient())
            StartCoroutine(changeTurnIndicator(false));
        else
            StartCoroutine(changeTurnIndicator(false));
    }


    IEnumerator opponentMovePiece()
    {

        //get move from network
        if (!Player.isSinglePlayer)
        {
            //wait until OnEvent is triggered, get the index, run the animation, increment opponentIndex, decrement outOfBoardOpponent, change the player
            yield return StartCoroutine("waitForChangePlayer");
            yield return StartCoroutine(waitForSeconds(2));
        }
        else if(!gameOver && !isDraw)
        {
            threadedAI();
            yield return StartCoroutine("waitForChangePlayer");
            yield return StartCoroutine(waitForSeconds(2));
        }
    }

    IEnumerator getMoveFromIndex()
    {
        foreach (GameObject boardSpace in boardSpaces)
        {
            boardSpace.GetComponent<BoxCollider2D>().enabled = false;
        }

        moveFromPiece = true;

        yield return new WaitWhile(() => moveFromPiece);

        moveFromPiece = false;
    }

    IEnumerator getMoveToIndex()
    {
        enableGameObjects(true);

        moveToPiece = true;

        yield return new WaitWhile(() => moveToPiece);

        moveToPiece = false;
    }

    IEnumerator executeAIMovePhaseTwo()
    {
        yield return new WaitForSeconds(3f);
        if (!isDraw)
        {
            from = opponentPlayer.from;
            to = opponentPlayer.to;

            Debug.Log("AI moving from: " + from);
            Debug.Log("AI moving to:" + to);

            positionIndex = to + 1;

            //place the piece and update the gameboard
            Game.gameInstance.moveOpponentPiece(from, to);


            startPosition = piecesPositions[from];
            endPosition = GameObject.Find(positionIndex.ToString());
            animationPhaseTwo(startPosition, startPosition, endPosition);

            piecesPositions[to] = piecesPositions[from];
            piecesPositions[from] = null;

            foreach (GameObject piece in opponentPieces)
            {
                if (piece != null && piece.GetComponent<GamePiece>().location == from + 1)
                {
                    piece.GetComponent<GamePiece>().location = to + 1;
                    break;
                }
            }

            printPieceLocations();
            //check if the placement created a mill
            if (Game.gameInstance.createdMill(to))
            {
                Debug.Log("AI created a mill");

                pieceToRemove = opponentPlayer.remove;
                yield return StartCoroutine("removeAIPiece");
            }

            changePlayer();
            print("AI move done");
        }
        else
        {
            yield return new WaitWhile(() => isDraw);
            yield return StartCoroutine("executeAIMovePhaseTwo");
        }      
    }


    private void selectAnimation(GameObject gamePiece)
    {
        LeanTween.scale(gamePiece, new Vector3(.58f, .58f, .5f), .3f);
        LeanTween.scale(gamePiece, new Vector3(0.5f, 0.5f, 0.5f), .3f).setDelay(.6f);
    }

    private void animationPhaseTwo(GameObject gamePiece, GameObject startPosition, GameObject endPosition)
    {
  

        gamePiece.transform.position = startPosition.transform.position;

        
        LeanTween.move(gamePiece, endPosition.transform.position, 1.7f).setEase(LeanTweenType.easeOutQuint).setDelay(0.2f);
    }

    #endregion

    #region phase three

    IEnumerator pieceFlyPhase()
    {
        //if it's the local player's turn, and there are still pieces left to place
        if (isLocalPlayerTurn && remainingLocal == 3)
        {
            Debug.Log("Phase 3 - Local");
            phase = 3;
            enableGameObjects(true);
            yield return StartCoroutine(localFlyPiece());
        }
        else if(isLocalPlayerTurn && remainingLocal > 3)
        {
            enableGameObjects(true);
            Debug.Log("Phase 2 - Local");
            phase = 2;
            yield return StartCoroutine(localMovePiece());
        }

        //get move from AI or network
        if (!isLocalPlayerTurn && remainingOpponent == 3)
        {
            updateHintText(opponentFlyingHint);

            Debug.Log("Phase 3 - Opponent");
            phase = 3;
            enableGameObjects(false);

            yield return StartCoroutine(opponentFlyPiece());

            updateHintText(opponentFlyingHint);
        }
        else if (!isLocalPlayerTurn && remainingOpponent > 3)
        {
            Debug.Log("Phase 2 - Opponent");
            phase = 2;
            enableGameObjects(false);
            yield return StartCoroutine(opponentMovePiece());
        }

        checkGameOver();
    }

    IEnumerator localFlyPiece()
    {
        moveToIndex = 0; moveFromIndex = 0;

        updateHintText(flyingPhaseHint);

        do
        {
            yield return StartCoroutine(getFlyFromIndex());
            yield return StartCoroutine(getFlyToIndex());

        } while (!Game.gameInstance.validFly(flyFromIndex, flyToIndex));

        updateHintText("");

        //place the piece and update the gameboard
        Game.gameInstance.moveLocalPiece(flyFromIndex, flyToIndex);
       

        //send move over network if it's a networked game
        if (!Player.isSinglePlayer)
        {
            networkManager.flyPiece(flyFromIndex, flyToIndex);
        }

        //play move animation
        foreach (GameObject piece in localPieces)
        {
            if (piece != null && piece.GetComponent<GamePiece>().location == flyFromIndex)
            {
                //start position needs to be the gamepiece with location at moveFromIndex
                if (isSinglePlayer)
                {
                    piecesPositions[flyToIndex - 1] = piece;
                    piecesPositions[flyFromIndex - 1] = null;
                }
                startPosition = piece;
                endPosition = GameObject.Find(flyToIndex.ToString());
                animationPhaseOne(startPosition, startPosition, endPosition);
                piece.GetComponent<GamePiece>().location = flyToIndex;
                break;
            }
        }

        printPieceLocations();

        //check if the placement created a mill
        if (Game.gameInstance.createdMill(flyToIndex))
        {
            Debug.Log("Local created a mill");
            Board.boardInstance.printBoard();

            //allow the local player to select an opponent's piece to remove
            yield return StartCoroutine("getPieceToRemove");
        }

        Debug.Log("Local turn over");
        changePlayer();
        networkManager.changePlayer();
        //turn indicator
        if (networkManager.isMasterClient())
            StartCoroutine(changeTurnIndicator(false));
        else
            StartCoroutine(changeTurnIndicator(false));
    }

    IEnumerator opponentFlyPiece()
    {

        //get move from network
        if (!Player.isSinglePlayer)
        {
            //wait until OnEvent is triggered, get the index, run the animation, increment opponentIndex, decrement outOfBoardOpponent, change the player
            yield return StartCoroutine("waitForChangePlayer");
            yield return StartCoroutine(waitForSeconds(2));
        }
        else
        {
            threadedAI();
            yield return StartCoroutine("waitForChangePlayer");
            yield return StartCoroutine(waitForSeconds(2));

        }
    }

    IEnumerator getFlyFromIndex()
    {
        foreach (GameObject boardSpace in boardSpaces)
        {
            boardSpace.GetComponent<BoxCollider2D>().enabled = false;
        }

        flyFromPiece = true;

        yield return new WaitWhile(() => flyFromPiece);

        flyFromPiece = false;
    }

    IEnumerator getFlyToIndex()
    {
        enableGameObjects(true);

        flyToPiece = true;

        yield return new WaitWhile(() => flyToPiece);

        flyToPiece = false;
    }

    IEnumerator executeAIMovePhaseThree()
    {
        if (!isDraw)
        {
            from = opponentPlayer.from;
            to = opponentPlayer.to;
            Debug.Log("AI moving from: " + from);
            Debug.Log("AI moving to:" + to);

            positionIndex = to + 1;


            //place the piece and update the gameboard
            Game.gameInstance.moveOpponentPiece(from, to);

            startPosition = piecesPositions[from];
            endPosition = GameObject.Find(positionIndex.ToString());
            animationPhaseOne(startPosition, startPosition, endPosition);

            piecesPositions[to] = piecesPositions[from];
            piecesPositions[from] = null;

            //check if the placement created a mill
            if (Game.gameInstance.createdMill(to))
            {
                Debug.Log("AI created a mill");

                pieceToRemove = opponentPlayer.remove;
                yield return StartCoroutine("removeAIPiece");
            }

            changePlayer();
            print("AI move done");

        }
        else
        {
            yield return new WaitWhile(() => isDraw);
            yield return StartCoroutine("executeAIMovePhaseThree");
        }
    }
    #endregion

    #region utility methods

    public void updateHintText(String value)
    {
        if (showHelfulHints)
            hintText.text = value;
        else
            hintText.text = "";
    }

    public void printPieceLocations()
    {
        string localPieceLocations = "";
        string opponentPieceLocations = "";

        foreach (GameObject piece in localPieces)
        {
            if (piece != null)
            {
                localPieceLocations = localPieceLocations + piece.GetComponent<GamePiece>().location + " ";
            }
        }
        foreach (GameObject piece in opponentPieces)
        {
            if (piece != null)
            {
                opponentPieceLocations = opponentPieceLocations + piece.GetComponent<GamePiece>().location + " ";
            }
        }

        Debug.Log("local pieces: " + localPieceLocations);
        Debug.Log("local opponent: " + opponentPieceLocations);
    }

 
    private void enableGameObjects(bool enable)
    {
        if(enable)
        {
            foreach (GameObject boardSpace in boardSpaces)
            {
                boardSpace.GetComponent<BoxCollider2D>().enabled = true;
            }
            foreach (GameObject piece in localPieces)
            {
                if (piece != null)
                    piece.GetComponent<CircleCollider2D>().enabled = true;
            }
            foreach (GameObject piece in opponentPieces)
            {
                if (piece != null)
                    piece.GetComponent<CircleCollider2D>().enabled = true;
            }
        }
        else
        {
            foreach (GameObject boardSpace in boardSpaces)
            {
                boardSpace.GetComponent<BoxCollider2D>().enabled = false;
            }
            foreach (GameObject piece in localPieces)
            {
                if (piece != null)
                    piece.GetComponent<CircleCollider2D>().enabled = false;
            }
            foreach (GameObject piece in opponentPieces)
            {
                if (piece != null)
                    piece.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }

    IEnumerator getPieceToRemove()
    {
        //set all of the boardspaces inactive while selecting a piece to remove
        foreach(GameObject boardSpace in boardSpaces)
        {
            boardSpace.GetComponent<BoxCollider2D>().enabled = false;
        }

        removePiece = true;

        yield return new WaitWhile(() => removePiece);
        Debug.Log("Local removing opponent piece :" + pieceToRemove);

        if (!GamePiece.validPiece)
        {
            updateHintText(falseRemoveHint);

            removePiece = true;

            yield return new WaitWhile(() => removePiece && GamePiece.validPiece != false);

            updateHintText("");

        }

        removePiece = false;

        //pieceToRemove is set by the user clicking on a piece in GamePiece.cs
        Game.gameInstance.removeOpponentPiece(pieceToRemove);

        if (isSinglePlayer)
        {         
            animationRemove(piecesPositions[pieceToRemove - 1], characterOpponentPlayer);
            piecesPositions[pieceToRemove - 1] = null;
        }
        remainingOpponent--;

        if (!Player.isSinglePlayer)
        {
            networkManager.removePiece(pieceToRemove);

            foreach (GameObject gameObj in opponentPieces)
            {
                if (gameObj != null && gameObj.GetComponent<GamePiece>().location == pieceToRemove)
                {
                    animationRemove(gameObj, characterOpponentPlayer);
                    gameObj.GetComponent<GamePiece>().location = 0;
                    Destroy(gameObj);
                    Game.gameInstance.removeOpponentPiece(pieceToRemove);
                    Debug.Log("removed.");
                    break;
                }
            }

        }

        //reset them to active when piece is removed
        foreach (GameObject boardSpace in boardSpaces)
        {
            boardSpace.GetComponent<BoxCollider2D>().enabled = true;
        }
    }


    IEnumerator removeAIPiece()
    {
        yield return StartCoroutine(waitForSeconds(4));

        foreach (GameObject boardSpace in boardSpaces)
        {
            boardSpace.GetComponent<BoxCollider2D>().enabled = false;
        }

        Debug.Log("Piece to remove " + pieceToRemove);


        animationRemove(piecesPositions[pieceToRemove], characterLocalPlayer);
      //  Destroy(piecesPositions[pieceToRemove]);
       piecesPositions[pieceToRemove] = null;

        printPieceLocations();

        Game.gameInstance.removeLocalPiece(pieceToRemove);
        remainingLocal--;

        foreach (GameObject boardSpace in boardSpaces)
        {
            boardSpace.GetComponent<BoxCollider2D>().enabled = true;
        }

    }

    IEnumerator waitForSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }


    public void changePlayer()
    {
        if (isLocalPlayerTurn) // also check if it a draw ?
        {
            isLocalPlayerTurn = false;

            if (Player.firstPlayer)
            {
                StartCoroutine(changeTurnIndicator(false));
            }
            else
            {
                StartCoroutine(changeTurnIndicator(true));
            }

        }
        else if (!isLocalPlayerTurn)
        {
            isLocalPlayerTurn = true;

            if (isSinglePlayer)
            {
                StartCoroutine(changeTurnIndicator(true));

            }
            else
            {
                if (Player.firstPlayer)
                {
                    StartCoroutine(changeTurnIndicator(false));
                }
                else
                {
                    StartCoroutine(changeTurnIndicator(true));
                }

            }
        }
        Debug.Log("changed player");
    }

    public void checkGameOver()
    {
        if (remainingOpponent < 3 || remainingLocal < 3 || !Game.gameInstance.playerCanMove() || !Game.gameInstance.opponentCanMove())
        {
            gameOver = true;
           // mainMenu.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            shadow.GetComponent<Renderer>().enabled = false;
           // mainMenu.GetComponent<BoxCollider2D>().enabled = true;
            drawButton.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
            drawButton.GetComponent<BoxCollider2D>().enabled = false; ;
        }

    }

    private void checkMultiplayerGameOver()
    {
        if(gameOver && !isSinglePlayer)
        {
            networkManager.LeaveRoom();
        }
    }

    private void Update()
    {
        //checkMultiplayerGameOver();

        //Debug.Log("update has been called");
        if (!showHelfulHints)
        {
            updateHintText("");
        }
        if (gameOver)
        {
            if (remainingOpponent < 3 || !Game.gameInstance.opponentCanMove())
            {
                Debug.Log("you won");
				displayWinMessage ();
                if(!isSinglePlayer)
                {
                    networkManager.sendWin(1);
                }
            }
            else if (remainingLocal < 3 || !Game.gameInstance.playerCanMove())
            {
                Debug.Log("you lost");
				displayLossMessage ();
            }
        }
        if (isDraw)
        {
            Debug.Log("you drew");
            drawButton.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
            drawButton.GetComponent<BoxCollider2D>().enabled = false;
            displayTieMessage();
            isDraw = false;
            //display draw message
        }
        //AI move has returned with a value and these will call the functions for each phase
        if (!gameOver && isSinglePlayer && !isLocalPlayerTurn && phase == 1 && opponentPlayer.aiMadeMove)
        {
            Debug.Log("#3 (1) begin");
            aiThread.Abort();
            aiThreadRunning = false;
            opponentPlayer.aiMadeMove = false;

            StartCoroutine("executeAIMovePhaseOne");
            Debug.Log("#3 (1) end");
        }
        if (!gameOver && isSinglePlayer && !isLocalPlayerTurn && phase == 2 && opponentPlayer.aiMadeMove)
        {
            Debug.Log("#3 (2) begin");
            aiThread.Abort();
            aiThreadRunning = false;
            opponentPlayer.aiMadeMove = false;

            StartCoroutine("executeAIMovePhaseTwo");
            Debug.Log("#3 (2) end");
        }
        if (!gameOver && isSinglePlayer && !isLocalPlayerTurn && phase == 3 && opponentPlayer.aiMadeMove)
        {
            Debug.Log("#3 (3) begin");
            aiThread.Abort();
            aiThreadRunning = false;
            opponentPlayer.aiMadeMove = false;

            StartCoroutine("executeAIMovePhaseThree");
            Debug.Log("#3 (3) end");
        }
    }

    public void pauseGame()
    {
          foreach( GameObject boardSpace in boardSpaces2)
        {
            boardSpace.SetActive(false);
        }
    }

    public void resumeGame()
    {
        foreach (GameObject boardSpace in boardSpaces2)
        {
            boardSpace.SetActive(true);
        }
    }

    public void displayWinMessage()
    {
        //disable buttons and pieces
        //set game object active
        //dislay animation 
        pauseGame();

        muffinTurnOn.SetActive(false);
        cupcakeTurnOn.SetActive(false);
        win.GetComponent<Renderer> ().enabled = true;
       // LeanTween.moveY(muffinTurnOff, muffinTurnOff.transform.position.y + 150, .3f).setDelay(.5f);
        LeanTween.move(muffinTurnOff, offTheBoard.transform.position, .5f).setDelay(1f);
        LeanTween.move(cupcakeTurnOff, offTheBoard.transform.position, .5f).setDelay(1f);
       // LeanTween.moveY(cupcakeTurnOff, cupcakeTurnOff.transform.position.y + 150,.3f).setDelay(.5f);

        LeanTween.move(win, gameOverPosition.transform.position, .6f).setDelay(1.5f);

      


    }

    public void displayLossMessage()
    {
        //disable buttons and pieces
        //set game object active
        //dislay animation 
        pauseGame();

        lose.GetComponent<Renderer> ().enabled = true;

        muffinTurnOn.SetActive(false);
        cupcakeTurnOn.SetActive(false);

       // LeanTween.moveY(muffinTurnOff, muffinTurnOff.transform.position.y + 150, .3f).setDelay(.5f);
        LeanTween.move(muffinTurnOff, offTheBoard.transform.position, .5f).setDelay(1f);
        LeanTween.move(cupcakeTurnOff, offTheBoard.transform.position, .5f).setDelay(1f);
       // LeanTween.moveY(cupcakeTurnOff, cupcakeTurnOff.transform.position.y + 150, .3f).setDelay(.5f);

        LeanTween.move(lose, gameOverPosition.transform.position, .6f).setDelay(1.5f);
    }

    public void displayTieMessage()
    {
        //disable buttons and pieces
        //set game object active
        //dislay animation 
        pauseGame();
        muffinTurnOn.SetActive(false);
        cupcakeTurnOn.SetActive(false);
        LeanTween.move(muffinTurnOff, offTheBoard.transform.position, .5f).setDelay(1f);
        LeanTween.move(cupcakeTurnOff, offTheBoard.transform.position, .5f).setDelay(1f);
        LeanTween.move(draw, gameOverPosition.transform.position, .6f).setDelay(1.5f);
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

    private void disableColliders()
    {
        //disable all 2d colliders
        colliders = FindObjectsOfType<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void resetBoard()
    {
        //reset BitArrays
        //Board.boardInstance.resetBoard();
        Game.gameInstance.resetBoard();
        //enable all colliders
        enableColliders();

        //enable all game pieces
        //enableGamePieces();

        //set all game pieces to correct position
        //reset so many variables
        resetSoManyVariables();
        //how can we prevent game objects from getting destroyed?
    }

    private void enableColliders()
    {
        colliders = FindObjectsOfType<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }
    }

    private void enableGamePieces()
    {
        foreach (GameObject opponentPiece in opponentPieces)
        {
            opponentPiece.SetActive(true);
        }
        foreach (GameObject localPiece in localPieces)
        {
            localPiece.SetActive(true);
        }
    }

    private void resetSoManyVariables()
    {
        isSinglePlayer = false;
        isLocalPlayerTurn = false;
        localPlayerWon = false;
        placeIndex = 0; moveFromIndex = 0; moveToIndex = 0; flyFromIndex = 0; flyToIndex = 0; from = 0; to = 0; pieceToRemove = 0; positionIndex = 0;
        firstPlayer = "";
        gameStarted = false;
        localIndex = 0; opponentIndex = 0; remainingLocal = 9; remainingOpponent = 9; outOfBoardOpponent = 9; outOfBoardLocal = 9;
        gameOver = false; isDraw = false;
        removePiece = false; placePiece = false;
        moveFromPiece = false; moveToPiece = false; flyFromPiece = false; flyToPiece = false; validMove = false;
        phase = 1;
        createdMill = false;
        clickedFirst = null;
        clickedSecond = null;
        NetworkGameManager.placeIndex = 0; NetworkGameManager.removeIndex = 0; NetworkGameManager.moveFromIndex = 0; NetworkGameManager.moveToIndex = 0; NetworkGameManager.flyFromIndex = 0; NetworkGameManager.flyToIndex = 0;
        NetworkGameManager.localReady = false; NetworkGameManager.opponentReady = false;
        NetworkGameManager.drawResponseRecieved = false; NetworkGameManager.drawAccepted = false;
        aiThreadRunning = false;
}

    private void threadedAI()
    {
        Debug.Log("#1 begin");

        if (!aiThreadRunning && !gameOver)
        {
            aiThreadRunning = true;
            aiThread = new Thread(() => opponentPlayer.getThreadedAIMove(Board.boardInstance.getLocalPlayerBoard(), Board.boardInstance.getOpponentPlayerBoard()));
            aiThread.Start();
        }
        Debug.Log("#1 end");
    }

    #endregion

    #region Network Methods

    private void OnEvent(byte eventCode, object content, int senderid)
    {
        //if eventcode is 0, then it's placePiece
        if (eventCode == 0)
        {
            byte[] selected = (byte[])content;
            NetworkGameManager.placeIndex = selected[0];

            Debug.Log("recieved place: " + NetworkGameManager.placeIndex);

            //update gameboard
            Game.gameInstance.placePiece(NetworkGameManager.placeIndex, false);
            //Board.boardInstance.placePiece(NetworkGameManager.placeIndex, false);

            //run the animation
            if (remainingOpponent > 0)
            {
                to = NetworkGameManager.placeIndex;
                startPosition = opponentPieces[opponentIndex];
                endPosition = GameObject.Find(to.ToString());
                animationPhaseOne(startPosition, startPosition, endPosition);
                opponentIndex++;
                outOfBoardOpponent--;
            }
        }
        //if eventCode is 1, then it's removePiece
        else if (eventCode == 1)
        {
            byte[] selected = (byte[])content;
            NetworkGameManager.removeIndex = selected[0];

            Debug.Log("recieved remove: " + NetworkGameManager.removeIndex);

            //animationRemove(piecesPositions[NetworkGameManager.removeIndex], characterLocalPlayer);
            //piecesPositions[pieceToRemove] = null;

            foreach (GameObject gameObj in localPieces)
            {
                if (gameObj != null && gameObj.GetComponent<GamePiece>().location == NetworkGameManager.removeIndex)
                {
                    Debug.Log("removed.");
                    //animationRemove(gameObj, characterLocalPlayer);
                    Destroy(gameObj);
                    gameObj.GetComponent<GamePiece>().location = 0;
                    Game.gameInstance.removeLocalPiece(NetworkGameManager.removeIndex);
                    break;
                }
            }
            --remainingLocal;
        }
        //if eventCode is 2, then it's movePiece
        else if (eventCode == 2)
        {
            byte[] selected = (byte[])content;
            NetworkGameManager.moveFromIndex = selected[0];
            NetworkGameManager.moveToIndex = selected[1];

            Game.gameInstance.moveOpponentPiece(NetworkGameManager.moveFromIndex, NetworkGameManager.moveToIndex);
            //Board.boardInstance.moveOpponentPiece(NetworkGameManager.moveFromIndex, NetworkGameManager.moveToIndex);

            foreach (GameObject piece in opponentPieces)
            {
                if (piece != null && piece.GetComponent<GamePiece>().location == NetworkGameManager.moveFromIndex)
                {
                    //start position needs to be the gamepiece with location at moveFromIndex
                    startPosition = piece;
                    piece.GetComponent<GamePiece>().location = NetworkGameManager.moveToIndex;
                    break;
                }
            }
            endPosition = GameObject.Find(NetworkGameManager.moveToIndex.ToString());
            animationPhaseOne(startPosition, startPosition, endPosition);
        }
        //if eventCode is 3, then it's flyPiece
        else if (eventCode == 3)
        {
            byte[] selected = (byte[])content;
            NetworkGameManager.flyFromIndex = selected[0];
            NetworkGameManager.flyToIndex = selected[1];

            Game.gameInstance.moveOpponentPiece(NetworkGameManager.flyFromIndex, NetworkGameManager.flyToIndex);

            foreach (GameObject piece in opponentPieces)
            {
                if (piece != null && piece.GetComponent<GamePiece>().location == NetworkGameManager.flyFromIndex)
                {
                    //start position needs to be the gamepiece with location at moveFromIndex
                    startPosition = piece;
                    piece.GetComponent<GamePiece>().location = NetworkGameManager.flyToIndex;
                    break;
                }
            }
            endPosition = GameObject.Find(NetworkGameManager.flyToIndex.ToString());
            animationPhaseOne(startPosition, startPosition, endPosition);
        }
        else if (eventCode == 4)
        {
            isLocalPlayerTurn = true;

            //turn indicator
            if (networkManager.isMasterClient())
                StartCoroutine(changeTurnIndicator(true));
            else
                StartCoroutine(changeTurnIndicator(true));
        }

        else if (eventCode == 8)
        {
            //opponent replied to draw
            NetworkGameManager.drawAccepted = false;

            byte[] selected = (byte[])content;

            if (selected[0] == 0)
            {
                //opponent declined
                NetworkGameManager.drawAccepted = false;
                Debug.Log("opponent declined draw");
            }
            else if (selected[0] == 1)
            {
                //opponent accepted
                NetworkGameManager.drawAccepted = true;
                gameOver = true;
                isDraw = true;
                Debug.Log("opponent accepted draw");
                networkManager.LeaveRoom();
                //displayTieMessage();
                //networkManager.acceptDraw(1);
            }
        }
        else if (eventCode == 9)
        {
            byte[] selected = (byte[])content;

            gameOver = true;

            if (selected[0] == 1)
            {
                //displayLossMessage();
                remainingLocal = 2;
            }
            else if(selected[0] == 0)
            {
                Debug.Log("you win!");
                //displayWinMessage();
                remainingOpponent = 2;
            }

            networkManager.LeaveRoom();
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        //disconnected.GetComponentInChildren<Text>().text = "Opponent has disconnected";
        if (!gameOver)
        {
            Debug.Log("testing");
            disableColliders();
            networkManager.LeaveRoom();
            disconnected.SetActive(true);
        }
    }

    public override void OnDisconnectedFromPhoton()
    {
        //NetworkGameManager.opponentDisconnected = true;
        disconnected.GetComponentInChildren<Text>().text = "You have disconnected";
        disableColliders();
        //networkManager.LeaveRoom();
        networkManager.LeaveRoom();
        MultiplayerSetup.selectedCharacter = "";
        Player.characterLocalPlayer = "";
        Player.characterOpponentPlayer = "";
        NetworkGameManager.localReady = false;
        NetworkGameManager.opponentReady = false;
        disconnected.SetActive(true);
    }

    public void ReturnToLobby()
    {
        disconnected.SetActive(false);

        if (PhotonNetwork.connected)
        {
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            SceneManager.LoadScene("mainMenu");
        }
    }

    IEnumerator waitForChangePlayer()
    {
        yield return new WaitUntil(() => isLocalPlayerTurn);
    }

    #endregion

    #region Animations

    private void animationPhaseOne(GameObject gamePiece, GameObject startPosition, GameObject endPosition)
    {
        if (shadow != null)
        {
            shadow.SetActive(true);
        }
        gamePiece.transform.position = startPosition.transform.position;

        if (shadow != null)
        {
            shadow.transform.position = new Vector3(startPosition.transform.position.x,
            startPosition.transform.position.y,
            startPosition.transform.position.z + 2);
        }

        //scale piece
        LeanTween.scale(gamePiece, new Vector3(.65f, .65f, .5f), .5f);
        LeanTween.scale(gamePiece, new Vector3(0.5f, 0.5f, 0.5f), .5f).setDelay(2.5f);

        //move piece up and then to the endPosition
        LeanTween.moveY(gamePiece, startPosition.transform.position.y + 140f, .5f);
        LeanTween.move(gamePiece, endPosition.transform.position, 2f).setEase(LeanTweenType.easeInOutQuint).setDelay(.6f);

        if (shadow != null)
        {
            //Move shadow
            LeanTween.move(shadow, new Vector3(endPosition.transform.position.x,
            endPosition.transform.position.y,
            endPosition.transform.position.z + 2), 2f).setEase(LeanTweenType.easeInOutQuint).setDelay(.63f);
        }

        gamePiece.GetComponent<GamePiece>().location = Convert.ToInt32(endPosition.name);

    }

    public void animationCreatedMill(int itemOne, int itemTwo, int itemThree)
    {

        foreach (GameObject space in boardSpaces2)
        {

            if (space.name == itemOne.ToString())
            {
                millPosition = space;
            }
            else if (space.name == itemTwo.ToString())
            {
                millPosition2 = space;
            }
            else if (space.name == itemThree.ToString())
            {
                millPosition3 = space;
            }

        }

        if(isLocalPlayerTurn)
        {
            foreach (GameObject piece in localPieces)
            {
                if (piece != null)
                {
                    if (piece.transform.position == millPosition.transform.position)
                    {
                        LeanTween.scale(piece, new Vector3(.65f, .65f, .5f), .5f);
                        LeanTween.rotateAround(piece, Vector3.forward, -180f, 1f);
                        LeanTween.scale(piece, new Vector3(.5f, .5f, .5f), .5f).setDelay(.5f);
                    }
                    else if (piece.transform.position == millPosition2.transform.position)
                    {
                        LeanTween.scale(piece, new Vector3(.65f, .65f, .5f), .5f);
                        LeanTween.rotateAround(piece, Vector3.forward, -180f, 1f);
                        LeanTween.scale(piece, new Vector3(.5f, .5f, .5f), .5f).setDelay(.5f);

                    }
                    else if (piece.transform.position == millPosition3.transform.position)
                    {
                        LeanTween.scale(piece, new Vector3(.65f, .65f, .5f), .5f);
                        LeanTween.rotateAround(piece, Vector3.forward, -180f, 1f);
                        LeanTween.scale(piece, new Vector3(.5f, .5f, .5f), .5f).setDelay(.5f);

                    }
                }

            }
        }

        else
        {
            foreach (GameObject piece in opponentPieces)
            {
                if (piece != null)
                {
                    if (piece.transform.position == millPosition.transform.position)
                    {
                        LeanTween.scale(piece, new Vector3(.65f, .65f, .5f), .5f);
                        LeanTween.rotateAround(piece, Vector3.forward, -180f, 1f);
                        LeanTween.scale(piece, new Vector3(.5f, .5f, .5f), .5f).setDelay(.5f);
                    }
                    else if (piece.transform.position == millPosition2.transform.position)
                    {
                        LeanTween.scale(piece, new Vector3(.65f, .65f, .5f), .5f);
                        LeanTween.rotateAround(piece, Vector3.forward, -180f, 1f);
                        LeanTween.scale(piece, new Vector3(.5f, .5f, .5f), .5f).setDelay(.5f);

                    }
                    else if (piece.transform.position == millPosition3.transform.position)
                    {
                        LeanTween.scale(piece, new Vector3(.65f, .65f, .5f), .5f);
                        LeanTween.rotateAround(piece, Vector3.forward, -180f, 1f);
                        LeanTween.scale(piece, new Vector3(.5f, .5f, .5f), .5f).setDelay(.5f);

                    }
                }

            }
        }
    }

    private void showAvailableMoves(List<int> availableMoves)
    {

        if (lastAvailableSpaces.Count > 0)
        {
            destroyAvailableMoves();
        }
        lastAvailableSpaces = availableMoves;
        foreach (int m in availableMoves)
        {
            availableSpace = Instantiate(availableSpaceImage) as GameObject;
            piecePosition = GameObject.Find(m.ToString());

            availableSpace.name = "AS" + m.ToString();

            availableSpace.transform.position = piecePosition.transform.position;
            
        }
    }

    private void destroyAvailableMoves()
    {
        if (lastAvailableSpaces.Count > 0)
        {
            foreach (int m in lastAvailableSpaces)
            {
                Destroy(GameObject.Find("AS" + m.ToString()));
            }
        }

        lastAvailableSpaces.Clear();
    }
    private void animationRemove(GameObject gamePiece, GameObject player)
    {
        destroyPosition = gamePiece;
        StartCoroutine(playRemoveSound(0f));
        Destroy(gamePiece);

        switch (player.name)
        {
            case "berryGamePiece":
                setUpDestroyPieces(destroyBerry, destroy2);
                break;
            case "chipGamePiece":
                setUpDestroyPieces(destroyChip, destroy2);
                break;
            case "lemonGamePiece":
                setUpDestroyPieces(destroyLemon, destroyY2);
                break;
            case "chocolateGamePiece":
                setUpDestroyPieces(destroyChocolate, destroy2);
                break;
            case "redGamePiece":
                setUpDestroyPieces(destroyRed, destroy2);
                break;
            case "whiteGamePiece":
                setUpDestroyPieces(destroyWhite, destroy2);
                break;
            default:
                break;

        }
    }

    
    void setUpDestroyPieces(GameObject destroyFirst, GameObject destroySecond)
    {

       tempDestroy1 = Instantiate(destroyFirst) as GameObject;
        tempDestroy2 = Instantiate(destroySecond) as GameObject;

        tempDestroy1.SetActive(true);
        tempDestroy2.SetActive(true);


        tempDestroy1.transform.position = destroyPosition.transform.position;

        tempDestroy2.transform.position = destroyPosition.transform.position;
        
        Destroy(tempDestroy1, .7f);
        StartCoroutine(playRemoveSound(.45f));
        Destroy(tempDestroy2, 1.2f);

        StartCoroutine(playRemoveSound(1.05f));
    }


    IEnumerator playRemoveSound(float time)
    {
        yield return new WaitForSeconds(time);
        if (Music.playSoundEffects)
        {
            audioSource.PlayOneShot(removeSound, .5f);
        }

    }


    #endregion
}


