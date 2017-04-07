using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameboardButtons : MonoBehaviour {

    public GameObject current;
    public NetworkGameManager networkManager;
	private GameObject musicSwitchRight, musicSwitchLeft, switchMusic, onSwitchMusic, offSwitchMusic;
	private GameObject hintsSwitchRight , hintsSwitchLeft, switchHints, onSwitchHints, offSwitchHints;
	private bool hints = true;


	private void Awake()
	{

		musicSwitchRight = GameObject.Find ("musicSwitchRight");
		musicSwitchLeft = GameObject.Find ("musicSwitchLeft");
		switchMusic = GameObject.Find ("switchMusic");
		onSwitchMusic = GameObject.Find ("onSwitchMusic");
		offSwitchMusic = GameObject.Find ("offSwitchMusic");


		hintsSwitchRight = GameObject.Find ("hintsSwitchRight");
		hintsSwitchLeft = GameObject.Find ("hintsSwitchLeft");
		switchHints = GameObject.Find ("switchHints");
		onSwitchHints = GameObject.Find ("onSwitchHints");
		offSwitchHints = GameObject.Find ("offSwitchHints");


		onSwitchHints.GetComponent<Renderer> ().enabled = true;
		offSwitchHints.GetComponent<Renderer> ().enabled = false;
		switchHints.transform.position = hintsSwitchRight.transform.position;


		Music musicInstance = Music.getInstance ();

		if (musicInstance.vol.value == 0f) {
			onSwitchMusic.GetComponent<Renderer> ().enabled = false;
			offSwitchMusic.GetComponent<Renderer> ().enabled = true;
			switchMusic.transform.position = musicSwitchLeft.transform.position;
		} else {

			onSwitchMusic.GetComponent<Renderer> ().enabled = true;
			offSwitchMusic.GetComponent<Renderer> ().enabled = false;
			switchMusic.transform.position = musicSwitchRight.transform.position;

		}


	}

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
		case "musicMute":
				// turn off the music here
			Music musicInstance = Music.getInstance ();
			if (musicInstance.vol.value == 0f) {
				musicInstance.vol.value = musicInstance.getLastVolume ();
				onSwitchMusic.GetComponent<Renderer> ().enabled = true;
				offSwitchMusic.GetComponent<Renderer> ().enabled = false;
				switchMusic.transform.position = musicSwitchRight.transform.position;
			} else {
				musicInstance.setLastVolume (musicInstance.vol.value);
				musicInstance.vol.value = 0f;
				onSwitchMusic.GetComponent<Renderer> ().enabled = false;
				offSwitchMusic.GetComponent<Renderer> ().enabled = true;
				switchMusic.transform.position = musicSwitchLeft.transform.position;


			}
			break;
		case "hints":
			if (hints) {
				App.showHelfulHints = !App.showHelfulHints;
				onSwitchHints.GetComponent<Renderer> ().enabled = false;
				offSwitchHints.GetComponent<Renderer> ().enabled = true;
				switchHints.transform.position = hintsSwitchLeft.transform.position;
				hints = false;
			} else {
				App.showHelfulHints = true;
				onSwitchHints.GetComponent<Renderer> ().enabled = true;
				offSwitchHints.GetComponent<Renderer> ().enabled = false;
				switchHints.transform.position = hintsSwitchRight.transform.position;
				hints = true;
			}
			break;
        case "requestDraw":
                if (Game.gameInstance.isDraw())
                {
                    // confirm draw and display draw message
                        networkManager.offerDraw();
                        //wait for response
                        startCoroutine();
                }
                else
                {
                    // deny draw 
                }
            break;
		default:
			break;


		}

		 

	}

    private IEnumerator startCoroutine()
    {
        yield return StartCoroutine(getDrawResponse());
    }

    private IEnumerator getDrawResponse()
    {
        NetworkGameManager.drawResponseRecieved = false;

        yield return new WaitUntil(() => NetworkGameManager.drawResponseRecieved);

        NetworkGameManager.drawResponseRecieved = false;

        if (NetworkGameManager.drawAccepted)
        {
            //draw
        }
        else if(!NetworkGameManager.drawAccepted)
        {
            //continue game
        }
    }
}
