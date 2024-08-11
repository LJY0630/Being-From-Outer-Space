using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpMpInfo : MonoBehaviour
{
    [SerializeField]
    PlayerStat stat;

    [SerializeField]
    private Slider sliderHP;

    [SerializeField]
    private TextMeshProUGUI textHP;

    [SerializeField]
    private Slider sliderMP;

    [SerializeField]
    private TextMeshProUGUI textMP;

    [SerializeField]
    private Slider sliderExp;

    [SerializeField]
    private TextMeshProUGUI textExp;

    [SerializeField]
    private TextMeshProUGUI Gold;

    [SerializeField]
    private TextMeshProUGUI Level;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sliderHP != null) sliderHP.value = Mathf.Lerp(sliderHP.value, (float)stat.Hp / (float)stat.MaxHp, Time.deltaTime * 10);
        if (textHP != null) textHP.text = $"{stat.Hp:F0}/{stat.MaxHp:F0}";

        if (sliderMP != null) sliderMP.value = Mathf.Lerp(sliderMP.value, (float)stat.Mp / (float)stat.MaxMp, Time.deltaTime * 10);
        if (textMP != null) textMP.text = $"{stat.Mp:F0}/{stat.MaxMp:F0}";

        if (sliderExp != null) sliderExp.value = Mathf.Lerp(sliderExp.value, (float)stat.GetExp / (float)stat.Exp, Time.deltaTime * 10);
        if (textExp != null) textExp.text = $"{stat.GetExp:F0}/{stat.Exp:F0}";

        if (Gold != null) Gold.text = $"{stat.Gold}";

        if (Level != null) Level.text = $"Lv. {stat.Level}";
    }
}
