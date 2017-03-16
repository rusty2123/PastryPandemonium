using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {

    public GameObject backButton;
    public Lobby gameLobby;

    private void Awake()
    {
        backButton = GameObject.Find("backButton");
    }


    public void OnMouseEnter()
    {
        //Scales menu options to indicate that you can click on them
        LeanTween.scale(backButton, new Vector3(2.2f, 2.2f, 2.2f), .075f);
    }
    public void OnMouseExit()
    {
        //Sets menu options back to their original size
        LeanTween.scale(backButton, new Vector3(2f, 2f, 2f), .05f);
    }

    public void OnMouseUp()
    {

        //Finds what option you clicked on
        if (backButton != null)
        {
            gameLobby.Leave();
        }
        else
        {
            //display connection problems
        }
    }
}
