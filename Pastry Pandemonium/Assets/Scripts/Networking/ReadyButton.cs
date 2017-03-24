using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : MonoBehaviour {

    public GameObject checkBox, checkMark;

    public NetworkGameManager networkManager;

    private void Awake()
    {
        //setOpponentCheckMark();

        checkBox = GameObject.Find("checkBoxLocal");
        checkMark = GameObject.Find("checkMarkLocal");
        checkMark.SetActive(false);
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

    // Use this for initialization
    void Start () {	}
	
	// Update is called once per frame
	void Update () {
    }
}
