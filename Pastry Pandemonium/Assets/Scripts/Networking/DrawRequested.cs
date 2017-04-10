using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRequested : MonoBehaviour {

    public GameObject current;
    private GameObject requestDrawPopup;

    public NetworkGameManager networkManager;



    private void Awake()
    {
        requestDrawPopup = GameObject.Find("requestDrawPopup");

        current.SetActive(false);
    }

    public void OnMouseEnter()
    {
        //Scales menu options to indicate that you can click on them
        LeanTween.scale(current, new Vector3(1.1f, 1.1f, 1.1f), .075f);
    }
    public void OnMouseExit()
    {
        //Sets menu options back to their original size
        LeanTween.scale(current, new Vector3(1f, 1f, 1f), .05f);
    }

    public void OnMouseUp()
    {
        if (current != null)
        {
            switch (current.name)
            {
                case "acceptButton":

                    //send draw declined to opponent
                    networkManager.acceptDraw(1);
                    //make popup go away
                    requestDrawPopup.SetActive(false);

                    break;


                case "declineButton":

                    //send draw declined to opponent
                    networkManager.acceptDraw(0);
                    //make popup go away
                    requestDrawPopup.SetActive(false);

                    break;
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
