using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceClick : MonoBehaviour
{


    private GameObject gameObj;
    public AudioSource audioSource; //piece click audio
    private float range;

    void Start()
    {
        //range = Random.Range(0f, 1f);

        //if (!(gameObject.GetComponent<Animation>() == null))
        //    StartCoroutine(startAnimation());
        //else
        //    StartCoroutine(startAnimation());

    }

    //IEnumerator startAnimation()
    //{
    //    yield return new WaitForSeconds(range);
    //    gameObject.GetComponent<Animation>().Play();
    //}

    void OnMouseDown()
    {
        gameObj = GameObject.FindWithTag("gameBoard");
        //audioSource.Play();

       switch (App.phase)
        {
            case 1:
            gameObj.GetComponent<App>().setClickedObjects(gameObject);
                break;
            case 2:
                break;
            case 3:
                break;
        }

    }


}
