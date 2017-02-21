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
        LeanTween.scale(joinButton, new Vector3(.45f, .45f, .45f), .075f);
    }
    public void OnMouseExit()
    {
        //Sets menu options back to their original size
        LeanTween.scale(joinButton, new Vector3(0.3615471f, 0.3615471f, 0.3615471f), .05f);
    }

    public void OnMouseUp()
    {

        //Finds what option you clicked on
        if (joinButton != null)
        {
            switch (joinButton.name)
            {
                case "hostGame":
                    //gameLobby.Host();
                    //break;
                case "join":
                    //parameter for join must be highlighted text
                    //gameLobby.Join();
                    break;

                default:
                    break;
            }
        }
    }
}
