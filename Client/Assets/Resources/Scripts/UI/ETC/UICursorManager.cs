using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICursorManager : MonoBehaviour
{

    [SerializeField]
    private PlayerStat stat;

    [SerializeField]
    private ExitUI exitUI;

    [SerializeField]
    private InventotyUI Inven;

    [SerializeField]
    private BossEnterUI boss;

    [SerializeField]
    private StatsInfo statsInfo;

    [SerializeField]
    private ShopUI shop;

    // Update is called once per frame
    void Update()
    {
        if (Inven.isInventory || exitUI.isExitUI || stat.isDead || boss.isBossUIOn || statsInfo.isStat || shop.isShopOn)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
