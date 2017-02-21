using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpExitButtons : MonoBehaviour {

	public GameObject current;
	public GameObject canvas;


	public void OnMouseEnter()
	{
		if (current.name != "background") {
			//Scales menu options to indicate that you can click on them
			LeanTween.scale (current, new Vector3 (.2f, .2f, .2f), .075f);
		}
	}
	public void OnMouseExit()
	{
		if (current.name != "background") {
			//Sets menu options back to their original size
			LeanTween.scale (current, new Vector3 (0.1188901f, 0.1188901f, 0.1188901f), .05f);
		}
	}

	public void OnMouseUp()
	{
		if (current.name != "background") {
			LeanTween.scale (current, new Vector3 (0.1188901f, 0.1188901f, 0.1188901f), .05f);
			if (current.name == "exitButton") {
				Application.Quit ();
				Debug.Log ("quit application");
			} else {
				canvas.SetActive (true);
			}
		} else {
			canvas.SetActive (false);

		}
	}
}
