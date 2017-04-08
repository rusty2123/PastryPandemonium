using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showAvailableSpaces : MonoBehaviour {

	public GameObject current;
	private GameObject space1, space2, space3, space4, space5, space6, space7, space8, space9, space10;
	private GameObject space11, space12, space13, space14, space15, space16, space17, space18, space19, space20, space21, space22, space23, space24;



	private void Awake()
	{
		//Yes, I know I could have done this a better way..

		space1 = GameObject.Find ("availableSpace1");
		space2 = GameObject.Find ("availableSpace2");
		space3 = GameObject.Find ("availableSpace3");
		space4 = GameObject.Find ("availableSpace4");
		space5 = GameObject.Find ("availableSpace5");
		space6 = GameObject.Find ("availableSpace (6)");
		space7 = GameObject.Find ("availableSpace (7)");
		space8 = GameObject.Find ("availableSpace (8)");
		space9 = GameObject.Find ("availableSpace (9)");
		space10 = GameObject.Find ("availableSpace (10)");
		space11 = GameObject.Find ("availableSpace (11)");
		space12 = GameObject.Find ("availableSpace (12)");
		space13 = GameObject.Find ("availableSpace (13)");
		space14 = GameObject.Find ("availableSpace (14)");
		space15 = GameObject.Find ("availableSpace (15)");
		space16 = GameObject.Find ("availableSpace (16)");
		space17 = GameObject.Find ("availableSpace (17)");
		space18 = GameObject.Find ("availableSpace (18)");
		space19 = GameObject.Find ("availableSpace (19)");
		space20 = GameObject.Find ("availableSpace (20)");
		space21 = GameObject.Find ("availableSpace (21)");
		space22 = GameObject.Find ("availableSpace (22)");
		space23 = GameObject.Find ("availableSpace (23)");
		space24 = GameObject.Find ("availableSpace (24)");

		space1.GetComponent<Renderer> ().enabled = false;
		space2.GetComponent<Renderer> ().enabled = false;
		space3.GetComponent<Renderer> ().enabled = false;
		space4.GetComponent<Renderer> ().enabled = false;
		space5.GetComponent<Renderer> ().enabled = false;
		space6.GetComponent<Renderer> ().enabled = false;
		space7.GetComponent<Renderer> ().enabled = false;
		space8.GetComponent<Renderer> ().enabled = false;
		space9.GetComponent<Renderer> ().enabled = false;
		space10.GetComponent<Renderer> ().enabled = false;
		space11.GetComponent<Renderer> ().enabled = false;
		space12.GetComponent<Renderer> ().enabled = false;
		space13.GetComponent<Renderer> ().enabled = false;
		space14.GetComponent<Renderer> ().enabled = false;
		space15.GetComponent<Renderer> ().enabled = false;
		space16.GetComponent<Renderer> ().enabled = false;
		space17.GetComponent<Renderer> ().enabled = false;
		space18.GetComponent<Renderer> ().enabled = false;
		space19.GetComponent<Renderer> ().enabled = false;
		space20.GetComponent<Renderer> ().enabled = false;
		space21.GetComponent<Renderer> ().enabled = false;
		space22.GetComponent<Renderer> ().enabled = false;
		space23.GetComponent<Renderer> ().enabled = false;
		space24.GetComponent<Renderer> ().enabled = false;

	}


    public void OnMouseEnter()
    {
		if (App.phase == 1 || App.phase == 3 && App.isLocalPlayerTurn) {
			current.GetComponent<Renderer> ().enabled = true;
		}
	}


	public void OnMouseExit()
	{
		if (App.phase == 1 || App.phase == 3  && App.isLocalPlayerTurn) {
			current.GetComponent<Renderer> ().enabled = false;
		}


	}
}
