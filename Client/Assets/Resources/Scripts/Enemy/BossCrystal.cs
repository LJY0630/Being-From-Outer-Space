using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricLines;

public class BossCrystal : BaseController
{
    [SerializeField]
    private VolumetricLineBehavior laser;

    [SerializeField]
    private CapsuleCollider capsule;

    private RaycastHit hit;
    private int layermask = (1 << 6);

    public override void Init()
    {
        laser.EndPos = Vector3.zero;
        capsule.enabled = false;
    }

    private void Update()
    {
        
    }

    public void Laser(Vector3 EndPos) 
    {
        laser.StartPos = Vector3.zero;
        laser.EndPos = EndPos;
    }

    public void setIdle() 
    {
        State = Define.State.Idle;
        anim.CrossFade("Idle", 0.1f);
    }

    public void setAttack()
    {
        State = Define.State.Attack;
        anim.CrossFade("Attack1", 0.1f);
    }

    public void Activate(bool set) 
    {
        laser.gameObject.SetActive(set);
    }

    public void setCollider(bool set)
    {
        capsule.enabled = set;
    }
}
