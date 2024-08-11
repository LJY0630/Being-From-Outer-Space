using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    public PlayerStat playerStat;

    public Transform ownplayer;

    [SerializeField]
    private ParticleSystemRenderer[] AttackEffect;

    [SerializeField]
    private SphereCollider sphere;

    private Color[] color;

    private Color setColor;

    void Start() 
    {

    }

    public void SetLighit(PlayerStat player) 
    {
        sphere.enabled = false;
        ownplayer = player.transform.root;
        color = new Color[AttackEffect.Length];
        for (int i = 0; i < color.Length; i++)
        {
            color[i] = AttackEffect[i].material.GetColor("_TintColor");
            color[i] = new Color(color[i].r, color[i].g, color[i].b, 0);
        }
        for (int i = 0; i < AttackEffect.Length; i++)
        {
            AttackEffect[i].material.SetColor("_TintColor", color[i]);
        }

       playerStat.SetStat(player.Level);
       playerStat.Attack = player.Attack;
       playerStat.power = player.power;
       playerStat.heal = player.heal;
       playerStat.magic = player.magic;

        StartCoroutine("Light");
    }

    IEnumerator Light() 
    {
        Color tempcolor = new Color(0, 0, 0, Time.fixedDeltaTime * 2.4f);
        Color setalpha = new Color(0, 0, 0, 0);
        while (1 > 0)
        {
            setalpha += tempcolor;
            for (int i = 0; i < AttackEffect.Length; i++)
            {
                setColor = new Color(color[i].r, color[i].g, color[i].b, setalpha.a);
                AttackEffect[i].material.SetColor("_TintColor", setColor);
            }
            if (AttackEffect[AttackEffect.Length - 1].material.GetColor("_TintColor").a >= 1)
            {
                break;
            }
            yield return null;
        }

        sphere.enabled = true;
        yield return new WaitForSeconds(6.0f);

        while (1 > 0)
        {
            setalpha -= tempcolor;

            for (int i = 0; i < AttackEffect.Length; i++)
            {
                setColor = new Color(color[i].r, color[i].g, color[i].b, setalpha.a);
                AttackEffect[i].material.SetColor("_TintColor", setColor);
            }

            if (AttackEffect[AttackEffect.Length - 1].material.GetColor("_TintColor").a <= 0)
            {
                break;
            }
            yield return null;
        }
        sphere.enabled = false;

        Destroy(gameObject);
    }

}
