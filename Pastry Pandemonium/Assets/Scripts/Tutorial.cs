using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	public GameObject current;
	private GameObject menu, singlePlayer, multiplayer, tutorial, 
					   options, help, exit, info, mills, placement, 
	    		 	   moving, flying, backArrow, forwardArrow,
						phaseOne, phaseTwo, phaseThree;


	private GameObject startPosition, endPosition, piece;

	public static int position;

	private void Awake()
	{
		menu = GameObject.Find("TutorialPopup");
		multiplayer = GameObject.FindGameObjectWithTag("multiplayer");
		tutorial = GameObject.FindGameObjectWithTag("tutorial");
		options = GameObject.FindGameObjectWithTag("options");
		singlePlayer = GameObject.FindGameObjectWithTag("singleplayer");
		help = GameObject.FindGameObjectWithTag("help");
		exit = GameObject.FindGameObjectWithTag("exit");
		piece = GameObject.Find("redGamePiece");

		position = 1;

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

		backArrow.GetComponent<Renderer> ().enabled = false;
		mills.GetComponent<Renderer> ().enabled = false;
		placement.GetComponent<Renderer> ().enabled = false;
		moving.GetComponent<Renderer> ().enabled = false;
		flying.GetComponent<Renderer> ().enabled = false;
		phaseOne.GetComponent<Renderer> ().enabled = false;
		phaseTwo.GetComponent<Renderer> ().enabled = false;
		phaseThree.GetComponent<Renderer> ().enabled = false;
		piece.GetComponent<Renderer> ().enabled = false;

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

	private void animatePhaseOne()
	{
		startPosition = GameObject.Find ("phase1Start");
		endPosition = GameObject.Find ("phase1End");

		piece.transform.position = startPosition.transform.position;

		LeanTween.scale (piece, new Vector3(.1f, .1f, .1f), 1.3f).setEase(LeanTweenType.easeInQuint).setDelay(.5f);
		LeanTween.scale (piece, new Vector3(0.07441013f, 0.07441013f, 0.07441013f), 1.3f).setEase(LeanTweenType.easeOutQuint).setDelay(2.9f);
		LeanTween.move (piece, endPosition.transform.position, 3f).setEase(LeanTweenType.easeInOutQuint).setDelay(.9f);

	}

	private void setTutorial(int position)
	{
		switch (position) {

		case 1:
			backArrow.GetComponent<Renderer> ().enabled = false;
			info.GetComponent<Renderer> ().enabled = true;
			mills.GetComponent<Renderer> ().enabled = false;
			break;
		case 2:
			backArrow.GetComponent<Renderer> ().enabled = true;
			info.GetComponent<Renderer> ().enabled = false;
			mills.GetComponent<Renderer> ().enabled = true;
			placement.GetComponent<Renderer> ().enabled = false;
			phaseOne.GetComponent<Renderer> ().enabled = false;
			piece.GetComponent<Renderer> ().enabled = false;

			break;
		case 3:
			placement.GetComponent<Renderer> ().enabled = true;
			mills.GetComponent<Renderer> ().enabled = false;
			moving.GetComponent<Renderer> ().enabled = false;
			phaseTwo.GetComponent<Renderer> ().enabled = false;
			phaseOne.GetComponent<Renderer> ().enabled = true;
			piece.GetComponent<Renderer> ().enabled = true;
			animatePhaseOne ();
			break;
		case 4:
			placement.GetComponent<Renderer> ().enabled = false;
			phaseOne.GetComponent<Renderer> ().enabled = false;
			moving.GetComponent<Renderer> ().enabled = true;
			flying.GetComponent<Renderer> ().enabled = false;
			forwardArrow.GetComponent<Renderer> ().enabled = true;
			phaseThree.GetComponent<Renderer> ().enabled = false;
			phaseTwo.GetComponent<Renderer> ().enabled = true;
			break;
		case 5:
			moving.GetComponent<Renderer> ().enabled = false;
			phaseTwo.GetComponent<Renderer> ().enabled = false;
			flying.GetComponent<Renderer> ().enabled = true;
			phaseThree.GetComponent<Renderer> ().enabled = true;
			forwardArrow.GetComponent<Renderer> ().enabled = false;
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
				menu.SetActive (false);
				multiplayer.SetActive (true);
				tutorial.SetActive (true);
				options.SetActive (true);
				singlePlayer.SetActive (true);
				help.SetActive (true);
				exit.SetActive (true);
				break;
			case "forwardArrow":
				if (position < 5) {
					++position;
				}
				setTutorial (position);

				break;
			case "backArrow":
				if (position > 1) {
					--position;
				}
				setTutorial (position);
				break;
			default:
				break;          
			}
		}
	}
}
