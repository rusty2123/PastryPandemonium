using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameboardButtons : MonoBehaviour {

    public GameObject current;
    public NetworkGameManager networkManager;

	public void OnMouseUp()
	{
		switch (current.name) 
		{
		case "mainMenuButton":
				if (!App.isSinglePlayer) {
					networkManager.LeaveRoom ();
					networkManager.Disconnect ();
				}
				SceneManager.LoadScene ("mainMenu");
			break;
		case "mute":
				// turn off the music here
			Music musicInstance = Music.getInstance ();
			if (musicInstance.vol.value == 0f) {
				musicInstance.vol.value = musicInstance.getLastVolume ();
			} else {
				musicInstance.setLastVolume (musicInstance.vol.value);
				musicInstance.vol.value = 0f;
			}
			break;
		case "helpfulHints":
			App.showHelfulHints = !App.showHelfulHints;
			break;
		default:
			break;


		}

		 

	}
}
