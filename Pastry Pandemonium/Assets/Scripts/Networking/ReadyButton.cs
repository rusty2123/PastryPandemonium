using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : MonoBehaviour {

    public GameObject checkBox, checkMark, checkMarkOpp;

    public NetworkGameManager networkManager;

    private void Awake()
    {
        PhotonNetwork.OnEventCall += this.OnEvent;

        setOpponentCheckMark();

        checkBox = GameObject.Find("checkBoxLocal");
        checkMark = GameObject.Find("checkMarkLocal");
        checkMarkOpp = GameObject.Find("checkMarkOpp");
        checkMark.SetActive(false);
        //checkMarkOpp.SetActive(false);
    }

    public void OnMouseEnter()
    {
        //Scales menu options to indicate that you can click on them
        LeanTween.scale(checkBox, new Vector3(.6f, .6f, .6f), .075f);
    }
    public void OnMouseExit()
    {
        //Sets menu options back to their original size
        LeanTween.scale(checkBox, new Vector3(.5f, .5f, .5f), .05f);
    }

    public void OnMouseUp()
    {
        //Finds what option you clicked on
        if (checkBox != null)
        {
            if (NetworkGameManager.localReady)
            {
                NetworkGameManager.localReady = false;
                networkManager.ready(0);
                checkMark.SetActive(false);
            }
            else if (!NetworkGameManager.localReady)
            {
                NetworkGameManager.localReady = true;
                networkManager.ready(1);
                checkMark.SetActive(true);
            }
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
            Debug.Log("recieved ready selection");
            byte[] selected = (byte[])content;

            if(selected[0] == 0)
            {
                NetworkGameManager.opponentReady = false;
            }
            else if(selected[0] == 1)
            {
                NetworkGameManager.opponentReady = true;
            }
        }
    }

    private void setOpponentCheckMark()
    {
        //if (checkMarkOpp != null)
        {
            if (NetworkGameManager.opponentReady)
            {
                checkMarkOpp.SetActive(true);
            }
            else if (!NetworkGameManager.opponentReady)
            {
                checkMarkOpp.SetActive(false);
            }
        }
    }

    // Use this for initialization
    void Start () {	}
	
	// Update is called once per frame
	void Update () {
        setOpponentCheckMark();
    }
}
