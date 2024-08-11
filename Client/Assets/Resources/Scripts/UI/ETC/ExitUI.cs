using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitUI : MonoBehaviour
{

    [SerializeField]
    private PlayerStat stat;

    [SerializeField]
    private GameObject ExitPanel;

    public bool isExitUI = false;

    private void Start()
    {
        ExitPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stat.isDead)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isExitUI = !isExitUI;
            }

            if (isExitUI)
            {
                ExitPanel.SetActive(true);
            }
            else if (!isExitUI)
            {
                ExitPanel.SetActive(false);
            }
        }
        else
            ExitPanel.SetActive(false);
    }

    public void CancleButton()
    {
        isExitUI = false;
        ExitPanel.SetActive(false);
    }

    public void ExitButton() 
    {
        Application.Quit();
    }
}
