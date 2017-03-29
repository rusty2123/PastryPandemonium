using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerSetup : Photon.PunBehaviour
{

    public GameObject character, disconnected;
    public static string selectedCharacter = "";

    private GameObject[] characters;
    private Collider2D[] colliders;

    public NetworkGameManager networkManager;

    private void Awake()
    {
        disconnected.SetActive(false);

        PhotonNetwork.OnEventCall += this.OnEvent;

        Player.characterLocalPlayer = "";

        if (PhotonNetwork.isMasterClient)
        {
            Player.characterLocalPlayer = "redCupcake"; selectedCharacter = "redCupcake";
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
          (Player.characterOpponentPlayer == "" || character.name[character.name.Length - 1] != Player.characterOpponentPlayer[Player.characterOpponentPlayer.Length - 1]))
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

    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        disconnected.GetComponentInChildren<Text>().text = "Host has disconnected";
        disableColliders();
        networkManager.LeaveRoom();
        disconnected.SetActive(true);
    }

    public override void OnDisconnectedFromPhoton()
    {
        disconnected.GetComponentInChildren<Text>().text = "You have disconnected";
        disableColliders();
        //networkManager.LeaveRoom();
        MultiplayerSetup.selectedCharacter = "";
        Player.characterLocalPlayer = "";
        Player.characterOpponentPlayer = "";
        NetworkGameManager.localReady = false;
        NetworkGameManager.opponentReady = false;
        disconnected.SetActive(true);
    }

    private void disableColliders()
    {
        //disable all 2d colliders
        colliders = FindObjectsOfType<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void ReturnToLobby()
    {
        disconnected.SetActive(false);
        if(PhotonNetwork.connected)
        {
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            SceneManager.LoadScene("mainMenu");
        }
    }

    public void sendCharacterSelection(string character)
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
            Debug.Log("recieved character selection: " + selected[0]);
            Debug.Log("Player.characterLocalPlayer: " + Player.characterLocalPlayer);

            if (Player.characterLocalPlayer == "")
            {
                //default to berry muffin if host is cupcake and vice versa
                if(selected[0] == 0 || selected[0] == 1 || selected[0] == 2)
                {
                    Debug.Log("defaulting to muffin");
                    Player.characterLocalPlayer = "berryMuffin"; selectedCharacter = "berryMuffin";
                }
                else if(selected[0] == 3 || selected[0] == 4 || selected[0] == 5)
                {
                    Debug.Log("defaulting to cupcake");
                    Player.characterLocalPlayer = "redCupcake"; selectedCharacter = "redCupcake";
                }

                sendCharacterSelection(Player.characterLocalPlayer);

                characters = GameObject.FindGameObjectsWithTag("character");

                for (int i = 0; i < characters.Length; i++)
                {
                    if (characters[i] != null)
                    {
                        if (characters[i].name != selectedCharacter)
                        {
                            LeanTween.alpha(characters[i], 0.35f, 0f);
                        }
                        if (characters[i].name == selectedCharacter)
                        {
                            LeanTween.alpha(character, 1f, 0f);
                        }
                    }
                }
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
