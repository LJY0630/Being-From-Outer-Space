using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    public Animator anim;
    [SerializeField]
    public PlayerAttckSkill playeras;
    [SerializeField]
    public Other_PlayerAttckSkill otherPlayeras;
    [SerializeField]
    public PlayerSound sound;

    [SerializeField]
    public PlayerManager playerManager;

    protected Define.State state = Define.State.Idle;

    public virtual Define.State State
    {
        get { return state; }
        set 
        {
            state = value;

            switch (state) 
            {
                case Define.State.Idle:
                    anim.SetBool("Walk", false);
                    anim.SetBool("Run", false);
                    anim.SetBool("Jump", false);
                    anim.SetBool("Hit", false);
                    anim.SetBool("Die", false);
                    anim.SetBool("Attack1", false);
                    anim.SetBool("Skill1", false);
                    anim.SetBool("Skill2", false);
                    break;
                case Define.State.Running:
                    anim.SetBool("Walk", true);
                    anim.SetBool("Run",true);
                    break;
                case Define.State.Die:
                    anim.SetBool("Die", true);
                    anim.CrossFade("Die", 0.1f);
                    break;
                case Define.State.Hit:
                    anim.SetBool("Hit", true);
                    anim.CrossFade("Hit", 0.1f);
                    break;
                case Define.State.Jump:
                    anim.SetBool("Jump", true);
                    anim.SetBool("Land", false);
                    anim.CrossFade("Jump", 0.1f);
                    break;
                case Define.State.Attack:
                    anim.SetBool("Attack1", true);
                    anim.CrossFade("Attack1", 0.1f);
                    break;
                case Define.State.Skill1:
                    anim.SetBool("Skill1", true);
                    anim.CrossFade("Skill1", 0.1f);
                    break;
                case Define.State.Skill2:
                    anim.SetBool("Skill2", true);
                    anim.CrossFade("Skill2", 0.1f);
                    break;
                case Define.State.Walk:
                    anim.SetBool("Walk", true);
                    anim.SetBool("Run", false);
                    break;
            }
        }
    }

    void Update()
    {
        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Running:
                UpdateRun();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Hit:
                UpdateHit();
                break;
            case Define.State.Jump:
                UpdateJump();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
            case Define.State.Skill1:
                UpdateSkill1();
                break;
            case Define.State.Skill2:
                UpdateSkill2();
                break;
            case Define.State.Walk:
                UpdateWalk();
                break;
        }
    }

    private void Start()
    {
        Init();
    }

    public abstract void Init();

    protected virtual void UpdateDie() { }
    protected virtual void UpdateRun() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill1() { }
    protected virtual void UpdateSkill2() { }
    protected virtual void UpdateHit() { }
    protected virtual void UpdateJump() { }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateWalk() { }

}
