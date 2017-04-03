using System.Collections;
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

            if (!App.isSinglePlayer)
            {
                networkManager.LeaveRoom();
                networkManager.Disconnect();
            }
            SceneManager.LoadScene("mainMenu");
        }
        if (current.name == "mute")
        {
            // turn off the music here
            Music musicInstance = Music.getInstance();
            if (musicInstance.vol.value == 0f)
            {
                musicInstance.vol.value = musicInstance.getLastVolume();
            }
            else
            {
                musicInstance.setLastVolume(musicInstance.vol.value);
                musicInstance.vol.value = 0f;
            }
        }
        if (current.name == "helpfulHints")
        {
            App.showHelfulHints = !App.showHelfulHints;
        }

	}
}
