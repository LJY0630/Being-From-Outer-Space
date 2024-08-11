using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacters : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //foreach(NetPlayerManager.PlayerInfo p in NetPlayerManager.Instance._players.Values)
        //{
        //    if(p.opc!=null)
        //    {
        //        p.manager.transform.root.position = new Vector3(-448.87f, -30.67f, 39.93f);
        //        p.manager.transform.localPosition = new Vector3(4744.821f, 320.18f, 157.69f);
        //        p.opc.transform.localPosition = new Vector3(4744.821f, 333.67f, 157.69f);
        //        Debug.Log($"{p.manager.PlayerId}번 캐릭터 위치변경");
        //    }
        //}
        StartCoroutine("startmoveControl");

        Destroy(gameObject, 7f);
    }

    IEnumerator startmoveControl()
    {
        SpawnManager.instance.SetEmptyMonsterList();
        yield return new WaitForSeconds(0.05f);
        foreach (NetPlayerManager.PlayerInfo p in NetPlayerManager.Instance._players.Values)
        {
            if (p.opc != null)
            {
                p.opc.inBoss = false;
                //p.rigid.transform.root.GetComponent<PlayerMove_Server>().SetValue();
            }
        }
        NetPlayerManager.Instance.isBossLoad = false;
        NetPlayerManager.Instance._playerManager.transform.root.GetComponentInChildren<PlayerController>().StartMoveTime();
    }

}