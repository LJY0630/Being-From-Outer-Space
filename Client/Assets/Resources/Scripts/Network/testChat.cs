using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class testChat : MonoBehaviour
{
    [SerializeField]
    TMP_InputField input;

    [SerializeField]
    public TextMeshProUGUI[] texts = new TextMeshProUGUI[6];

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InputChat()
    {
        Debug.Log("ä�� �Է� �Ϸ�");
        C_Chat c_Chat = new C_Chat();
        c_Chat.chat = input.text;
        NetPlayerManager.Instance.Session.Send(c_Chat.Write());

        for (int i = 5; i >= 1; i--)
            texts[i].text = texts[i - 1].text;

        texts[0].text = input.text;
        input.text = "";
    }

    public void UpdateChatText(S_BroadcastChat packet)
    {
        // ���� ��ĭ �� �ø���
        Debug.Log("ä�� ��Ŷ �޾Ƽ� �����!");

        for(int i=5;i>=1;i--)
            texts[i].text = texts[i-1].text;

        texts[0].text = $"{packet.playerId}�� ���� : {packet.chat}";
    }
}
