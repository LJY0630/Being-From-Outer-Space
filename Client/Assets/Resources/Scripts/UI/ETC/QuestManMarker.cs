using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManMarker : MonoBehaviour
{
    public GameObject QuestManQuestMini;
    public GameObject QuestManQuestMark;

    public GameObject ShopQuestMini;
    public GameObject ShopQuestMark;

    private void Start()
    {
        QuestManQuestMini.SetActive(false);
        QuestManQuestMark.SetActive(false);

        ShopQuestMini.SetActive(false);
        ShopQuestMark.SetActive(false);
    }

    void Update()
    {
        if (NetPlayerManager.Instance._playerManager != null)//&& NetPlayerManager.Instance._playerManager.getIsSelf()
        {
            if ((!NetPlayerManager.Instance._playerManager.quest.isLook))
            {
                QuestManQuestMini.SetActive(false);
                QuestManQuestMark.SetActive(false);
                ShopQuestMini.SetActive(false);
                ShopQuestMark.SetActive(false);
                if (NetPlayerManager.Instance._playerManager.quest.QuestObject != null)
                {
                    NetPlayerManager.Instance._playerManager.quest.QuestObject.GetComponent<QuestChest>().SetMarker(false);
                }
            }

            switch (NetPlayerManager.Instance._playerManager.quest.questId)
            {
                case 100:
                    if (NetPlayerManager.Instance._playerManager.quest.isLook)
                    {
                        QuestManQuestMini.SetActive(true);
                        QuestManQuestMark.SetActive(true);
                    }
                    break;
                case 101:
                    if (NetPlayerManager.Instance._playerManager.quest.isLook)
                    {
                        if (NetPlayerManager.Instance._playerManager.quest.isEquip)
                        {
                            ShopQuestMini.SetActive(false);
                            ShopQuestMark.SetActive(false);
                            QuestManQuestMini.SetActive(true);
                            QuestManQuestMark.SetActive(true);
                        }
                        else
                        {
                            ShopQuestMini.SetActive(true);
                            ShopQuestMark.SetActive(true);
                            QuestManQuestMini.SetActive(false);
                            QuestManQuestMark.SetActive(false);
                        }
                    }
                    break;
                case 102:
                    if (NetPlayerManager.Instance._playerManager.quest.isLook)
                    {
                        if (NetPlayerManager.Instance._playerManager.quest.skeletonkillCount >= 10)
                        {
                            QuestManQuestMini.SetActive(true);
                            QuestManQuestMark.SetActive(true);
                        }
                        else
                        {
                            QuestManQuestMini.SetActive(false);
                            QuestManQuestMark.SetActive(false);
                        }
                    }
                    break;
                case 103:
                    if (NetPlayerManager.Instance._playerManager.quest.isLook)
                    {
                        if (NetPlayerManager.Instance._playerManager.quest.skeletonArchorkillCount >= 10)
                        {
                            QuestManQuestMini.SetActive(true);
                            QuestManQuestMark.SetActive(true);
                        }
                        else
                        {
                            QuestManQuestMini.SetActive(false);
                            QuestManQuestMark.SetActive(false);
                        }
                    }
                    break;
                case 104:
                    if (NetPlayerManager.Instance._playerManager.quest.isLook)
                    {
                        if (NetPlayerManager.Instance._playerManager.quest.QuestObject != null)
                        {
                            if (NetPlayerManager.Instance._playerManager.quest.chestCount >= 5)
                            {
                                NetPlayerManager.Instance._playerManager.quest.QuestObject.GetComponent<QuestChest>().SetMarker(false);
                                QuestManQuestMini.SetActive(true);
                                QuestManQuestMark.SetActive(true);
                            }
                            else
                            {
                                NetPlayerManager.Instance._playerManager.quest.QuestObject.GetComponent<QuestChest>().SetMarker(true);
                                QuestManQuestMini.SetActive(false);
                                QuestManQuestMark.SetActive(false);
                            }
                        }
                    }
                    break;
            }
        }
    }
}