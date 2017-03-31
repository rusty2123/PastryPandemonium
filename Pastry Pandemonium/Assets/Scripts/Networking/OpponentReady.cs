using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentReady : MonoBehaviour {

    public GameObject checkMarkOpp;

    private void Awake()
    {
        //PhotonNetwork.OnEventCall = null;
        PhotonNetwork.OnEventCall += this.OnEvent;
        checkMarkOpp = GameObject.Find("checkMarkOpp");
        setOpponentCheckMark();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        setOpponentCheckMark();

    }


    private void setOpponentCheckMark()
    {
        if (checkMarkOpp != null)
        {
            if (NetworkGameManager.opponentReady)
            {
                checkMarkOpp.transform.localScale = new Vector3(.5f, .5f, .5f);
            }
            else if (!NetworkGameManager.opponentReady)
            {
                checkMarkOpp.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    private void OnEvent(byte eventCode, object content, int senderid)
    {
        //if eventcode is 6, then it's ready
        if (eventCode == 6)
        {
            byte[] selected = (byte[])content;

            if (selected[0] == 0)
            {
                NetworkGameManager.opponentReady = false;
                checkMarkOpp.transform.localScale = new Vector3(0, 0, 0);
            }
            else if (selected[0] == 1)
            {
                NetworkGameManager.opponentReady = true;
                checkMarkOpp.transform.localScale = new Vector3(.5f, .5f, .5f);
            }
            Debug.Log("recieved ready selection: " + selected[0]);
        }
    }
}
