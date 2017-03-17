using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour {

    public GameObject current;
	public GameObject canvas, volSlider;
	private GameObject multiplayer, tutorial, options, singleplayer, help, exit;
	private Vector3 popupSize;
	public AudioClip menuClick;
	private AudioSource audioSource;


	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		multiplayer = GameObject.FindGameObjectWithTag("multiplayer");
		tutorial = GameObject.FindGameObjectWithTag("tutorial");
		options = GameObject.FindGameObjectWithTag("options");
		singleplayer = GameObject.FindGameObjectWithTag("singleplayer");
		help = GameObject.FindGameObjectWithTag("help");
		exit = GameObject.FindGameObjectWithTag("exit");
		volSlider.SetActive (false);


    }
    public void OnMouseEnter()
    {
		//Scales menu options to indicate that you can click on them
        LeanTween.scale(current, new Vector3(.45f, .45f, .45f), .075f);
    }
	public void OnMouseExit()
	{
		//Sets menu options back to their original size
		LeanTween.scale(current, new Vector3(0.3615471f, 0.3615471f, 0.3615471f), .05f);
	}

    public void OnMouseUp()
    {


		playSound ();
		StartCoroutine(setPopup());


    }

	private void playSound(){

		audioSource.PlayOneShot (menuClick, .5f);

	}

	IEnumerator setPopup()
	{
		yield return new WaitForSeconds (.3f);

		LeanTween.scale(current, new Vector3(0.3615471f, 0.3615471f, 0.3615471f), .05f);
		canvas.SetActive(true);

		if (canvas.name == "OptionsPopup") {
			volSlider.SetActive (true);
		}


		singleplayer.SetActive(false);
		multiplayer.SetActive(false);
		tutorial.SetActive (false);
		options.SetActive (false);
		help.SetActive (false);
		exit.SetActive (false);
	}


}
