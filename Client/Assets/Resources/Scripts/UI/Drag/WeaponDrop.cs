using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponDrop : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private WeaponDrag equipdrag;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private WeaponChanger changItem;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.ToString().Contains("Item"))
        {
            if (eventData.pointerDrag.TryGetComponent<DraggableUI>(out DraggableUI drag))
            {
                if (!eventData.pointerDrag.GetComponent<DraggableUI>().previousParent.Equals(this.transform))
                {
                    if (eventData.pointerDrag.GetComponent<DraggableUI>().previousParent.GetComponent<Slot>().item.itemType == ItemType.Weapon &&
                       (playerController.playeras == null || (!playerController.playeras.skill1 && !playerController.playeras.skill2)))
                    {
                        ItemEffectWeapon itemeft = (ItemEffectWeapon)eventData.pointerDrag.GetComponent<DraggableUI>().previousParent.GetComponent<Slot>().item.efts[0];
                        if (equipdrag.previousItem != null)
                        {
                            if (!eventData.pointerDrag.GetComponent<DraggableUI>().previousParent.GetComponent<Slot>().item.itemName.Equals(this.transform.GetComponent<Slot>().item.itemName))
                            {
                                itemeft = (ItemEffectWeapon)equipdrag.previousItem.efts[0];

                                if (itemeft.UnEquip(transform.root))
                                {

                                    C_UnEquipped c_UnEquipped = new C_UnEquipped();
                                    c_UnEquipped.itemId = equipdrag.previousItem.itemcode.ToString();
                                    NetPlayerManager.Instance.Session.Send(c_UnEquipped.Write());

                                    this.transform.GetComponent<Slot>().item = eventData.pointerDrag.GetComponent<DraggableUI>().previousParent.GetComponent<Slot>().item;
                                    if (this.transform.GetComponent<Slot>().item.Use(transform.root))
                                    {

                                        //여기서 무기 보내기
                                        C_Equipped c_Equipped = new C_Equipped();
                                        c_Equipped.itemId = this.transform.GetComponent<Slot>().item.itemcode.ToString();
                                        NetPlayerManager.Instance.Session.Send(c_Equipped.Write());

                                        this.transform.GetComponent<Slot>().UpdateSlotUI();
                                        eventData.pointerDrag.GetComponent<DraggableUI>().previousParent.GetComponent<Slot>().RemoveSlot();
                                        transform.root.GetChild(1).GetComponent<Inventory>().RemoveWithoutServerItem(eventData.pointerDrag.GetComponent<DraggableUI>().previousParent.GetComponent<Slot>().slotNum);
                                        transform.root.GetChild(1).GetComponent<Inventory>().AddWithoutServerItem(equipdrag.previousItem);
                                        equipdrag.previousItem = this.transform.GetComponent<Slot>().item;
                                        changItem.WeaponChange(this.transform.GetComponent<Slot>().item.itemName);

                                        Debug.Log("무기 장착 완료");
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.transform.GetComponent<Slot>().item = eventData.pointerDrag.GetComponent<DraggableUI>().previousParent.GetComponent<Slot>().item;
                            if (this.transform.GetComponent<Slot>().item.Use(transform.root))
                            {

                                Debug.Log("웨폰드랍 1");
                                //여기서 무기 보내기
                                C_Equipped c_Equipped = new C_Equipped();
                                c_Equipped.itemId = this.transform.GetComponent<Slot>().item.itemcode.ToString();
                                NetPlayerManager.Instance.Session.Send(c_Equipped.Write());

                                this.transform.GetComponent<Slot>().UpdateSlotUI();
                                eventData.pointerDrag.GetComponent<DraggableUI>().previousParent.GetComponent<Slot>().RemoveSlot();
                                transform.root.GetChild(1).GetComponent<Inventory>().RemoveWithoutServerItem(eventData.pointerDrag.GetComponent<DraggableUI>().previousParent.GetComponent<Slot>().slotNum);
                                equipdrag.previousItem = this.transform.GetComponent<Slot>().item;
                                changItem.WeaponChange(this.transform.GetComponent<Slot>().item.itemName);
                            }
                        }
                    }
                }
            }
        }
    }
}