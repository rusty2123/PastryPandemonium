using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPopup : MonoBehaviour {

	public GameObject current;


	private GameObject menu, singlePlayer, multiplayer, tutorial, options, help, exit;

	private void Awake()
	{
		menu = GameObject.Find ("ExitPopup");
		multiplayer = GameObject.FindGameObjectWithTag ("multiplayer");
		tutorial = GameObject.FindGameObjectWithTag ("tutorial");
		options = GameObject.FindGameObjectWithTag ("options");
		singlePlayer = GameObject.FindGameObjectWithTag ("singleplayer");
		help = GameObject.FindGameObjectWithTag ("help");
		exit = GameObject.FindGameObjectWithTag ("exit");


	}

	public void OnMouseEnter()
	{
		//Scales objects to indicate you can click on them
		LeanTween.scale(current, new Vector3(0.63f, .63f, .63f), .075f);

	}


	public void OnMouseExit()
	{
		//Sets objects back to their original size
		LeanTween.scale(current, new Vector3(0.5090503f, 0.5090503f, 0.5090503f), .05f);

	}



	public void OnMouseUp()
	{

		//Finds what option you clicked on
		if (current != null) {
			switch (current.name) 
			{
			case "noButton":
				menu.SetActive (false);
				multiplayer.SetActive (true);
				tutorial.SetActive (true);
				options.SetActive (true);
				singlePlayer.SetActive (true);
				help.SetActive (true);
				exit.SetActive (true);
				LeanTween.scale(current, new Vector3(0.5090503f, 0.5090503f, 0.5090503f), .05f);
				break;
			case "yesButton":
				Application.Quit ();
				Debug.Log ("quit application");
				break;
			default:
				break;          
			}
		}
	}
}
