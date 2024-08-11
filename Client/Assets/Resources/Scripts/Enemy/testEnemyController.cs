using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testEnemyController : BaseController
{
    [SerializeField]
    private Stat stat;

    [SerializeField]
    private EnemySound enemySound;

    public int monsterId;

    private Rigidbody rigid;
    private CapsuleCollider capsule;
    private Animator enemyAnim;
    private bool Delay = false;
    private bool Starting = false;
    private bool isHit = false;
    private bool Dying = false;
    public bool isAttack = false;

    [SerializeField]
    public NavMeshAgent nav;

    private float startWaitTime = 3;
    public float timeToRotate = 2;
    public float speedWalk = 20;
    public float speedRun = 60;

    public float viewRadius = 100;
    public float viewAngle = 120;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;

    public Vector3[] waypoints;
    int m_CurrentWaypointIndex;


    /// <summary>
    ///  서버 통신 관련 변수

    public float moveSpeed = 20;
    public Vector3 targetPos_server;
    public Vector3 targetRot_server;
    public Transform targetTransform_server;
    public bool isMoving = false;
    /// </summary>

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;
    Transform player;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_playerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    public override void Init()
    {
        rigid = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        enemyAnim = GetComponent<Animator>();

        nav.updateRotation = false;

        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;

        nav.isStopped = false;
        nav.speed = speedWalk;


        if (NetPlayerManager.Instance.isHost)
        {
            StartCoroutine("Monster_moveTime");
            nav.SetDestination(waypoints[m_CurrentWaypointIndex]);
        }
        else
            nav.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 호스트 일 때 동작
        if (NetPlayerManager.Instance.isHost)
        {
            nav.enabled = true;
            if (!stat.isDead && !isHit && !isAttack)
            {
                if (player != null)
                {
                    if (player.root.GetComponent<PlayerStat>().isDead)
                    {
                        player = null;
                        m_PlayerPosition = Vector3.zero;
                    }
                }
                EnviromentView();
                if ((nav.velocity != Vector3.zero) && !(transform.position.x == nav.destination.x && transform.position.z == nav.destination.z))
                {
                    Vector2 forward = new Vector2(transform.position.z, transform.position.x);
                    Vector2 steeringTarget = new Vector2(nav.steeringTarget.z, nav.steeringTarget.x);

                    Vector2 dir = steeringTarget - forward;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    transform.eulerAngles = Vector3.up * angle;
                }

                if (!m_IsPatrol)
                {

                    Chasing();
                }
                else
                {
                    Patroling();
                    //EnviromentView();
                }

                OnAnimatorMove();
            }
            else if (stat.isDead)
            {
                if (!Dying)
                {
                    StopAllCoroutines();
                    StartCoroutine("Dead");
                }
            }
            else
            {
                if (!isHit)
                {
                    if (m_IsPatrol)
                    {
                        Patroling();
                    }
                    else
                    {
                        if (!isAttack)
                        {
                            Chasing();
                        }
                        else
                        {
                            nav.speed = 0.0f;
                        }
                    }
                }
            }
        }

        // host가 아닐때 동작
        else
        {
            OnAnimatorMove_Server();
        }
    }


    private void OnAnimatorMove_Server()
    {

        if (moveSpeed == 0.0f)
        {
            State = Define.State.Idle;
        }
        else if (moveSpeed == speedWalk)
        {
            State = Define.State.Walk;
        }
        else if (moveSpeed == speedRun)
        {
            State = Define.State.Running;
        }

    }
    #region 호스트관련 함수
    // 추적하다
    private void Chasing()
    {
        if (player != null)
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;

            if (!m_CaughtPlayer)
            {
                Move(speedRun);
                moveSpeed = speedRun;
                //Debug.Log("3");
                //Debug.Log(nav.remainingDistance);
                //Debug.Log(nav.stoppingDistance);
                nav.SetDestination(m_PlayerPosition);
            }
            if (nav.remainingDistance <= nav.stoppingDistance)
            {
                if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, player.position) >= 5f)
                {
                    m_IsPatrol = true;
                    m_PlayerNear = false;
                    Move(speedWalk);
                    moveSpeed = speedWalk;
                    m_TimeToRotate = timeToRotate;
                    m_WaitTime = startWaitTime;
                    nav.SetDestination(waypoints[m_CurrentWaypointIndex]);
                    Debug.Log("2");
                }
                else
                {
                    if (Vector3.Distance(transform.position, player.position) >= 5f)
                    {
                        Debug.Log("1");
                        Stop();
                    }
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
        else
        {
            m_IsPatrol = true;
            m_PlayerNear = false;
            Move(speedWalk);
            moveSpeed = speedWalk;
            m_TimeToRotate = timeToRotate;
            m_WaitTime = startWaitTime;
            nav.SetDestination(waypoints[m_CurrentWaypointIndex]);
        }
    }



    private void OnAnimatorMove()
    {
        if (!isAttack && !isHit)
        {
            if (nav.speed == 0.0f)
            {
                State = Define.State.Idle;
            }
            else if (nav.speed == speedWalk)
            {
                State = Define.State.Walk;
            }
            else if (nav.speed == speedRun)
            {
                State = Define.State.Running;
            }
        }
    }

    // 순찰
    private void Patroling()
    {
        // 가까이 있을 때
        if (m_PlayerNear)
        {
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                moveSpeed = speedWalk;
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            playerLastPosition = Vector3.zero;
            player = null;
            nav.SetDestination(waypoints[m_CurrentWaypointIndex]);
            if (nav.remainingDistance <= nav.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    Move(speedWalk);
                    moveSpeed = speedWalk;
                    NextPoint();
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
            else
            {
                Move(speedWalk);
                moveSpeed = speedWalk;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (NetPlayerManager.Instance.isHost)
        {
            if (!stat.isDead)
            {
                if (other.gameObject.tag == "PowerWeapon" || other.gameObject.tag == "MagicWeapon" || other.gameObject.tag == "HealWeapon")
                {
                    if (!Delay)
                    {
                        StopAllCoroutines();
                        stat.OnAttacked(other.gameObject.transform);
                        StartCoroutine("stopHit");

                    }
                }
            }
        }
    }

    public void Damaged(int damaged)
    {
        if (!stat.isDead)
        {
            if (!Delay)
            {
                StopAllCoroutines();
                //Debug.Log(damaged);
                //stat.Hp -= damaged;
                stat.OnAttacked_Server(damaged);
                if (stat.Hp <= 0)
                {
                    stat.Hp = 0;
                    stat.OnDead_Server();
                    DiedMonster_Server();
                }
                StartCoroutine("stopHit");
            }
        }
    }

    public void DiedMonster_Server()
    {
        StopAllCoroutines();
        StartCoroutine("Dead_Server");
    }

    IEnumerator stopHit()
    {
        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true)
        {
            isHit = true;
            isAttack = false;
            Starting = false;
            enemySound.HitSound();
            State = Define.State.Idle;
            State = Define.State.Hit;
            yield return new WaitForSeconds(0.5f);
            State = Define.State.Idle;
            isHit = false;
            enemyAnim.CrossFade("Idle", 0.1f);
            Debug.Log("Hit");
            Delay = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    public bool getHit()
    {
        return isHit;
    }

    void Move(float speed)
    {
        if (!isAttack)
        {
            nav.speed = speed;
            nav.isStopped = false;
        }
        else
        {
            nav.speed = 0.0f;
        }
    }

    void Stop()
    {
        nav.speed = 0.0f;
        nav.isStopped = true;
    }

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        nav.SetDestination(waypoints[m_CurrentWaypointIndex]);
    }

    void LookingPlayer(Vector3 player)
    {
        nav.SetDestination(player);
        Debug.Log("Looking");

        if (Vector3.Distance(transform.position, player) <= 0.3f)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                moveSpeed = speedWalk;
                nav.SetDestination(waypoints[m_CurrentWaypointIndex]);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            if (player == null)
            {
                player = playerInRange[i].transform;
            }
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;
                    m_IsPatrol = false;
                }
                else
                {
                    m_playerInRange = false;
                }
            }
            // 범위 밖이면
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                m_playerInRange = false;
            }
            if (m_playerInRange)
            {
                // 플레이어 세팅
                m_PlayerPosition = player.transform.position;
            }
        }
    }

    public float getNavSpeed()
    {
        return nav.speed;
    }

    public void setNavSpeed(float speed)
    {
        nav.speed = speed;
    }

    public void setNavStop(bool stop)
    {
        nav.isStopped = stop;
    }

    IEnumerator Dead()
    {
        Dying = true;
        enemySound.DieSound();
        Stop();
        State = Define.State.Idle;
        State = Define.State.Die;
        gameObject.layer = 8;

        yield return new WaitForSeconds(2.4f);
        ItemDataBase.instance.GetItem(transform.position);
        if (transform.gameObject.name.Contains("Warrior"))
        {
            SpawnManager.instance.currentWCount--;
        }
        else if (transform.gameObject.name.Contains("Archor"))
        {
            SpawnManager.instance.currentACount--;
        }
        SpawnManager.instance.monsterList.Remove(monsterId);
        Destroy(gameObject);
    }

    IEnumerator Dead_Server()
    {
        Dying = true;
        enemySound.DieSound();
        //Stop();
        State = Define.State.Idle;
        State = Define.State.Die;
        gameObject.layer = 8;

        yield return new WaitForSeconds(2.4f);
        if (transform.gameObject.name.Contains("Warrior"))
        {
            SpawnManager.instance.currentWCount--;
        }
        else if (transform.gameObject.name.Contains("Archor"))
        {
            SpawnManager.instance.currentACount--;
        }
        SpawnManager.instance.monsterList.Remove(monsterId);
        Destroy(gameObject);
    }

    IEnumerator Monster_moveTime()
    {
        // 1초에 4번 실행되게끔
        yield return new WaitForSeconds(0.25f);
        C_MonsterMove c_MonsterMove = new C_MonsterMove();
        c_MonsterMove.monsterId = monsterId;
        c_MonsterMove.posX = rigid.gameObject.transform.position.x;
        c_MonsterMove.posY = rigid.gameObject.transform.position.y;
        c_MonsterMove.posZ = rigid.gameObject.transform.position.z;
        // isMoving은 필요하면 만들자.
        c_MonsterMove.isMoving = (moveSpeed != 0) ? true : false;
        c_MonsterMove.directionX = 0;
        c_MonsterMove.directionY = (float)rigid.transform.rotation.y;
        c_MonsterMove.directionZ = 0;
        c_MonsterMove.moveSpeed = moveSpeed;
        NetPlayerManager.Instance.Session.Send(c_MonsterMove.Write());
        StartCoroutine("Monster_moveTime");
    }
    #endregion
}