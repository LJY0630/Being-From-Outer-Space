using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other_WizardAttackSkill : Other_PlayerAttckSkill
{
	public Transform shootTransform;

	public GameObject Fire;

	public GameObject Lighting;

	[SerializeField]
	private GameObject IceBall;

	IEnumerator Attack()
	{
		yield return new WaitForSeconds(0.42f);
		GameObject ball = Instantiate(IceBall, shootTransform.position, Quaternion.identity);
		shootTransform.GetComponent<AudioSource>().Play();
		ball.GetComponent<IceBall>().ownplayer = transform.root;
		ball.GetComponent<IceBall>().playerVector = transform.root.GetChild(1).forward;
		ball.GetComponent<IceBall>().playerStat.SetStat(transform.root.GetComponent<PlayerStat>().Level);
		ball.GetComponent<IceBall>().playerStat.Attack = transform.root.GetComponent<PlayerStat>().Attack;
		ball.GetComponent<IceBall>().playerStat.power = transform.root.GetComponent<PlayerStat>().power;
		ball.GetComponent<IceBall>().playerStat.heal = transform.root.GetComponent<PlayerStat>().heal;
		ball.GetComponent<IceBall>().playerStat.magic = transform.root.GetComponent<PlayerStat>().magic;
		ball.SetActive(true);
		ball.GetComponent<IceBall>().ThrowBall();
		yield return new WaitForSeconds(0.447f);
		player.playerManager.isAttack = false;
		Base.State = Define.State.Idle;
		anim.CrossFade("Idle", 0.025f);
		StartCoroutine("SetDelay");
	}

	IEnumerator Skill1()
	{
		yield return new WaitForSeconds(0.45f);
		playerSound.WizardSkil1Sound();
		Fire.SetActive(true);
		yield return new WaitForSeconds(2.6f);
		playerSound.StopSound();
		Fire.SetActive(false);
		yield return new WaitForSeconds(1.0f);
		player.playerManager.isSkill = false;
		Base.State = Define.State.Idle;
		anim.applyRootMotion = false;
		anim.CrossFade("Idle", 0.025f);
		StartCoroutine("SetDelay");
	}

	IEnumerator Skill2()
	{
		yield return new WaitForSeconds(2.4f);
		GameObject light1 = Instantiate(Lighting, transform.parent.GetChild(1).transform.position + transform.parent.GetChild(1).transform.forward * 4.0f + transform.parent.GetChild(1).transform.up * 1.5f, Quaternion.identity);
		GameObject light2 = Instantiate(Lighting, transform.parent.GetChild(1).transform.position - transform.parent.GetChild(1).transform.forward * 4.0f + transform.parent.GetChild(1).transform.up * 1.5f, Quaternion.identity);
		GameObject light3 = Instantiate(Lighting, transform.parent.GetChild(1).transform.position + transform.parent.GetChild(1).transform.right * 4.0f + transform.parent.GetChild(1).transform.up * 1.5f, Quaternion.identity);
		GameObject light4 = Instantiate(Lighting, transform.parent.GetChild(1).transform.position - transform.parent.GetChild(1).transform.right * 4.0f + transform.parent.GetChild(1).transform.up * 1.5f, Quaternion.identity);
		light1.GetComponent<Lighting>().SetLighit(transform.root.GetComponent<PlayerStat>());
		light2.GetComponent<Lighting>().SetLighit(transform.root.GetComponent<PlayerStat>());
		light3.GetComponent<Lighting>().SetLighit(transform.root.GetComponent<PlayerStat>());
		light4.GetComponent<Lighting>().SetLighit(transform.root.GetComponent<PlayerStat>());
		yield return new WaitForSeconds(1.053f);
		//weaponcollider.enabled = false;
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
		yield return new WaitForSeconds(0.4f);
		attack = false;
	}

	public override void StopAll()
	{
		StopAllCoroutines();
		attack = false;
		Fire.SetActive(false);
	}
}
