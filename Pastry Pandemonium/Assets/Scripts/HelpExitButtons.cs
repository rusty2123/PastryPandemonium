using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpExitButtons : MonoBehaviour {

	public GameObject current;
	public GameObject canvas;

	private GameObject multiplayer, tutorial, options, singleplayer, help, exit;

	private void Awake()
	{
		multiplayer = GameObject.FindGameObjectWithTag("multiplayer");
		tutorial = GameObject.FindGameObjectWithTag("tutorial");
		options = GameObject.FindGameObjectWithTag("options");
		singleplayer = GameObject.FindGameObjectWithTag("singleplayer");
		help = GameObject.FindGameObjectWithTag("help");
		exit = GameObject.FindGameObjectWithTag("exit");


	}
	public void OnMouseEnter()
	{
		if (current.name != "background") {
			//Scales menu options to indicate that you can click on them
			LeanTween.scale (current, new Vector3 (.37f, .37f, .37f), .075f);
		}
	}
	public void OnMouseExit()
	{
		if (current.name != "background") {
			//Sets menu options back to their original size
			LeanTween.scale (current, new Vector3 (0.2958171f, 0.2958171f, 0.2958171f), .05f);
		}
	}

	public void OnMouseUp()
	{
		if (current.name != "background") {
			LeanTween.scale (current, new Vector3 (0.2958171f, 0.2958171f, 0.2958171f), .05f);
			if (current.name == "exitButton") {

				canvas.SetActive (true);


				singleplayer.SetActive(false);
				multiplayer.SetActive(false);
				tutorial.SetActive (false);
				options.SetActive (false);
				help.SetActive (false);
				exit.SetActive (false);


			} else {
				canvas.SetActive (true);
			}
		} else {
			canvas.SetActive (false);

		}
	}
}
