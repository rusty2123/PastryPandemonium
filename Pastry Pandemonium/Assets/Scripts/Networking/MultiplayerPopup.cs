﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerPopup : MonoBehaviour {

    public Lobby lobby;
	public GameObject current;
	private GameObject menu, singlePlayer, multiplayer, tutorial, options, exit, help;

    public void OnMouseEnter()
	{
		//Scales objects to indicate you can click on them
		LeanTween.scale(current, new Vector3(0.4f, .4f, .4f), .075f);
	}


	public void OnMouseExit()
	{
		//Sets objects back to their original size
		LeanTween.scale(current, new Vector3(0.3522516f, 0.3522516f, 0.3522516f), .05f);
	}

	private void Awake()
	{
		menu = GameObject.Find("MultiplayerPopup");
		multiplayer = GameObject.FindGameObjectWithTag("multiplayer");
		singlePlayer = GameObject.FindGameObjectWithTag("singleplayer");
		tutorial = GameObject.FindGameObjectWithTag("tutorial");
		options = GameObject.FindGameObjectWithTag("options");
		help = GameObject.FindGameObjectWithTag("help");
		exit = GameObject.FindGameObjectWithTag("exit");
        
    }



	public void OnMouseUp()
	{
		//Finds what option you clicked on
		if (current != null) {
			switch (current.name) 
			{
                case "enterLobby":
                    if (lobby.Connect())
                    {
                        Player.isSinglePlayer = false;
                        SceneManager.LoadScene("Lobby");
                    }
                    //else let user know they couldn't connect
                    break;
                case "menuButton":
				menu.SetActive (false);
				multiplayer.SetActive (true);
				tutorial.SetActive (true);
				options.SetActive (true);
				singlePlayer.SetActive (true);
				help.SetActive (true);
				exit.SetActive (true);
				break;
			default:
				break;          
			}
		}
	}
}
