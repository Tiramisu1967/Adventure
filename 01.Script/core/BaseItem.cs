using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    forage,
    equipment,
    expendables
}

public enum ItemUse
{
    noen,
    Sword,
    FlashLight,
    Medicine,
    Oxygen,
    Booster,
    Strong_Booster,
    Compass,
    Deodorant
}

[System.Serializable]
public struct BaseItem 
{
    public ItemType type;
    public ItemUse use;
    public string itemName;
    public Sprite itemImage;
    public GameObject ItemObject;
    public int price;
    public int weight;

}
