using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    public GameObject current;

    // Use this for initialization
    void Start () {
		
	}

    public void OnMouseEnter()
    {
        //if (App.phase == 1 || App.phase == 3 && App.isLocalPlayerTurn) {
        current.GetComponent<Renderer>().enabled = true;
        //}
    }


    public void OnMouseExit()
    {
        //	if (App.phase == 1 || App.phase == 3  && App.isLocalPlayerTurn) {
        current.GetComponent<Renderer>().enabled = false;
        //}


    }
}
