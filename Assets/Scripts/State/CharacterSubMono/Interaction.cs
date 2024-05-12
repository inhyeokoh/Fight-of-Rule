using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : SubMono<PlayerController>
{
    public List<GameObject> InGameItems;

    protected override void _Clear()
    {
       
    }

    protected override void _Excute()
    {
        ItemInteraction();
    }

    protected override void _Init()
    {
        InGameItems = new List<GameObject>();
    }


    private void ItemInteraction()
    {
        if (InGameItems.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GameObject clone = InGameItems[0];
                InGameItems.RemoveAt(0);
                clone.SetActive(false);
            }
        }     
    }

    public void InGameItemEnter(GameObject go)
    {
        InGameItems.Add(go);
    }

    public void InGameItemExit(GameObject go)
    {
        InGameItems.Remove(go);
    }
}
