using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListController : MonoBehaviour
{

    public GameObject ListItemPrefab;
    private RoomInfo[] roomsList;
    public int yOffset = 10;


    private void Update()
    {
        onRefresh();
    }


    public void onRefresh()
    {
        for (int i = 0; i < roomsList.Length; ++i)
        {
            GameObject newMultiplayerRoom = Instantiate(ListItemPrefab) as GameObject;
            ListItemController controller = newMultiplayerRoom.GetComponent(typeof(ListItemController)) as ListItemController;
            controller.name.text = roomsList[i].Name;
            newMultiplayerRoom.name = roomsList[i].Name;
            Debug.Log(controller.name.text);
            Debug.Log(newMultiplayerRoom.name);
        }
    }



    //public GameObject ContentPanel;
    //public GameObject ListItemPrefab;

    //private RoomInfo[] roomsList;

    //void Start()
    //{
    //    if (roomsList != null)
    //    {
    //        for (int i = 0; i < roomsList.Length; ++i)
    //        {
    //            GameObject newMultiplayerRoom = Instantiate(ListItemPrefab) as GameObject;
    //            ListItemController controller = newMultiplayerRoom.GetComponent(typeof(ListItemController)) as ListItemController;
    //            controller.name.text = roomsList[i].Name;
    //            newMultiplayerRoom.transform.parent = ContentPanel.transform;
    //            newMultiplayerRoom.transform.localScale = Vector3.one;
    //        }
    //    }
    //}
}
