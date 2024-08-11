using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
	[SerializeField]
	protected int Maxexp;

	[SerializeField]
	protected int mana;

	[SerializeField]
	protected int Maxmana;

	[SerializeField]
	protected int Power = 0;

	[SerializeField]
	protected int Magic = 0;

	[SerializeField]
	protected int Heal = 0;

	[SerializeField]
	protected float addHealth = 0;

	[SerializeField]
	protected int addMana = 0;

	[SerializeField]
	protected int addDefense = 0;

	public int power { get { return Power; } set { Power = value; } }
	public int magic { get { return Magic; } set { Magic = value; } }
	public int heal { get { return Heal; } set { Heal = value; } }
	public float addheath { get { return addHealth; } set { addHealth = value; } }
	public int addmana { get { return addMana; } set { addMana = value; } }
	public int adddefense { get { return addDefense; } set { addDefense = value; } }

	public int Exp // 경험치 추가, 레벨업 처리, 동기화와 DB에 전송
	{
		get {
			if (exp >= Maxexp)
			{
				int curExp = exp - Maxexp;

				exp = curExp;
				level++;
				transform.GetChild(1).GetChild(5).GetComponent<EffectManager>().LevelCo();
				SetStat(level);
				C_SendLevel c_SendLevel = new C_SendLevel();
				c_SendLevel.level = level;
				NetPlayerManager.Instance.Session.Send(c_SendLevel.Write());

				C_PlayerStat c_PlayerStat = new C_PlayerStat();
				c_PlayerStat.attack = Attack;
				c_PlayerStat.heal = heal;
				c_PlayerStat.magic = magic;
				c_PlayerStat.power = power;
				NetPlayerManager.Instance.Session.Send(c_PlayerStat.Write());
			}
		    return Maxexp; }
	}

	public int Mp { get { return mana; } set { mana = value; } }

	public int MaxMp { get { return Maxmana; } set { Maxmana = value; } }

	public int Gold { get { return gold; } set { gold = value; } }

	private void Start()
	{
		//SetStat(level);
	}

	public void SetStat(int level) // 레벨에 맞는 능력치 처리
	{
		maxHp = addHealth + 120 + (50 * (level - 1));
		hp = maxHp;
		attack = 10 + (5 * (level - 1));
		defense = addDefense + 5 + (1 * (level - 1));
		Maxexp = Maxexp + (20 * (level - 1));
		Maxmana = addMana + 100 + (50 * (level - 1));
		mana = Maxmana;
	}

	public void SetCurrentStat(int level) // 현재 능력치 설정
	{
		maxHp = addHealth + 120 + (50 * (level - 1));
		if (Hp >= maxHp)
			Hp = maxHp;
		attack = 10 + (5 * (level - 1));
		defense = addDefense + 5 + (1 * (level - 1));
		//Maxexp = Maxexp + (20 * (level - 1));
		Maxmana = addMana + 100 + (50 * (level - 1));
		if (Mp >= Maxmana)
			Mp = Maxmana;
	}

	public override void OnAttacked(Transform playerweapon) // 공격 받으면 동작
	{
		Debug.Log(playerweapon.gameObject.name);
		int other_attack = playerweapon.root.GetComponent<Stat>().Attack;
		int damage = Mathf.Max(0, other_attack - Defense);
		Hp -= damage;
		//Debug.Log(Hp);
		if (Hp <= 0)
		{
			Debug.Log("Dead");
			Hp = 0;
			OnDead(playerweapon);
		}
	}

	protected override void OnDead(Transform playerWeapon) // 죽었을 때 동작
	{
		if (transform.root.GetComponentInChildren<PlayerManager>().getIsSelf())
		{
			isDead = true;
		}
	}

}


