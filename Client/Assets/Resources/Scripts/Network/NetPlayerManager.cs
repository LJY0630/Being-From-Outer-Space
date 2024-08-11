using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using ServerCore;
// 서버로부터 패킷을 수신하는 클래스
public class NetPlayerManager : MonoBehaviour
{
    public class PlayerInfo
    {
        public PlayerManager manager;
        public Vector3 moveDir;
        public Rigidbody rigid;
        public Other_PlayerController opc;
        public PlayerController pc;
    }
    public Dictionary<int, PlayerInfo> _players = new Dictionary<int, PlayerInfo>();

    PlayerController _myPlayer;
    public PlayerManager _playerManager;
    public CameraController camera;
    static NetPlayerManager _net;
    public static NetPlayerManager Instance { get { return _net; } }
    public ServerSession Session { get; set; } = null;

    public bool isHost = false;

    public bool isBossLoad = false;

    public bool inBoss = false;

    private void Awake()
    {
        if (_net == null)
        {
            Debug.Log("Awake함수 netplayerManager");
            _net = GameObject.Find("NetPlayerManager").GetComponent<NetPlayerManager>();

            DontDestroyOnLoad(gameObject);

            if (_net == null)
                Debug.Log("여전히 널임");
        }
    }


    public PlayerManager GetPlayerManagerById(int id)
    {
        PlayerInfo player = null;
        if (_players.TryGetValue(id, out player))
        {
            return player.manager;
        }
        else
            return null;
    }

    // 처음에 입장했을 때 받은 플레이어 리스트
    public void Add(S_PlayerList packet)
    {
        //Debug.Log("Add 함수 들어옴");
        foreach (S_PlayerList.Player p in packet.players)
        {
            if (p.isSelf)
            {
                UnityEngine.Object obj = Resources.Load("Prefabs/Character");
                GameObject go = UnityEngine.Object.Instantiate(obj) as GameObject;
                PlayerManager player = go.AddComponent<PlayerManager>();

                go.name = "Character" + p.playerId;
                PlayerController myPlayer = go.GetComponentInChildren<PlayerController>();

                Manager.Instance.ControlPlayer = go;
                Manager.Instance.player = player;
                player.PlayerId = p.playerId;
                _playerManager = player;

                player.transform.position = new Vector3(-448.87f, -30.7f, 39.93f);

                _myPlayer = myPlayer;
                PlayerInfo playerInfo = new PlayerInfo();
                Rigidbody rigid = player.gameObject.GetComponentInChildren<Rigidbody>();
                playerInfo.manager = player;
                playerInfo.rigid = rigid;
                playerInfo.pc = myPlayer;
                playerInfo.moveDir = Vector3.forward;
                _players.Add(p.playerId, playerInfo);
                player.Init(true);
                myPlayer.Init();
                C_RequestQuestInfo questP = new C_RequestQuestInfo();
                Session.Send(questP.Write());
                C_RequestPlayerInfo invenP = new C_RequestPlayerInfo();
                Session.Send(invenP.Write());
                // 호스트면
                if (isHost)
                    SpawnManager.instance.InitF();

                camera = player.gameObject.GetComponentInChildren<CameraController>();

                // 다시 한번 보낸다. 
                // 자신의 저장된 정보를 달라고 
            }
            else
            {
                UnityEngine.Object obj = Resources.Load("Prefabs/Other_Character");
                GameObject go = UnityEngine.Object.Instantiate(obj) as GameObject;
                PlayerManager player = go.AddComponent<PlayerManager>();

                go.name = "Character" + p.playerId;
                Rigidbody rigid = player.gameObject.GetComponentInChildren<Rigidbody>();
                Other_PlayerController opc = player.GetComponentInChildren<Other_PlayerController>();

                player.PlayerId = p.playerId;
                player.transform.position = new Vector3(-448.87f, -30.7f, 39.93f);

                rigid.gameObject.transform.localPosition = new Vector3(p.posX, p.posY, p.posZ);

                PlayerInfo playerInfo = new PlayerInfo();
                playerInfo.manager = player;
                playerInfo.rigid = rigid;
                playerInfo.opc = opc;

                playerInfo.moveDir = Vector3.forward;
                //if(_players.TryGetValue(p.playerId,out playerInfo)==false)
                _players.Add(p.playerId, playerInfo);

                player.Init(false);

                go.transform.root.GetComponentInChildren<PlayerStat>().Hp = 20000;
            }
        }
    }


    // 키 누를 때 방향 보내줘야함 현재 위치하고
    public void Move(S_BroadcastMove packet)
    {
        if (isBossLoad)
            return;

        // 자신이면 클라이언트에서 이동시키기때문에 상관x
        if (_playerManager.PlayerId == packet.playerId)
        {
            return;
        }
        // 다른 플레이어면
        else
        {
            if (isBossLoad == true)
                return;

            PlayerInfo player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                //Debug.Log("캐릭터 찾음! Id : " + packet.playerId + $" isMoving = {packet.isMoving}");

                if (packet.isMoving == true)
                {
                    if (!player.manager.isSkill && !player.manager.isAttack)
                    {
                        if (packet.moveSpeed == 5.0f)
                        {
                            player.manager.playerAnim.SetBool("Walk", true);
                            player.manager.playerAnim.SetBool("Run", false);
                        }
                        else if (packet.moveSpeed == 10.0f)
                        {
                            player.manager.playerAnim.SetBool("Walk", true);
                            player.manager.playerAnim.SetBool("Run", true);
                        }
                    }
                    if(!player.opc.playerhit.isHit)
                    // 호출되는 함수에서 자연스런 움직임 구현하면 됨.
                        player.rigid.GetComponentInParent<PlayerMove_Server>().MovePlayer(packet);
                }
                else
                {
                    if (!player.manager.isSkill && !player.manager.isAttack)
                    {
                        player.manager.playerAnim.SetBool("Walk", false);
                        player.manager.playerAnim.SetBool("Run", false);
                    }
                    player.rigid.GetComponentInParent<PlayerMove_Server>().MovePlayer(packet);
                }
            }
        }
    }



    // 아이디 찾고 해당 오브젝트의 other_playercontroller불러서 함수실행 방식?
    public void Jump(S_BroadcastJump packet)
    {
        if (_playerManager.PlayerId == packet.playerId)
            return;

        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            player.manager.player.GetComponent<Other_PlayerController>().DoJump();
        }
    }

    public void Attack(S_BroadcastAttack packet)
    {
        // packet.playerId로 캐릭터 찾아서 공격 애니메이션 재생
        if (_playerManager.PlayerId == packet.playerId)
            return;

        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            player.manager.playerAnim.SetBool("Attack1", true);
            player.manager.playerAnim.CrossFade("Attack1", 0.1f);
            player.manager.player.GetComponent<Other_PlayerController>().DoAttack();
        }
    }


    public void Skill(S_BroadcastSkill packet)
    {
        if (_playerManager.PlayerId == packet.playerId)
            return;

        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            switch (packet.skillNum)
            {
                case 1:
                    player.manager.playerAnim.SetBool("Skill1", true);
                    player.manager.playerAnim.CrossFade("Skill1", 0.1f);
                    player.manager.player.GetComponent<Other_PlayerController>().DoSkill1();
                    break;
                case 2:
                    player.manager.playerAnim.SetBool("Skill2", true);
                    player.manager.playerAnim.CrossFade("Skill2", 0.1f);
                    player.manager.player.GetComponent<Other_PlayerController>().DoSkill2();
                    break;
            }
        }
    }


    // 다른 유저가 입장했을 때
    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (packet.playerId == _playerManager.PlayerId)
            return;

        UnityEngine.Object obj = Resources.Load("Prefabs/Other_Character");
        GameObject go = UnityEngine.Object.Instantiate(obj) as GameObject;
        go.name = "Character" + packet.playerId;
        PlayerManager player = go.AddComponent<PlayerManager>();

        Other_PlayerController opc = player.GetComponentInChildren<Other_PlayerController>();

        player.PlayerId = packet.playerId;

        player.transform.position = new Vector3(-448.87f, -30.7f, 39.93f);

        PlayerInfo p = new PlayerInfo();
        p.manager = player;
        p.moveDir = Vector3.forward;
        p.rigid = player.GetComponentInChildren<Rigidbody>();
        p.opc = opc;

        p.rigid.gameObject.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        // 뭔가 여기서 opc가 저장이안되는듯 널문제발생


        //if (_players.TryGetValue(packet.playerId, out p) == false)
        _players.Add(packet.playerId, p);
        player.Init(false);


        // 만약 첫번째 유저면 다른유저에게 몬스터 정보보내주기
        if (isHost == true)
        {
            C_MonsterSpawn c_Monster = new C_MonsterSpawn();

            c_Monster.to_playerId = packet.playerId;
            foreach(GameObject mon in SpawnManager.instance.monsterList.Values)
            {
                C_MonsterSpawn.MonsterId id = new C_MonsterSpawn.MonsterId();
                id.monsterid = mon.GetComponent<EnemyController>().monsterId;
                c_Monster.monsterIds.Add(id);

                C_MonsterSpawn.PositionX px = new C_MonsterSpawn.PositionX();
                px.posX = mon.transform.position.x;
                c_Monster.positionXs.Add(px);

                C_MonsterSpawn.PositionY py = new C_MonsterSpawn.PositionY();
                py.posY = mon.transform.position.y;
                c_Monster.positionYs.Add(py);

                C_MonsterSpawn.PositionZ pz = new C_MonsterSpawn.PositionZ();
                pz.posZ = mon.transform.position.z;
                c_Monster.positionZs.Add(pz);

                C_MonsterSpawn.WayPoint way = new C_MonsterSpawn.WayPoint();
                way.way0_X = mon.GetComponent<EnemyController>().waypoints[0].x;
                way.way0_Y = mon.GetComponent<EnemyController>().waypoints[0].y;
                way.way0_Z = mon.GetComponent<EnemyController>().waypoints[0].z;
                way.way1_X = mon.GetComponent<EnemyController>().waypoints[1].x;
                way.way1_Y = mon.GetComponent<EnemyController>().waypoints[1].y;
                way.way1_Z = mon.GetComponent<EnemyController>().waypoints[1].z;
                c_Monster.wayPoints.Add(way);
            }
            Session.Send(c_Monster.Write());
        }
    }

    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        if (_playerManager.PlayerId == packet.playerId)
        {
            GameObject.Destroy(_myPlayer.gameObject);
            _myPlayer = null;
        }
        else
        {
            PlayerInfo player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                GameObject.Destroy(player.manager.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }

    public void DeadPlayer(S_BroadcastDead packet)
    {
        if (_playerManager.PlayerId == packet.playerId)
            return;

        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            player.opc.startCoroutineDead();
            Debug.Log("데드플레이어패킷받음");
        }
    }

    public void RespawnPlayer(S_BroadcastRespawnPlayer packet)
    {
        if (_playerManager.PlayerId == packet.playerId)
            return;

        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            player.opc.transform.root.GetComponentInChildren<DeadUI>().InstantRespawnServer(player.opc);
            Debug.Log("리스폰플레이어패킷받음");
        }
    }

    public void PlayerStat(S_BroadcastPlayerStat packet)
    {
        if (packet.playerId ==_playerManager.PlayerId)
            return;

        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            player.opc.playerstat.Attack = packet.attack;
            player.opc.playerstat.heal = packet.heal;
            player.opc.playerstat.power = packet.power;
            player.opc.playerstat.magic = packet.magic;
        }
    }

    public void PotionEat(S_BroadcastPotionEat packet)
    {
        if (_playerManager.PlayerId == packet.playerId)
            return;

        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            // heal Potion
            if(packet.potionType==1)
            {
                player.rigid.gameObject.GetComponentInChildren<EffectManager>().HealCo();
            }
            // mana potion
            else
            {
                player.rigid.gameObject.GetComponentInChildren<EffectManager>().ManaCo();
            }
            
        }
    }

    public void LoadBossScene()
    {
        camera.boss.Only();// 씬 입장후 리스트를 다시 한번 줘야 할듯 플레이어
    }

    public void BossPaternRecv(S_BroadcastRandBossPatern packet)
    {
        // 호스트는 이미 정해서 전송하기 때문에 받을 필요 x
        if (isHost)
            return;
        GameObject boss = _playerManager.findBoss();
        boss.GetComponent<Boss>().isRecv = true;
        boss.GetComponent<Boss>().randomAction = packet.paternNum;
        boss.GetComponent<Boss>().recvPacket = packet;
    }

    public void BossDamaged(S_BroadcastBossDamaged packet)
    {
        if (NetPlayerManager.Instance._playerManager.PlayerId == packet.playerId)
            return;
        GameObject boss = _playerManager.findBoss();
        boss.GetComponent<Boss>().stat.Hp -= packet.damaged;
        if(boss.GetComponent<Boss>().stat.Hp<=0)
        {
            boss.GetComponent<Boss>().stat.isDead = true;
            
        }
    }

    public void BossDead()
    {
        GameObject boss = _playerManager.findBoss();
        boss.GetComponent<Boss>().stat.isDead = true;
    }

    public void MonsterDead(S_BroadcastMonsterDead packet)
    {
        //if (packet.playerId == _playerManager.PlayerId)
          //  return;

        SpawnManager.instance.DeadMonster(packet);
    }

    // 한번 테스트해봐야함
    public void UpdateChat(S_BroadcastChat packet)
    {
        if (packet.playerId == _playerManager.PlayerId)
            return;

        _playerManager.testChat.UpdateChatText(packet);
    }

    public void ReceivePlayerInfo(S_PlayerInfo packet)
    {
        // 자신이 받아야함
        if (packet.playerId != _playerManager.PlayerId)
        {
            Debug.Log("자신의 정보가 아님!");
            return;
        }

        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            player.pc.InvenInit(packet);
        }

        // 정보를 업데이트해야함 
        //Debug.Log($"{packet.playerId}번 유저 정보\n 돈 : {packet.Money} 아이템 id : {packet.itemss[0]} 아이템 개수 : {packet.itemss.Count}");
        // 플레이어 스탯관련 
        // 플레이어 인벤토리 관련
    }

    public void UpdateExp(S_BroadcastExp packet)
    {
        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            player.rigid.gameObject.GetComponent<PlayerStat>().GetExp += packet.exp;
            Debug.Log($"업데이트 패킷 받음  현재 경험치 : {player.rigid.gameObject.GetComponent<PlayerStat>().GetExp}");
            player.rigid.gameObject.GetComponentInChildren<EffectManager>().LevelCo();
        }
    }

    public void UpdateLevel(S_SendLevel packet)
    {
        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            player.rigid.gameObject.GetComponentInChildren<EffectManager>().LevelCo();
        }
    }

    public void ReceiveQuestInfo(S_QuestInfo packet)
    {
        Debug.Log($"퀘스트 정보 수신 완료 : 퀘스트 개수 : {packet.questInfos.Count}");

        // 퀘스트 정보 저장하면 됨.
        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            player.pc.QuestInit(packet);
        }
    }

    // 캐릭터아이디
    // 캐릭터아이디
    public void UpdateEquipped(S_BroadcastEquipped packet)
    {
        // 캐릭터 아이디로 찾고
        // 해당 아이템을 장착시킨다.
        if (packet.playerId == _playerManager.PlayerId)
            return;

        // 플레이어인포에 스크립트 더 넣어놔야할듯
        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            Debug.Log("UpdateEquipped 실행");

            // 장비 아이디에 따라서 다르게 호출
            int pid = Convert.ToInt32(packet.itemId);

            string itemName = GameObject.Find("ItemDataBase").GetComponent<ItemDataBase>().
                FindItemName(Convert.ToInt32(packet.itemId));
            if ((pid >= 2 && pid <= 6) || (pid >= 22 && pid <= 26) || (pid >= 42 && pid <= 46))
            {
                // head
                Debug.Log($"{packet.playerId}번 캐릭터 : {pid}번 모자 장착");
                player.opc.changItemHead.SetItem(itemName);
            }
            else if ((pid >= 7 && pid <= 11) || (pid >= 27 && pid <= 31) || (pid >= 47 && pid <= 51))
            {
                // body
                Debug.Log($"{packet.playerId}번 캐릭터 : {pid}번 갑옷 장착");
                player.opc.changItemBody.SetItem(itemName);
            }
            else if ((pid >= 12 && pid <= 16) || (pid >= 32 && pid <= 36) || (pid >= 52 && pid <= 56))
            {
                // leg
                Debug.Log($"{packet.playerId}번 캐릭터 : {pid}번 leg 장착");
                player.opc.changItemLeg.SetItem(itemName);
            }
            else if ((pid >= 17 && pid <= 21) || (pid >= 37 && pid <= 41) || (pid >= 57 && pid <= 61))
            {
                // foot
                Debug.Log($"{packet.playerId}번 캐릭터 : {pid}번 foot 장착");
                player.opc.changItemFoot.SetItem(itemName);
            }
            else
            {
                // weapon
                Debug.Log($"{packet.playerId}번 캐릭터 : {pid}번 무기 장착");
                player.opc.owc.WeaponChange(itemName);
            }

        }
    }

    public void UpdateUnEquipped(S_BroadcastUnEquipped packet)
    {
        // 캐릭터 아이디로 찾고
        // 해당 아이템을 해제시킨다.
        if (packet.playerId == _playerManager.PlayerId)
            return;

        PlayerInfo player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            Debug.Log("UpdateUnEquipped 실행");
            int pid = Convert.ToInt32(packet.itemId);

            string itemName = GameObject.Find("ItemDataBase").GetComponent<ItemDataBase>().
                FindItemName(Convert.ToInt32(packet.itemId));
            if ((pid >= 2 && pid <= 6) || (pid >= 22 && pid <= 26) || (pid >= 42 && pid <= 46))
            {
                // head
                Debug.Log($"{packet.playerId}번 캐릭터 : {pid}번 모자 해제");
                player.opc.changItemHead.NoEquip();
            }
            else if ((pid >= 7 && pid <= 11) || (pid >= 27 && pid <= 31) || (pid >= 47 && pid <= 51))
            {
                // body
                Debug.Log($"{packet.playerId}번 캐릭터 : {pid}번 갑옷 해제");
                player.opc.changItemBody.NoEquip();
            }
            else if ((pid >= 12 && pid <= 16) || (pid >= 32 && pid <= 36) || (pid >= 52 && pid <= 56))
            {
                // leg
                Debug.Log($"{packet.playerId}번 캐릭터 : {pid}번 leg 해제");
                player.opc.changItemLeg.NoEquip();
            }
            else if ((pid >= 17 && pid <= 21) || (pid >= 37 && pid <= 41) || (pid >= 57 && pid <= 61))
            {
                // foot
                Debug.Log($"{packet.playerId}번 캐릭터 : {pid}번 foot 해제");
                player.opc.changItemFoot.NoEquip();
            }
            else
            {
                // weapon
                Debug.Log($"{packet.playerId}번 캐릭터 : {pid}번 무기 해제");
                player.opc.owc.SetNothing();
            }
        }
    }


    // 씬 시작
    public void AuthenticationLogin(S_CheckLogin packet)
    {
        //Debug.Log("로그인 완료");

        // 로그인 체크에 표시하기
        // 여부에 따라 start버튼 누를 수 있게
        GameObject.Find("Canvas").GetComponent<StartUI>().SetLoginCheck(packet.successLogin);
    }

    public void SetHost(S_SelectHost packet)
    {
        Debug.Log("호스트로 정해졌습니다.");
        isHost = true;
    }

    public void ResultRegister(S_SuccessRegister packet)
    {
        GameObject.Find("Canvas").GetComponent<StartUI>().SetRegisterCheck(packet.Registed);
    }
}