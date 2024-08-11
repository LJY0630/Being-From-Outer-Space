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

    // 이걸  dictionary 로 만들어서 아이디로 invoke함수 실행
    //public static PlayerManager Input { get { return Instance.player; } }
    public static SceneManagement Scene { get { return Instance.scene; } }

    public GameObject ControlPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (m_instance == null)
        {
            // 싱글일 때는 여기서 캐릭터 한번 생성하는거였음.
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
            // 왜 널? network
            foreach (NetPlayerManager.PlayerInfo p in network._players.Values)
            {
                // 움직이지 않아서 일단 추가함. 다른 곳에 input 함수 실행되는 곳 있으면 살펴봐야함
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
