using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttckSkill : MonoBehaviour
{
	public bool Delay = false;

	public bool attack = false;
	public bool skill1 = false;
	public bool skill2 = false;

	public int skill1mana;
	public int skill2mana;

    public PlayerSound playerSound;

	public Collider weaponcollider;

	public BaseController Base;

	public PlayerController player;

	public Animator anim;

	public void AttackActive() // 공격 함수
	{
		StartCoroutine("Attack");
		C_Attack c_Attack = new C_Attack();
		NetPlayerManager.Instance.Session.Send(c_Attack.Write());
	}

	public void Skill1MoveActive() // 스킬1 함수
	{
		StartCoroutine("Skill1");
		C_Skill c_Skill = new C_Skill();
		c_Skill.skillNum = 1;
		NetPlayerManager.Instance.Session.Send(c_Skill.Write());
	}

	public void Skill2MoveActive() // 스킬2 함수
	{
		StartCoroutine("Skill2");

		C_Skill c_Skill = new C_Skill();
		c_Skill.skillNum = 2;
		NetPlayerManager.Instance.Session.Send(c_Skill.Write());
	}

	public virtual void StopAll() // 모든 전투 행위 중지
	{
		StopAllCoroutines();
		attack = false;
	} 
}
