using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinglePlayerMenu : MonoBehaviour {

    public GameObject current, character;
    private GameObject moveAi, moveUser, difficultyNormal, difficultyHard, menu, singlePlayer, multiplayer, tutorial, options;
	private GameObject[] players;

    public void OnMouseEnter()
    {
		//Scales objects to indicate you can click on them
        LeanTween.scale(current, new Vector3(0.5f, .5f, .5f), .075f);
		LeanTween.scale(character, new Vector3(0.3f, .3f, .3f), .03f);
	}
    

	public void OnMouseExit()
	{
		//Sets objects back to their original size
		LeanTween.scale(current, new Vector3(0.4268945f, 0.4268945f, 0.4268945f), .05f);
		LeanTween.scale(character, new Vector3(0.222469f, 0.222469f, 0.222469f), .01f);


	}

    private void Awake()
    {
		//Initiating all the game objects needed
        moveUser = GameObject.Find("userSelected");
        moveAi = GameObject.Find("aiSelected");
        difficultyNormal = GameObject.Find("normalSelected");
        difficultyHard = GameObject.Find("hardSelected");
        menu = GameObject.Find("Popup");
		multiplayer = GameObject.FindGameObjectWithTag("multiplayer");
		tutorial = GameObject.FindGameObjectWithTag("tutorial");
		options = GameObject.FindGameObjectWithTag("options");
		singlePlayer = GameObject.FindGameObjectWithTag("singleplayer");

		players = GameObject.FindGameObjectsWithTag ("Player");
		for(int i = 0; i < players.Length; i++)
		{
			if (players [i].name != "redCupcake") {
				LeanTween.alpha (players [i], 0.35f, 0f);
			}
		}
			
		//Set selection defaults (AI, redCupcake, Normal) 
        difficultyNormal.GetComponent<Renderer>().enabled = true;
        difficultyHard.GetComponent<Renderer>().enabled = false;
        moveAi.GetComponent<Renderer>().enabled = false;
        moveUser.GetComponent<Renderer>().enabled = true;


    }

    public void OnMouseUp()
    {
		
		//Finds what option you clicked on
		if (current != null) {
			switch (current.name) 
			{
				case "startButton":
					SceneManager.LoadScene ("GameBoard");
					break;
				case "aiButton":
					moveAi.GetComponent<Renderer> ().enabled = true;
					moveUser.GetComponent<Renderer> ().enabled = false;
					break;
				case "userButton":
					moveAi.GetComponent<Renderer> ().enabled = false;
					moveUser.GetComponent<Renderer> ().enabled = true;
					break;
				case "normalButton":
					difficultyNormal.GetComponent<Renderer> ().enabled = true;
					difficultyHard.GetComponent<Renderer> ().enabled = false;
					break;
				case "hardButton":
					difficultyNormal.GetComponent<Renderer> ().enabled = false;
					difficultyHard.GetComponent<Renderer> ().enabled = true;
					break;
			case "menuButton":
				menu.SetActive (false);
				multiplayer.SetActive (true);
				tutorial.SetActive (true);
				options.SetActive (true);
				singlePlayer.SetActive (true);
					break;
				default:
					break;          
			}
		}


		//Finds what character you clicked on
		if (character != null) 
		{
			//resets all characters to loo unselected
			for(int i = 0; i < players.Length; i++)
			{
				LeanTween.alpha (players[i], 0.35f, 0f);
			}

			//sets the character selected to look selected
			LeanTween.alpha (character, 1f, .5f);
		} 
    }
}
