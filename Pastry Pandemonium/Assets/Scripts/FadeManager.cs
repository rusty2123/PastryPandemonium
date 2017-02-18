using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{

    public static FadeManager Instance { get; set; }

    public Image fadeImage;
    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;
    bool called = false;
    public float delayTime = 4;
    public bool done = false;
    private float timer;

    private void Awake()
    {
        timer = delayTime;
        Instance = this;
        Fade (false, 1.50f);
    }

    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.duration = duration;
        transition = isShowing ? 0 : 1;
    }


    private void Update()
    {
        Debug.Log(timer);
        if(timer>-4)
        {
            timer -= Time.deltaTime;
        }

        if (timer < 0)
        {
            if(called == false)
            {
                Fade (true, 1.25f);
                called = true;
            }

            if(timer < -1.25)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                if(currentScene.name == "Tomcat")
                {
                    SceneManager.LoadScene("PastryPandemonium");
                }
                else if(currentScene.name == "PastryPandemonium")
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
        
        if (!isInTransition)
        {
            return;
        }

        transition += (isShowing) ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);

        fadeImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);

        if(transition > 1 || transition < 0)
        {
            isInTransition = false;
        }
    }
    public void Skip()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Tomcat")
        {
            SceneManager.LoadScene("PastryPandemonium");
        }
        else if (currentScene.name == "PastryPandemonium")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
