using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Music2 : MonoBehaviour
{

    public static AudioSource audioSource;
    public Slider vol;
    private static Music2 instance;

    public static Music2 getInstance()
    {
        return instance;
    }

    private float lastVolume;

    public float getLastVolume()
    {
        return lastVolume;
    }

    public void setLastVolume(float volume)
    {
        lastVolume = volume;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
      //  -
      //-  GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
      //  if (objs.Length > 1)
      //  {
      //      Destroy(this.gameObject);
      //  }
      //  DontDestroyOnLoad(this.gameObject);
		



        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void VolumeController()
    {
        lastVolume = audioSource.volume;
        audioSource.volume = vol.value; 
    }

}
