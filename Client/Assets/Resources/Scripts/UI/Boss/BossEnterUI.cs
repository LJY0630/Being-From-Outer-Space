using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossEnterUI : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;

    public bool isBossUIOn = false;

    public TextMeshProUGUI Text;

    private void Start()
    {
        Panel.SetActive(false);
        Text.gameObject.SetActive(false);
        isBossUIOn = false;
    }

    public void SetPanel(bool active)
    {
        Panel.SetActive(active);
        isBossUIOn = active;
    }

    public void QuitButton()
    {
        SetPanel(false);
    }

    public void Multi()
    {
        if (NetPlayerManager.Instance._playerManager.getIsSelf())
        {
            // 잠시변경
            //&& NetPlayerManager.Instance._playerManager.quest.questId >= 105
            if (transform.root.GetComponent<PlayerStat>().Level >= 1)
            {
                C_EnterBoss c_Enter = new C_EnterBoss();
                NetPlayerManager.Instance.Session.Send(c_Enter.Write());
                Text.gameObject.SetActive(true);
            }
            //foreach (NetPlayerManager.PlayerInfo p in NetPlayerManager.Instance._players.Values)
            //{
            //    if (p.opc != null)
            //        p.rigid.useGravity = false;
            //}
        }
    }


    public void Only()
    {
        // 잠시변경
        //if (NetPlayerManager.Instance._playerManager.getIsSelf())
        {
            //if (transform.root.GetComponent<PlayerStat>().Level >= 3 && NetPlayerManager.Instance._playerManager.quest.questId >= 105)
            {
                NetPlayerManager.Instance.isBossLoad = true;
                transform.root.GetComponentInChildren<PlayerController>().StopMoveTime();
                foreach (NetPlayerManager.PlayerInfo p in NetPlayerManager.Instance._players.Values)
                {
                    if (p.opc != null)
                    {
                        p.opc.inBoss = true;
                    }
                }
                //transform.root.GetChild(1).position = new Vector3(25.52f, 1.21f, 24.07f);
                //foreach (NetPlayerManager.PlayerInfo p in NetPlayerManager.Instance._players.Values)
                //{
                //    if (p.opc != null)
                //    {
                //        //p.rigid.useGravity = false;
                //        //p.opc.transform.root.GetChild(1).position = new Vector3(25.52f, 1.21f, 24.07f);
                //        //p.opc.transform.position = new Vector3(25.52f, 13.21f, 24.07f);
                //        p.opc.transform.root.GetChild(1).position = new Vector3(25.52f, 4.21f, 24.07f);
                //    }

                //}

                SetPanel(false);
                StartCoroutine(WaitForLoad());
                Manager.Scene.LoadBossScene();
            }
        }
    }

    IEnumerator WaitForLoad()
    {
        while (Manager.Scene.GetScene() != "Boss")
        {
            yield return null;
        }

        transform.root.GetChild(1).position = new Vector3(25.52f, 1.21f, 24.07f);
        foreach (NetPlayerManager.PlayerInfo p in NetPlayerManager.Instance._players.Values)
        {
            if (p.opc != null)
            {
                p.opc.transform.root.GetChild(1).position = new Vector3(25.52f, 1.21f, 24.07f);
            }
        }
    }
}
