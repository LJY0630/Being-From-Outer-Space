using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventotyUI : MonoBehaviour
{
    [SerializeField]
    private PlayerStat stat;

    [SerializeField]
    private Inventory inven;

    [SerializeField]
    private ShopUI shop;

    [SerializeField]
    private GameObject InventoryPanel;
    
    public bool isInventory = false;

    public Slot[] slots;
    public Transform slotHolder;

    void Awake()
    {
        isInventory = true;
        InventoryPanel.SetActive(true);
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onChangeItem += RedrawSlotUI;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotNum = i;
        }
        RedrawSlotUI();
        isInventory = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stat.isDead)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                isInventory = !isInventory;
            }

            if (isInventory)
            {
                InventoryPanel.SetActive(true);
                ShopOpen(shop.isShopOn);
            }
            else
            {
                ShopOpen(false);
                InventoryPanel.SetActive(false);
            }
        }
        else
            InventoryPanel.SetActive(false);
    }

    public void RedrawSlotUI() 
    {
        for (int i = 0; i < slots.Length; i++) 
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inven.items.Count; i++) 
        {
            slots[i].item = inven.items[i];
            slots[i].textHowMany.text = inven.count[i].ToString();
            slots[i].UpdateSlotUI();
        }
    }

    public void ShopOpen(bool isOpen) 
    {
        for (int i = 0; i < inven.items.Count; i++)
        {
            slots[i].isShopMode = isOpen;
        }
    }

    public void ShopClose()
    {
        for (int i = 0; i < inven.items.Count; i++)
        {
            slots[i].isSell = false;
        }
    }

    public void SellItem()
    {
        for (int i = inven.items.Count - 1; i >= 0; i--)
        {
            if (slots[i].isSell)
            {
                slots[i].SellItem();
                transform.GetChild(1).GetComponent<AudioSource>().Play();
            }
        }

        C_SendMoney sendMoney = new C_SendMoney();
        sendMoney.money = transform.root.GetComponent<PlayerStat>().Gold;
        NetPlayerManager.Instance.Session.Send(sendMoney.Write());
        inven.onChangeItem();
    }
}
