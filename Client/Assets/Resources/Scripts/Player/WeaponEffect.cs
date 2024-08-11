using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffect : MonoBehaviour
{
    [SerializeField]
    PlayerManager playerManager;

    [SerializeField]
    private PlayerStat playerstat;

    [SerializeField]
    private GameObject DefaultEffect;

    [SerializeField]
    private GameObject Skill1;

    [SerializeField]
    private CapsuleCollider capsule;

    // Start is called before the first frame update
    void Start()
    {
        capsule.enabled = false;
        DefaultEffect.SetActive(false);
        Skill1.SetActive(false);
       // Manager.Input.WeaponEffect -= SkillEffect;
      //  Manager.Input.WeaponEffect += SkillEffect;
    }

    public void Init()
    {
        playerManager = GetComponentsInParent<PlayerManager>()[0];
        playerManager.WeaponEffect -= SkillEffect;
        playerManager.WeaponEffect += SkillEffect;
    }


    void SkillEffect()
    {
        //Debug.Log($"{playerManager.PlayerId}유저! 스킬이펙트 실행!");
        if (!playerstat.isDead)
        {
            if (playerManager.isAttack)
            {
                DefaultEffect.SetActive(true);
                Skill1.SetActive(false);
            }
            else
            {
                DefaultEffect.SetActive(false);
            }

            if (playerManager.isSkill)
            {
                DefaultEffect.SetActive(false);
                Skill1.SetActive(true);
            }
            else
            {
                Skill1.SetActive(false);
            }
        }
    }
}
