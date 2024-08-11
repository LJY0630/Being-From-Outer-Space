using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType 
{
    Weapon,
    Equipment,
    Consumables,
    Etc
}

[System.Serializable]
public class Item
{
    public int itemcode;
    public ItemType itemType;
    public string itemName;
    public int cost;
    public int sellcost;
    public Sprite itemImage;
    public List<ItemEffect> efts;

    public bool Use(Transform transform) 
    {
        bool isUsed = false;
        foreach (ItemEffect eft in efts)
        {
            isUsed = eft.ExecuteRole(transform);
        }
        return isUsed;
    }
}
