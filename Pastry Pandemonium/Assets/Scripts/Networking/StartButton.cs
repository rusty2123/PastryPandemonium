using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

    public GameObject startButton;
    public Lobby gameLobby;
    public NetworkGameManager networkManager;

    private void Awake()
    {
        startButton = GameObject.Find("startButton");
    }

    public void OnMouseEnter()
    {
        //Scales menu options to indicate that you can click on them
        LeanTween.scale(startButton, new Vector3(2.2f, 2.2f, 2.2f), .075f);
    }
    public void OnMouseExit()
    {
        //Sets menu options back to their original size
        LeanTween.scale(startButton, new Vector3(2f, 2f, 2f), .05f);
    }

    public void OnMouseUp()
    {
        //Finds what option you clicked on
        if (startButton != null)
        {
            //go to a room creation pop-up similar to that of singleplayer then create game
            networkManager.LoadNetworkGame();
        }
        else
        {
            //display connection problems
        }
    }
}
