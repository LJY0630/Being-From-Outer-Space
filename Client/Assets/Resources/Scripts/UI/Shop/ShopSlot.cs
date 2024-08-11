using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopSlot : MonoBehaviour, IPointerUpHandler
{
    public Item item;
    public Image itemIcon;

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (transform.root.GetComponent<PlayerStat>().Gold >= item.cost)
            {
                if (transform.root.GetChild(1).GetComponent<Inventory>().AddItem(item))
                {
                    transform.root.GetComponent<PlayerStat>().Gold -= item.cost;
                    transform.parent.GetComponent<AudioSource>().Play();

                    C_SendMoney sendMoney = new C_SendMoney();
                    sendMoney.money = transform.root.GetComponent<PlayerStat>().Gold;
                    NetPlayerManager.Instance.Session.Send(sendMoney.Write());
                }
            }
        }
    }
}
