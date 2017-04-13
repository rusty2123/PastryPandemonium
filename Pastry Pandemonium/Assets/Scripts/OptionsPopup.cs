using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPopup : MonoBehaviour {

	public GameObject current, musicSlider;
	public Slider slider;
    public Music music;
    public Music2 music2;



	private GameObject menu, singlePlayer, multiplayer, tutorial, options, help, exit;

	private GameObject switchRight, switchLeft, offSwitch, onSwitch, effectsSwitch, mute, unmute;

	private void Awake()
	{
        

           menu = GameObject.Find ("OptionsPopup");
		multiplayer = GameObject.FindGameObjectWithTag ("multiplayer");
		tutorial = GameObject.FindGameObjectWithTag ("tutorial");
		options = GameObject.FindGameObjectWithTag ("options");
		singlePlayer = GameObject.FindGameObjectWithTag ("singleplayer");
		help = GameObject.FindGameObjectWithTag ("help");
		exit = GameObject.FindGameObjectWithTag ("exit");
        mute = GameObject.Find("mute");
        unmute = GameObject.Find("unmute");

		switchRight = GameObject.Find ("switchRight");
		switchLeft = GameObject.Find ("switchLeft");
		effectsSwitch = GameObject.Find ("switch");
		offSwitch = GameObject.Find ("offSwitch");
		onSwitch = GameObject.Find ("onSwitch");

        music = Music.getInstance();
        music2 = Music2.getInstance();
       

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

    public void Update()
    {
        if(slider.value == 0)
        {
            mute.GetComponent<Renderer>().enabled = true;
            unmute.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            unmute.GetComponent<Renderer>().enabled = true;
            mute.GetComponent<Renderer>().enabled = false;
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
                   
                    if (music2.vol.value == 0f)
                    {
                        music2.vol.value = music2.getLastVolume();
                       // mute.GetComponent<Renderer>().enabled = false;
                       // unmute.GetComponent<Renderer>().enabled = true;
                    }
                    else
                    {
                        music2.setLastVolume(music2.vol.value);
                        music2.vol.value = 0f;
                      //  mute.GetComponent<Renderer>().enabled = true;
                      //  unmute.GetComponent<Renderer>().enabled = false;
                    }
                    slider.value = music2.vol.value;
                    break;
			case "onSwitch":

				if (Music.playSoundEffects) {
					offSwitch.GetComponent<Renderer> ().enabled = true;

					onSwitch.GetComponent<Renderer> ().enabled = false;

					effectsSwitch.transform.position = switchLeft.transform.position;
					Music.playSoundEffects = false;
				} else {
					Music.playSoundEffects = true;
					offSwitch.GetComponent<Renderer> ().enabled = false;

					onSwitch.GetComponent<Renderer> ().enabled = true;

					effectsSwitch.transform.position = switchRight.transform.position;
				}
				
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
