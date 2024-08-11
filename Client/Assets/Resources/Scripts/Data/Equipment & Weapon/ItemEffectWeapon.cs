using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Weapon")]
public class ItemEffectWeapon : ItemEffect
{
    public int addPower = 0;
    public int addMagic = 0;
    public int addHeal = 0;

    public override bool ExecuteRole(Transform transform)
    {
        PlayerStat playerStat = transform.GetComponent<PlayerStat>();
        if (!playerStat.isDead)
        {
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
            playerStat.power -= addPower;
            playerStat.magic -= addMagic;
            playerStat.heal -= addHeal;
            playerStat.SetCurrentStat(playerStat.Level);
            return true;
        }
        return false;
    }
}
