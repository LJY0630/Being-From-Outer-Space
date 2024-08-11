using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrcAttack : BaseController
{
    [SerializeField]
    private BoxCollider Box;

    [SerializeField]
    private BoxCollider swordCapsule1;

    [SerializeField]
    private BoxCollider swordCapsule2;

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
        swordCapsule1.enabled = false;
        swordCapsule2.enabled = false;
    }

    public void Update()
    {
        if (!enemyStat.isDead)
        {
            if (controller.getHit())
            {
                StopAllCoroutines();
                swordCapsule1.enabled = false;
                swordCapsule2.enabled = false;
                Delay = false;
                Starting = false;
                controller.isAttack = false;
            }
            else
            {

            }
        }
        else
        {
            StopAllCoroutines();
            swordCapsule1.enabled = false;
            swordCapsule2.enabled = false;
            Delay = false;
            Starting = false;
            Box.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enemyStat.isDead || !controller.getHit())
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
        if (Delay == true && Starting == true && !controller.getHit())
        {
            Starting = false;
            controller.isAttack = true;
            State = Define.State.Attack;
            yield return new WaitForSeconds(0.23f);
            enemySound.AttackSound();
            swordCapsule1.enabled = true;
            swordCapsule2.enabled = true;
            yield return new WaitForSeconds(0.27f);
            enemySound.AttackSound();
            yield return new WaitForSeconds(0.1f);
            swordCapsule1.enabled = false;
            swordCapsule2.enabled = false;
            yield return new WaitForSeconds(1.2f);
            if (controller.moveSpeed <= 0.2f)
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
            yield return new WaitForSeconds(0.1f);
            controller.isAttack = false;
            Delay = false;
        }
    }

    IEnumerator reactivate_Server()
    {
        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true && !controller.getHit())
        {
            Starting = false;
            controller.isAttack = true;
            State = Define.State.Attack;
            yield return new WaitForSeconds(0.23f);
            enemySound.AttackSound();
            swordCapsule1.enabled = true;
            swordCapsule2.enabled = true;
            yield return new WaitForSeconds(0.27f);
            enemySound.AttackSound();
            yield return new WaitForSeconds(0.1f);
            swordCapsule1.enabled = false;
            swordCapsule2.enabled = false;
            yield return new WaitForSeconds(0.4f);
            if (controller.moveSpeed <= 0.2f)
            {
                State = Define.State.Idle;
                anim.CrossFade("Idle", 0.1f);
            }
            else //if (controller.getNavSpeed() == controller.speedRun)
            {
                State = Define.State.Idle;
                State = Define.State.Running;
                anim.CrossFade("Run", 0.1f);
            }
            yield return new WaitForSeconds(0.1f);
            controller.isAttack = false;
            Delay = false;
        }
    }
}
