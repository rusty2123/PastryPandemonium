using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPopup : MonoBehaviour {

	public GameObject current, musicSlider;

	private GameObject menu, singlePlayer, multiplayer, tutorial, options, help, exit;


	private void Awake()
	{
		menu = GameObject.Find ("OptionsPopup");
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
		LeanTween.scale(current, new Vector3(0.43f, .43f, .43f), .075f);

	}


	public void OnMouseExit()
	{
		//Sets objects back to their original size
		LeanTween.scale(current, new Vector3(0.37f, 0.37f, 0.37f), .05f);

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
				musicSlider.SetActive (false);

				break;
			default:
				break;          
			}
		}
	}
}
