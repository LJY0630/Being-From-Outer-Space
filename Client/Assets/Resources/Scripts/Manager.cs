using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    GameObject character;

    static Manager m_instance;

    public static Manager Instance { get { Init(); return m_instance; } }

    public PlayerManager player;
    public static NetPlayerManager network;
    SceneManagement scene = new SceneManagement();

    // �̰�  dictionary �� ���� ���̵�� invoke�Լ� ����
    //public static PlayerManager Input { get { return Instance.player; } }
    public static SceneManagement Scene { get { return Instance.scene; } }

    public GameObject ControlPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (m_instance == null)
        {
            // �̱��� ���� ���⼭ ĳ���� �ѹ� �����ϴ°ſ���.
            Init();
            //ControlPlayer = Instantiate(character, new Vector3(-448.87f, -30.68f, 39.93f), Quaternion.identity);
            //player.Init();
        }
    }

    private void FixedUpdate()
    {
        if (ControlPlayer != null)
        {
            //player.OnUpdate();
        }
    }

    private void Update()
    {
        if (ControlPlayer != null && player != null)
        {
            // �� ��? network
            foreach (NetPlayerManager.PlayerInfo p in network._players.Values)
            {
                // �������� �ʾƼ� �ϴ� �߰���. �ٸ� ���� input �Լ� ����Ǵ� �� ������ ���������
                if (NetPlayerManager.Instance._playerManager.PlayerId == p.manager.PlayerId)
                    p.manager.Input();
                p.manager.OnUpdate();
            }
        }
    }
    static void Init() 
    {
        GameObject single = GameObject.Find("*Manager");
        GameObject networkObj = GameObject.Find("NetworkManager");
        network = NetPlayerManager.Instance;
        if (single == null) 
        {
            single = new GameObject { name = "*Manager" };
            single.AddComponent<Manager>();
        }
        DontDestroyOnLoad(single);
        //DontDestroyOnLoad(networkObj);

        m_instance = single.GetComponent<Manager>();
    }
}
