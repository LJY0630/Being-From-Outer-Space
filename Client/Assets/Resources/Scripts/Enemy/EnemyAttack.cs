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
    public override void Init() // �ʱ� ���� ����
    {
        swordCapsule.enabled = false;
    }

    public void Update()
    {
        if (!enemyStat.isDead) // ���� �ʾ��� ��
        {
            if (controller.getHit()) // ������ ���� ����
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
            StopAllCoroutines(); // �׾ ���� ����
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
            if (NetPlayerManager.Instance.isHost) // ȣ��Ʈ��
            {
                if (other.gameObject.tag == "Player") // ���� �ʰų� ���� ���� ���¿� �÷��̾ ���� �Ÿ��� ����
                {
                    if (!Delay)
                    {
                        StartCoroutine("reactivate");
                        if (NetPlayerManager.Instance.isHost) // ȣ��Ʈ�� ������ �ٸ� Ŭ���̾�Ʈ���Ե� ����ȭ
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

    public void MonsterAttack() // ����ȭ �� ���� ���� ����
    {
        Debug.Log("MonsterAttack���� ����");
        StartCoroutine("reactivate_Server");
    }

    IEnumerator reactivate() // ���� ���� �۾�
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

    IEnumerator reactivate_Server() // ����ȭ �� ���� ���� �۾�
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
