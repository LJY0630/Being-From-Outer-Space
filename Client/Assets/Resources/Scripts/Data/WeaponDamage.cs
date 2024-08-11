using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{

    public float getFullAttackDmg() 
    {
        return transform.root.GetComponent<PlayerStat>().power + (float)(transform.root.GetComponent<PlayerStat>().magic / 2.0f) 
            + (float)(transform.root.GetComponent<PlayerStat>().heal / 3.0f) + transform.root.GetComponent<PlayerStat>().Attack;
    }

}
