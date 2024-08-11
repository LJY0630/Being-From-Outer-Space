using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Inventory inven;

    [SerializeField]
    private PlayerController playerController;

    /// <summary>
    /// ���콺 ����Ʈ�� ���� ������ ���� ���� ���η� �� �� 1ȸ ȣ��
    /// </summary>


    /// <summary>
    /// ���� ������ ���� ���� ���ο��� ����� ���� �� 1ȸ ȣ��
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.ToString().Contains("Item"))
        {
            if (eventData.pointerDrag.TryGetComponent<EquipDrag>(out EquipDrag equip))
            {
                if (!eventData.pointerDrag.GetComponent<EquipDrag>().previousParent.IsChildOf(this.transform.GetChild(0)))
                {
                    if (inven.items.Count < 28)
                    {
                        if (transform.GetComponent<Slot>().item == null)
                        {
                            inven.AddWithoutServerItem(eventData.pointerDrag.GetComponent<EquipDrag>().previousParent.GetComponent<Slot>().item);
                            eventData.pointerDrag.GetComponent<EquipDrag>().previousParent.GetComponent<Slot>().item = null;
                            eventData.pointerDrag.transform.GetComponent<EquipDrag>().previousParent.GetComponent<Slot>().RemoveSlot();
                        }
                        else
                        {
                            if (transform.GetComponent<Slot>().item.itemType == ItemType.Equipment)
                            {
                                ItemEffectEquipment eft = (ItemEffectEquipment)transform.GetComponent<Slot>().item.efts[0];
                                if (eft.itemcode == eventData.pointerDrag.GetComponent<EquipDrag>().itemcode)
                                {
                                    inven.AddWithoutServerItem(eventData.pointerDrag.GetComponent<EquipDrag>().previousParent.GetComponent<Slot>().item);
                                    eventData.pointerDrag.GetComponent<EquipDrag>().previousParent.GetComponent<Slot>().item = transform.GetComponent<Slot>().item;
                                    eventData.pointerDrag.transform.GetComponent<EquipDrag>().previousParent.GetComponent<Slot>().UpdateSlotUI();
                                    inven.RemoveWithoutServerItem(transform.GetComponent<Slot>().slotNum);

                                    Debug.Log("�巡�׿��̺�1");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (transform.GetComponent<Slot>().item.itemType == ItemType.Equipment)
                        {
                            Item temp = eventData.pointerDrag.GetComponent<EquipDrag>().previousParent.GetComponent<Slot>().item;
                            ItemEffectEquipment eft = (ItemEffectEquipment)transform.GetComponent<Slot>().item.efts[0];
                            if (eft.itemcode == eventData.pointerDrag.GetComponent<EquipDrag>().itemcode)
                            {
                                eventData.pointerDrag.GetComponent<EquipDrag>().previousParent.GetComponent<Slot>().item = transform.GetComponent<Slot>().item;
                                eventData.pointerDrag.transform.GetComponent<EquipDrag>().previousParent.GetComponent<Slot>().UpdateSlotUI();
                                inven.RemoveWithoutServerItem(transform.GetComponent<Slot>().slotNum);
                                inven.AddWithoutServerItem(temp);
                                Debug.Log("�巡�׿��̺�2");
                            }
                        }
                    }
                }
            }
            else if (eventData.pointerDrag.TryGetComponent<WeaponDrag>(out WeaponDrag weap))
            {
                if (!eventData.pointerDrag.GetComponent<WeaponDrag>().previousParent.IsChildOf(this.transform.GetChild(0)))
                {
                    if (inven.items.Count < 28)
                    {
                        if (transform.GetComponent<Slot>().item == null &&
                        (playerController.playeras == null || (!playerController.playeras.skill1 && !playerController.playeras.skill2)))
                        {
                            inven.AddWithoutServerItem(eventData.pointerDrag.GetComponent<WeaponDrag>().previousParent.GetComponent<Slot>().item);
                            eventData.pointerDrag.GetComponent<WeaponDrag>().previousParent.GetComponent<Slot>().item = null;
                            eventData.pointerDrag.transform.GetComponent<WeaponDrag>().previousParent.GetComponent<Slot>().RemoveSlot();
                            Debug.Log("�巡�׿��̺�3");
                        }
                        else
                        {
                            if (transform.GetComponent<Slot>().item.itemType == ItemType.Weapon && !playerController.playeras.skill1 && !playerController.playeras.skill2)
                            {
                                ItemEffectWeapon eft = (ItemEffectWeapon)transform.GetComponent<Slot>().item.efts[0];
                                inven.AddWithoutServerItem(eventData.pointerDrag.GetComponent<WeaponDrag>().previousParent.GetComponent<Slot>().item);
                                eventData.pointerDrag.GetComponent<WeaponDrag>().previousParent.GetComponent<Slot>().item = transform.GetComponent<Slot>().item;
                                eventData.pointerDrag.transform.GetComponent<WeaponDrag>().previousParent.GetComponent<Slot>().UpdateSlotUI();
                                inven.RemoveWithoutServerItem(transform.GetComponent<Slot>().slotNum);
                                Debug.Log("�巡�׿��̺�4");
                            }
                        }
                    }
                    else
                    {
                        if (transform.GetComponent<Slot>().item.itemType == ItemType.Weapon &&
                        (playerController.playeras == null || (!playerController.playeras.skill1 && !playerController.playeras.skill2)))
                        {
                            Item temp = eventData.pointerDrag.GetComponent<WeaponDrag>().previousParent.GetComponent<Slot>().item;
                            ItemEffectWeapon eft = (ItemEffectWeapon)transform.GetComponent<Slot>().item.efts[0];
                            eventData.pointerDrag.GetComponent<WeaponDrag>().previousParent.GetComponent<Slot>().item = transform.GetComponent<Slot>().item;
                            eventData.pointerDrag.transform.GetComponent<WeaponDrag>().previousParent.GetComponent<Slot>().UpdateSlotUI();
                            inven.RemoveWithoutServerItem(transform.GetComponent<Slot>().slotNum);
                            inven.AddWithoutServerItem(temp);
                            Debug.Log("�巡�׿��̺�5");
                        }
                    }
                }
            }
        }
    }
}