using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinButton : MonoBehaviour
{
    public GameObject joinButton;
    public Lobby gameLobby;

    private void Awake()
    {
        joinButton = GameObject.Find("join");
    }


    public void OnMouseEnter()
    {
        //Scales menu options to indicate that you can click on them
        LeanTween.scale(joinButton, new Vector3(.23f, .23f, .23f), .075f);
    }
    public void OnMouseExit()
    {
        //Sets menu options back to their original size
        LeanTween.scale(joinButton, new Vector3(.2f, 0.2f, 0.2f), .05f);
    }

    public void OnMouseUp()
    {

        //Finds what option you clicked on
        if (joinButton != null && PhotonNetwork.connected)
        {
            //parameter for join must be highlighted text
            gameLobby.Join("bob's Game");
        }
    }
}
