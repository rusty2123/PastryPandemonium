using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameboardButtons : MonoBehaviour {

    public GameObject current , canvas;
    public NetworkGameManager networkManager;
	private GameObject musicSwitchRight, musicSwitchLeft, switchMusic, onSwitchMusic, offSwitchMusic;
	private GameObject hintsSwitchRight , hintsSwitchLeft, switchHints, onSwitchHints, offSwitchHints;
	private bool hints = true;
    private App app = new App();
    public Music music;
    public Music2 music2;
    public AudioSource audioSource;

    private void Awake()
	{
        audioSource = GetComponent<AudioSource>();

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


		Music2 musicInstance = Music2.getInstance ();

		if (OptionsPopup.isMute) {
			onSwitchMusic.GetComponent<Renderer> ().enabled = false;
			offSwitchMusic.GetComponent<Renderer> ().enabled = true;
			switchMusic.transform.position = musicSwitchLeft.transform.position;
            //audioSource.volume = 0f;

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

                if (App.gameOver)
                {
                    Debug.Log("you clicked main menu1");
                    if (!App.isSinglePlayer)
                    {
                        networkManager.LeaveRoom();
                        //networkManager.Disconnect();
                    }
                    SceneManager.LoadScene("mainMenu");
                }
                else
                {
                    Debug.Log("you clicked main menu2");
                    canvas.SetActive(true);
                }
			break;
		case "musicMute":
				// turn off the music here
			//Music musicInstance = Music.getInstance ();
			if (OptionsPopup.isMute) {
				//musicInstance.vol.value = musicInstance.getLastVolume ();
				onSwitchMusic.GetComponent<Renderer> ().enabled = true;
				offSwitchMusic.GetComponent<Renderer> ().enabled = false;
				switchMusic.transform.position = musicSwitchRight.transform.position;
                    audioSource.Play();
                    OptionsPopup.isMute = false;
			} else {
				//musicInstance.setLastVolume (musicInstance.vol.value);
				//musicInstance.vol.value = 0f;
				onSwitchMusic.GetComponent<Renderer> ().enabled = false;
				offSwitchMusic.GetComponent<Renderer> ().enabled = true;
				switchMusic.transform.position = musicSwitchLeft.transform.position;
                    audioSource.Stop();
                    OptionsPopup.isMute = true;

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
                if (App.isSinglePlayer)
                {
                   // App.isDraw = true;
                }
                canvas.SetActive(true);
                //app.pauseGame();

                break;
		default:
			break;

		}
	 

	}


}
