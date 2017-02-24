using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomNameText : MonoBehaviour
{
    public Text roomNamePrefab;
    public Lobby gameLobby;

    private void Awake()
    {
        roomNamePrefab = GameObject.Find("roomNamePrefab").GetComponent<Text>();
    }


    public void OnMouseEnter()
    {
        roomNamePrefab.fontSize = 65;
    }
    public void OnMouseExit()
    {
        roomNamePrefab.fontSize = 60;
    }

    public void OnMouseUp()
    {

        //Finds what option you clicked on
        if (roomNamePrefab != null && PhotonNetwork.connected)
        {
            //must also set all other rooms back to white
            roomNamePrefab.color = new Color(255, 140, 140);
            gameLobby.setRoomSelection(roomNamePrefab.text);
        }
        else
        {
            //display connection problems
        }
    }
}
