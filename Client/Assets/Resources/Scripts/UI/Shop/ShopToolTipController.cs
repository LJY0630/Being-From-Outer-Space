using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopToolTipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTipConsume consume;
    public ToolTipWeapon weapon;
    public ToolTipEquip equip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Item item = GetComponent<ShopSlot>().item;

        if (item != null)
        {
            if (item.itemType == ItemType.Consumables)
            {
                consume.gameObject.SetActive(true);
                weapon.gameObject.SetActive(false);
                equip.gameObject.SetActive(false);

                if (item.itemName.Contains("Health"))
                {
                    ItemHealingEft eft = (ItemHealingEft)item.efts[0];
                    consume.SetToolTip(item.itemName, item.sellcost, item.cost, eft.healingPoint);
                }
                else if (item.itemName.Contains("Mana"))
                {
                    ItemManaEft eft = (ItemManaEft)item.efts[0];
                    consume.SetToolTip(item.itemName, item.sellcost, item.cost, eft.manaPoint);
                }
            }
            else if (item.itemType == ItemType.Weapon)
            {
                consume.gameObject.SetActive(false);
                weapon.gameObject.SetActive(true);
                equip.gameObject.SetActive(false);

                ItemEffectWeapon eft = (ItemEffectWeapon)item.efts[0];
                weapon.SetToolTip(item.itemName, item.sellcost, item.cost, eft.addPower, eft.addMagic, eft.addHeal);

            }
            else if (item.itemType == ItemType.Equipment)
            {
                consume.gameObject.SetActive(false);
                weapon.gameObject.SetActive(false);
                equip.gameObject.SetActive(true);

                ItemEffectEquipment eft = (ItemEffectEquipment)item.efts[0];
                equip.SetToolTip(item.itemName, item.sellcost, item.cost, eft.addHp, eft.addMana, eft.addDef, eft.addPower, eft.addMagic, eft.addHeal);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        consume.gameObject.SetActive(false);
        weapon.gameObject.SetActive(false);
        equip.gameObject.SetActive(false);
    }

}
