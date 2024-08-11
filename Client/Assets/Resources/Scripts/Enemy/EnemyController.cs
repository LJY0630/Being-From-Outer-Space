using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : BaseController
{
    [SerializeField]
    private Stat stat;

    [SerializeField]
    private EnemySound enemySound;

    public int monsterId;

    public bool isPlayingMoveTime = false;

    private Rigidbody rigid;
    private CapsuleCollider capsule;
    private Animator enemyAnim;
    private bool Delay = false;
    private bool Starting = false;
    [SerializeField]
    private bool isHit = false;
    private bool Dying = false;
    public bool isAttack = false;
    public bool isStay = false;

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

    public int hitPlayerId = 0;

    /// <summary>
    ///  서버 통신 관련 변수

    public float moveSpeed = 20;
    public float recvMoveSpeed = 10;
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

    public override void Init() // 초기 동기화, 컴포넌트 연결
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
    void Update() // 몬스터 상태 머신이 돌아가는 부분
    {
        // 호스트 일 때 동작
        if (NetPlayerManager.Instance.isHost)
        {
            nav.enabled = true;
            if (!stat.isDead && !isHit && !isAttack) // 죽거나 공격을 당하거나 때리지 않았을 때만 작동
            {
                if (player != null)
                {
                    if (player.root.GetComponent<PlayerStat>().isDead) // 죽으면 추격 중지
                    {
                        player = null;
                        m_PlayerPosition = Vector3.zero;
                    }
                }
                EnviromentView(); // 플레이어가 거리에 있는지 판단
                if ((nav.velocity != Vector3.zero) && !(transform.position.x == nav.destination.x && transform.position.z == nav.destination.z)) // 몬스터가 보는 방향 설정
                {
                    Vector2 forward = new Vector2(transform.position.z, transform.position.x);
                    Vector2 steeringTarget = new Vector2(nav.steeringTarget.z, nav.steeringTarget.x);

                    Vector2 dir = steeringTarget - forward;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    transform.eulerAngles = Vector3.up * angle;
                }

                if (!m_IsPatrol) // 추격
                {

                    Chasing();
                }
                else // 순찰
                {
                    Patroling();
                    //EnviromentView();
                }
            }
            else if (stat.isDead) // 죽었을 때
            {
                if (!Dying) // 죽었을 때 한번 작동함
                {
                    StopAllCoroutines();
                    StartCoroutine("Dead");
                }
            }
            // 죽지 않았지만 맞거나 공격일때
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
            OnAnimatorMove(); // 움직일 때 애니메이션
        }

        // host가 아닐때 동작(동기화를 받아 움직임)
        else
        {
            OnAnimatorMove_Server();
        }
    }


    private void OnAnimatorMove_Server() // 움직임 애니매이션 동기화
    {

        if (recvMoveSpeed <= 0.2f)
        {
            //enemyAnim.SetBool("Walk", false);
            //enemyAnim.SetBool("Run", false);
            Debug.Log("아더몬스터 idle");
            State = Define.State.Idle;
        }
        else if (recvMoveSpeed <= speedWalk + 0.2f)
        {
            Debug.Log("아더몬스터 walk");
            State = Define.State.Walk;
        }
        else //if (recvMoveSpeed == speedRun)
        {
            Debug.Log("아더몬스터 run");
            State = Define.State.Running;
        }

    }
    #region 호스트관련 함수
    // 추적하다
    private void Chasing()
    {
        if (isStay)
            return;
        if (player != null)
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;

            if (!m_CaughtPlayer)
            {
                Move(speedRun);
                moveSpeed = speedRun;
                nav.SetDestination(m_PlayerPosition);
            }
            if ((nav.remainingDistance <= nav.stoppingDistance))
            {
                if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, player.position) >= viewRadius)
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
                    if (Vector3.Distance(transform.position, player.position) >= viewRadius)
                    {
                        Debug.Log("1");
                        Stop();
                        moveSpeed = 0;
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



    private void OnAnimatorMove() // 움직임 상태에서 애니메이션
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
                moveSpeed = 0;
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
                    moveSpeed = 0;
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
            if (!stat.isDead)
            {
                if (other.gameObject.tag == "PowerWeapon" || other.gameObject.tag == "MagicWeapon" || other.gameObject.tag == "HealWeapon") // 무기에 맞음
                {

                    if (other.TryGetComponent<IceBall>(out IceBall ice))
                    {
                        if (NetPlayerManager.Instance._playerManager.PlayerId
                                == ice.ownplayer.root.GetComponent<PlayerManager>().PlayerId)
                        {
                            if (!Delay)
                            {
                                //StopAllCoroutines();
                                stat.OnAttacked(other.gameObject.transform);
                                hitPlayerId = ice.ownplayer.GetComponent<PlayerManager>().PlayerId;
                                StartCoroutine("stopHit");
                        }
                    }
                    }
                    else if (other.TryGetComponent<Lighting>(out Lighting light))
                    {
                        if (NetPlayerManager.Instance._playerManager.PlayerId
                                == light.ownplayer.root.GetComponent<PlayerManager>().PlayerId)
                        {
                            if (!Delay)
                            {
                                //StopAllCoroutines();
                                stat.OnAttacked(other.gameObject.transform);
                                hitPlayerId = light.ownplayer.GetComponent<PlayerManager>().PlayerId;
                                StartCoroutine("stopHit");
                        }
                    }
                    }
                    else
                    {
                        if (NetPlayerManager.Instance._playerManager.PlayerId
                            == other.transform.root.GetComponent<PlayerManager>().PlayerId)
                        {
                            if (!Delay)
                            {
                                //StopAllCoroutines();
                                stat.OnAttacked(other.gameObject.transform);
                                hitPlayerId = other.transform.root.GetComponentInChildren<PlayerManager>().PlayerId;
                                StartCoroutine("stopHit");
                            }
                        }
                    }
                }
            }
    }

    public void Damaged(int damaged) // 데미지 처리
    {
        if (!stat.isDead)
        {
            if (!Delay)
            {
                stat.OnAttacked_Server(damaged);
                if (stat.Hp <= 0)
                {
                    stat.Hp = 0;
                    stat.OnDead_Server();
                    DiedMonster_Server();
                }
                StartCoroutine("stopHit_Server");
            }
        }
    }

    public void DiedMonster_Server() // 죽음 처리
    {
        StopAllCoroutines();
        StartCoroutine("Dead_Server");
    }

    IEnumerator stopHit() // 맞았을 때 일어나는 작업
    {
        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true)
        {
            isHit = true;
            if (!transform.gameObject.name.Contains("Ogre"))
            {
                isAttack = false;
            }
            Starting = false;
            enemySound.HitSound();
            if (!transform.gameObject.name.Contains("Ogre"))
            {
                State = Define.State.Idle;
                State = Define.State.Hit;
                yield return new WaitForSeconds(0.65f);
                State = Define.State.Idle;
                isHit = false;
                enemyAnim.CrossFade("Idle", 0.1f);
            }
            else 
            {
                yield return new WaitForSeconds(0.6f);
                isHit = false;
            }
            Debug.Log("Hit");
            Delay = false;
           // StartCoroutine("Monster_moveTime");
        }
    }

    IEnumerator stopHit_Server() // 호스트가 아닐때 맞았을 때 동기화 작업
    {
        Delay = true;
        Starting = true;
        if (Delay == true && Starting == true)
        {
            isHit = true;
            if (!transform.gameObject.name.Contains("Ogre"))
            {
                isAttack = false;
            }
            Starting = false;
            enemySound.HitSound();
            if (!transform.gameObject.name.Contains("Ogre"))
            {
                State = Define.State.Idle;
                State = Define.State.Hit;
                yield return new WaitForSeconds(0.65f);
                State = Define.State.Idle;
                isHit = false;
                enemyAnim.CrossFade("Idle", 0.1f);
            }
            else
            {
                yield return new WaitForSeconds(0.6f);
                isHit = false;
            }
            Debug.Log("Hit");
            Delay = false;
            // StartCoroutine("Monster_moveTime");
        }
    }

    private void OnCollisionStay(Collision collision) // 플레이어한테 밀려나지 않게
    {
        if (isStay==false&&collision.gameObject.tag == "Player")
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            isStay = true;
        }
    }
    private void OnCollisionExit(Collision collision) // 플레이어가 사라지면 밀려나지 않는 설정 취소
    {
        if (isStay==true&&collision.gameObject.tag == "Player")
            isStay = false;
    }

    public bool getHit() // 맞음 판단 돌려받기
    {
        return isHit;
    }

    void Move(float speed) // 움직임
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

    void Stop() // 움직임 멈춤
    {
        nav.speed = 0.0f;
        nav.isStopped = true;
    }

    public void NextPoint() // 다음 순찰 포인트
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        nav.SetDestination(waypoints[m_CurrentWaypointIndex]);
    }

    void LookingPlayer(Vector3 player) // 순찰
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
                moveSpeed = 0;
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView() // 시야에 플레이어 있는지 판단 후 추격, 순찰 판단
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

    public float getNavSpeed() // 움작임 속도 반환
    {
        return nav.speed;
    }

    public void setNavSpeed(float speed) // 움직임 속도 설정
    {
        nav.speed = speed;
    }

    public void setNavStop(bool stop) // 길찾기 멈춤, 재개 설정
    {
        nav.isStopped = stop;
    }

    IEnumerator Dead() // 죽었을 때 판단 후 일어나는 작업
    {
        Dying = true;
        enemySound.DieSound();
        Stop();
        moveSpeed = 0;
        State = Define.State.Idle;
        State = Define.State.Die;
        gameObject.layer = 8;

        if (transform.gameObject.name.Contains("ork"))
        {
            yield return new WaitForSeconds(3.0f);
        }
        else if(transform.gameObject.name.Contains("Warrior") || transform.gameObject.name.Contains("Archor"))
        {
            yield return new WaitForSeconds(2.4f);
        }
        else if (transform.gameObject.name.Contains("Ogre"))
        {
            yield return new WaitForSeconds(4.2f);
        }

        if (hitPlayerId==NetPlayerManager.Instance._playerManager.PlayerId)
            ItemDataBase.instance.GetItem(transform.position);

        if (transform.gameObject.name.Contains("Warrior"))
        {
            SpawnManager.instance.currentWCount--;
        }
        else if (transform.gameObject.name.Contains("Archor"))
        {
            SpawnManager.instance.currentACount--;
        }
        else if (transform.gameObject.name.Contains("ork")) 
        {
            SpawnManager.instance.currentOCount--;
        }
        else if (transform.gameObject.name.Contains("Ogre"))
        {
            SpawnManager.instance.currentOgCount--;
        }

        SpawnManager.instance.monsterList.Remove(monsterId);
        Destroy(gameObject);
    }

    IEnumerator Dead_Server() // 호스트가 아닐 때 일어나는 죽음 작업
    {
        stat.Hp = 0;
        Dying = true;
        enemySound.DieSound();
        //Stop();
        recvMoveSpeed = 0;
        moveSpeed = 0;
        State = Define.State.Idle;
        State = Define.State.Die;
        gameObject.layer = 8;

        if (transform.gameObject.name.Contains("ork"))
        {
            yield return new WaitForSeconds(3.0f);
        }
        else if (transform.gameObject.name.Contains("Warrior") || transform.gameObject.name.Contains("Archor"))
        {
            yield return new WaitForSeconds(2.4f);
        }
        else if (transform.gameObject.name.Contains("Ogre")) 
        {
            yield return new WaitForSeconds(4.2f);
        }

        if (hitPlayerId == NetPlayerManager.Instance._playerManager.PlayerId)
            ItemDataBase.instance.GetItem(transform.position);

        if (transform.gameObject.name.Contains("Warrior"))
        {
            SpawnManager.instance.currentWCount--;
        }
        else if (transform.gameObject.name.Contains("Archor"))
        {
            SpawnManager.instance.currentACount--;
        }
        else if (transform.gameObject.name.Contains("ork"))
        {
            SpawnManager.instance.currentOCount--;
        }
        else if (transform.gameObject.name.Contains("Ogre"))
        {
            SpawnManager.instance.currentOgCount--;
        }

        SpawnManager.instance.monsterList.Remove(monsterId);
        Debug.Log($"{monsterId}번 몬스터 삭제");
        Destroy(gameObject);
    }

    IEnumerator Monster_moveTime() // 몬스터 동기화
    {
        yield return new WaitForSeconds(0.013f);
        isPlayingMoveTime = true;
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
        isPlayingMoveTime = false;
        StartCoroutine("Monster_moveTime");
    }
    #endregion
}

