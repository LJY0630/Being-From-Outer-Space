using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrgeAttack : BaseController
{
    [SerializeField]
    private BoxCollider Box;

    [SerializeField]
    private CapsuleCollider swordCapsule;

    [SerializeField]
    private EnemyController controller;

    [SerializeField]
    private Stat enemyStat;

    [SerializeField]
    private EnemySound enemySound;

    private bool Delay = false;
    private bool Starting = false;

    // Start is called before the first frame update
    public override void Init()
    {
        swordCapsule.enabled = false;
    }

    public void Update()
    {
        if (!enemyStat.isDead)
        {
            
        }
        else
        {
            StopAllCoroutines();
            swordCapsule.enabled = false;
            Delay = false;
            Starting = false;
            Box.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enemyStat.isDead)
        {
            if (NetPlayerManager.Instance.isHost)
            {
                if (other.gameObject.tag == "Player")
                {
                    if (!Delay)
                    {
                        StartCoroutine("reactivate");
                        if (NetPlayerManager.Instance.isHost)
                        {
                            C_MonsterAttack c_MonsterAttack = new C_MonsterAttack();
                            c_MonsterAttack.monsterId = controller.monsterId;
                            NetPlayerManager.Instance.Session.Send(c_MonsterAttack.Write());
                        }
                    }
                }
            }

        }
    }

    public void MonsterAttack()
    {
        Debug.Log("MonsterAttack서버 실행");
        StartCoroutine("reactivate_Server");
    }

    IEnumerator reactivate()
    {
        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true)
        {
            Starting = false;
            controller.isAttack = true;
            State = Define.State.Attack;
            yield return new WaitForSeconds(1.12f);
            enemySound.AttackSound();
            swordCapsule.enabled = true;
            yield return new WaitForSeconds(0.17f);
            swordCapsule.enabled = false;
            yield return new WaitForSeconds(1.16f);
            if (controller.getNavSpeed() == 0.0f)
            {
                State = Define.State.Idle;
                anim.CrossFade("Idle", 0.1f);
            }
            else if (controller.getNavSpeed() == controller.speedRun)
            {
                State = Define.State.Idle;
                State = Define.State.Running;
                anim.CrossFade("Run", 0.1f);
            }
            else
            {
                State = Define.State.Idle;
                anim.CrossFade("Idle", 0.1f);
            }
            yield return new WaitForSeconds(0.15f);
            controller.isAttack = false;
            Delay = false;
        }
    }

    IEnumerator reactivate_Server()
    {
        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true)
        {
            Starting = false;
            controller.isAttack = true;
            State = Define.State.Attack;
            yield return new WaitForSeconds(1.12f);
            enemySound.AttackSound();
            swordCapsule.enabled = true;
            yield return new WaitForSeconds(0.17f);
            swordCapsule.enabled = false;
            yield return new WaitForSeconds(1.16f);
            if (controller.getNavSpeed() == 0.0f)
            {
                State = Define.State.Idle;
                anim.CrossFade("Idle", 0.1f);
            }
            else if (controller.getNavSpeed() == controller.speedRun)
            {
                State = Define.State.Idle;
                State = Define.State.Running;
                anim.CrossFade("Run", 0.1f);
            }
            else
            {
                State = Define.State.Idle;
                anim.CrossFade("Idle", 0.1f);
            }
            yield return new WaitForSeconds(0.15f);
            controller.isAttack = false;
            Delay = false;
        }
    }
}
