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
    public GameObject[] outOfBoardSpaces = new GameObject[18];
    public GameObject[] boardSpaces = new GameObject[24];

    public static bool isSinglePlayer, gameOver = false, isLocalPlayerTurn, localPlayerWon, removePiece = false;
    public static Player localPlayer, opponentPlayer;
    public static int phase = 1;

    private GameObject clickedFirst = null;
    private GameObject clickedSecond = null;

    //goal is to use selectedGamePiece to indicate which piece is clicked when deciding when to remove or move a piece
    public static int from, to, pieceToRemove;

    #endregion

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
	
		setUpTurnIndicator ();

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
        startGame();
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

		cupcakeTurnOn.transform.position = new Vector3(cupcakeTurnOff.transform.position.x+2,
			cupcakeTurnOff.transform.position.y+2, 
			cupcakeTurnOff.transform.position.z - 3f);

		muffinTurnOn.transform.position = new Vector3(muffinTurnOff.transform.position.x-10,
			muffinTurnOff.transform.position.y+5, 
			muffinTurnOff.transform.position.z - 3f);
	}

	private void changeTurnIndicator(bool firstPlayerTurn)
	{

		if (firstPlayerTurn) 
		{
			if (firstPlayer == "muffin") 
			{
				muffinTurnOn.GetComponent<Renderer> ().enabled = true;
				cupcakeTurnOn.GetComponent<Renderer> ().enabled = false;
			} 
			else 
			{
				muffinTurnOn.GetComponent<Renderer> ().enabled = false;
				cupcakeTurnOn.GetComponent<Renderer> ().enabled = true;
			}
		} 
		else 
		{

			if (firstPlayer == "muffin") {
				muffinTurnOn.GetComponent<Renderer> ().enabled = false;
				cupcakeTurnOn.GetComponent<Renderer> ().enabled = true;
			} else {
				muffinTurnOn.GetComponent<Renderer> ().enabled = true;
				cupcakeTurnOn.GetComponent<Renderer> ().enabled = false;
			}

		}

	}

    private void startGame()
    {
        if(isLocalPlayerTurn)
        {

        }
        else
        {
            piecePlacementPhase(-1);
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
            isLocalPlayerTurn = true;
            characterLocalPlayer = redCupcake;
            setUpPiecesLocal(characterLocalPlayer);

            characterOpponentPlayer = berryMuffin;
            setUpPiecesOpponent(characterOpponentPlayer);
        }
        else
        {
            //for now client will always go first, and will play as berry muffins
            isLocalPlayerTurn = false;
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

    private void animationPhaseOne(GameObject gamePiece, GameObject startPosition, GameObject endPosition)
    {
        shadow.SetActive(true);
        gamePiece.transform.position = startPosition.transform.position;
        
		shadow.transform.position = new Vector3(startPosition.transform.position.x, 
			startPosition.transform.position.y,
			startPosition.transform.position.z+2);

        //scale piece
        LeanTween.scale(gamePiece, new Vector3(.65f, .65f, .65f), .6f).setDelay(.2f);
        LeanTween.scale(gamePiece, new Vector3(0.5f, 0.5f, 0.5f), 1.3f).setDelay(2.7f);

        //move piece up and then to the endPosition
        LeanTween.moveY(gamePiece, startPosition.transform.position.y + 140f, .6f).setDelay(.2f);
        LeanTween.move(gamePiece, endPosition.transform.position, 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(.9f);

        //Move shadow
		LeanTween.move(shadow, new Vector3 (endPosition.transform.position.x,
			endPosition.transform.position.y,
			endPosition.transform.position.z+2), 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(.93f);

        gamePiece.GetComponent<GamePiece>().location = Convert.ToInt32(endPosition.name);

    }

    IEnumerator executeAIMovePhaseOne()
    {
        yield return new WaitForSeconds(4);
        
                int[] move = opponentPlayer.getAIMove();

                to = move[1];

        if (game.validPlace(to))
        {
            Debug.Log("ai not valid move");
        }

        Debug.Log("valid move");
        startPosition = opponentPieces[opponentIndex];
        endPosition = GameObject.Find(to.ToString());
        animationPhaseOne(startPosition, startPosition, endPosition);
        opponentIndex++;
        outOfBoardOpponent--;
        game.placePiece(to);
       
        print("delay");
        changePlayer();
    }

    public void piecePlacementPhase(int selected)
    {
        //need to verify the moves and update the game board
        if (isLocalPlayerTurn && outOfBoardLocal > 0)
        {
            //wait until piece clicked
            //check to make sure the player is allowed to move there
            if (game.validPlace(selected))
            {
                //Debug.Log("valid move");

                game.placePiece(selected);

                //send move over network
                if(!Player.isSinglePlayer)
                {
                    networkManager.placePiece(selected);
                }

                //play move animation
                startPosition = localPieces[localIndex];
                endPosition = GameObject.Find(selected.ToString());
                animationPhaseOne(startPosition, startPosition, endPosition);
                //Debug.Log("location: " + localPieces[localIndex].GetComponent<GamePiece>().location);
                localIndex++;
                outOfBoardLocal--;
                game.placePiece(selected);

                //check if created mill
                if (game.createdMill(selected))
                {
                    //TODO: remove piece
                    Debug.Log("created mill");

                    StartCoroutine(getPieceToRemove());
                }
                //opponent's turn
                changePlayer();
            }
            else
                Debug.Log("please select another");
        }
        //get move from AI or network
        if ((!isLocalPlayerTurn || selected == -1) && outOfBoardOpponent > 0)
        {
            //get move from ai
            if (Player.isSinglePlayer)
            {
                //check to make sure the player is allowed to move there
                StartCoroutine(executeAIMovePhaseOne());
            }
            //get move from network
            else if(!Player.isSinglePlayer)
            {
                //wait until OnEvent is triggered, get the index, run the animation, increment opponentIndex, decrement outOfBoardOpponent, change the player
                NetworkGameManager.placeIndex = 0;
                StartCoroutine(getNetworkPlacement());
                waitForNetworkPlaceIndex();
                Debug.Log("recieved: " + NetworkGameManager.placeIndex);
            }
        }
        if (outOfBoardOpponent == 0 && outOfBoardLocal == 0)
        {
            phase = 2;
            Debug.Log("phase 2");
        }
    }

    IEnumerator getPieceToRemove()
    {
        removePiece = true;

        yield return new WaitUntil(() => !removePiece);

        game.removePiece(pieceToRemove);

        if (!Player.isSinglePlayer)
        {
            networkManager.removePiece(pieceToRemove);
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
    }

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

    #region Network Methods

    private void OnEvent(byte eventCode, object content, int senderid)
    {
        //if eventcode is 0, then it's placePiece
        if (eventCode == 0)
        {
            byte[] selected = (byte[])content;
            NetworkGameManager.placeIndex = selected[0];
        }
        //if eventCode is 1, then it's removePiece
        else if(eventCode == 1)
        {
            byte[] selected = (byte[])content;
            NetworkGameManager.removeIndex = selected[0];
        }
        //if eventCode is 2, then it's movePiece
        else if(eventCode == 2)
        {
            byte[] selected = (byte[])content;
            NetworkGameManager.moveFromIndex = selected[0];
            NetworkGameManager.moveToIndex = selected[1];
        }
        //if eventCode is 3, then it's flyPiece
        else if(eventCode == 3)
        {
            byte[] selected = (byte[])content;
            NetworkGameManager.flyFromIndex = selected[0];
            NetworkGameManager.flyToIndex = selected[1];
        }

    }

    IEnumerator getNetworkPlacement()
    {
        yield return new WaitUntil(() => NetworkGameManager.placeIndex != 0);
        if (remainingOpponent > 0)
        {
            to = NetworkGameManager.placeIndex;
            startPosition = opponentPieces[opponentIndex];
            endPosition = GameObject.Find(to.ToString());
            animationPhaseOne(startPosition, startPosition, endPosition);
            opponentIndex++;
            outOfBoardOpponent--;
            changePlayer();
        }
    }

    private void waitForNetworkPlaceIndex()
    {
        float elapsedTime = 0.0f;

        while (NetworkGameManager.placeIndex == 0)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 10.0f)
            {
                break;
            }
        }
    }

    IEnumerator getNetworkRemove()
    {
        yield return new WaitUntil(() => NetworkGameManager.removeIndex != 0);
    }

    private void waitForNetworkRemoveIndex()
    {
        float elapsedTime = 0.0f;

        while (NetworkGameManager.removeIndex == 0)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 10.0f)
            {
                break;
            }
        }
    }

    IEnumerator getNetworkMove()
    {
        yield return new WaitUntil(() => NetworkGameManager.moveFromIndex != 0 && NetworkGameManager.moveToIndex != 0);
    }

    private void waitForNetworkMove()
    {
        float elapsedTime = 0.0f;

        while (NetworkGameManager.moveFromIndex == 0 && NetworkGameManager.moveToIndex == 0)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 10.0f)
            {
                break;
            }
        }
    }

    IEnumerator getNetworkFly()
    {
        yield return new WaitUntil(() => NetworkGameManager.flyFromIndex != 0 && NetworkGameManager.flyToIndex != 0);
    }

    private void waitForNetworkFly()
    {
        float elapsedTime = 0.0f;

        while (NetworkGameManager.flyFromIndex == 0 && NetworkGameManager.flyToIndex == 0)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 10.0f)
            {
                break;
            }
        }
    }

    #endregion

}




