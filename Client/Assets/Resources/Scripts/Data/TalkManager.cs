using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager
{
    Dictionary<int, string[]> talkData;

    public void Init()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData() 
    {
        talkData.Add(1 + 100, new string[] {"도와주셔서 감사합니다.", "부탁드리겠습니다"});
        talkData.Add(1 + 101, new string[] { "도와주셔서 감사합니다.", "우선 저 앞에 대장간에서 무기를 구매하시고 오세요", 
            "특별히 제일 낮은 등급의 무기는 공짜로 해드리겠습니다.", "우클릭으로 장비를 사고 좌클릭으로 장비를 팔 수 있습니다."});
        talkData.Add(1 + 102, new string[] {"잘하셨습니다. 무기가 마음에 들으셨으면 좋겠네요.", "무기는 총 세가지 종류로 회복사, 마법사, 전사의 무기로 구별되고무기에 따라 스킬과 공격이 다릅니다. 이미 숱한 경험을 가지셔서 다 아시겠지만요",
            "자 이제 다음으로 필드에 있는 스켈레톤을 격파하여 마을에 들어오는 보급이 다시 들어오게끔 도와주시면 감사하겠습니다."});
        talkData.Add(1 + 103, new string[] {"스켈레톤 전사의 세력이 조금 약해졌군요! 정말 좋습니다. 다음은 스켈레톤 아쳐 10마리를 격파 하시면 감사하겠습니다. 먼저 스켈레톤 전사들을 격파해야 스켈레톤 궁수가 있는 곳으로 갈 수 있으니 주의하세요"});
        talkData.Add(1 + 104, new string[] { "감사합니다. 이제 다시 보급이 들어올 수 있겠군요.", "문제는 다시 보내기 전에 버틸 물자가 없다는 겁니다.", "필드에 있는 미쳐 마을까지 오지 못한 물자들을 직접 가져와야 버틸 수 있는 실정입니다.", "필드에서 보급을 수색하고 찾아와주시면 감사하겠습니다." });
        talkData.Add(1 + 105, new string[] { "정말 감사드립니다.", "이제는 진짜 끝을 보아야 할 때가 왔습니다. 이 사태를 일으킨 원인이 되는 충돌 현장에 가서 무엇이 있는지 확인하여 다시 이 마을에 평화를 가져다 주시면 감사하겠습니다." });
        talkData.Add(1 + 106, new string[] { "다시 마을에 평화가 찾아왔습니다. 감사합니다.", "이 은혜를 잊지 안겠습니다." });
    }

    // 서버에서는 ( 퀘스트, 상태 )
    public string GetTalk(int id, int talkIndex) 
    {

        if (talkIndex >= talkData[id].Length)
        {
            return null;
        }
        else 
        {
            return talkData[id][talkIndex];
        }
    }
}
