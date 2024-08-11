using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField]
    WarriorAttackSkill warriorskill;

    [SerializeField]
    WizardAttackSkill wizardskill;

    [SerializeField]
    HealerAttackSkill healerskill;

    [SerializeField]
    BaseController player;

    [SerializeField]
    AnimationClip warriorAttack;

    [SerializeField]
    AnimationClip warriorSkill1;

    [SerializeField]
    AnimationClip warriorSkill2;

    [SerializeField]
    AnimationClip wizardAttack;

    [SerializeField]
    AnimationClip wizardSkill1;

    [SerializeField]
    AnimationClip wizardSkill2;

    [SerializeField]
    AnimationClip healerAttack;

    [SerializeField]
    AnimationClip healerSkill1;

    [SerializeField]
    AnimationClip healerSkill2;

    [SerializeField]
    GameObject warriorWeapon;

    [SerializeField]
    GameObject wizardWeapon;

    [SerializeField]
    GameObject healerWaepon;

    [SerializeField]
    SkillUI skillUI;

    [SerializeField]
    GameObject WarriorSkill1icon;

    [SerializeField]
    GameObject WarriorSkill2icon;

    [SerializeField]
    GameObject WizardSkill1icon;

    [SerializeField]
    GameObject WizardSkill2icon;

    [SerializeField]
    GameObject HealerSkill1icon;

    [SerializeField]
    GameObject HealerSkill2icon;

    public TextMeshProUGUI skill1mana;
    public TextMeshProUGUI skill2mana;

    private AnimatorOverrideController overrideController;

    // Start is called before the first frame update
    void Awake()
    {
        player.playeras = null;
        wizardskill.Fire.SetActive(false);
        SetFirstNothing();
    }

    public void SetFirstNothing()
    {
        player.playeras = null;

        warriorWeapon.gameObject.SetActive(false);
        wizardWeapon.gameObject.SetActive(false);
        healerWaepon.gameObject.SetActive(false);

        WarriorSkill1icon.gameObject.SetActive(false);
        WarriorSkill2icon.gameObject.SetActive(false);

        WizardSkill1icon.gameObject.SetActive(false);
        WizardSkill2icon.gameObject.SetActive(false);

        HealerSkill1icon.gameObject.SetActive(false);
        HealerSkill2icon.gameObject.SetActive(false);

        skill1mana.text = "";
        skill2mana.text = "";
    }

    public void SetNothing()
    {
        player.playeras = null;

        PlayerManager playerManager = GetComponentsInParent<PlayerManager>()[0];
        playerManager.WeaponEffect = null;

        warriorWeapon.gameObject.SetActive(false);
        wizardWeapon.gameObject.SetActive(false);
        healerWaepon.gameObject.SetActive(false);

        WarriorSkill1icon.gameObject.SetActive(false);
        WarriorSkill2icon.gameObject.SetActive(false);

        WizardSkill1icon.gameObject.SetActive(false);
        WizardSkill2icon.gameObject.SetActive(false);

        HealerSkill1icon.gameObject.SetActive(false);
        HealerSkill2icon.gameObject.SetActive(false);

        skill1mana.text = "";
        skill2mana.text = "";
    }

    public void WeaponChange(string weaponname)
    {
        if (weaponname.Contains("Warrior"))
        {
            healerskill.countUpHeal = 0;
            healerskill.upText.text = "";
            healerskill.UpSkill.color = new Color(0, 255, 0);
            healerskill.effectManager.UpHealEft(false);

            player.playeras = warriorskill;

            skill1mana.text = player.playeras.skill1mana.ToString();
            skill2mana.text = player.playeras.skill2mana.ToString();

            skillUI.skill1cooltime = 3.268f;
            skillUI.skill2cooltime = 3.655f;
            skillUI.skill1remain = skillUI.skill1cooltime;
            skillUI.skill2remain = skillUI.skill2cooltime;

            player.playeras.playerSound = player.sound;

            overrideController = new AnimatorOverrideController(player.playeras.anim.runtimeAnimatorController);
            player.playeras.anim.runtimeAnimatorController = overrideController;

            overrideController["MeeleeAttack_OneHanded"] = warriorAttack;
            overrideController["SpinAttack"] = warriorSkill1;
            overrideController["Great Sword Slash"] = warriorSkill2;

            warriorWeapon.gameObject.SetActive(true);
            wizardWeapon.gameObject.SetActive(false);
            healerWaepon.gameObject.SetActive(false);

            WarriorSkill1icon.gameObject.SetActive(true);
            WarriorSkill2icon.gameObject.SetActive(true);

            WizardSkill1icon.gameObject.SetActive(false);
            WizardSkill2icon.gameObject.SetActive(false);

            HealerSkill1icon.gameObject.SetActive(false);
            HealerSkill2icon.gameObject.SetActive(false);

            if (weaponname.Contains("Lv1"))
            {
                warriorWeapon.transform.GetChild(0).gameObject.SetActive(true);
                warriorWeapon.transform.GetChild(1).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(2).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(0).GetComponent<WeaponEffect>().Init();
                player.playeras.weaponcollider = warriorWeapon.transform.GetChild(0).GetComponent<CapsuleCollider>();
                player.playeras.weaponcollider.enabled = false;
                warriorWeapon.transform.GetChild(0).GetComponent<WeaponEffect>().Init();
            }
            else if (weaponname.Contains("Lv2"))
            {
                warriorWeapon.transform.GetChild(0).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(1).gameObject.SetActive(true);
                warriorWeapon.transform.GetChild(2).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(1).GetComponent<WeaponEffect>().Init();
                player.playeras.weaponcollider = warriorWeapon.transform.GetChild(1).GetComponent<CapsuleCollider>();
                player.playeras.weaponcollider.enabled = false;
                warriorWeapon.transform.GetChild(1).GetComponent<WeaponEffect>().Init();
            }
            else if (weaponname.Contains("Lv3"))
            {
                warriorWeapon.transform.GetChild(0).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(1).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(2).gameObject.SetActive(true);
                warriorWeapon.transform.GetChild(2).GetComponent<WeaponEffect>().Init();
                player.playeras.weaponcollider = warriorWeapon.transform.GetChild(2).GetComponent<CapsuleCollider>();
                player.playeras.weaponcollider.enabled = false;
                warriorWeapon.transform.GetChild(2).GetComponent<WeaponEffect>().Init();
            }
        }
        else if (weaponname.Contains("Wizard"))
        {
            healerskill.countUpHeal = 0;
            healerskill.upText.text = "";
            healerskill.UpSkill.color = new Color(0, 255, 0);
            healerskill.effectManager.UpHealEft(false);

            player.playeras = wizardskill;

            skill1mana.text = player.playeras.skill1mana.ToString();
            skill2mana.text = player.playeras.skill2mana.ToString();

            skillUI.skill1cooltime = 20f;
            skillUI.skill2cooltime = 40f;
            skillUI.skill1remain = skillUI.skill1cooltime;
            skillUI.skill2remain = skillUI.skill2cooltime;

            player.playeras.playerSound = player.sound;

            overrideController = new AnimatorOverrideController(player.playeras.anim.runtimeAnimatorController);
            player.playeras.anim.runtimeAnimatorController = overrideController;

            overrideController["MeeleeAttack_OneHanded"] = wizardAttack;
            overrideController["SpinAttack"] = wizardSkill1;
            overrideController["Great Sword Slash"] = wizardSkill2;

            warriorWeapon.gameObject.SetActive(false);
            wizardWeapon.gameObject.SetActive(true);
            healerWaepon.gameObject.SetActive(false);

            WarriorSkill1icon.gameObject.SetActive(false);
            WarriorSkill2icon.gameObject.SetActive(false);

            WizardSkill1icon.gameObject.SetActive(true);
            WizardSkill2icon.gameObject.SetActive(true);

            HealerSkill1icon.gameObject.SetActive(false);
            HealerSkill2icon.gameObject.SetActive(false);

            if (weaponname.Contains("Lv1"))
            {
                wizardWeapon.transform.GetChild(0).gameObject.SetActive(true);
                wizardWeapon.transform.GetChild(1).gameObject.SetActive(false);
                wizardWeapon.transform.GetChild(2).gameObject.SetActive(false);
                player.playeras.GetComponent<WizardAttackSkill>().shootTransform = wizardWeapon.transform.GetChild(0).GetChild(0).transform;
                player.playeras.GetComponent<WizardAttackSkill>().Fire.SetActive(false);
                player.playeras.weaponcollider = null;
            }
            else if (weaponname.Contains("Lv2"))
            {
                wizardWeapon.transform.GetChild(0).gameObject.SetActive(false);
                wizardWeapon.transform.GetChild(1).gameObject.SetActive(true);
                wizardWeapon.transform.GetChild(2).gameObject.SetActive(false);
                player.playeras.GetComponent<WizardAttackSkill>().shootTransform = wizardWeapon.transform.GetChild(1).GetChild(0).transform;
                player.playeras.GetComponent<WizardAttackSkill>().Fire.SetActive(false);
                player.playeras.weaponcollider = null;
            }
            else if (weaponname.Contains("Lv3"))
            {
                wizardWeapon.transform.GetChild(0).gameObject.SetActive(false);
                wizardWeapon.transform.GetChild(1).gameObject.SetActive(false);
                wizardWeapon.transform.GetChild(2).gameObject.SetActive(true);
                player.playeras.GetComponent<WizardAttackSkill>().shootTransform = wizardWeapon.transform.GetChild(2).GetChild(0).transform;
                player.playeras.GetComponent<WizardAttackSkill>().Fire.SetActive(false);
                player.playeras.weaponcollider = null;
            }
        }
        else if (weaponname.Contains("Heal"))
        {
            player.playeras = healerskill;

            skill1mana.text = player.playeras.skill1mana.ToString();
            skill2mana.text = player.playeras.skill2mana.ToString();

            skillUI.skill1cooltime = 10f;
            skillUI.skill2cooltime = 30f;
            skillUI.skill1remain = skillUI.skill1cooltime;
            skillUI.skill2remain = skillUI.skill2cooltime;

            player.playeras.playerSound = player.sound;

            overrideController = new AnimatorOverrideController(player.playeras.anim.runtimeAnimatorController);
            player.playeras.anim.runtimeAnimatorController = overrideController;

            overrideController["MeeleeAttack_OneHanded"] = healerAttack;
            overrideController["SpinAttack"] = healerSkill1;
            overrideController["Great Sword Slash"] = healerSkill2;

            warriorWeapon.gameObject.SetActive(false);
            wizardWeapon.gameObject.SetActive(false);
            healerWaepon.gameObject.SetActive(true);

            WarriorSkill1icon.gameObject.SetActive(false);
            WarriorSkill2icon.gameObject.SetActive(false);

            WizardSkill1icon.gameObject.SetActive(false);
            WizardSkill2icon.gameObject.SetActive(false);

            HealerSkill1icon.gameObject.SetActive(true);
            HealerSkill2icon.gameObject.SetActive(true);

            if (weaponname.Contains("Lv1"))
            {
                healerWaepon.transform.GetChild(0).gameObject.SetActive(true);
                healerWaepon.transform.GetChild(1).gameObject.SetActive(false);
                healerWaepon.transform.GetChild(2).gameObject.SetActive(false);
                player.playeras.weaponcollider.enabled = false;
            }
            else if (weaponname.Contains("Lv2"))
            {
                healerWaepon.transform.GetChild(0).gameObject.SetActive(false);
                healerWaepon.transform.GetChild(1).gameObject.SetActive(true);
                healerWaepon.transform.GetChild(2).gameObject.SetActive(false);
                player.playeras.weaponcollider.enabled = false;
            }
            else if (weaponname.Contains("Lv3"))
            {
                healerWaepon.transform.GetChild(0).gameObject.SetActive(false);
                healerWaepon.transform.GetChild(1).gameObject.SetActive(false);
                healerWaepon.transform.GetChild(2).gameObject.SetActive(true);
                player.playeras.weaponcollider.enabled = false;
            }
        }

        PlayerStat p = ((PlayerController)player).playerstat;
        C_PlayerStat c_PlayerStat = new C_PlayerStat();
        c_PlayerStat.attack = p.Attack;
        c_PlayerStat.heal = p.heal;
        c_PlayerStat.magic = p.magic;
        c_PlayerStat.power = p.power;
        NetPlayerManager.Instance.Session.Send(c_PlayerStat.Write());
    }

}