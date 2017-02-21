using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public GameObject menuButton;

    private void Awake()
    {
        menuButton = GameObject.Find("menuButton");
    }


    public void OnMouseEnter()
    {
        //Scales menu options to indicate that you can click on them
        LeanTween.scale(menuButton, new Vector3(.45f, .45f, .45f), .075f);
    }
    public void OnMouseExit()
    {
        //Sets menu options back to their original size
        LeanTween.scale(menuButton, new Vector3(0.3615471f, 0.3615471f, 0.3615471f), .05f);
    }

    public void OnMouseUp()
    {

        //Finds what option you clicked on
        if (menuButton != null)
        {
            switch (menuButton.name)
            {
                case "menuButton":
                    //go to a room creation pop-up similar to that of singleplayer then create game
                    SceneManager.LoadScene("MainMenu");
                    break;
                default:
                    break;
            }
        }
    }
}
