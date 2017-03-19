using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : MonoBehaviour {

    public GameObject readyButton;

    public NetworkGameManager networkManager;

    private bool ready = false;


    private void Awake()
    {
        readyButton = GameObject.Find("readyButton");
    }

    public void OnMouseEnter()
    {
        //Scales menu options to indicate that you can click on them
        LeanTween.scale(readyButton, new Vector3(2.2f, 2.2f, 2.2f), .075f);
    }
    public void OnMouseExit()
    {
        //Sets menu options back to their original size
        LeanTween.scale(readyButton, new Vector3(2f, 2f, 2f), .05f);
    }

    public void OnMouseUp()
    {
        //Finds what option you clicked on
        if (readyButton != null)
        {
            ready = true;
            networkManager.ready();
        }
        else
        {
            //display connection problems
        }
    }

    private void OnEvent(byte eventCode, object content, int senderid)
    {
        //if eventcode is 6, then it's ready
        if (eventCode == 6)
        {
            byte[] selected = (byte[])content;
            NetworkGameManager.opponentReady = true;
            //do something to signify opponent selection
        }
    }

    // Use this for initialization
    void Start () {	}
	
	// Update is called once per frame
	void Update () { }
}
