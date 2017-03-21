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

    private GameObject turnPositionLeft, turnPositionRight, muffinTurnOff, muffinTurnOn, cupcakeTurnOff, cupcakeTurnOn;
    private string firstPlayer;

    public GameObject shadow, chipMuffin, berryMuffin, lemonMuffin, chocolateCupcake, redCupcake, whiteCupcake;

    public NetworkGameManager networkManager;

    public Game game = Game.gameInstance;

    public Board gameBoard = Board.boardInstance;

    private static int localIndex = 0, opponentIndex = 0, remainingLocal = 9, remainingOpponent = 9, outOfBoardOpponent = 9, outOfBoardLocal = 9;

    private GameObject[] opponentPieces = new GameObject[9];
    private GameObject[] localPieces = new GameObject[9];
    private List<GameObject> localPiecesList = new List<GameObject>(9);
    public GameObject[] outOfBoardSpaces = new GameObject[18];
    public GameObject[] boardSpaces = new GameObject[24];
    public static  GameObject[] piecesPositions = new GameObject[24];

    public static bool isSinglePlayer, gameOver = false, isLocalPlayerTurn, localPlayerWon, removePiece = false, placePiece = false,
                        moveFromPiece = false, moveToPiece = false, flyFromPiece = false, flyToPiece = false, validMove=false;

    public static Player localPlayer, opponentPlayer;
    public static int phase = 1;
    bool createdMill = false;

    private GameObject clickedFirst = null;
    private GameObject clickedSecond = null;

    //goal is to use selectedGamePiece to indicate which piece is clicked when deciding when to remove or move a piece
    public static int placeIndex, moveFromIndex, moveToIndex, flyFromIndex, flyToIndex, from, to, pieceToRemove, positionIndex;

    #endregion


    #region setup methods

    void Awake()
    {
        //initialize event system
        PhotonNetwork.OnEventCall += this.OnEvent;

        gameBoard.initializeBoard();

        //why do we have two variables for the same game object?
        gameInstance = GameObject.FindGameObjectWithTag("gameBoard");
        boardInstance = GameObject.FindGameObjectWithTag("gameBoard");

        //why do we need this variable? why can't we just use Player.isSinglePlayer?
        isSinglePlayer = Player.isSinglePlayer;

        localPlayer = gameObject.AddComponent<Player>();
        opponentPlayer = gameObject.AddComponent<Player>();




        muffinTurnOn = GameObject.Find("muffinTurn");
        cupcakeTurnOn = GameObject.Find("cupcakeTurn");
        turnPositionLeft = GameObject.Find("TurnIndicator1");
        turnPositionRight = GameObject.Find("TurnIndicator2");

        //setUpTurnIndicator ();

        if (isSinglePlayer)
        {
            Debug.Log(Player.playerGoFirst);
            Debug.Log(Player.difficultyLevel);
            if (Player.playerGoFirst)
            {
                Debug.Log("player goes first");
                isLocalPlayerTurn = true;

                //change UI turn indicator
                changeTurnIndicator(true);
            }
            else
            {
                Debug.Log("ai goes first");
                isLocalPlayerTurn = false;
                //start ai by passing it an invalid move

                //change UI turn indicator
                changeTurnIndicator(false);
            }
            setUpPlayerPieces();
        }
        //if multiplayer
        else
        {
            if (Player.firstPlayer)
            {
                changeTurnIndicator(true);
            }
            else
            {
                changeTurnIndicator(false);
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

    private void changeTurnIndicator(bool firstPlayerTurn)
    {

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
        //18 because piecePlacementPhase is called every time it is not your turn and when it is your turn
        for (int i = 0; i < 18; i++)
        {
            yield return StartCoroutine(piecePlacementPhase());
            //Board.boardInstance.printBoard();
        }
        while (!gameOver)
        {
            if (remainingLocal > 3)
            {
                phase = 2;
                Debug.Log("phase 2");
                yield return StartCoroutine(pieceMovePhase());
                //Board.boardInstance.printBoard();
            }
            else if (remainingOpponent > 3)
            {
                phase = 2;
                Debug.Log("phase 2");
                yield return StartCoroutine(pieceMovePhase());
            }
            else
            {
                phase = 3;
                Debug.Log("phase 3");
                yield return StartCoroutine(pieceFlyPhase());
            }
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
        //    Debug.Log("starting game");
        //    //for now host will always go second, and will play as red cupcakes
            isLocalPlayerTurn = true;
        //    characterLocalPlayer = redCupcake;
        //    setUpPiecesLocal(characterLocalPlayer);

        //    characterOpponentPlayer = berryMuffin;
        //    setUpPiecesOpponent(characterOpponentPlayer);
        }
        else
        {
        //    //for now client will always go first, and will play as berry muffins
            isLocalPlayerTurn = false;
        //    characterLocalPlayer = berryMuffin;
        //    setUpPiecesLocal(characterLocalPlayer);

        //    characterOpponentPlayer = redCupcake;
        //    setUpPiecesOpponent(characterOpponentPlayer);
        }

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
            //localPiecesList[i - 1] = piece;
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

            if (remainingOpponent < 3 || remainingLocal < 3)
            {
                gameOver = true;
            }
        }

        //get move from AI or network
        if (!isLocalPlayerTurn && outOfBoardOpponent > 0)
        {
            enableGameObjects(false);

            yield return StartCoroutine(opponentPlacePiece());

            if (remainingOpponent < 3 || remainingLocal < 3)
            {
                gameOver = true;
            }
        }
    
        Debug.Log("local= " + remainingLocal + " opponent= " + remainingOpponent);
        Debug.Log(gameOver);     
    }

    IEnumerator getPlaceIndex()
    {
        placePiece = true;

        yield return new WaitWhile(() => placePiece);
    }

    IEnumerator executeAIMovePhaseOne()
    {
        yield return new WaitForSeconds(4);

        int[] move = opponentPlayer.getAIMove();

        to = move[1];
        positionIndex = to + 1;

        if (!Game.gameInstance.validPlace(to))
        {
            Debug.Log("ai not valid move");
            yield return StartCoroutine("executeAIMovePhaseOne");
        }
        else if (Game.gameInstance.validPlace(to))
        {

            //Debug.Log("valid move");
            Game.gameInstance.placePiece(to, false);
            piecesPositions[to] = opponentPieces[opponentIndex];
            Debug.Log(piecesPositions[to].name);

            startPosition = opponentPieces[opponentIndex];
            endPosition = GameObject.Find(positionIndex.ToString());
            animationPhaseOne(startPosition, startPosition, endPosition);

            opponentIndex++;
            outOfBoardOpponent--;

            //check if the placement created a mill
            //if (createdMill)
            if (Game.gameInstance.createdMill(to))
            {
                Debug.Log("you created mill");
                //allow the local player to select an opponent's piece to remove
                yield return StartCoroutine("removeAIPiece");
            }

           // createdMill = true;
            changePlayer();
            //print("AI move done");

        } 
    }

    IEnumerator localPlacePiece()
    {
        yield return StartCoroutine("getPlaceIndex");

        //place the piece and update the gameboard

        while (!validMove)
        {
            Debug.Log("not valid move.. select another place");
            yield return StartCoroutine("getPlaceIndex");

        }

        Game.gameInstance.placePiece(placeIndex, true);
        //Board.boardInstance.placePiece(placeIndex, true);
        piecesPositions[placeIndex - 1] = localPieces[localIndex];

        //send move over network if it's a networked game
        if (!Player.isSinglePlayer)
        {
            networkManager.placePiece(placeIndex);
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
            Debug.Log("you created mill");
            //allow the local player to select an opponent's piece to remove
            yield return StartCoroutine("getPieceToRemove");
        }

        Debug.Log("turn over");
        //isLocalPlayerTurn = false;
        changePlayer();
        networkManager.changePlayer();
    }

    IEnumerator opponentPlacePiece()
    {
        Debug.Log("your turn: " + isLocalPlayerTurn);

        //get move from ai
        if (Player.isSinglePlayer)
        {
            //check to make sure the player is allowed to move there
            yield return StartCoroutine("executeAIMovePhaseOne");
        }
        //get move from network
        else if (!Player.isSinglePlayer)
        {
            //wait until OnEvent is triggered, get the index, run the animation, increment opponentIndex, decrement outOfBoardOpponent, change the player
            yield return StartCoroutine("waitForChangePlayer");
            yield return StartCoroutine(waitForSeconds(2));
        }
    }

    private void animationPhaseOne(GameObject gamePiece, GameObject startPosition, GameObject endPosition)
    {
        shadow.SetActive(true);
        gamePiece.transform.position = startPosition.transform.position;

        shadow.transform.position = new Vector3(startPosition.transform.position.x,
            startPosition.transform.position.y,
            startPosition.transform.position.z + 2);

        //scale piece
        LeanTween.scale(gamePiece, new Vector3(.65f, .65f, .65f), .6f).setDelay(.2f);
        LeanTween.scale(gamePiece, new Vector3(0.5f, 0.5f, 0.5f), 1.3f).setDelay(2.7f);

        //move piece up and then to the endPosition
        LeanTween.moveY(gamePiece, startPosition.transform.position.y + 140f, .6f).setDelay(.2f);
        LeanTween.move(gamePiece, endPosition.transform.position, 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(.9f);

        //Move shadow
        LeanTween.move(shadow, new Vector3(endPosition.transform.position.x,
            endPosition.transform.position.y,
            endPosition.transform.position.z + 2), 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(.93f);

        gamePiece.GetComponent<GamePiece>().location = Convert.ToInt32(endPosition.name);
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
        }
        else if (!isLocalPlayerTurn && Player.isSinglePlayer)
        {
            yield return StartCoroutine(executeAIMovePhaseTwo());
        }

        //get move from AI or network
        if (!isLocalPlayerTurn && !Player.isSinglePlayer && remainingOpponent > 3)
        {
            enableGameObjects(false);

            yield return StartCoroutine(opponentMovePiece());
        }

        if (remainingOpponent < 3 || remainingLocal < 3)
        {
            gameOver = true;
        }
    }

    IEnumerator localMovePiece()
    {
        moveToIndex = 0; moveFromIndex = 0;

       do
       {
            yield return StartCoroutine(getMoveFromIndex());
            yield return StartCoroutine(getMoveToIndex());

       } while (!Game.gameInstance.validMove(moveFromIndex, moveToIndex));

        //place the piece and update the gameboard
        Game.gameInstance.moveLocalPiece(moveFromIndex, moveToIndex);
        //Board.boardInstance.moveLocalPiece(from, to);


        //send move over network if it's a networked game
        if (!Player.isSinglePlayer)
        {
            networkManager.movePiece(moveFromIndex, moveToIndex);
        }

        //play move animation
        foreach(GameObject piece in localPieces)
        {
            if(piece != null && piece.GetComponent<GamePiece>().location == moveFromIndex)
            {
                //start position needs to be the gamepiece with location at moveFromIndex
                startPosition = piece;
                endPosition = GameObject.Find(moveToIndex.ToString());
                animationPhaseOne(startPosition, startPosition, endPosition);
                piece.GetComponent<GamePiece>().location = moveToIndex;
                break;
            }
        }

        printPieceLocations();

        //check if the placement created a mill
        if (Game.gameInstance.createdMill(moveToIndex))
        {
            Debug.Log("you created mill");
            Board.boardInstance.printBoard();
            //allow the local player to select an opponent's piece to remove
            yield return StartCoroutine("getPieceToRemove");
        }

        Debug.Log("turn over");
        changePlayer();
        networkManager.changePlayer();
    }

    IEnumerator executeAIMovePhaseTwo()
    {
        yield return new WaitForSeconds(2);

        int[] move = opponentPlayer.getAIMove();

        from = move[0];
        to = move[1];

        positionIndex = to + 1;
        if (!Game.gameInstance.validPlace(to))
        {
            Debug.Log("ai not valid move");
        }
        else if (Game.gameInstance.validPlace(to))
        {

            Debug.Log("valid move");
            Game.gameInstance.placePiece(to, false);

            //piecesPositions[to] = opponentPieces[opponentIndex];
            //Debug.Log(piecesPositions[to].name);

            startPosition = piecesPositions[from];
            endPosition = GameObject.Find(positionIndex.ToString());
            animationPhaseOne(startPosition, startPosition, endPosition);


            //check if the placement created a mill
            if (Game.gameInstance.createdMill(to))
            {
                Debug.Log("you created mill");
                //allow the local player to select an opponent's piece to remove
                yield return StartCoroutine("removeAIPiece");
            }

            changePlayer();
            print("AI move done");

        }
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

    #endregion

    #region phase three

    IEnumerator pieceFlyPhase()
    {
        //if it's the local player's turn, and there are still pieces left to place
        if (isLocalPlayerTurn)
        {
            enableGameObjects(true);

            yield return StartCoroutine(localFlyPiece());
        }

        //get move from AI or network
        if (!isLocalPlayerTurn && outOfBoardOpponent > 0)
        {
            enableGameObjects(false);

            yield return StartCoroutine(opponentFlyPiece());
        }
    }

    IEnumerator localFlyPiece()
    {
        moveToIndex = 0; moveFromIndex = 0;

        do
        {
            yield return StartCoroutine(getFlyFromIndex());
            yield return StartCoroutine(getFlyToIndex());

        } while (!Game.gameInstance.validFly(flyFromIndex, flyToIndex));

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
            Debug.Log("you created mill");
            Board.boardInstance.printBoard();
            //allow the local player to select an opponent's piece to remove
            yield return StartCoroutine("getPieceToRemove");
        }

        Debug.Log("turn over");
        changePlayer();
        networkManager.changePlayer();
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

    #endregion

    #region utility methods

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

        removePiece = false;

        //pieceToRemove is set by the user clicking on a piece in GamePiece.cs
        Game.gameInstance.removeOpponentPiece(pieceToRemove);
        --remainingOpponent;

        if (!Player.isSinglePlayer)
        {
            networkManager.removePiece(pieceToRemove);
        }

        //reset them to active when piece is removed
        foreach (GameObject boardSpace in boardSpaces)
        {
            boardSpace.GetComponent<BoxCollider2D>().enabled = true;
        }
    }


    IEnumerator removeAIPiece()
    {
        yield return new WaitForSeconds(2);

        Debug.Log("removeAIPiece called");

        //get AI piece to remove
        

        foreach (GameObject boardSpace in boardSpaces)
        {
            boardSpace.GetComponent<BoxCollider2D>().enabled = false;
        }

        pieceToRemove = 0;

        //Debug.Log(piecesPositions[pieceToRemove].name);
        Destroy(piecesPositions[pieceToRemove]);


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
                changeTurnIndicator(true);
            }
            else
            {
                changeTurnIndicator(false);
            }

        }
        else if (!isLocalPlayerTurn)
        {
            isLocalPlayerTurn = true;

            if (isSinglePlayer)
            {
                changeTurnIndicator(false);

            }
            else
            {
                if (Player.firstPlayer)
                {
                    changeTurnIndicator(true);
                }
                else
                {
                    changeTurnIndicator(false);
                }

            }
        }
        Debug.Log("changed player");
    }

    private void Update()
    {
        //Debug.Log("update has been called");
        if (gameOver)
        {
            if (remainingOpponent < 3)
            {
                Debug.Log("you won");
            }
            else if (remainingLocal < 3)
            {
                Debug.Log("you lost");
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

            foreach (GameObject gameObj in localPieces)
            {
                if (gameObj != null && gameObj.GetComponent<GamePiece>().location == NetworkGameManager.removeIndex)
                {
                    gameObj.GetComponent<GamePiece>().location = 0;
                    Destroy(gameObj);
                    Game.gameInstance.removeLocalPiece(NetworkGameManager.removeIndex);
                    Debug.Log("removed.");
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
        }
    }


    IEnumerator waitForChangePlayer()
    {
        yield return new WaitUntil(() => isLocalPlayerTurn);
    }

    #endregion

}


