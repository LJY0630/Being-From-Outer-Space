using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeadUI : BaseController
{
    [SerializeField]
    private PlayerStat stat;

    [SerializeField]
    private GameObject dead;

    [SerializeField]
    private GameObject respawnButton;

    private Transform player;

    bool isSelf = false;

    float count = 10;

    [SerializeField]
    private TextMeshProUGUI counter;

    public override void Init()
    {
        respawnButton.SetActive(false);
        player = transform.root.GetChild(1);
        if (transform.root.GetComponent<PlayerMove_Server>() == null)
            isSelf = true;
        else
            isSelf = false;
        dead.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelf==true&&stat.isDead) 
        {
            dead.SetActive(true);

            if (count > 0)
            {
                count -= Time.deltaTime;
                counter.text = (count).ToString("00");
            }
            else
            {
                counter.text = "";
                //respawnButton.SetActive(true);
            }
        }
    }

    public void RespawnTown()
    {
        player.gameObject.SetActive(true);
        if (Manager.Scene.GetScene() == "SampleScene")
        {
            transform.root.GetChild(1).position = new Vector3(-448.87f, -30.68f, 39.93f);
        }
        else
        {
            player.root.GetChild(0).GetComponent<PlayerController>().Dying = false;
        }
        State = Define.State.Idle;
        stat.isDead = false;
        anim.CrossFade("Idle", 0.1f);
        stat.SetStat(player.root.GetComponent<PlayerStat>().Level);
        count = 10f;
        respawnButton.SetActive(false);
        player.root.GetChild(0).GetComponent<PlayerController>().Dying = false;
        dead.SetActive(false);
    }


    public void InstantRespawn()
    {
        if (player.root.GetComponent<PlayerStat>().Gold >= 0)
        {
            player.root.GetComponent<PlayerStat>().Gold -= 0;

            C_SendMoney sendMoney = new C_SendMoney();
            sendMoney.money = transform.root.GetComponent<PlayerStat>().Gold;
            NetPlayerManager.Instance.Session.Send(sendMoney.Write());


            C_RespawnPlayer c_RespawnPlayer = new C_RespawnPlayer();
            NetPlayerManager.Instance.Session.Send(c_RespawnPlayer.Write());
            player.gameObject.SetActive(true);

            stat.isDead = false;
            player.root.GetChild(0).GetComponent<PlayerController>().Dying = false;
            dead.SetActive(false);
            anim.CrossFade("Idle", 0.1f);
            State = Define.State.Idle;
            stat.SetStat(player.root.GetComponent<PlayerStat>().Level);
            //gameObject.transform.root.GetComponent<PlayerManager>().weaponDamage.gameObject.SetActive(true);
        }
    }

    public void InstantRespawnServer(Other_PlayerController opcS)
    {
        anim = opcS.anim;
        sound = opcS.sound;
        playerManager = opcS.playerManager;
        otherPlayeras = opcS.otherPlayeras;
        player = transform.root.GetChild(1);

        player.gameObject.SetActive(true);
        opcS.playerstat.isDead = false;
        State = Define.State.Idle;
        anim.CrossFade("Idle", 0.1f);

        //stat.SetStat(player.root.GetComponent<PlayerStat>().Level);
        player.root.GetChild(0).GetComponent<Other_PlayerController>().Dying = false;
        State = Define.State.Idle;
        anim.CrossFade("Idle", 0.1f);
        if (NetPlayerManager.Instance.isBossLoad)
            player.root.GetChild(1).position = new Vector3(25.52f, 4.21f, 24.07f);
        else
            player.root.GetChild(1).position = new Vector3(player.root.GetChild(1).position.x, player.root.GetChild(1).position.y, player.root.GetChild(1).position.z);
        //
        //dead.SetActive(false);
        Debug.Log("인스턴트 서버 실행");
    }

    //IEnumerator WaitForLoad()
    //{
    //    while (Manager.Scene.GetScene() != "SampleScene")
    //    {
    //        yield return null;
    //    }

    //    transform.root.GetChild(1).position = new Vector3(-448.87f, -30.68f, 39.93f);
    //}
}
