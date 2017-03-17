using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Preferences : MonoBehaviour {


	public Slider musicVolSlider;


	// Use this for initialization
	void Start () {


		musicVolSlider.value = PlayerPrefs.GetFloat ("musicSlider");
		
	}
	
	// Update is called once per frame
	void Update () {

		PlayerPrefs.SetFloat ("musicSlider", musicVolSlider.value);

	}
}
