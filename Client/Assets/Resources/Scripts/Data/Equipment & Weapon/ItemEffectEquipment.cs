using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Equipment")]
public class ItemEffectEquipment : ItemEffect
{
    public int itemcode = 1;

    public float addHp = 0;
    public int addMana = 0;
    public int addDef = 0;
    public int addPower = 0;
    public int addMagic = 0;
    public int addHeal = 0;
    public override bool ExecuteRole(Transform transform)
    {
        PlayerStat playerStat = transform.GetComponent<PlayerStat>();
        if (!playerStat.isDead)
        {
            playerStat.addheath += addHp;
            playerStat.addmana += addMana;
            playerStat.adddefense += addDef;
            playerStat.power += addPower;
            playerStat.magic += addMagic;
            playerStat.heal += addHeal;
            playerStat.SetCurrentStat(playerStat.Level);
            return true;
        }
        return false;
    }

    public bool UnEquip(Transform transform)
    {
        PlayerStat playerStat = transform.GetComponent<PlayerStat>();
        if (!playerStat.isDead)
        {
            playerStat.addheath -= addHp;
            playerStat.addmana -= addMana;
            playerStat.adddefense -= addDef;
            playerStat.power -= addPower;
            playerStat.magic -= addMagic;
            playerStat.heal -= addHeal;
            playerStat.SetCurrentStat(playerStat.Level);
            return true;
        }
        return false;
    }
}
