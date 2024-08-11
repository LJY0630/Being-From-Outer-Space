using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : BaseController
{

    //[SerializeField]
    //private PlayerController player;

    [SerializeField]
    private PlayerStat stat;

    //[SerializeField]
    //private PlayerSound playerSound;

    [SerializeField]
    private EffectManager Eff;

    private CapsuleCollider capsule;

    private bool Delay = false;
    private bool Starting = false;
    private bool isheal = false;
    private bool isUp = false;
    private float heal = 0;

    private int OriginAttack = 0;
    private int OriginDef = 0;

    public bool isHit = false;

    public override void Init()
    {
        capsule = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!stat.isDead)
        {
            if (other.gameObject.tag == "EnemyWeapon")
            {
                // 만약 스킬 시전 중이었다면 데미지만 입고 hit Sound만 나게하기
                //if(gameObject.transform.root.GetComponent<PlayerManager>().isAttack==true||
                //    gameObject.transform.root.GetComponent<PlayerManager>().isSkill==true)
                //{
                //    if (!Delay)
                //    {
                //        stat.OnAttacked(other.gameObject.transform);
                //        //DamageAnimation();
                //        StartCoroutine("stopHitSkill");
                //    }
                //}
                //else
                //{
                    if (!Delay)
                    {
                        stat.OnAttacked(other.gameObject.transform);
                        DamageAnimation();
                    }
                //}
                
            }
            else if (other.gameObject.tag == "HealingArea")
            {
                if (other.TryGetComponent<HealingCircle>(out HealingCircle circle))
                {
                    if (other.GetComponent<HealingCircle>().healing != 0)
                    {
                        heal = other.GetComponent<HealingCircle>().healing;
                        if (!isheal)
                        {
                            StartCoroutine("Heal");
                        }
                    }
                }
            }
            else if (other.gameObject.tag == "UpHealingArea")
            {
                if (other.TryGetComponent<UpHealingCircle>(out UpHealingCircle circle))
                {
                    if (other.GetComponent<UpHealingCircle>().healing != 0)
                    {
                        heal = other.GetComponent<UpHealingCircle>().healing;
                        if (!isheal)
                        {
                            StartCoroutine("UpHeal");
                        }
                        if (!isUp)
                        {
                            OriginAttack = transform.root.GetComponent<PlayerStat>().Attack;
                            OriginDef = transform.root.GetComponent<PlayerStat>().Defense;
                            StartCoroutine("UpStat");
                        }
                    }
                }
            }
            else if (other.gameObject.tag == "Chest") 
            {
              NetPlayerManager.Instance._playerManager.quest.chestCount += 1;
              other.gameObject.SetActive(false);
            }
        }
        else 
        {
            StopAllCoroutines();
            isHit = false;
            Delay = false;
            Starting = false;
            isheal = false;
            isUp = false;
             heal = 0;

            if (OriginAttack != 0 && OriginAttack != transform.root.GetComponent<PlayerStat>().Attack) 
            {
                transform.root.GetComponent<PlayerStat>().Attack -= (int)(heal / 2);
                OriginAttack = 0;
            }

            if (OriginDef != 0 && OriginDef != transform.root.GetComponent<PlayerStat>().Defense) 
            {
                transform.root.GetComponent<PlayerStat>().Defense -= (int)(heal / 2);
                OriginDef = 0;
            }
            //gameObject.transform.root.GetComponent<PlayerManager>().weaponDamage.gameObject.SetActive(false);
            Eff.TurnHeal(false);
            Eff.UpHealEft(false);
        }
    }

    IEnumerator stopHit()
    {
        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true)
        {
            Debug.Log("Hit");
            isHit = true;
            sound.HitSound();
            Starting = false;
            State = Define.State.Idle;
            State = Define.State.Hit;
            yield return new WaitForSeconds(1.233f);
            State = Define.State.Idle;
            isHit = false;
            anim.CrossFade("Idle", 0.1f);
            Delay = false;
            State = Define.State.Idle;
        }
        //isHit = false;
        //Delay = false;
        //anim.CrossFade("Idle", 0.1f);
    }

    IEnumerator stopHitSkill()
    {
        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true)
        {
            Debug.Log("Hit");
            isHit = true;
            sound.HitSound();
            Starting = false;
            //State = Define.State.Idle;
            //State = Define.State.Hit;
            yield return new WaitForSeconds(1.233f);
            //State = Define.State.Idle;
            isHit = false;
            //anim.CrossFade("Idle", 0.1f);
            Delay = false;
            //State = Define.State.Idle;
        }
    }

    public void SetStopHit()
    {
        Starting = false;
        State = Define.State.Idle;
        isHit = false;
        anim.CrossFade("Idle", 0.1f);
        Delay = false;
        State = Define.State.Idle;
    }

    IEnumerator Heal() 
    {
        isheal = true;
        if (transform.root.GetComponent<PlayerStat>().Hp < transform.root.GetComponent<PlayerStat>().MaxHp)
        {
            if (transform.root.GetComponent<PlayerStat>().Hp + heal > transform.root.GetComponent<PlayerStat>().MaxHp)
            {
                transform.root.GetComponent<PlayerStat>().Hp = transform.root.GetComponent<PlayerStat>().MaxHp;
            }
            else
                transform.root.GetComponent<PlayerStat>().Hp += heal;
            Eff.TurnHeal(true);
        }
        yield return new WaitForSeconds(1.0f);
        Eff.TurnHeal(false);
        isheal = false;
    }

    IEnumerator UpHeal()
    {
        isheal = true;
        if (transform.root.GetComponent<PlayerStat>().Hp < transform.root.GetComponent<PlayerStat>().MaxHp)
        {
            if (transform.root.GetComponent<PlayerStat>().Hp + heal > transform.root.GetComponent<PlayerStat>().MaxHp)
            {
                transform.root.GetComponent<PlayerStat>().Hp = transform.root.GetComponent<PlayerStat>().MaxHp;
            }
            else
                transform.root.GetComponent<PlayerStat>().Hp += heal;
            Eff.UpHealEft(true);
        }

        if (transform.root.GetComponent<PlayerStat>().Mp < transform.root.GetComponent<PlayerStat>().MaxMp)
        {
            if (transform.root.GetComponent<PlayerStat>().Mp + heal > transform.root.GetComponent<PlayerStat>().MaxMp)
            {
                transform.root.GetComponent<PlayerStat>().Mp = transform.root.GetComponent<PlayerStat>().MaxMp;
            }
            else
                transform.root.GetComponent<PlayerStat>().Mp += (int)heal;
            Eff.UpHealEft(true);
        }
        yield return new WaitForSeconds(1.0f);
        isheal = false;
    }

    IEnumerator UpStat() 
    {
        isUp = true;
        transform.root.GetComponent<PlayerStat>().Attack += (int)(heal / 2);
        transform.root.GetComponent<PlayerStat>().Defense += (int)(heal / 2);
        yield return new WaitForSeconds(3.0f);
        transform.root.GetComponent<PlayerStat>().Attack -= (int)(heal / 2);
        transform.root.GetComponent<PlayerStat>().Defense -= (int)(heal / 2);
        Eff.UpHealEft(false);
        OriginAttack = 0;
        OriginDef = 0;
        isUp = false;
    }

    public void DamageAnimation() 
    {
        StartCoroutine("stopHit");
    }
}
