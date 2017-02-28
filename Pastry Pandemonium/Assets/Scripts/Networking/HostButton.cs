using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostButton : MonoBehaviour
{
    public GameObject hostButton;
    public Lobby gameLobby;

    private void Awake()
    {
        hostButton = GameObject.Find("hostGame");
    }


    public void OnMouseEnter()
    {
        //Scales menu options to indicate that you can click on them
        LeanTween.scale(hostButton, new Vector3(.23f, .23f, .23f), .075f);
    }
    public void OnMouseExit()
    {
        //Sets menu options back to their original size
        LeanTween.scale(hostButton, new Vector3(0.2f, 0.2f, 0.2f), .05f);
    }

    public void OnMouseUp()
    {

        //Finds what option you clicked on
        if (hostButton != null && PhotonNetwork.connected)
        {
            //go to a room creation pop-up similar to that of singleplayer then create game
            gameLobby.Host();
        }
        else
        {
            //display connection problems
        }
    }
}
