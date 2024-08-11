using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArchorAttack : BaseController
{
    [SerializeField]
    private BoxCollider Box;

    [SerializeField]
    private EnemyController controller;

    [SerializeField]
    private Stat enemyStat;

    [SerializeField]
    private Vector3 playerPosition;

    Vector3 playerPosition_Server;

    [SerializeField]
    private EnemySound enemySound;

    [SerializeField]
    private Transform shootTransfrom;

    [SerializeField]
    private GameObject arrow;

    public GameObject targetPlayer = null;
    

    public bool Delay = false;
    private bool Starting = false;

    public override void Init()
    {

    }

    public void Update()
    {
        if (!enemyStat.isDead)
        {
            if (targetPlayer != null && targetPlayer.gameObject.transform.root.GetComponent<PlayerStat>().isDead) 
            {
                targetPlayer = null;
            }

            if (controller.getHit())
            {
                Debug.Log("아야!");
                StopAllCoroutines();
                Delay = false;
                Starting = false;
                controller.isAttack = false;
            }
            else
            {
                if (targetPlayer != null && (Vector3.Distance(controller.transform.position, targetPlayer.gameObject.transform.position) <= 10f))
                {
                    if (!Delay)
                    {
                        if (NetPlayerManager.Instance.isHost)
                        {
                            StartCoroutine("reactivate");
                        }
                        else
                        {
                            StartCoroutine("reactivate_Server");
                        }
                    }
                }
                else 
                {
                    if (!Delay)
                    {
                        StopAllCoroutines();
                        Starting = false;
                        controller.isAttack = false;
                    }
                }
            }
        }
        else
        {
            StopAllCoroutines();
            Delay = false;
            Starting = false;
            Box.enabled = false;
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //        Debug.Log("stay");
    //}

    private void OnTriggerStay(Collider other)
    {
        //if (!enemyStat.isDead || !controller.getHit())
        {
            //playerPosition = other.gameObject.transform.position;
            //transform.root.forward = new Vector3(other.gameObject.transform.position.x - transform.root.position.x, 0, other.gameObject.transform.position.z - transform.root.position.z);
            //if (NetPlayerManager.Instance.isHost)
            //{
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("enter");
                if (targetPlayer==null)
                {
                    targetPlayer=other.gameObject;
                    //playerPosition = other.gameObject.transform.position;
                    //transform.root.forward = new Vector3(other.gameObject.transform.position.x - transform.root.position.x, 0, other.gameObject.transform.position.z - transform.root.position.z);
                    if(NetPlayerManager.Instance.isHost)
                    {
                        //C_MonsterAttack c_MonsterAttack = new C_MonsterAttack();
                        //c_MonsterAttack.monsterId = controller.monsterId;
                        //c_MonsterAttack.targetPlayerId = other.GetComponentInParent<PlayerManager>().PlayerId;
                        //NetPlayerManager.Instance.Session.Send(c_MonsterAttack.Write());
                    }
                }
                //else
                //{
                //    playerPosition = targetPlayer.transform.position;
                //    transform.root.forward = new Vector3(playerPosition.x - transform.root.position.x, 0, playerPosition.z - transform.root.position.z);
                //}
                
                //if (!Delay)
                //{
                //    if (NetPlayerManager.Instance.isHost)
                //    {
                //        StartCoroutine("reactivate");
                //        //C_MonsterAttack c_MonsterAttack = new C_MonsterAttack();
                //        //c_MonsterAttack.monsterId = controller.monsterId;
                //        //c_MonsterAttack.targetPlayerId = other.GetComponentInParent<PlayerManager>().PlayerId;
                //        //NetPlayerManager.Instance.Session.Send(c_MonsterAttack.Write());
                //    }
                //    else
                //    {
                //        StartCoroutine("reactivate_Server");
                //    }
                //}
            }
            //}
        }
    }

    public void MonsterAttack(int targetPlayerid)
    {
        Debug.Log("MonsterAttack서버 실행");
        NetPlayerManager.PlayerInfo target = null;
        if (NetPlayerManager.Instance._players.TryGetValue(targetPlayerid, out target))
        {
            //playerPosition_Server = target.rigid.gameObject.transform.position;
            //transform.root.forward = new Vector3(playerPosition_Server.x - transform.root.position.x, 0, playerPosition_Server.z - transform.root.position.z);
            //Quaternion rot = Quaternion.LookRotation(dir.normalized);
            //transform.rotation = rot;

            if (!Delay)
                StartCoroutine("reactivate_Server");
            return;
        }
        Debug.Log("타겟 캐릭터 못찾음!");
    }

    IEnumerator reactivate()
    {
        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true && !controller.getHit())
        {
            playerPosition = targetPlayer.transform.position;
            transform.root.forward = new Vector3(targetPlayer.gameObject.transform.position.x - transform.root.position.x, 0,
                targetPlayer.gameObject.transform.position.z - transform.root.position.z);
            controller.isAttack = true;
            Starting = false;
            State = Define.State.Attack;
            enemySound.AttackSound();
            yield return new WaitForSeconds(0.57f);
            GameObject shootarrow = Instantiate(arrow, shootTransfrom.position, Quaternion.identity);
            //shootarrow.GetComponent<Arrow>().shotVector = (playerPosition - transform.root.position).normalized;
            shootarrow.GetComponent<Arrow>().shotVector = transform.root.forward;
            shootarrow.GetComponent<Arrow>().Enemystat.Level = transform.root.GetComponent<Stat>().Level;
            shootarrow.GetComponent<Arrow>().Enemystat.Attack = transform.root.GetComponent<Stat>().Attack;
            shootarrow.SetActive(true);
            shootarrow.GetComponent<Arrow>().ShotArrow();
            yield return new WaitForSeconds(1.0f);

            if (controller.getNavSpeed() <= 0.3f)
            {
                State = Define.State.Idle;
                anim.CrossFade("Idle", 0.1f);
            }
            else if (controller.getNavSpeed() <= controller.speedRun)
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

    IEnumerator reactivate_Server()
    {

        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true && !controller.getHit())
        {
            playerPosition = targetPlayer.transform.position;
            transform.root.forward = new Vector3(targetPlayer.gameObject.transform.position.x - transform.root.position.x, 0,
                targetPlayer.gameObject.transform.position.z - transform.root.position.z);
            controller.isAttack = true;
            Starting = false;
            State = Define.State.Attack;
            enemySound.AttackSound();
            yield return new WaitForSeconds(0.57f);
            GameObject shootarrow = Instantiate(arrow, shootTransfrom.position, Quaternion.identity);
            shootarrow.GetComponent<Arrow>().shotVector = transform.root.forward;
            shootarrow.GetComponent<Arrow>().Enemystat.Level = transform.root.GetComponent<Stat>().Level;
            shootarrow.GetComponent<Arrow>().Enemystat.Attack = transform.root.GetComponent<Stat>().Attack;
            shootarrow.SetActive(true);
            shootarrow.GetComponent<Arrow>().ShotArrow();
            yield return new WaitForSeconds(1.0f);

            if (controller.moveSpeed <= 0.2f)
            {
                State = Define.State.Idle;
                anim.CrossFade("Idle", 0.1f);
            }
            else if (controller.moveSpeed <= controller.speedRun + 0.2f)
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
