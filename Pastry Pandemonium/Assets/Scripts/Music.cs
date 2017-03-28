using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour {

	public static bool playSoundEffects = true;
	static bool AudioBegin = false; 
	public Slider vol; 
	private AudioSource audioSource;
	//public AudioClip audio;

	void Awake()
	{
		
		if (!AudioBegin) {
			
			setMusic ();

			AudioBegin = true;
		} else 
		{
			if (SceneManager.GetActiveScene().name == "MainMenu") 
			{
				Destroy (GameObject.Find("music"));
				setMusic ();
			}
		}

	}

	private void setMusic()
	{
		audioSource = GetComponent<AudioSource> ();

		audioSource.Play ();
		DontDestroyOnLoad (audioSource);
		DontDestroyOnLoad (vol);

	}

	public void changeVolume()
	{
		audioSource.volume = vol.value;
	}
		
}
