using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentSelection : Photon.PunBehaviour
{

    public RectTransform opponentSelection;

    private void Awake()
    {
       
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        setOpponentSelectionPos();
        setOpponentSelectionVisibility();
    }

    private void setOpponentSelectionPos()
    {
        switch (Player.characterOpponentPlayer)
        {
            case "redCupcake":
                opponentSelection.anchoredPosition = new Vector3(-1470, 0, 0);
                break;
            case "chocolateCupcake":
                opponentSelection.anchoredPosition = new Vector3(-1000, 0, 0);
                break;
            case "whiteCupcake":
                opponentSelection.anchoredPosition = new Vector3(-550, 0, 0);
                break;
            case "berryMuffin":
                opponentSelection.anchoredPosition = new Vector3(0, 0, 0);
                break;
            case "chipMuffin":
                opponentSelection.anchoredPosition = new Vector3(500, 0, 0);
                break;
            case "lemonMuffin":
                opponentSelection.anchoredPosition = new Vector3(1000, 0, 0);
                break;
        }
    }

    private void setOpponentSelectionVisibility()
    {
        if(PhotonNetwork.playerList.Length == 1)
        {
            opponentSelection.transform.localScale = new Vector3(0f, 0f, 0f);
        }
        else
        {
            opponentSelection.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
