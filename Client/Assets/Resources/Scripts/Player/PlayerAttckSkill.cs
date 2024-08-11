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

	public void AttackActive() // ���� �Լ�
	{
		StartCoroutine("Attack");
		C_Attack c_Attack = new C_Attack();
		NetPlayerManager.Instance.Session.Send(c_Attack.Write());
	}

	public void Skill1MoveActive() // ��ų1 �Լ�
	{
		StartCoroutine("Skill1");
		C_Skill c_Skill = new C_Skill();
		c_Skill.skillNum = 1;
		NetPlayerManager.Instance.Session.Send(c_Skill.Write());
	}

	public void Skill2MoveActive() // ��ų2 �Լ�
	{
		StartCoroutine("Skill2");

		C_Skill c_Skill = new C_Skill();
		c_Skill.skillNum = 2;
		NetPlayerManager.Instance.Session.Send(c_Skill.Write());
	}

	public virtual void StopAll() // ��� ���� ���� ����
	{
		StopAllCoroutines();
		attack = false;
	} 
}
