using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ForfeitPopup : MonoBehaviour {


	public GameObject current;
	private GameObject menu;
    public NetworkGameManager networkManager;
    private App app = new App();

	// Use this for initialization
	void Start () {

		menu = GameObject.Find ("ForfeitPopup");
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void OnMouseEnter()
	{
		LeanTween.scale (current, new Vector3 (2.2f, 2.2f, 2.2f), .075f);
	}



	public void OnMouseExit()
	{
		LeanTween.scale (current, new Vector3 (1.950775f, 1.950775f, 1.950775f), .05f);
	}


	public void OnMouseUp()
	{
		if (current != null) {
			switch (current.name) {
			case "yesButton":

					LeanTween.scale (current, new Vector3 (1.950775f, 1.950775f, 1.950775f), .05f);

                    if(App.isSinglePlayer)
                    {
                        SceneManager.LoadScene("MainMenu");
                    }

                    if(!App.isSinglePlayer)
                    {
                        if(!App.gameOver)
                        {
                            //other player wins and end game
                            App.gameOver = true;
                            networkManager.sendWin(0);
                            Debug.Log("sent loss message");
                            //app.displayLossMessage();
                            App.remainingLocal = 2;
                            menu.SetActive(false);
                        }
                    }
				break;
			case "noButton":
				LeanTween.scale (current, new Vector3 (1.950775f, 1.950775f, 1.950775f), .05f);
				menu.SetActive (false);
				app.resumeGame ();
				break;
			default:
				break;
			}
		}
	}
			


}
