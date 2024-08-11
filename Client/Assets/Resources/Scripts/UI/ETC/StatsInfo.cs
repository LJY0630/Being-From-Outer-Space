using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsInfo : MonoBehaviour
{

    [SerializeField]
    private PlayerStat stat;

    [SerializeField]
    private GameObject StatPanel;

    [SerializeField]
    private TextMeshProUGUI textHP;

    [SerializeField]
    private TextMeshProUGUI textMP;

    [SerializeField]
    private TextMeshProUGUI textDF;

    [SerializeField]
    private TextMeshProUGUI textAt;

    [SerializeField]
    private TextMeshProUGUI textPo;

    [SerializeField]
    private TextMeshProUGUI textMag;

    [SerializeField]
    private TextMeshProUGUI textHeal;

    public bool isStat = false;

    // Start is called before the first frame update
    void Start()
    {
        StatPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (!stat.isDead)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isStat = !isStat;
            }

            if (isStat)
            {
                StatPanel.SetActive(true);
            }
            else if (!isStat)
            {
                StatPanel.SetActive(false);
            }
        }
        else
            StatPanel.SetActive(false);

        if (textHP != null) textHP.text = $"{stat.MaxHp}";
        if (textMP != null) textMP.text = $"{stat.MaxMp}";
        if (textDF != null) textDF.text = $"{stat.Defense}";
        if (textAt != null) textAt.text = $"{stat.Attack}";
        if (textPo != null) textPo.text = $"{stat.power}";
        if (textMag != null) textMag.text = $"{stat.magic}";
        if (textHeal != null) textHeal.text = $"{stat.heal}";
    }
}
