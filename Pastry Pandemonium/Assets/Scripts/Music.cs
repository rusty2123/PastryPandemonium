using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour {

	static bool AudioBegin = false; 
	public Slider vol; 
	private AudioSource audioSource;
	//public AudioClip audio;

	void Awake()
	{
		audioSource = GetComponent<AudioSource> ();

		if (!AudioBegin) {


			audioSource.Play ();
			DontDestroyOnLoad (audioSource);

			AudioBegin = true;
		}
	}


	public void changeVolume()
	{
		audioSource.volume = vol.value;
	}
}
