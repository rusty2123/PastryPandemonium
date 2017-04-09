using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour {

    private static Music instance;
	public static bool playSoundEffects = true;
	static bool AudioBegin = false; 
	public Slider vol; 
	private AudioSource audioSource;
    private float lastVolume;

    public float getLastVolume()
    {
        return lastVolume;
    }

    public void setLastVolume(float volume)
    {
        lastVolume = volume;
    }

    public static Music getInstance()
    {
        return instance;
    }

	void Awake()
	{
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                setMusic();
                instance = this;
            }
        }
	}

	private void setMusic()
	{
        DontDestroyOnLoad(audioSource);
        DontDestroyOnLoad(vol);

        audioSource = GetComponent<AudioSource> ();
		audioSource.Play ();

	}

	public void changeVolume()
	{
        lastVolume = audioSource.volume;
        audioSource.volume = vol.value;
	}
		
}
