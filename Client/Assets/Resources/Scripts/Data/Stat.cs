using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int level;
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float maxHp;
    [SerializeField]
    protected int attack;
    [SerializeField]
    protected int defense;
    [SerializeField]
    protected int exp;
    [SerializeField]
    protected int gold;

    public bool isDead = false;

    public int Level { get { return level; } set { level = value; } }
    public float Hp { get { return hp; } set { hp = value; } }
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public int Defense { get { return defense; } set { defense = value; } }
    public int GetExp { get { return exp; } set { exp = value; SendExpPacket(); } }

    void SendExpPacket() // 경험치 DB에 보냄
    {
        C_SendExp c_SendExp = new C_SendExp();
        c_SendExp.exp = exp;
        NetPlayerManager.Instance.Session.Send(c_SendExp.Write());
    }

    private void Start()
    {

    }

    public virtual void OnAttacked(Transform playerweapon) // 플레이어에게 공격을 맞았을 때 데미지 처리
    {
        if (playerweapon != null)
        {
            float other_attack = playerweapon.gameObject.transform.GetComponent<WeaponDamage>().getFullAttackDmg();
            float damage = Mathf.Max(0, other_attack - Defense);
            Debug.Log(damage);

            Debug.Log("OnAttacked들어옴");


            if (Hp > 0)
            {
                if (transform.root.GetComponent<Boss>() == null)
                {
                    Hp -= damage;
                    C_MonsterDamaged c_MonsterDamaged = new C_MonsterDamaged();
                    c_MonsterDamaged.playerId = NetPlayerManager.Instance._playerManager.PlayerId;
                    c_MonsterDamaged.monsterId = gameObject.GetComponent<EnemyController>().monsterId;
                    c_MonsterDamaged.damaged = (int)damage;
                    NetPlayerManager.Instance.Session.Send(c_MonsterDamaged.Write());
                }
                else
                {
                    if (playerweapon.TryGetComponent<IceBall>(out IceBall ice))
                    {
                        if (NetPlayerManager.Instance._playerManager.PlayerId
                                == ice.ownplayer.GetComponentInChildren<PlayerManager>().PlayerId)
                        {
                            Hp -= damage;
                        }
                    }
                    else if (playerweapon.TryGetComponent<Lighting>(out Lighting light))
                    {
                        if (NetPlayerManager.Instance._playerManager.PlayerId
                                == light.ownplayer.GetComponentInChildren<PlayerManager>().PlayerId)
                        {
                            Hp -= damage;
                        }
                    }
                    else
                    {
                        if (playerweapon.root.GetComponent<PlayerManager>().PlayerId ==
                        NetPlayerManager.Instance._playerManager.PlayerId)
                            Hp -= damage;
                    }
                    C_BossDamaged c_BossDamaged = new C_BossDamaged();
                    c_BossDamaged.damaged = (int)damage;


                    NetPlayerManager.Instance.Session.Send(c_BossDamaged.Write());
                }

            }

            if (Hp <= 0)
            {
                if (transform.root.GetComponent<Boss>() == null)
                {
                    Debug.Log("패킷 몬스터 데드");
                    C_MonsterDead c_MonsterDead = new C_MonsterDead();
                    c_MonsterDead.monsterId = gameObject.GetComponent<EnemyController>().monsterId;
                    c_MonsterDead.playerId = NetPlayerManager.Instance._playerManager.PlayerId;
                    c_MonsterDead.killPlayerId = gameObject.GetComponent<EnemyController>().hitPlayerId;
                    NetPlayerManager.Instance.Session.Send(c_MonsterDead.Write());
                }
                else
                {

                }
                Hp = 0;
                OnDead(playerweapon);
            }

        }
    }

    public void OnAttacked_Server(int damaged) // 호스트가 아닐 때 공격을 맞았을 때
    {
        //float damage = Mathf.Max(0, damaged - Defense);
        //Debug.Log(damaged);
        Hp -= damaged;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead_Server();
        }
    }

    public void OnDead_Server() // 죽음 처리
    {
        isDead = true;
    }

    protected virtual void OnDead(Transform playerWeapon) // 죽었을 때 경험치, 골드를 플레이어와 DB에게 전달, 관련 퀘스트가 있으면 퀘스트 처리
    {
        isDead = true;
        C_SendMoney sendMoney = new C_SendMoney();
        if (playerWeapon.TryGetComponent<IceBall>(out IceBall ice))
        {
            playerWeapon.GetComponent<IceBall>().ownplayer.GetComponent<PlayerStat>().GetExp += exp;
            playerWeapon.GetComponent<IceBall>().ownplayer.GetComponent<PlayerStat>().Gold += gold;

            if (gameObject.name.Contains("SkeletonWarrior") && ice.ownplayer.GetComponent<PlayerManager>().quest.questId == 102)
            {
                Debug.Log(playerWeapon.root.GetChild(1).name);
                ice.ownplayer.GetComponent<PlayerManager>().quest.skeletonkillCount += 1;
            }
            else if (gameObject.name.Contains("SkeletonArchor") && ice.ownplayer.GetComponent<PlayerManager>().quest.questId == 103)
            {
                Debug.Log(playerWeapon.root.GetChild(1).name);
                ice.ownplayer.GetComponent<PlayerManager>().quest.skeletonArchorkillCount += 1;
            }

            sendMoney.money = playerWeapon.GetComponent<IceBall>().ownplayer.GetComponent<PlayerStat>().Gold;
        }
        else if (playerWeapon.TryGetComponent<Lighting>(out Lighting light))
        {
            playerWeapon.GetComponent<Lighting>().ownplayer.GetComponent<PlayerStat>().GetExp += exp;
            playerWeapon.GetComponent<Lighting>().ownplayer.GetComponent<PlayerStat>().Gold += gold;

            if (gameObject.name.Contains("SkeletonWarrior") && light.ownplayer.GetComponentInChildren<PlayerManager>().quest.questId == 102)
            {
                Debug.Log(playerWeapon.root.GetChild(1).name);
                light.ownplayer.GetComponent<PlayerManager>().quest.skeletonkillCount += 1;
            }
            else if (gameObject.name.Contains("SkeletonArchor") && light.ownplayer.GetComponentInChildren<PlayerManager>().quest.questId == 103)
            {
                Debug.Log(playerWeapon.root.GetChild(1).name);
                light.ownplayer.GetComponent<PlayerManager>().quest.skeletonArchorkillCount += 1;
            }

            sendMoney.money = playerWeapon.GetComponent<Lighting>().ownplayer.GetComponent<PlayerStat>().Gold;
        }
        else
        {
            playerWeapon.root.gameObject.GetComponent<PlayerStat>().GetExp += exp;
            playerWeapon.root.gameObject.GetComponent<PlayerStat>().Gold += gold;

            if (gameObject.name.Contains("SkeletonWarrior") && playerWeapon.root.GetComponentInChildren<PlayerManager>().quest.questId == 102)
            {
                Debug.Log(playerWeapon.root.GetChild(1).name);
                playerWeapon.root.GetComponent<PlayerManager>().quest.skeletonkillCount += 1;
            }
            else if (gameObject.name.Contains("SkeletonArchor") && playerWeapon.root.GetComponentInChildren<PlayerManager>().quest.questId == 103)
            {
                Debug.Log(playerWeapon.root.GetChild(1).name);
                playerWeapon.root.GetComponent<PlayerManager>().quest.skeletonArchorkillCount += 1;
            }
            sendMoney.money = playerWeapon.root.gameObject.GetComponent<PlayerStat>().Gold;
        }

        NetPlayerManager.Instance.Session.Send(sendMoney.Write());
    }
}