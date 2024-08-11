using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    public Image skill1cool;

    public TextMeshProUGUI skill1cooltext;

    public Image skill2cool;

    public TextMeshProUGUI skill2cooltext;

    public PlayerController controller;

    public float skill1cooltime;
    public float skill1remain;
    public float skill2cooltime;
    public float skill2remain;

    // Start is called before the first frame update
    void Start()
    {
        skill1cool.gameObject.SetActive(false);
        skill2cool.gameObject.SetActive(false);
        skill1cooltext.gameObject.SetActive(false);
        skill2cooltext.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.playeras != null)
        {
            if (controller.playeras.skill1)
            {
                skill1cool.gameObject.SetActive(true);
                skill1cooltext.gameObject.SetActive(true);
                // StartCoroutine("Skill1CoolDown");
                skill1remain -= Time.deltaTime;
                skill1cooltext.text = skill1remain.ToString("00.0");
                skill1cool.fillAmount = skill1remain / skill1cooltime;

                if (skill1remain <= 0)
                {
                    skill1remain = skill1cooltime;
                    skill1cool.gameObject.SetActive(false);
                    skill1cooltext.gameObject.SetActive(false);
                    controller.playeras.skill1 = false;
                }
            }

            if (controller.playeras.skill2)
            {
                skill2cool.gameObject.SetActive(true);
                skill2cooltext.gameObject.SetActive(true);
                //StartCoroutine("Skill2CoolDown");
                skill2remain -= Time.deltaTime;
                skill2cooltext.text = skill2remain.ToString("00.0");
                skill2cool.fillAmount = skill2remain / skill2cooltime;

                if (skill2remain <= 0) 
                {
                    skill2remain = skill2cooltime;
                    skill2cool.gameObject.SetActive(false);
                    skill2cooltext.gameObject.SetActive(false);
                    controller.playeras.skill2 = false;
                }
            }
        }
    }

    public void SetStart() 
    {
        skill1cool.gameObject.SetActive(false);
        skill2cool.gameObject.SetActive(false);
        skill1cooltext.gameObject.SetActive(false);
        skill2cooltext.gameObject.SetActive(false);
        skill1remain = skill1cooltime;
        skill2remain = skill2cooltime;
    }

}
