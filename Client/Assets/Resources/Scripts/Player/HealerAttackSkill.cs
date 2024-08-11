using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class HealerAttackSkill : PlayerAttckSkill
{

	[SerializeField]
	Transform portal;

	[SerializeField]
	private ParticleSystemRenderer[] AttackEffect;

	[SerializeField]
	public EffectManager effectManager;

	[SerializeField]
	public Image UpSkill;

	[SerializeField]
	public TextMeshProUGUI upText;

	public int countUpHeal = 0;

	private Color[] color;

	[SerializeField]
	private GameObject HealingCircle;

	[SerializeField]
	private GameObject UpHealingCircle;

	private void Awake()
    {
		weaponcollider.enabled = false;
		color = new Color[AttackEffect.Length];
		for (int i = 0; i < color.Length; i++)
		{
			color[i] = AttackEffect[i].material.color;
			color[i] = new Color(color[i].r, color[i].g, color[i].b, 0);
		}
		for (int i = 0; i < AttackEffect.Length; i++) 
		{
			AttackEffect[i].material.color = color[i];
		}
		portal.gameObject.SetActive(false);
    }

    IEnumerator Attack()
	{
		portal.gameObject.SetActive(true);
		Color tempcolor = new Color(0, 0, 0, 0);
		tempcolor = new Color(0, 0, 0, Time.fixedDeltaTime * 2.4f);
		playerSound.HealerAttackSound();
		while (1 > 0)
		{
			for (int i = 0; i < AttackEffect.Length; i++)
			{
				AttackEffect[i].material.color += tempcolor;
			}

			if (AttackEffect[AttackEffect.Length - 1].material.color.a >= 1)
			{
				break;
			}
			yield return null;
		}
		weaponcollider.enabled = true;
		while (1 > 0)
		{
			for (int i = 0; i < AttackEffect.Length; i++)
			{
				AttackEffect[i].material.color -= tempcolor;
			}
			if (AttackEffect[AttackEffect.Length - 1].material.color.a <= 0)
			{
				break;
			}
			yield return null;
		}
		for (int i = 0; i < AttackEffect.Length; i++)
		{
			AttackEffect[i].material.color = color[i];
		}
		yield return new WaitForSeconds(0.35f);
		portal.gameObject.SetActive(false);
		weaponcollider.enabled = false;
		Base.State = Define.State.Idle;
		player.playerManager.isAttack = false;
		anim.CrossFade("Idle", 0.025f);
		StartCoroutine("SetDelay");
	}

	IEnumerator Skill1()
	{
		yield return new WaitForSeconds(1.7f);
		if (countUpHeal > 0)
		{
			GameObject Circle = Instantiate(UpHealingCircle, transform.parent.GetChild(1).transform.position, Quaternion.identity);
			playerSound.HealerSkil1Sound();
			Circle.GetComponent<UpHealingCircle>().healing = transform.root.GetComponent<PlayerStat>().heal;
			countUpHeal--;
			upText.text = countUpHeal.ToString();
			if (countUpHeal == 0) 
			{
				upText.text = "";
				UpSkill.color = new Color(0, 255, 0);
				effectManager.HealUp(false);
			}
		}
		else
		{
			GameObject Circle = Instantiate(HealingCircle, transform.parent.GetChild(1).transform.position, Quaternion.identity);
			playerSound.HealerSkil1Sound();
			Circle.GetComponent<HealingCircle>().healing = transform.root.GetComponent<PlayerStat>().heal;
		}
		yield return new WaitForSeconds(1.3f);
		player.playerManager.isSkill = false;
		Base.State = Define.State.Idle;
		anim.applyRootMotion = false;
		anim.CrossFade("Idle", 0.025f);
		StartCoroutine("SetDelay");
	}

	IEnumerator Skill2()
	{
		yield return new WaitForSeconds(0.7f);
		effectManager.HealUp(true);
		yield return new WaitForSeconds(2.0f);
		countUpHeal = 2;
		upText.text = countUpHeal.ToString();
		UpSkill.color = new Color(255, 255, 0);
		player.playerManager.isSkill = false;
		Base.State = Define.State.Idle;
		anim.applyRootMotion = false;
		anim.CrossFade("Idle", 0.025f);
		StartCoroutine("SetDelay");
	}

	IEnumerator SetDelay()
	{
		yield return new WaitForSeconds(0.1f);
		for (int i = 0; i < AttackEffect.Length; i++)
		{
			AttackEffect[i].material.color = color[i];
		}
		Delay = false;
		yield return new WaitForSeconds(0.4f);
		attack = false;

	}

	public override void StopAll() 
	{
		StopAllCoroutines();
		attack = false;

		if (countUpHeal == 0) 
		{
			effectManager.HealUp(false);
		}

		for (int i = 0; i < AttackEffect.Length; i++)
		{
			AttackEffect[i].material.color = color[i];
		}
		portal.gameObject.SetActive(false);
	}
}
