using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RequestDrawPopup : MonoBehaviour {

	public GameObject current, canvas;
	public NetworkGameManager networkManager;
	private GameObject menu;
    private App app = new App();

	// Use this for initialization
	void Start () {
		menu = GameObject.Find ("RequestPopup");
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
		if(current != null)
		{
			switch (current.name)
			{
                case "yesButton":
					LeanTween.scale (current, new Vector3 (1.950775f, 1.950775f, 1.950775f), .05f);

                    if (!App.isSinglePlayer)
                    {
                        networkManager.offerDraw();
                        menu.SetActive(false);
                    }

                    else
                    {
                        if (Game.gameInstance.isDraw())
                        {
                            if (App.isSinglePlayer)
                            {
                                App.gameOver = true;
                                menu.SetActive(false);
                                App.isDraw = true;
                                // app.displayTieMessage();

                                //SceneManager.LoadScene("MainMenu");
                            }
                        }
                        else
                        {
                            canvas.SetActive(true);
                            menu.SetActive(false);
							App.isDraw = false;
                        }
                    }
				break;
			case "noButton":
				LeanTween.scale (current, new Vector3 (1.950775f, 1.950775f, 1.950775f), .05f);
				menu.SetActive (false);
                App.isDraw = false;
                app.resumeGame();
				break;
			case "okButton":
				canvas.SetActive (false);
				App.isDraw = false;
				app.resumeGame ();
				LeanTween.scale (current, new Vector3 (1.950775f, 1.950775f, 1.950775f), .05f);
				break;
			default:
				break;
			}
		}

	}




	private IEnumerator startCoroutine()
	{
		yield return StartCoroutine(getDrawResponse());
	}

	private IEnumerator getDrawResponse()
	{
		NetworkGameManager.drawResponseRecieved = false;

		yield return new WaitUntil(() => NetworkGameManager.drawResponseRecieved);

		NetworkGameManager.drawResponseRecieved = false;

		if (NetworkGameManager.drawAccepted)
		{
			//draw
		}
		else if(!NetworkGameManager.drawAccepted)
		{
			//continue game
		}
	}
}
