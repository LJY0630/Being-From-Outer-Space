using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Mana")]
public class ItemManaEft : ItemEffect
{
    public int manaPoint;

    public override bool ExecuteRole(Transform transform)
    {
        PlayerStat playerStat = transform.GetComponent<PlayerStat>();

        if (!playerStat.isDead)
        {
            if (playerStat.Mp < playerStat.MaxMp)
            {
                C_SendPotionEat p = new C_SendPotionEat();
                p.potionType = 2;
                NetPlayerManager.Instance.Session.Send(p.Write());

                if (playerStat.Mp + manaPoint > playerStat.MaxMp)
                {
                    playerStat.Mp = playerStat.MaxMp;
                }
                else
                    playerStat.Mp += manaPoint;
                playerStat.transform.GetChild(1).GetChild(5).GetComponent<EffectManager>().ManaCo();
                return true;
            }
            return false;
        }
        return false;
    }
}
