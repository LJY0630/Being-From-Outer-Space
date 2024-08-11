using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other_PlayerController : BaseController
{

    [SerializeField]
    public PlayerHit playerhit;

    [SerializeField]
    public PlayerStat playerstat;

    [SerializeField]
    public float walkSpeed;

    [SerializeField]
    public float runSpeed;

    bool jumping;

    [SerializeField]
    float jumpPower;

    [SerializeField]
    WeaponEffect weapon;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private CapsuleCollider collider;

    [SerializeField]
    public Other_WeaponChanger owc;

    [SerializeField]
    public ChangItem changItemHead;

    [SerializeField]
    public ChangItem changItemBody;

    [SerializeField]
    public ChangItem changItemLeg;

    [SerializeField]
    public ChangItem changItemFoot;

    public bool inBoss = false;

    private float curSpeed;
    public bool Dying = false;
    public bool isMove = false;
    private bool isAirMove = false;


    public int myId;

    private void Awake()
    {
        Debug.Log($"{gameObject.transform.position.y}, {gameObject.transform.localPosition.y}");
    }

    public override void Init()
    {
        playerManager = transform.parent.GetComponentInChildren<PlayerManager>();
        playerManager.PlayerAction -= OnAction;
        playerManager.PlayerAction += OnAction;

        if (otherPlayeras != null)
        {
            //Debug.Log("아더플레이어 in playeras 널 아님");
            otherPlayeras.playerSound = base.sound;
            otherPlayeras.weaponcollider.enabled = false;
        }
    }

    public void DoJump()
    {
        jumping = true;

    }

    private void FixedUpdate()
    {
        if (Dying) return;

        if (jumping)
        {
            jumping = false;

            sound.JumpSound();
            rigidbody.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
            State = Define.State.Jump;
            playerManager.isJumping = true;
        }

        if (!playerhit.isHit && State == Define.State.Hit)
        {
            anim.CrossFade("Idle", 0.1f);
        }

    }

    public void DoAttack()
    {
        otherPlayeras.Delay = true;
        playerManager.isAttack = true;
        otherPlayeras.attack = true;
        State = Define.State.Attack;
        otherPlayeras.AttackActive();
    }

    public void DoSkill1()
    {
        //Debug.Log("DoSkill1");
        anim.applyRootMotion = true;
        otherPlayeras.Delay = true;
        playerManager.isSkill = true;
        otherPlayeras.skill1 = true;
        State = Define.State.Skill1;
        otherPlayeras.Skill1MoveActive();
    }

    public void DoSkill2()
    {
        //Debug.Log("DoSkill2");
        anim.applyRootMotion = true;
        otherPlayeras.Delay = true;
        playerManager.isSkill = true;
        otherPlayeras.skill2 = true;
        State = Define.State.Skill2;
        otherPlayeras.Skill2MoveActive();
    }

    void OnAction()
    {
        if (!playerstat.isDead)
        {
            if (playerhit.isHit)
            {
                if (otherPlayeras != null)
                {
                    otherPlayeras.StopAll();
                    otherPlayeras.Delay = false;
                    if (otherPlayeras.weaponcollider != null)
                    {
                        otherPlayeras.weaponcollider.enabled = false;
                    }
                }
                playerManager.isAttack = false;
                playerManager.isSkill = false;
                curSpeed = 0;
                anim.applyRootMotion = false;
            }
        }
    }

    public void startCoroutineDead()
    {
        playerstat.isDead = true;
        if (!Dying)
        {
            Debug.Log("아더 다이");
            if (playeras != null)
            {
                otherPlayeras.StopAll();
                otherPlayeras.Delay = false;
                otherPlayeras.playerSound = sound;
                if (playeras.weaponcollider != null)
                {
                    playeras.weaponcollider.enabled = false;
                }
                anim.applyRootMotion = false;
            }
            playerManager.isAttack = false;
            playerManager.isSkill = false;
            StartCoroutine("Dead");
        }
    }

    IEnumerator Dead()
    {
        Dying = true;
        if (playeras != null)
        {
            playeras.skill1 = false;
            playeras.skill2 = false;
        }
        State = Define.State.Idle;
        State = Define.State.Die;
        gameObject.layer = 8;
        curSpeed = 0;

        sound.DieSound();

        yield return new WaitForSeconds(4.28f);
        /*
        for (int i = 0; i < transform.root.GetChild(1).childCount; i++) 
        {
           Destroy(transform.root.GetChild(1).GetChild(i).gameObject);
        }
        */
        if (playerstat.isDead)
        {
            transform.root.GetChild(1).gameObject.SetActive(false);
        }
    }


}