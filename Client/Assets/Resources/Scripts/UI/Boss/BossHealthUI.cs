using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField]
    Stat stat;

    [SerializeField]
    private Slider sliderHP;

    [SerializeField]
    private TextMeshProUGUI textHP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sliderHP != null) sliderHP.value = Mathf.Lerp(sliderHP.value, (float)stat.Hp / (float)stat.MaxHp, Time.deltaTime * 10);
        if (textHP != null) textHP.text = $"{stat.Hp:F0}/{stat.MaxHp:F0}";
    }
}
