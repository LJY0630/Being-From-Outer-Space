using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{

    [SerializeField]
    private GameObject HealEffect;

    [SerializeField]
    private GameObject ManaEffect;

    [SerializeField]
    private GameObject LevelEffect;

    [SerializeField]
    private GameObject HealUpEffect;

    [SerializeField]
    private GameObject UpHealingEffect;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("effect ´Ù ²û");
        HealEffect.SetActive(false);
        ManaEffect.SetActive(false);
        LevelEffect.SetActive(false);
        HealUpEffect.SetActive(false);
        UpHealingEffect.SetActive(false);
    }

    public void TurnHeal(bool able) 
    {
        //HealEffect.SetActive(able);
    }

    public void HealUp(bool able)
    {
        HealUpEffect.SetActive(able);
        if (able) 
        {
            HealUpEffect.GetComponent<AudioSource>().Play();
        }
    }

    public void UpHealEft(bool able)
    {
        UpHealingEffect.SetActive(able);
    }


    public void HealCo() 
    {
        StartCoroutine("Heal");
    }

    public void ManaCo()
    {
        StartCoroutine("Mana");
    }

    public void LevelCo() 
    {
        StartCoroutine("Level");
    }

    IEnumerator Heal() 
    {
        HealEffect.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        HealEffect.SetActive(false);
    }

    IEnumerator Mana()
    {
        ManaEffect.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        ManaEffect.SetActive(false);
    }

    IEnumerator Level() 
    {
        LevelEffect.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        LevelEffect.SetActive(false);
    }
}
