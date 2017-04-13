using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	public GameObject current;
	private GameObject menu, singlePlayer, multiplayer, tutorial, 
					   options, help, exit, info, mills, placement, 
	    		 	   moving, flying, backArrow, forwardArrow,
						phaseOne, phaseTwo, phaseThree;


	private GameObject startPosition, endPosition, piece, shadow, piece2, piece3, shadow3;

    public static int currentPage;

    public AudioClip changePage;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        menu = GameObject.Find("TutorialPopup");
        multiplayer = GameObject.FindGameObjectWithTag("multiplayer");
        tutorial = GameObject.FindGameObjectWithTag("tutorial");
        options = GameObject.FindGameObjectWithTag("options");
        singlePlayer = GameObject.FindGameObjectWithTag("singleplayer");
        help = GameObject.FindGameObjectWithTag("help");
        exit = GameObject.FindGameObjectWithTag("exit");
        piece = GameObject.Find("redGamePiece");
        shadow = GameObject.Find("shadow");
        piece2 = GameObject.Find("redGamePiece2");
        shadow3 = GameObject.Find("shadow3");
        piece3 = GameObject.Find("redGamePiece3");
        currentPage = 1;

        info = GameObject.Find("generalInfo");
        mills = GameObject.Find("mills");
        placement = GameObject.Find("placementPhase");
        moving = GameObject.Find("movingPhase");
        flying = GameObject.Find("flyingPhase");
        backArrow = GameObject.Find("backArrow");
        forwardArrow = GameObject.Find("forwardArrow");

        phaseOne = GameObject.Find("phaseOne");
        phaseTwo = GameObject.Find("phaseTwo");
        phaseThree = GameObject.Find("phaseThree");

        defaultSetup();
    }

    private void defaultSetup()
    {
		forwardArrow.GetComponent<Renderer> ().enabled = true;
        info.GetComponent<Renderer>().enabled = true;
        backArrow.GetComponent<Renderer>().enabled = false;
        mills.GetComponent<Renderer>().enabled = false;
        placement.GetComponent<Renderer>().enabled = false;
        moving.GetComponent<Renderer>().enabled = false;
        flying.GetComponent<Renderer>().enabled = false;
        phaseOne.GetComponent<Renderer>().enabled = false;
        phaseTwo.GetComponent<Renderer>().enabled = false;
        phaseThree.GetComponent<Renderer>().enabled = false;
        piece.GetComponent<Renderer>().enabled = false;
        shadow.GetComponent<Renderer>().enabled = false;
        piece2.GetComponent<Renderer>().enabled = false;
        piece3.GetComponent<Renderer>().enabled = false;
        shadow3.GetComponent<Renderer>().enabled = false;
    }
	public void OnMouseEnter()
	{
		//Scales objects to indicate you can click on them
		if (current.name == "menuButton") {
			LeanTween.scale (current, new Vector3 (0.43f, .43f, .43f), .075f);
		} else {
			LeanTween.scale (current, new Vector3 (0.3f, .3f, .3f), .075f);
		}

	}


	public void OnMouseExit()
	{
		//Sets objects back to their original size
		if (current.name == "menuButton") 
		{
			LeanTween.scale (current, new Vector3 (0.37f, 0.37f, 0.37f), .05f);
		}
		else {
			LeanTween.scale (current, new Vector3 (0.2023873f, .2023873f, .2023873f), .075f);
		}

	}

    private void Update()
    {
        if (currentPage != 3)
        {
             CancelInvoke("animatePhaseOne");
        }

        if (currentPage != 4)
        {
            CancelInvoke("animatePhaseTwo");
        }

        if (currentPage != 5)
        {
            CancelInvoke("animatePhaseThree");
        }
    }

    private void animatePhaseOne()
	{
		startPosition = GameObject.Find ("phase1Start");
		endPosition = GameObject.Find ("phase1End");

		piece.transform.position = startPosition.transform.position;
		shadow.transform.position = startPosition.transform.position;

		//scale piece
		LeanTween.scale (piece, new Vector3(.12f, .12f, .12f), .6f).setDelay(.5f);
		LeanTween.scale (piece, new Vector3(0.09006101f, 0.09006101f, 0.09006101f), 1.3f).setDelay(2.9f);

		//move piece up and then to the endPosition
		LeanTween.moveY (piece, startPosition.transform.position.y + 22f, .6f).setDelay(.5f);
		LeanTween.move (piece, endPosition.transform.position, 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(1.1f);

		//Move shadow
		LeanTween.move (shadow, endPosition.transform.position, 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(1.15f);    

	}

	private void animatePhaseTwo()
	{
		startPosition = GameObject.Find ("phase2Start");
		endPosition = GameObject.Find ("phase2End");

		piece2.transform.position = startPosition.transform.position;
        LeanTween.move(piece2, endPosition.transform.position, 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(1.1f);
    }

    private void animatePhaseThree()
    {
        startPosition = GameObject.Find("phase3Start");
        endPosition = GameObject.Find("phase3End");

        piece3.transform.position = startPosition.transform.position;
        shadow3.transform.position = startPosition.transform.position;

        //scale piece
        LeanTween.scale(piece3, new Vector3(.12f, .12f, .12f), .6f).setDelay(.5f);
        LeanTween.scale(piece3, new Vector3(0.09006101f, 0.09006101f, 0.09006101f), 1.3f).setDelay(2.9f);

        //move piece up and then to the endPosition
        LeanTween.moveY(piece3, startPosition.transform.position.y + 22f, .6f).setDelay(.5f);
        LeanTween.move(piece3, endPosition.transform.position, 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(1.1f);

        //Move shadow
        LeanTween.move(shadow3, endPosition.transform.position, 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(1.15f);

    }


    IEnumerator setTutorial(int currentPage)
	{
        yield return new WaitForSeconds(.5f);
		switch (currentPage) {

		case 1:
			backArrow.GetComponent<Renderer> ().enabled = false;
			info.GetComponent<Renderer> ().enabled = true;
			mills.GetComponent<Renderer> ().enabled = false;
			break;
		case 2:
			backArrow.GetComponent<Renderer> ().enabled = true;
            mills.GetComponent<Renderer>().enabled = true;
            info.GetComponent<Renderer> ().enabled = false;
			placement.GetComponent<Renderer> ().enabled = false;
			phaseOne.GetComponent<Renderer> ().enabled = false;
			piece.GetComponent<Renderer> ().enabled = false;
			shadow.GetComponent<Renderer> ().enabled = false;
			
			break;
		case 3:
			placement.GetComponent<Renderer> ().enabled = true;
            piece.GetComponent<Renderer>().enabled = true;
            shadow.GetComponent<Renderer>().enabled = true;
            mills.GetComponent<Renderer> ().enabled = false;
			moving.GetComponent<Renderer> ().enabled = false;
			phaseTwo.GetComponent<Renderer> ().enabled = false;
			phaseOne.GetComponent<Renderer> ().enabled = true;
            piece2.GetComponent<Renderer>().enabled = false;
            InvokeRepeating ("animatePhaseOne", 0.0001f, 5);
			break;
		case 4:
			placement.GetComponent<Renderer> ().enabled = false;
			phaseOne.GetComponent<Renderer> ().enabled = false;
			moving.GetComponent<Renderer> ().enabled = true;
			flying.GetComponent<Renderer> ().enabled = false;
			forwardArrow.GetComponent<Renderer> ().enabled = true;
			phaseThree.GetComponent<Renderer> ().enabled = false;
			phaseTwo.GetComponent<Renderer> ().enabled = true;
            piece3.GetComponent<Renderer>().enabled = false;
            shadow3.GetComponent<Renderer>().enabled = false;
            piece.GetComponent<Renderer>().enabled = false;
            shadow.GetComponent<Renderer>().enabled = false;
            piece2.GetComponent<Renderer>().enabled = true;
            InvokeRepeating ("animatePhaseTwo", 0.0001f, 5);
            break;
		case 5:
			moving.GetComponent<Renderer> ().enabled = false;
			phaseTwo.GetComponent<Renderer> ().enabled = false;
			flying.GetComponent<Renderer> ().enabled = true;
			phaseThree.GetComponent<Renderer> ().enabled = true;
			forwardArrow.GetComponent<Renderer> ().enabled = false;
            piece2.GetComponent<Renderer>().enabled = false;
            piece3.GetComponent<Renderer>().enabled = true;
            shadow3.GetComponent<Renderer>().enabled = true;
            InvokeRepeating("animatePhaseThree", 0.0001f, 5);
            break;
		default:
			break;

		}

	}

	public void OnMouseUp()
	{

		//Finds what option you clicked on
		if (current != null) {
			switch (current.name) 
			{
			case "menuButton":
                CancelInvoke();
                menu.SetActive (false);
				multiplayer.SetActive (true);
				tutorial.SetActive (true);
				options.SetActive (true);
				singlePlayer.SetActive (true);
				help.SetActive (true);
				exit.SetActive (true);
				currentPage = 1;
                defaultSetup();
                LeanTween.scale(current, new Vector3(0.37f, 0.37f, 0.37f), .05f);
                break;
			case "forwardArrow":
				if (currentPage < 5) {
					++currentPage;
				}
                    playChangePageSound();
                    StartCoroutine(disableArrows());
                    StartCoroutine(setTutorial(currentPage));
				break;
			case "backArrow":
				if (currentPage > 1) {
					--currentPage;
				}
                    playChangePageSound();
                    StartCoroutine(setTutorial(currentPage));
                    StartCoroutine(disableArrows());
                    break;
			default:
				break;          
			}
		}
	}


    private void playChangePageSound()
    {
        if (Music.playSoundEffects)
        {
            audioSource.PlayOneShot(changePage, .25f);
        }

    }

    IEnumerator disableArrows()
    {
        forwardArrow.GetComponent<Collider2D>().enabled = false;
        backArrow.GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(1);

        forwardArrow.GetComponent<Collider2D>().enabled = true;
        backArrow.GetComponent<Collider2D>().enabled = true;


    }
}
