using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartUI : MonoBehaviour
{
    bool logined = false;
    [SerializeField]
    private GameObject[] Scene;

    [SerializeField]
    private GameObject NextButton;

    [SerializeField]
    private TextMeshProUGUI LoginID;

    [SerializeField]
    private TextMeshProUGUI LoginPassword;

    [SerializeField]
    private GameObject RegisterPanel;

    [SerializeField]
    private TextMeshProUGUI RegisterID;

    [SerializeField]
    private TextMeshProUGUI RegisterPassword;

    [SerializeField]
    private TextMeshProUGUI RegisterCheck;

    [SerializeField]
    private TextMeshProUGUI LoginCheck;

    [SerializeField]
    private TextMeshProUGUI firstLoginCheck;

    int scenecount = 0;


    // ���۹�ư ������ �� ��Ʈ��ũ �Ŵ��� ����ǰ� �ٲٱ�
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);

        for (int i = 0; i < Scene.Length; i++)
        {
            if (i == 0)
            {
                Scene[i].SetActive(true);
            }
            else
            {
                Scene[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    public void ActiveNextButton()
    {
        if (scenecount < Scene.Length - 1)
        {
            Scene[scenecount].SetActive(false);
            scenecount++;
            if (scenecount == 5)
            {
                firstLoginCheck.gameObject.SetActive(false);
                RegisterPanel.gameObject.SetActive(false);
                NextButton.SetActive(false);
            }
            Scene[scenecount].SetActive(true);
        }
    }
    public void ActiveMianScene()
    {
        if (logined == false)
            return;

        LoadingUI.LoadScene("SampleScene");
    }

    public void RegisterOpenButton()
    {
        //Debug.Log("ȸ�����Թ�ư����");
        RegisterPanel.SetActive(true);
        RegisterCheck.gameObject.SetActive(false);
    }

    public void RegisterQuitButton()
    {
        RegisterPanel.SetActive(false);
    }

    public void LoginButton()
    {
        SendLoginPacket();
    }

    void SendLoginPacket()
    {
        if (LoginID.text.Length<1 || LoginPassword.text.Length<1)
        {
            Debug.Log("���̵�� ��й�ȣ�� ���ڿ��� �Ұ����մϴ�.");
            return;
        }
        Debug.Log($"id : {LoginID.text}");
        Debug.Log($"pass : {LoginPassword.text}");
        C_RequestLogin p = new C_RequestLogin();
        p.Id = LoginID.text;
        p.Password = LoginPassword.text;
        NetPlayerManager.Instance.Session.Send(p.Write());
        //Debug.Log("�α��ι�ư ����");
    }

    public void SetLoginCheck(bool login)
    {
        // �ƴϸ� �α���üũ Text True, Text �������� �ȵǾ��ٰ� �˷��ֱ�
        LoginCheck.gameObject.SetActive(true);
        if (login == true)
        {
            string id = LoginID.text;

            Scene[scenecount].SetActive(false);
            scenecount++;
            Scene[scenecount].SetActive(true);

            LoginCheck.text = id + " Success Login!";
            logined = true;
        }
        else
        {
            firstLoginCheck.gameObject.SetActive(true);
            firstLoginCheck.text = "Failed Login! Try again";
            logined = false;
        }
        Debug.Log($"�α��� ��� : {login}");
    }

    public void RegisterCheckButton()
    {
        // RegisterCheck�� Text ��ü�� ���� ��ϵ�, �ߺ��� Check
        //Debug.Log("Register22");

        SendRegisterPacket();

    }

    void SendRegisterPacket()
    {
        if (RegisterID.text == "" || RegisterPassword.text == "")
        {
            Debug.Log("���̵�� ��й�ȣ�� ���ڿ��� �Ұ����մϴ�.");

            return;
        }

        C_RequestRegist p = new C_RequestRegist();
        p.Id = RegisterID.text;
        p.Password = RegisterPassword.text;
        NetPlayerManager.Instance.Session.Send(p.Write());
    }

    public void SetRegisterCheck(bool regist)
    {
        RegisterCheck.gameObject.SetActive(true);
        if (regist)
        {
            RegisterCheck.text = "Success!";
        }
        else
        {
            RegisterCheck.text = "Failed!";
        }
        Debug.Log($"ȸ������ ��� : {regist}!");
    }
}