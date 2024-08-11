using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other_WarriorAttackSkill : Other_PlayerAttckSkill
{
	IEnumerator Attack()
	{
		yield return new WaitForSeconds(0.22f);
		weaponcollider.enabled = true;
		playerSound.WarriorAttackSound();
		yield return new WaitForSeconds(0.2f);
		weaponcollider.enabled = false;
		yield return new WaitForSeconds(0.447f);
		player.playerManager.isAttack = false;
		Base.State = Define.State.Idle;
		anim.CrossFade("Idle", 0.025f);
		StartCoroutine("SetDelay");
	}

	IEnumerator Skill1()
	{
		yield return new WaitForSeconds(0.47f);
		weaponcollider.enabled = true;
		playerSound.WarriorSkil1Sound();
		yield return new WaitForSeconds(1.0f);
		weaponcollider.enabled = false;
		yield return new WaitForSeconds(1.698f);
		player.playerManager.isSkill = false;
		Base.State = Define.State.Idle;
		anim.applyRootMotion = false;
		anim.CrossFade("Idle", 0.025f);
		StartCoroutine("SetDelay");
	}

	IEnumerator Skill2()
	{
		yield return new WaitForSeconds(0.38f);
		weaponcollider.enabled = true;
		playerSound.WarriorSkil2Sound();
		yield return new WaitForSeconds(1.1f);
		playerSound.WarriorSkil2Sound();
		yield return new WaitForSeconds(1.0f);
		playerSound.WarriorSkil2Sound();
		yield return new WaitForSeconds(1.053f);
		weaponcollider.enabled = false;
		player.playerManager.isSkill = false;
		Base.State = Define.State.Idle;
		anim.applyRootMotion = false;
		anim.CrossFade("Idle", 0.025f);
		StartCoroutine("SetDelay");
	}

	IEnumerator SetDelay()
	{
		yield return new WaitForSeconds(0.1f);
		Delay = false;
		attack = false;
	}
}
