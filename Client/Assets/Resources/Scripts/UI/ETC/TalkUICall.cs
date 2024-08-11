using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkUICall : MonoBehaviour
{

    [SerializeField]
    private PlayerController player;

    public int talkIndex = 0;

    ObjData obj;

    [SerializeField]
    private TalkUI talk;

    string talkText;

    private bool canTalk;

    private bool onlyOneQuest = false;

    int questTalkIndex = 0;

    [SerializeField]
    private Slot slotUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canTalk)
        {
            if (talkText != null)
            {
                if (talkIndex == 0)
                {
                    onlyOneQuest = true;
                    QuestClearCheck();
                    questTalkIndex = player.playerManager.quest.GetQuestTalkIndex();
                    talkText = player.playerManager.talk.GetTalk(obj.id + questTalkIndex, talkIndex);
                }
                talk.ActiveTalk(true);
                talk.SetNPCTalkText(talkText);
                talkIndex++;
            }
            else
            {
                talk.ActiveTalkBefore(false);
                talk.ActiveTalk(false);
                talk.QuestText.text = player.playerManager.quest.CheckQuest();
                player.playerManager.quest.isLook = true;
                talkIndex = 0;
                onlyOneQuest = false;
            }
        }

        if (player.playerManager.quest.questId == 101)
        {
            if (slotUI.item != null)
            {
                player.playerManager.quest.isEquip = true;
            }
            else
            {
                player.playerManager.quest.isEquip = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            talkIndex = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            obj = other.GetComponent<ObjData>();
            questTalkIndex = player.playerManager.quest.GetQuestTalkIndex();
            talkText = player.playerManager.talk.GetTalk(obj.id + questTalkIndex, talkIndex);

            if (talk.isTexting)
            {
                talk.ActiveTalkBefore(false);
            }
            else
            {
                if (talkText != null)
                {
                    canTalk = true;
                    talk.SetNPCName(obj.id);
                    talk.ActiveTalkBefore(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "NPC")
        {
            talkIndex = 0;
            talk.ActiveTalkBefore(false);
            canTalk = false;
        }
    }

    private void QuestClearCheck()
    {
        switch (player.playerManager.quest.questId)
        {
            case 100:
                if (canTalk && onlyOneQuest)
                {
                    onlyOneQuest = false;
                    player.playerManager.quest.QuestClear(transform.root);
                }
                break;
            case 101:
                if (slotUI.item != null && onlyOneQuest)
                {
                    onlyOneQuest = false;
                    player.playerManager.quest.QuestClear(transform.root);
                }
                break;
            case 102:
                if (player.playerManager.quest.skeletonkillCount >= 10)
                {
                    onlyOneQuest = false;
                    player.playerManager.quest.QuestClear(transform.root);
                    player.playerManager.quest.skeletonkillCount = 0;
                }
                break;
            case 103:
                if (player.playerManager.quest.skeletonArchorkillCount >= 10)
                {
                    onlyOneQuest = false;
                    player.playerManager.quest.QuestClear(transform.root);
                    player.playerManager.quest.skeletonkillCount = 0;
                }
                break;
            case 104:
                if (player.playerManager.quest.chestCount == 5)
                {
                    onlyOneQuest = false;
                    player.playerManager.quest.QuestClear(transform.root);
                    player.playerManager.quest.chestCount = 0;
                }
                break;
            case 105:
                if (player.playerManager.quest.boss)
                {
                    player.playerManager.quest.QuestClear(transform.root);
                    player.playerManager.quest.boss = false;
                }
                break;
        }
    }
}