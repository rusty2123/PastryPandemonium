﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameboardButtons : MonoBehaviour {

    public GameObject current;
    public NetworkGameManager networkManager;

	public void OnMouseUp()
	{
		if  (current.name == "exitButton") {
			Application.Quit ();
			Debug.Log ("quit application");
		} 

		if (current.name == "menuButton") {

            if(!App.isSinglePlayer)
            {
                networkManager.Disconnect();
            }

			SceneManager.LoadScene ("mainMenu");
		}

	}
}
