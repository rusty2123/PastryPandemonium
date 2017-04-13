using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroy : MonoBehaviour {

	public static int count = 0;

	private void Awake()
	{
		/*GameObject[] objs = GameObject.FindGameObjectsWithTag("slider");
		if (objs.Length > 1)
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);*/

		/*GameObject objs = GameObject.FindGameObjectWithTag("slider");

		if (count > 0) {
			Destroy (objs);
		} else {
			DontDestroyOnLoad (objs);
			count++;
		}*/



	}

}
