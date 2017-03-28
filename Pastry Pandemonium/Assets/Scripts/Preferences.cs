using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Preferences : MonoBehaviour {


	public Slider musicVolSlider;


	// Use this for initialization
	void Start () {

		int temp = PlayerPrefs.GetInt ("soundEffects");

		if (temp == 0) 
		{
			Music.playSoundEffects = false;
		} else 
		{
			Music.playSoundEffects = true;
		}
		musicVolSlider.value = PlayerPrefs.GetFloat ("musicSlider");
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Music.playSoundEffects) 
		{
			PlayerPrefs.SetInt ("soundEffects", 1);
		} 
		else 
		{
			PlayerPrefs.SetInt ("soundEffects", 0);
		}

		PlayerPrefs.SetFloat ("musicSlider", musicVolSlider.value);

	}
}
