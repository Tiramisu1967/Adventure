using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForageInteraction : BaseInteraction
{
    public GameObject[] destoryObject;
    public BaseItem _getItem;
    private int count;

    private void Start()
    {
        count = 0;
    }

    public override void Interaction()
    {
        if(Inventory.instance.currentItems.Count < Inventory.instance.maxBag)
        {
            Inventory.instance.GetItem(_getItem);
            if(count == destoryObject.Length - 1)
            {
                Destroy(this.gameObject);
            }else
            {
                Destroy(destoryObject[count]);
            }
            count++;
        }
        base.Interaction();
    }
}
