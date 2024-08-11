using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other_PlayerAttckSkill : MonoBehaviour
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

	public Other_PlayerController player;

	public Animator anim;

	public void AttackActive()
	{
		StartCoroutine("Attack");
	}

	public void Skill1MoveActive()
	{
		StartCoroutine("Skill1");
	}

	public void Skill2MoveActive()
	{
		StartCoroutine("Skill2");
	}

	public virtual void StopAll()
	{
		StopAllCoroutines();
		attack = false;
	}
}
