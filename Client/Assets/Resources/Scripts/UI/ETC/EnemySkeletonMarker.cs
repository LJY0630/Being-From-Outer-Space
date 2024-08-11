using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonMarker : MonoBehaviour
{
    public GameObject SkellQuestMini;
    public GameObject SkellQuestMark;

    public int questnum;
    private void Start()
    {
        SkellQuestMini.SetActive(false);
        SkellQuestMark.SetActive(false);
        //if (NetPlayerManager.Instance.isHost == false)
        //{
        //    SkellQuestMini.SetActive(true);
        //    SkellQuestMark.SetActive(true);
        //}
    }

    private void Update()
    {
        if (NetPlayerManager.Instance._playerManager.getIsSelf() && (NetPlayerManager.Instance._playerManager.quest.isLook))
        {
            Debug.Log("???");
            if (NetPlayerManager.Instance._playerManager.quest.questId == questnum)
            {
                if (questnum == 102)
                {
                    if (NetPlayerManager.Instance._playerManager.quest.skeletonkillCount < 10)
                    {
                        SkellQuestMini.SetActive(true);
                        SkellQuestMark.SetActive(true);
                    }
                    else
                    {
                        SkellQuestMini.SetActive(false);
                        SkellQuestMark.SetActive(false);
                    }
                }
                else if (questnum == 103)
                {
                    if (NetPlayerManager.Instance._playerManager.quest.skeletonArchorkillCount < 10)
                    {
                        SkellQuestMini.SetActive(true);
                        SkellQuestMark.SetActive(true);
                    }
                    else
                    {
                        SkellQuestMini.SetActive(false);
                        SkellQuestMark.SetActive(false);
                    }
                }
            }
        }
    }
}