using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    public string questName;
    public int[] npcId;
    public int gold;

    public QuestData(string name, int[] npc, int gold)
    {
        questName = name;
        npcId = npc;
        this.gold = gold;
    }

}
