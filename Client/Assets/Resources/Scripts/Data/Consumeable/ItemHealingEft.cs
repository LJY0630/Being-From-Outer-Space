using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Health")]
public class ItemHealingEft : ItemEffect
{
    public int healingPoint;

    public override bool ExecuteRole(Transform transform)
    {
        PlayerStat playerStat = transform.GetComponent<PlayerStat>();

        if (!playerStat.isDead)
        {
            if (playerStat.Hp < playerStat.MaxHp)
            {
                C_SendPotionEat p = new C_SendPotionEat();
                p.potionType = 1;
                NetPlayerManager.Instance.Session.Send(p.Write());
                if (playerStat.Hp + healingPoint > playerStat.MaxHp)
                {
                    playerStat.Hp = playerStat.MaxHp;
                }
                else
                    playerStat.Hp += healingPoint;
                playerStat.transform.GetChild(1).GetChild(5).GetComponent<EffectManager>().HealCo();
                
                return true;
            }
            return false;
        }
        return false;
    }
}
