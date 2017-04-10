using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRequested : MonoBehaviour {

    public GameObject current;
    private GameObject requestDrawPopup, acceptButton, declineButton;

    public NetworkGameManager networkManager;



    private void Awake()
    {
        if (!NetworkGameManager.drawEventsAdded)
        {
            Debug.Log("opponent draw event added");
            PhotonNetwork.OnEventCall += this.OnEvent;
            NetworkGameManager.drawEventsAdded = true;
        }

        requestDrawPopup = GameObject.Find("requestDrawPopup");
        acceptButton = GameObject.Find("acceptButton");
        declineButton = GameObject.Find("declineButton");

        current.SetActive(true);

        requestDrawPopup.transform.localScale = new Vector3(0, 0, 0);
        acceptButton.transform.localScale = new Vector3(0, 0, 0);
        declineButton.transform.localScale = new Vector3(0, 0, 0);

        //current.SetActive(false);
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
                    requestDrawPopup.transform.localScale = new Vector3(0, 0, 0);
                    acceptButton.transform.localScale = new Vector3(0, 0, 0);
                    declineButton.transform.localScale = new Vector3(0, 0, 0);
                    //current.SetActive(false);
                    acceptButton.SetActive(false);
                    declineButton.SetActive(false);
                    break;


                case "declineButton":

                    //send draw declined to opponent
                    networkManager.acceptDraw(0);
                    //make popup go away
                    requestDrawPopup.transform.localScale = new Vector3(0, 0, 0);
                    acceptButton.transform.localScale = new Vector3(0, 0, 0);
                    declineButton.transform.localScale = new Vector3(0, 0, 0);
                    //current.SetActive(false);
                    acceptButton.SetActive(false);
                    declineButton.SetActive(false);

                    break;
            }
        }
    }

    private void OnEvent(byte eventCode, object content, int senderid)
    {
        if (eventCode == 7)
        {
            Debug.Log("opponent offered draw");
            //opponent offered draw
            current.SetActive(true);
            acceptButton.SetActive(true);
            declineButton.SetActive(true);
            requestDrawPopup.transform.localScale = new Vector3(1, 1, 1);
            acceptButton.transform.localScale = new Vector3(1, 1, 1);
            declineButton.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Use this for initialization
    void Start () {
        requestDrawPopup = GameObject.Find("requestDrawPopup");
        acceptButton = GameObject.Find("acceptButton");
        declineButton = GameObject.Find("declineButton");

        requestDrawPopup.transform.localScale = new Vector3(0, 0, 0);
        acceptButton.transform.localScale = new Vector3(0, 0, 0);
        declineButton.transform.localScale = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
