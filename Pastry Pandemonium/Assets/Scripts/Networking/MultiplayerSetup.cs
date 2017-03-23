using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSetup : MonoBehaviour {

    public GameObject character;
    public static string selectedCharacter;

    private GameObject[] characters;

    public NetworkGameManager networkManager;

    private void Awake()
    {
        PhotonNetwork.OnEventCall += this.OnEvent;

        if (PhotonNetwork.isMasterClient)
        {
            selectedCharacter = "redCupcake";
            Player.characterOpponentPlayer = "berryMuffin";
        }
        else
        {
            selectedCharacter = "berryMuffin";
            Player.characterOpponentPlayer = "redCupcake";
        }

        Player.characterLocalPlayer = selectedCharacter;

        characters = GameObject.FindGameObjectsWithTag("character");
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].name != selectedCharacter)
            {
                LeanTween.alpha(characters[i], 0.35f, 0f);
            }
        }
    }

    public void OnMouseEnter()
    {
        //Scales objects to indicate you can click on them
        LeanTween.scale(character, new Vector3(0.8f, .8f, .8f), .03f);
    }


    public void OnMouseExit()
    {
        //Sets objects back to their original size
        LeanTween.scale(character, new Vector3(0.7f, 0.7f, 0.7f), .01f);
    }


    public void OnMouseUp()
    {
        if(character != null && 
            (character.name != Player.characterOpponentPlayer) ||
            Player.characterOpponentPlayer == "")
        {
            //resets all characters to loo unselected
            for (int i = 0; i < characters.Length; i++)
            {
                LeanTween.alpha(characters[i], 0.35f, 0f);
            }

            //sets the character selected to look selected
            LeanTween.alpha(character, 1f, .5f);
            selectedCharacter = character.name;
            //then send character selection across network
            sendCharacterSelection(selectedCharacter);
        }

        if (selectedCharacter == null)
        {
            if (PhotonNetwork.isMasterClient)
            {
                selectedCharacter = "redCupcake";
            }
            else
            {
                selectedCharacter = "berryMuffin";
            }
        }

        Player.characterLocalPlayer = selectedCharacter;
    }

    private void sendCharacterSelection(string character)
    {
        switch(character)
        {
            case "redCupcake":
                networkManager.chooseCharacter(0);
                break;
            case "chocolateCupcake":
                networkManager.chooseCharacter(1);
                break;
            case "whiteCupcake":
                networkManager.chooseCharacter(2);
                break;
            case "berryMuffin":
                networkManager.chooseCharacter(3);
                break;
            case "chipMuffin":
                networkManager.chooseCharacter(4);
                break;
            case "lemonMuffin":
                networkManager.chooseCharacter(5);
                break;
        }
    }

    private void recvCharacterSelection(int character)
    {
        switch (character)
        {
            case 0:
                Player.characterOpponentPlayer = "redCupcake";
                break;
            case 1:
                Player.characterOpponentPlayer = "chocolateCupcake";
                break;
            case 2:
                Player.characterOpponentPlayer = "whiteCupcake";
                break;
            case 3:
                Player.characterOpponentPlayer = "berryMuffin";
                break;
            case 4:
                Player.characterOpponentPlayer = "chipMuffin";
                break;
            case 5:
                Player.characterOpponentPlayer = "lemonMuffin";
                break;
        }
    }

    private void OnEvent(byte eventCode, object content, int senderid)
    {
        //if eventcode is 5, then it's chooseCharacter
        if (eventCode == 5)
        {
            byte[] selected = (byte[])content;
            recvCharacterSelection(selected[0]);
            //do something to signify opponent selection
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
}
