using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform CameraArm;

    [SerializeField]
    private InventotyUI inven;

    [SerializeField]
    private ExitUI exitUI;

    [SerializeField]
    private PlayerStat stat;

    [SerializeField]
    public BossEnterUI boss;

    [SerializeField]
    private StatsInfo statsInfo;

    [SerializeField]
    private ShopUI shop;

    [SerializeField]
    private TalkUI talk;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inven.isInventory && !exitUI.isExitUI && !stat.isDead && !boss.isBossUIOn && !statsInfo.isStat && !shop.isShopOn && !talk.isTexting)
        {
            LookAround();
        }
    }

    void LookAround() 
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = CameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else 
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        CameraArm.rotation = Quaternion.Euler(camAngle.x - mouseDelta.y, camAngle.y + mouseDelta.x, camAngle.z);
    }
}
