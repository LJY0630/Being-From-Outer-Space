using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : MonoBehaviour
{
    public ShopSlot[] slots;
    public Transform slotHolder;
    // Start is called before the first frame update
    void Start()
    {
        slots = slotHolder.GetComponentsInChildren<ShopSlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].item = ItemDataBase.instance.itemDB[i];
            slots[i].UpdateSlotUI();
        }
    }

    // Update is called once per frame
}
