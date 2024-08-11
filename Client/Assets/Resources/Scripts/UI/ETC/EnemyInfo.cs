using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    [SerializeField]
    Stat stat;

    [SerializeField]
    private Slider sliderHP;

    // Update is called once per frame
    void Update()
    {
        if (sliderHP != null) sliderHP.value = Mathf.Lerp(sliderHP.value, (float)stat.Hp / (float)stat.MaxHp, Time.deltaTime * 10);
    }
}
