using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TalkUI : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    public TextMeshProUGUI NPCname;
    public TextMeshProUGUI Text;
    public TextMeshProUGUI OpeningText;
    public TextMeshProUGUI QuestText;
    public GameObject TextPanel;
    public bool isTexting = false;

    void Start()
    {
       TextPanel.SetActive(false);
       OpeningText.gameObject.SetActive(false);
       QuestText.text = player.playerManager.quest.CheckQuest();
    }

    public void SetNPCName(int id) 
    {
        if (id == 1) 
        {
            NPCname.text = "마을대장";
        }
    }

    public void ActiveTalk(bool active) 
    {
        TextPanel.SetActive(active);
        isTexting = active;
    }

    public void SetNPCTalkText(string talk) 
    {
        Text.text = talk;
    }

    public void ActiveTalkBefore(bool active) 
    {
        OpeningText.gameObject.SetActive(active);
    }
}
