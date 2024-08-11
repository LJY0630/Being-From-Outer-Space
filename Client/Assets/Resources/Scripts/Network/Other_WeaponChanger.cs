using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Other_WeaponChanger : MonoBehaviour
{
    [SerializeField]
    Other_WarriorAttackSkill other_warriorskill;

    [SerializeField]
    Other_WizardAttackSkill other_wizardskill;

    [SerializeField]
    Other_HealerAttackSkill other_healerskill;

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

    private AnimatorOverrideController overrideController;

    // Start is called before the first frame update
    void Awake()
    {
        player.otherPlayeras = null;
        SetNothing();
    }

    public void SetNothing()
    {
        //Debug.Log("아더 체인저 setnoting");
        player.otherPlayeras = null;
        warriorWeapon.gameObject.SetActive(false);
        wizardWeapon.gameObject.SetActive(false);
        healerWaepon.gameObject.SetActive(false);

    }

    public void WeaponChange(string weaponname)
    {
        if (weaponname.Contains("Warrior"))
        {

            player.otherPlayeras = other_warriorskill;
            player.otherPlayeras.playerSound = player.sound;

            overrideController = new AnimatorOverrideController(player.otherPlayeras.anim.runtimeAnimatorController);
            player.otherPlayeras.anim.runtimeAnimatorController = overrideController;

            overrideController["MeeleeAttack_OneHanded"] = warriorAttack;
            overrideController["SpinAttack"] = warriorSkill1;
            overrideController["Great Sword Slash"] = warriorSkill2;

            warriorWeapon.gameObject.SetActive(true);
            wizardWeapon.gameObject.SetActive(false);
            healerWaepon.gameObject.SetActive(false);


            if (weaponname.Contains("Lv1"))
            {
                warriorWeapon.transform.GetChild(0).gameObject.SetActive(true);
                warriorWeapon.transform.GetChild(1).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(2).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(0).GetComponent<WeaponEffect>().Init();
                player.otherPlayeras.weaponcollider = warriorWeapon.transform.GetChild(0).GetComponent<CapsuleCollider>();
                player.otherPlayeras.weaponcollider.enabled = false;
                warriorWeapon.transform.GetChild(0).GetComponent<WeaponEffect>().Init();
            }
            else if (weaponname.Contains("Lv2"))
            {
                warriorWeapon.transform.GetChild(0).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(1).gameObject.SetActive(true);
                warriorWeapon.transform.GetChild(2).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(1).GetComponent<WeaponEffect>().Init();
                player.otherPlayeras.weaponcollider = warriorWeapon.transform.GetChild(1).GetComponent<CapsuleCollider>();
                player.otherPlayeras.weaponcollider.enabled = false;
                warriorWeapon.transform.GetChild(1).GetComponent<WeaponEffect>().Init();
            }
            else if (weaponname.Contains("Lv3"))
            {
                warriorWeapon.transform.GetChild(0).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(1).gameObject.SetActive(false);
                warriorWeapon.transform.GetChild(2).gameObject.SetActive(true);
                warriorWeapon.transform.GetChild(2).GetComponent<WeaponEffect>().Init();
                player.otherPlayeras.weaponcollider = warriorWeapon.transform.GetChild(2).GetComponent<CapsuleCollider>();
                player.otherPlayeras.weaponcollider.enabled = false;
                warriorWeapon.transform.GetChild(2).GetComponent<WeaponEffect>().Init();
            }
        }
        else if (weaponname.Contains("Wizard"))
        {

            player.otherPlayeras = other_wizardskill;

            player.otherPlayeras.playerSound = player.sound;

            overrideController = new AnimatorOverrideController(player.otherPlayeras.anim.runtimeAnimatorController);
            player.otherPlayeras.anim.runtimeAnimatorController = overrideController;

            overrideController["MeeleeAttack_OneHanded"] = wizardAttack;
            overrideController["SpinAttack"] = wizardSkill1;
            overrideController["Great Sword Slash"] = wizardSkill2;

            warriorWeapon.gameObject.SetActive(false);
            wizardWeapon.gameObject.SetActive(true);
            healerWaepon.gameObject.SetActive(false);

            if (weaponname.Contains("Lv1"))
            {
                wizardWeapon.transform.GetChild(0).gameObject.SetActive(true);
                wizardWeapon.transform.GetChild(1).gameObject.SetActive(false);
                wizardWeapon.transform.GetChild(2).gameObject.SetActive(false);
                player.otherPlayeras.GetComponent<Other_WizardAttackSkill>().shootTransform = wizardWeapon.transform.GetChild(0).GetChild(0).transform;
                player.otherPlayeras.GetComponent<Other_WizardAttackSkill>().Fire.SetActive(false);
                player.otherPlayeras.weaponcollider = null;
            }
            else if (weaponname.Contains("Lv2"))
            {
                wizardWeapon.transform.GetChild(0).gameObject.SetActive(false);
                wizardWeapon.transform.GetChild(1).gameObject.SetActive(true);
                wizardWeapon.transform.GetChild(2).gameObject.SetActive(false);
                player.otherPlayeras.GetComponent<Other_WizardAttackSkill>().shootTransform = wizardWeapon.transform.GetChild(1).GetChild(0).transform;
                player.otherPlayeras.GetComponent<Other_WizardAttackSkill>().Fire.SetActive(false);
                player.otherPlayeras.weaponcollider = null;
            }
            else if (weaponname.Contains("Lv3"))
            {
                wizardWeapon.transform.GetChild(0).gameObject.SetActive(false);
                wizardWeapon.transform.GetChild(1).gameObject.SetActive(false);
                wizardWeapon.transform.GetChild(2).gameObject.SetActive(true);
                player.otherPlayeras.GetComponent<Other_WizardAttackSkill>().shootTransform = wizardWeapon.transform.GetChild(2).GetChild(0).transform;
                player.otherPlayeras.GetComponent<Other_WizardAttackSkill>().Fire.SetActive(false);
                player.otherPlayeras.weaponcollider = null;
            }
        }
        else if (weaponname.Contains("Heal"))
        {
            player.otherPlayeras = other_healerskill;

            player.otherPlayeras.playerSound = player.sound;

            overrideController = new AnimatorOverrideController(player.otherPlayeras.anim.runtimeAnimatorController);
            player.otherPlayeras.anim.runtimeAnimatorController = overrideController;

            overrideController["MeeleeAttack_OneHanded"] = healerAttack;
            overrideController["SpinAttack"] = healerSkill1;
            overrideController["Great Sword Slash"] = healerSkill2;

            warriorWeapon.gameObject.SetActive(false);
            wizardWeapon.gameObject.SetActive(false);
            healerWaepon.gameObject.SetActive(true);


            if (weaponname.Contains("Lv1"))
            {
                healerWaepon.transform.GetChild(0).gameObject.SetActive(true);
                healerWaepon.transform.GetChild(1).gameObject.SetActive(false);
                healerWaepon.transform.GetChild(2).gameObject.SetActive(false);
                player.otherPlayeras.weaponcollider.enabled = false;
            }
            else if (weaponname.Contains("Lv2"))
            {
                healerWaepon.transform.GetChild(0).gameObject.SetActive(false);
                healerWaepon.transform.GetChild(1).gameObject.SetActive(true);
                healerWaepon.transform.GetChild(2).gameObject.SetActive(false);
                player.otherPlayeras.weaponcollider.enabled = false;
            }
            else if (weaponname.Contains("Lv3"))
            {
                healerWaepon.transform.GetChild(0).gameObject.SetActive(false);
                healerWaepon.transform.GetChild(1).gameObject.SetActive(false);
                healerWaepon.transform.GetChild(2).gameObject.SetActive(true);
                player.otherPlayeras.weaponcollider.enabled = false;
            }
        }
    }

}