using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPopup : MonoBehaviour {

	public GameObject current, musicSlider;
	public Slider slider;
    public Music music;



	private GameObject menu, singlePlayer, multiplayer, tutorial, options, help, exit;

	private GameObject switchRight, switchLeft, offSwitch, onSwitch, effectsSwitch;

	private void Awake()
	{
		menu = GameObject.Find ("OptionsPopup");
		multiplayer = GameObject.FindGameObjectWithTag ("multiplayer");
		tutorial = GameObject.FindGameObjectWithTag ("tutorial");
		options = GameObject.FindGameObjectWithTag ("options");
		singlePlayer = GameObject.FindGameObjectWithTag ("singleplayer");
		help = GameObject.FindGameObjectWithTag ("help");
		exit = GameObject.FindGameObjectWithTag ("exit");

		switchRight = GameObject.Find ("switchRight");
		switchLeft = GameObject.Find ("switchLeft");
		effectsSwitch = GameObject.Find ("switch");
		offSwitch = GameObject.Find ("offSwitch");
		onSwitch = GameObject.Find ("onSwitch");



		if (Music.playSoundEffects) {
			offSwitch.GetComponent<Renderer> ().enabled = false;

			onSwitch.GetComponent<Renderer> ().enabled = true;

			effectsSwitch.transform.position = switchRight.transform.position;
		} else {

			offSwitch.GetComponent<Renderer> ().enabled = true;

			onSwitch.GetComponent<Renderer> ().enabled = false;

			effectsSwitch.transform.position = switchLeft.transform.position;

		}


	}


	public void OnMouseEnter()
	{
		//Scales objects to indicate you can click on them
		if (current.name != "onSwitch" && current.name != "offSwitch") {
			LeanTween.scale (current, new Vector3 (0.43f, .43f, .43f), .075f);
		}

	}


	public void OnMouseExit()
	{
		//Sets objects back to their original size
		if (current.name != "onSwitch" && current.name != "offSwitch") {
			LeanTween.scale (current, new Vector3 (0.37f, 0.37f, 0.37f), .05f);
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
				musicSlider.SetActive (false);

				break;
			case "mute":
                    music = Music.getInstance();
                    if (music.vol.value == 0f)
                    {
                        music.vol.value = music.getLastVolume();
                    }
                    else
                    {
                        music.setLastVolume(music.vol.value);
                        music.vol.value = 0f;
                    }
                    slider.value = music.vol.value;
                    break;
			case "onSwitch":
				offSwitch.GetComponent<Renderer> ().enabled = true;

				onSwitch.GetComponent<Renderer> ().enabled = false;

				effectsSwitch.transform.position = switchLeft.transform.position;
				
				break;
			case "offSwitch":
				
				offSwitch.GetComponent<Renderer> ().enabled = false;

				onSwitch.GetComponent<Renderer> ().enabled = true;

				effectsSwitch.transform.position = switchRight.transform.position;
				break;
			default:
				break;          
			}
		}
	}
}
