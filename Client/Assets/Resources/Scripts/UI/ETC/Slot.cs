using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Inventory inven;
    public Item item;
    public Image itemIcon;
    public TextMeshProUGUI textHowMany;
    public int slotNum;
    public ItemSound itemSound;
    public Vector3 start;
    public Vector3 end;
    public bool isShopMode;
    public bool isSell = false;
    public GameObject chkSell;

    [SerializeField]
    private Button button;

    private void Awake()
    {
        RemoveSlot();
        inven = transform.root.GetChild(1).GetComponent<Inventory>();
    }

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        textHowMany.gameObject.SetActive(true);
        //OnDisable();
        button.interactable = true;
    }
    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
        textHowMany.gameObject.SetActive(false);
        OnDisable();
        button.interactable = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        start = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        end = Input.mousePosition;

        if (button.IsInteractable() && (start == end))
        {
            if (!isShopMode)
            {
                if ((item.itemType == ItemType.Consumables))
                {
                    bool isUse = item.Use(transform.root);

                    if (isUse)
                    {
                        itemSound.DrinkSound();
                        //Debug.Log(inven.count[slotNum]);

                        if (inven.count[slotNum] != 1)
                        {
                            inven.count[slotNum] -= 1;
                            int index = inven.CheckInvenInfo(inven.items[slotNum].itemcode);
                            inven.Inveninfo.itemss[index].cnt--;
                            NetPlayerManager.Instance.Session.Send(inven.Inveninfo.Write());
                            inven.onChangeItem.Invoke();
                        }
                        else
                        {
                            inven.RemoveItem(slotNum);
                        }
                    }
                }
            }
            else
            {
                if (transform.parent.gameObject.name == "Content")
                {
                    if (!isSell)
                    {
                        isSell = true;
                        chkSell.SetActive(isSell);
                    }
                    else
                    {
                        isSell = false;
                        chkSell.SetActive(isSell);
                    }
                }
            }
        }
    }

    public void SellItem()
    {
        transform.root.GetComponent<PlayerStat>().Gold += item.sellcost * inven.count[slotNum];
        inven.RemoveItemSell(slotNum);
        isSell = false;
        chkSell.SetActive(isSell);
    }

    private void OnDisable()
    {
        isSell = false;
        chkSell.SetActive(isSell);
    }
}