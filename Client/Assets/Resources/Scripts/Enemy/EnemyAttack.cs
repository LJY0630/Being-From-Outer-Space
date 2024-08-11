using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : BaseController
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
    public override void Init() // 초기 상태 설정
    {
        swordCapsule.enabled = false;
    }

    public void Update()
    {
        if (!enemyStat.isDead) // 죽지 않았을 때
        {
            if (controller.getHit()) // 맞으면 공격 중지
            {
                StopAllCoroutines(); 
                swordCapsule.enabled = false;
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
            StopAllCoroutines(); // 죽어도 공격 중지
            swordCapsule.enabled = false;
            Delay = false;
            Starting = false;
            Box.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enemyStat.isDead || !controller.getHit()) 
        {
            if (NetPlayerManager.Instance.isHost) // 호스트면
            {
                if (other.gameObject.tag == "Player") // 죽지 않거나 맞지 않은 상태에 플레이어가 공격 거리에 들어옴
                {
                    if (!Delay)
                    {
                        StartCoroutine("reactivate");
                        if (NetPlayerManager.Instance.isHost) // 호스트면 때리고 다른 클라이언트에게도 동기화
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

    public void MonsterAttack() // 동기화 된 몬스터 공격 실행
    {
        Debug.Log("MonsterAttack서버 실행");
        StartCoroutine("reactivate_Server");
    }

    IEnumerator reactivate() // 몬스터 공격 작업
    {

        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true && !controller.getHit())
        {
            Starting = false;
            controller.isAttack = true;
            State = Define.State.Attack;
            enemySound.AttackSound();
            yield return new WaitForSeconds(0.57f);
            swordCapsule.enabled = true;
            yield return new WaitForSeconds(0.44f);
            swordCapsule.enabled = false;
            yield return new WaitForSeconds(0.7f);
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
            yield return new WaitForSeconds(0.15f);
            controller.isAttack = false;
            Delay = false;
        }
    }

    IEnumerator reactivate_Server() // 동기화 된 몬스터 공격 작업
    {

        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true && !controller.getHit())
        {
            Starting = false;
            controller.isAttack = true;
            State = Define.State.Attack;
            enemySound.AttackSound();
            yield return new WaitForSeconds(0.47f);
            swordCapsule.enabled = true;
            yield return new WaitForSeconds(0.34f);
            swordCapsule.enabled = false;
            yield return new WaitForSeconds(0.39f);
            if (controller.moveSpeed <= 0.2f)
            {
                State = Define.State.Idle;
                anim.CrossFade("Idle", 0.1f);
            }
            else //if (controller.moveSpeed == controller.speedRun)
            {
                State = Define.State.Idle;
                State = Define.State.Running;
                anim.CrossFade("Run", 0.1f);
            }
            yield return new WaitForSeconds(0.15f);
            controller.isAttack = false;
            Delay = false;
        }
    }
}
