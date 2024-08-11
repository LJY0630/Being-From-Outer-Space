using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossQuestMarker : MonoBehaviour
{
    public GameObject questMarker;
    public GameObject questMarkerBig;

    void Start()
    {
        questMarker.SetActive(false);
        questMarkerBig.SetActive(false);
        if(NetPlayerManager.Instance.isHost==false)
        {
            questMarker.SetActive(true);
            questMarkerBig.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (NetPlayerManager.Instance._playerManager != null && NetPlayerManager.Instance._playerManager.getIsSelf())//NetPlayerManager.Instance.isHost&&
        {
            if ((!NetPlayerManager.Instance._playerManager.quest.isLook))
            {
                questMarker.SetActive(false);
                questMarkerBig.SetActive(false);
            }

            if (NetPlayerManager.Instance._playerManager.quest.questId == 104)
            {
                if (NetPlayerManager.Instance._playerManager.quest.isLook)
                {
                    questMarker.SetActive(true);
                    questMarkerBig.SetActive(true);
                }
            }
        }
    }
}
