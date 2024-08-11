using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    public int questId = 100;
    public bool isClear = false;
    public bool isLook = true;
    public bool isEquip = false;
    public bool boss = false;
    public GameObject QuestObject;

    public int skeletonkillCount = 0;
    public int skeletonArchorkillCount = 0;
    public int chestCount = 0;

    Dictionary<int, QuestData> questList;

    public void Init()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();

        if (Manager.Scene.GetScene() == "SampleScene")
        {
            Debug.Log("Check!!");
            QuestObject = GameObject.Find("QuestChest");
        }
    }

    void GenerateData()
    {
        questList.Add(100, new QuestData("마을 대장과 대화 (50골드)", new int[] { 1 }, 50));
        questList.Add(101, new QuestData("무기 구매, 장착 후 마을 대장과 다시 대화 (100골드)", new int[] { 1 }, 100));
        questList.Add(102, new QuestData("스켈레톤 전사 10마리 잡기 후 마을 대장과 대화 (150골드)", new int[] { 1 }, 150));
        questList.Add(103, new QuestData("스켈레톤 궁수 10마리 잡기 후 마을 대장과 대화 (200골드)", new int[] { 1 }, 200));
        questList.Add(104, new QuestData("오지 못한 보급품 수급 후 마을 대장과 대화 (250골드)", new int[] { 1 }, 250));
        questList.Add(105, new QuestData("미지의 공간을 조사하여 사태 마무리 (300골드)", new int[] { 1 }, 300));
    }

    public int GetQuestTalkIndex()
    {
        if (questId >= 105)
        {
            return 105;
        }
        else
        {
            return questId;
        }
    }



    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    public void NextQuest()
    {
        if (isClear)
        {
            isClear = false;
            C_UpdateQuestInfo beforequestInfo = new C_UpdateQuestInfo();
            beforequestInfo.questId = questId;
            beforequestInfo.questState = 2;
            beforequestInfo.playerId = NetPlayerManager.Instance._playerManager.PlayerId;
            Debug.Log($"playermanager in playerId : {NetPlayerManager.Instance._playerManager.PlayerId}");
            Debug.Log($"manager in playerId : {Manager.Instance.player.PlayerId}");
            NetPlayerManager.Instance.Session.Send(beforequestInfo.Write());
            questId += 1;
            C_UpdateQuestInfo afterquestInfo = new C_UpdateQuestInfo();
            afterquestInfo.questId = questId;
            afterquestInfo.questState = 1;
            afterquestInfo.playerId = NetPlayerManager.Instance._playerManager.PlayerId;

            NetPlayerManager.Instance.Session.Send(afterquestInfo.Write());


            ControlObject();
        }
    }

    public void ControlObject()
    {
        switch (questId)
        {
            case 104:
                QuestObject.GetComponent<QuestChest>().SetChestActive(true);
                break;
        }
    }

    public void QuestClear(Transform transform)
    {
        isClear = true;
        isLook = false;
        transform.GetComponent<PlayerStat>().Gold += questList[questId].gold;

        C_SendMoney sendMoney = new C_SendMoney();
        sendMoney.money = transform.root.GetComponent<PlayerStat>().Gold;
        NetPlayerManager.Instance.Session.Send(sendMoney.Write());

        transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TalkUI>().QuestText.text = " 퀘스트 완료!!";
        NextQuest();
    }
}