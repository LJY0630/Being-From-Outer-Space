using ServerCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
namespace Server
{
    class GameRoom : IJobQueue
    {
        public class BarterInfo
        {
            public int rePid, sePid;
            // ok 변수는 거래창에서 승인완료를 눌렀는지를 저장하기 위한 변수.
            public bool reOk = false, seOk = false;
        }
        public List<BarterInfo> barterList = new List<BarterInfo>();

        List<ClientSession> _sessions = new List<ClientSession>();
        JobQueue _jobQueue = new JobQueue();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        int bossEnterUserCnt = 0;

        UserInfoDB myUserInfo = null;
        bool isEntered = false;
        // 호스트 관련 변수 
        bool isFirst = false;
        int onlinePlayerCnt = 0;
        int hostPlayerId = -1;

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
            foreach (ClientSession s in _sessions)
                s.Send(_pendingList);
            //Console.WriteLine($"Flushed {_pendingList.Count} items");
            _pendingList.Clear();
        }

        public void Broadcast(ArraySegment<byte> segment)
        {
            _pendingList.Add(segment);
        }

        public void Init(ClientSession session)
        {
            S_SetServerSession p = new S_SetServerSession();

            session.Room = this;
            Console.WriteLine("Init패킷보냄");
            session.Send(p.Write());
        }

        public void Enter(ClientSession session)
        {
            if (isFirst == false)
            {
                // 호스트로 지정
                isFirst = true;
                hostPlayerId = session.SessionId;
                S_SelectHost s_SelectHost = new S_SelectHost();
                s_SelectHost.hostId = hostPlayerId;
                session.Send(s_SelectHost.Write());
            }
            onlinePlayerCnt = onlinePlayerCnt + 1;
            Console.WriteLine($"온라인 접속 수 : {onlinePlayerCnt}");
            // 플레이어 추가하고
            _sessions.Add(session);
            //session.Room = this;

            // 신입생한테 모든 플레이어 목록 전송
            S_PlayerList players = new S_PlayerList();
            foreach (ClientSession s in _sessions)
            {
                players.players.Add(new S_PlayerList.Player()
                {
                    isSelf = (s == session),
                    playerId = s.SessionId,
                    posX = s.PosX,
                    posY = s.PosY,
                    posZ = s.PosZ,
                });
            }
            session.Send(players.Write());

            foreach (ClientSession s in _sessions)
            {

                foreach (Item it in s.PlayerInfo.info.items.Values)
                {
                    if (it.isEqiped)
                    {
                        S_BroadcastEquipped s_BroadcastEquipped = new S_BroadcastEquipped();
                        s_BroadcastEquipped.playerId = s.SessionId;
                        s_BroadcastEquipped.itemId = it.id;
                        session.Send(s_BroadcastEquipped.Write());
                    }
                }

            }

            Thread.Sleep(200);



            // 신입생 입장을 모두에게 알린다
            S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
            enter.playerId = session.SessionId;

            enter.posX = session.PosX;
            enter.posY = session.PosY;
            enter.posZ = session.PosZ;

            //Console.WriteLine($"x: {enter.posX}, y: {enter.posY}, z: {enter.posZ} 보냄!");
            Broadcast(enter.Write());

            isEntered = true;

            Console.WriteLine($"{session.SessionId}번 유저 입장!");
        }

        public void Leave(ClientSession session)
        {
            Console.WriteLine($"{session.SessionId}번 유저 퇴장!");

            UserInfoDB uid = UserDB.Instance.GetUserInfo(myUserInfo.id);
            uid.logined = false;

            uid.session = session;

            if (isEntered == false)
            {
                return;
            }

            Console.WriteLine($"user Longined? => {UserDB.Instance.GetUserInfo(myUserInfo.id).logined}");

            Console.WriteLine($"uid.session : {uid.session.PlayerInfo.info.money}");
            Console.WriteLine($"session : {session.PlayerInfo.info.money}");

            //Console.WriteLine($"로그아웃 db저장 받은 패킷\n 현재 저장할 아이템 개수 : {session.PlayerInfo.info.items.Count}");

            //foreach (Item it in session.PlayerInfo.info.items.Values)
            {
                //Console.WriteLine($"id : {it.id}, cnt : {it.cnt}, isEquip : {it.isEqiped}");
            }

            //Console.WriteLine($"로그아웃 db 저장 패킷 \n 저장할 아이템 개수 : {uid.session.PlayerInfo.info.items.Count}");
            //foreach (Item it in uid.session.PlayerInfo.info.items.Values)
            {
                //Console.WriteLine($"id : {it.id}, cnt : {it.cnt} ");
            }

            // 플레이어 제거하고
            _sessions.Remove(session);

            // 모두에게 알린다
            S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
            leave.playerId = session.SessionId;
            Broadcast(leave.Write());

            if (onlinePlayerCnt > 0)
                onlinePlayerCnt--;

            if (onlinePlayerCnt <= 0)
                isFirst = false;
            Console.WriteLine($"온라인 접속 수 : {onlinePlayerCnt}");
            if (_sessions.Count == 0)
            {
                hostPlayerId = -1;
                isFirst = false;
                Console.WriteLine("호스트 초기화");
            }
            // 새로운 호스트 지정
            foreach (ClientSession s in _sessions)
            {
                hostPlayerId = s.SessionId;
                S_SelectHost s_SelectHost = new S_SelectHost();
                s_SelectHost.hostId = hostPlayerId;
                s.Send(s_SelectHost.Write());
                break;
            }
        }

        public void Move(ClientSession session, C_Move packet)
        {
            // 좌표 바꿔주고
            session.DirectionX = packet.directionX;
            session.DirectionY = packet.directionY;
            session.DirectionZ = packet.directionZ;
            session.PosX = packet.posX;
            session.PosY = packet.posY;
            session.PosZ = packet.posZ;
            session.IsMoving = packet.isMoving;

            // 모두에게 알린다
            S_BroadcastMove move = new S_BroadcastMove();
            move.playerId = session.SessionId;
            move.directionX = session.DirectionX;
            move.directionY = session.DirectionY;
            move.directionZ = session.DirectionZ;
            move.posX = session.PosX;
            move.posY = session.PosY;
            move.posZ = session.PosZ;
            move.isMoving = session.IsMoving;
            move.moveSpeed = packet.moveSpeed;
            Broadcast(move.Write());
            //if(packet.moveSpeed!=0)
            //Console.WriteLine($"moveSpeed : {packet.moveSpeed}");
            //Console.WriteLine($"C_Move패킷 {session.SessionId}번의 위치 => x:{session.PosX},y:{session.PosY},z:{session.PosZ}");
            //Console.WriteLine($"C_Move패킷 {session.SessionId}번의 방향 => x:{session.DirectionX},y:{session.DirectionY},z:{session.DirectionZ}");
        }

        public void Jump(ClientSession session, C_Jump packet)
        {
            S_BroadcastJump p = new S_BroadcastJump();

            p.playerId = session.SessionId;

            Broadcast(p.Write());

            Console.WriteLine($"{session.SessionId}번 유저 점프!");
        }

        public void DeadPlayer(ClientSession session, C_Dead packet)
        {
            S_BroadcastDead p = new S_BroadcastDead();
            p.playerId = session.SessionId;
            Broadcast(p.Write());
        }

        public void RespawnPlayer(ClientSession session, C_RespawnPlayer packet)
        {
            Console.WriteLine("리스폰 패킷받음");
            S_BroadcastRespawnPlayer p = new S_BroadcastRespawnPlayer();
            p.playerId = session.SessionId;
            Broadcast(p.Write());
        }

        public void Attack(ClientSession session, C_Attack packet)
        {
            S_BroadcastAttack p = new S_BroadcastAttack();

            p.playerId = session.SessionId;

            Broadcast(p.Write());
            Console.WriteLine($"{session.SessionId}번 유저 공격!");
        }
        public void Skill(ClientSession session, C_Skill packet)
        {
            S_BroadcastSkill p = new S_BroadcastSkill();

            p.playerId = session.SessionId;
            p.skillNum = packet.skillNum;
            Broadcast(p.Write());
            Console.WriteLine($"{session.SessionId}번 유저 {packet.skillNum}번 스킬 사용!");
        }

        public void PotionEat(ClientSession session, C_SendPotionEat packet)
        {
            S_BroadcastPotionEat p = new S_BroadcastPotionEat();
            p.playerId = session.SessionId;
            p.potionType = packet.potionType;
            Broadcast(p.Write());
        }

        public void MonsterMove(ClientSession session, C_MonsterMove packet)
        {
            S_BroadcastMonsterMove p = new S_BroadcastMonsterMove();
            p.playerId = session.SessionId;
            p.monsterId = packet.monsterId;
            p.directionX = packet.directionX;
            p.directionY = packet.directionY;
            p.directionZ = packet.directionZ;
            p.posX = packet.posX;
            p.posY = packet.posY;
            p.posZ = packet.posZ;
            p.isMoving = packet.isMoving;
            p.moveSpeed = packet.moveSpeed;

            //Console.WriteLine($"몬스터 무브 id : {packet.monsterId}, moving:{packet.isMoving},x:{packet.posX},y:{packet.posY},z:{packet.posZ}");
            //Console.WriteLine($"몬스터 무스 id : {packet.monsterId}, moveSpeed : {packet.moveSpeed}");
            Broadcast(p.Write());
        }

        public void MonsterAttack(ClientSession session, C_MonsterAttack packet)
        {
            S_BroadcastMonsterAttack p = new S_BroadcastMonsterAttack();
            p.monsterId = packet.monsterId;
            p.targetPlayerId = packet.targetPlayerId;
            p.playerId = session.SessionId;
            Broadcast(p.Write());
        }

        public void MonsterDamaged(ClientSession session, C_MonsterDamaged packet)
        {
            S_BroadcastMonsterDamaged p = new S_BroadcastMonsterDamaged();
            p.playerId = packet.playerId;
            p.damaged = packet.damaged;
            p.monsterId = packet.monsterId;

            Console.WriteLine($"{packet.monsterId}번 몬스터 : {packet.damaged}피해입음");

            Broadcast(p.Write());

        }

        public void MonsterDead(ClientSession session, C_MonsterDead packet)
        {
            S_BroadcastMonsterDead p = new S_BroadcastMonsterDead();
            p.monsterId = packet.monsterId;
            p.playerId = packet.playerId;
            p.killPlayerId = packet.killPlayerId;
            Console.WriteLine($"{packet.monsterId}번 몬스터 : 죽음");
            Broadcast(p.Write());
        }

        public void RespawnMonster(ClientSession session, C_RespawnMonster packet)
        {
            S_BroadcastRespawnMonster p = new S_BroadcastRespawnMonster();
            p.monsterId = packet.monsterId;
            p.posX = packet.posX;
            p.posY = packet.posY;
            p.posZ = packet.posZ;
            Broadcast(p.Write());
        }

        // 첫번째 플레이어의 몬스터들 위치를 전송해줌
        public void MonsterList(ClientSession session, C_MonsterSpawn packet)
        {
            S_BroadcastMonsterSpawn p = new S_BroadcastMonsterSpawn();

            for (int i = 0; i < packet.positionXs.Count; i++)
            {
                S_BroadcastMonsterSpawn.MonsterId id = new S_BroadcastMonsterSpawn.MonsterId();
                id.monsterid = packet.monsterIds[i].monsterid;
                p.monsterIds.Add(id);

                S_BroadcastMonsterSpawn.PositionX px = new S_BroadcastMonsterSpawn.PositionX();
                px.posX = packet.positionXs[i].posX;
                p.positionXs.Add(px);

                S_BroadcastMonsterSpawn.PositionY py = new S_BroadcastMonsterSpawn.PositionY();
                py.posY = packet.positionYs[i].posY;
                p.positionYs.Add(py);

                S_BroadcastMonsterSpawn.PositionZ pz = new S_BroadcastMonsterSpawn.PositionZ();
                pz.posZ = packet.positionZs[i].posZ;
                p.positionZs.Add(pz);

                S_BroadcastMonsterSpawn.WayPoint way = new S_BroadcastMonsterSpawn.WayPoint();
                way.way0_X = packet.wayPoints[i].way0_X;
                way.way0_Y = packet.wayPoints[i].way0_Y;
                way.way0_Z = packet.wayPoints[i].way0_Z;
                way.way1_X = packet.wayPoints[i].way1_X;
                way.way1_Y = packet.wayPoints[i].way1_Y;
                way.way1_Z = packet.wayPoints[i].way1_Z;
                p.wayPoints.Add(way);
            }
            // 마지막에 들어온 유저에게 보내주기
            ClientSession sendPacket = SessionManager.Instance.Find(packet.to_playerId);
            if (sendPacket != null)
                sendPacket.Send(p.Write());
            Console.WriteLine($"호스트 아이디 : {session.SessionId},{hostPlayerId} => 수신할 아이디 : {packet.to_playerId}");
        }

        public void BossAddUser(ClientSession session, C_EnterBoss packet)
        {
            Console.WriteLine($"{session.SessionId}번 유저 보스 매칭 참가!");
            bossEnterUserCnt++;
            Console.WriteLine($"보스매칭 : {2}명 중 {bossEnterUserCnt}명 참가!");
            if (bossEnterUserCnt == 2)//_sessions.Count
            {
                S_EnterSignBoss s_EnterSign = new S_EnterSignBoss();
                Broadcast(s_EnterSign.Write());

                bossEnterUserCnt = 0;

                Console.WriteLine($"보스 매칭 완료!");
            }
        }

        public void BossPatern(ClientSession session, C_RandBossPatern packet)
        {
            S_BroadcastRandBossPatern p = new S_BroadcastRandBossPatern();
            p.paternNum = packet.paternNum;

            for (int i = 0; i < packet.downLaserPoss.Count; i++)
            {
                S_BroadcastRandBossPatern.DownLaserPos p2 = new S_BroadcastRandBossPatern.DownLaserPos();
                p2.posX = packet.downLaserPoss[i].posX;
                p2.posZ = packet.downLaserPoss[i].posZ;

                p.downLaserPoss.Add(p2);
            }
            Console.WriteLine($"Boss {packet.paternNum}번 패턴 실행");
            Broadcast(p.Write());
        }

        public void BossDamaged(ClientSession session, C_BossDamaged packet)
        {
            S_BroadcastBossDamaged p = new S_BroadcastBossDamaged();
            p.damaged = packet.damaged;
            p.playerId = session.SessionId;
            Broadcast(p.Write());
        }

        public void BossDead(ClientSession session, C_BossDead packet)
        {
            S_BroadcastBossDead p = new S_BroadcastBossDead();
            Broadcast(p.Write());

            Console.WriteLine("보스 사망!");
        }



        public void Chat(ClientSession session, C_Chat packet)
        {
            S_BroadcastChat p = new S_BroadcastChat();
            Console.WriteLine($"채팅 내용 => {session.SessionId} : {packet.chat}");
            p.playerId = session.SessionId;
            p.chat = packet.chat;
            Broadcast(p.Write());
        }

        #region 인벤 퀘스트 관련
        public void UpdateInvenInfo(ClientSession session, C_InvenInfo packet)
        {
            // item 리스트 업데이트
            foreach (C_InvenInfo.Items item in packet.itemss)
            {
                Item t = null;
                if (session.PlayerInfo.info.items.TryGetValue(item.itemId, out t))
                {
                    t.id = item.itemId;
                    int tId = Convert.ToInt32(t.id);
                    if (tId >= 2)
                    {
                        if (t.isEqiped == true)
                        {
                            if (t.cnt > item.cnt)
                            {
                                // 판 거
                                if (item.cnt == 0)
                                {
                                    //if (t.isEqiped == false)
                                    //   session.PlayerInfo.info.items.Remove(item.itemId);
                                    //else
                                    t.cnt = 1;
                                    //if(packet.money!=0)
                                    //   Console.WriteLine($"player {session.SessionId} 아이템판매 id => {t.id}, {t.cnt}보유");
                                    return;
                                }
                                t.cnt--;
                            }
                            else
                            {
                                Console.WriteLine($"{session.SessionId}번 유저 : 아이템{item.itemId}를 1개 획득, {item.cnt}보유");
                                t.cnt = item.cnt + 1;
                            }

                        }
                        else
                        {
                            if (t.cnt > item.cnt)
                            {
                                // 판 거

                                if (item.cnt == 0)
                                {
                                    session.PlayerInfo.info.items.Remove(item.itemId);
                                    Console.WriteLine($"player {session.SessionId} 아이템판매 id => {item.itemId}");
                                    return;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"{session.SessionId}번 유저 : 아이템{item.itemId}를 1개 획득, {item.cnt}보유");
                            }
                            t.cnt = item.cnt;
                        }
                    }
                    else
                    {
                        //Console.WriteLine("소비템입니다.");
                        if (t.cnt != item.cnt)
                        {
                            if (t.cnt < item.cnt)
                                Console.WriteLine($"{session.SessionId}번 유저 : 아이템{item.itemId}를 {item.cnt - t.cnt}개 획득");
                            else
                                Console.WriteLine($"{session.SessionId}번 유저 : 아이템{item.itemId}를 {t.cnt - item.cnt}개 사용");
                        }
                        t.cnt = item.cnt;
                    }
                }
                else
                {
                    // 기존에 없던 아이템이면 추가
                    Item it = new Item();
                    it.id = item.itemId;
                    it.cnt = item.cnt;
                    int tId = Convert.ToInt32(item.itemId);
                    if (tId >= 2)
                        it.isEqiped = false;

                    session.PlayerInfo.info.items.Add(it.id, it);
                    Console.WriteLine($"{session.SessionId}번 유저 : 아이템{item.itemId}를 {item.cnt}개 획득");
                }
            }
        }

        // 플레이어 정보 보내기
        public void SendPlayerInfo(ClientSession session, C_RequestPlayerInfo packet)
        {
            S_PlayerInfo p = new S_PlayerInfo();

            p.playerId = session.SessionId;
            p.Hp = session.PlayerInfo.hp;
            p.Mp = session.PlayerInfo.mp;
            p.Level = session.PlayerInfo.level;
            p.Money = session.PlayerInfo.info.money;
            p.exp = session.PlayerInfo.exp;
            foreach (Item item in session.PlayerInfo.info.items.Values)
            {

                S_PlayerInfo.Items items = new S_PlayerInfo.Items();
                items.itemId = item.id;
                items.cnt = item.cnt;
                items.isEquipped = item.isEqiped;
                p.itemss.Add(items);

            }
            session.Send(p.Write());
        }

        // 퀘스트 정보 보내기
        public void SendQuestInfo(ClientSession session, C_RequestQuestInfo packet)
        {
            S_QuestInfo p = new S_QuestInfo();

            p.playerId = session.SessionId;
            //Console.WriteLine($"퀘스트 개수 : {ClientSession.QUEST_CNT}");
            for (int i = 0; i < ClientSession.QUEST_CNT; i++)
            {
                S_QuestInfo.QuestInfo q = new S_QuestInfo.QuestInfo();

                q.questId = session.QuestInfos[i].questId;
                q.questState = (int)session.QuestInfos[i].state;
                p.questInfos.Add(q);
            }
            session.Send(p.Write());
        }

        // 특정 퀘스트 정보 변경 요청
        public void UpdateQuestInfo(ClientSession session, C_UpdateQuestInfo packet)
        {
            if (session.SessionId != packet.playerId)
            {
                Console.WriteLine($"잘못된 요청 -> C_UpdateQuestInfo UserId : {packet.playerId}, sessionId : {session.SessionId}");
                return;
            }
            if (packet.questState > ClientSession.QUEST_CNT)
            {
                Console.WriteLine($"잘못된 요청 -> C_UpdateQuestInfo UserId : {packet.playerId} \n" +
               $"퀘스트 진행상황 변수 오류 -> {packet.questState} 보냄");
            }

            session.QuestInfos[0].questId = packet.questId;
        }

        // 장비를 장착했을 때
        // 아이템리스트에서 해당 장비 state를 장착으로 바꿈
        public void UpdateEquipped(ClientSession session, C_Equipped packet)
        {
            Item item = null;
            if (session.PlayerInfo.info.items.TryGetValue(packet.itemId, out item))
            {
                item.isEqiped = true;

                Console.WriteLine($"장착!");

                S_BroadcastEquipped p = new S_BroadcastEquipped();
                p.itemId = packet.itemId;
                p.playerId = session.SessionId;
                Broadcast(p.Write());
            }
            else
            {
                Console.WriteLine($"잘못된 아이템 아이디입니다. => UpdateEquippped {packet.itemId}");
            }
        }

        // 장비를 해제했을 때
        // 아이템리스트에서 해당 장비 state를 해제로 바꿈
        public void UpdateUnEquipped(ClientSession session, C_UnEquipped packet)
        {
            Item item = null;
            if (session.PlayerInfo.info.items.TryGetValue(packet.itemId, out item))
            {
                item.isEqiped = false;
                Console.WriteLine($"장비 해제!");
                S_BroadcastUnEquipped p = new S_BroadcastUnEquipped();
                p.itemId = packet.itemId;
                p.playerId = session.SessionId;
                Broadcast(p.Write());
            }
            else
            {
                Console.WriteLine($"잘못된 아이템 아이디입니다. => UpdateEquippped {packet.itemId}");
            }
        }


        public void UpdateMoney(ClientSession session, C_SendMoney packet)
        {
            session.PlayerInfo.info.money = packet.money;
            Console.WriteLine($"현재 money : {packet.money}");
        }

        #endregion

        public void PlayerStat(ClientSession session, C_PlayerStat packet)
        {
            S_BroadcastPlayerStat p = new S_BroadcastPlayerStat();
            p.playerId = session.SessionId;
            p.power = packet.power;
            p.magic = packet.magic;
            p.attack = packet.attack;
            p.heal = packet.heal;


            Broadcast(p.Write());
        }


        public void UpdateExp(ClientSession session, C_SendExp packet)
        {
            session.PlayerInfo.exp = packet.exp;
            //S_BroadcastExp p = new S_BroadcastExp();
            //p.exp = packet.exp;
            //p.playerId = session.SessionId;
            //Broadcast(p.Write());
        }

        public void UpdateLevel(ClientSession session, C_SendLevel packet)
        {
            session.PlayerInfo.level = packet.level;

            S_SendLevel s_SendLevel = new S_SendLevel();
            s_SendLevel.playerId = session.SessionId;

            Broadcast(s_SendLevel.Write());
        }

        #region 거래관련

        public void RequestBarter(ClientSession session, C_RequestBarter packet)
        {
            S_RequestBarter s_RequestBarter = new S_RequestBarter();
            s_RequestBarter.playerId = session.SessionId;

            // sessionManager에서 session 찾아서 send하기 오류나면 걍 broadcast로 클라에서 자기 id일때만 받기로
            SessionManager.Instance.Find(packet.playerId).Send(s_RequestBarter.Write());

        }

        public void OkBarter(ClientSession session, C_OkBarter packet)
        {
            S_PlayerBarter s_PlayerBarter = new S_PlayerBarter();
            s_PlayerBarter.isOk = packet.isOk;

            BarterInfo barterinfo = new BarterInfo();
            // 거래 승낙했을 경우 거래 리스트에 등록하기
            if (packet.isOk)
            {
                barterinfo.rePid = packet.playerId;
                barterinfo.sePid = session.SessionId;

                barterList.Add(barterinfo);

                s_PlayerBarter.barterId = barterList.Count - 1;
            }

            // 거래 당사자 두명한테 거래 성사 유무 알리기
            s_PlayerBarter.otherplayerId = packet.playerId;
            session.Send(s_PlayerBarter.Write());
            s_PlayerBarter.otherplayerId = session.SessionId;
            SessionManager.Instance.Find(packet.playerId).Send(s_PlayerBarter.Write());
        }

        // 유저가 승인완료를 눌렀을 때
        public void OkFinish(ClientSession session, C_OkFinishBarter packet)
        {
            BarterInfo bi = barterList[packet.barterId];
            // 거래 요청자
            if (bi.rePid == session.SessionId)
            {
                bi.reOk = true;
            }
            else if (bi.sePid == session.SessionId)
            {
                bi.seOk = true;
            }

            // 둘 다 승인 완료 했으면 거래 성사 패킷 보내기
            if (bi.reOk == true && bi.seOk == true)
            {
                S_FinishBarter s_FinishBarter = new S_FinishBarter();
                s_FinishBarter.barterId = packet.barterId;

                SessionManager.Instance.Find(bi.rePid).Send(s_FinishBarter.Write());
                SessionManager.Instance.Find(bi.sePid).Send(s_FinishBarter.Write());
            }

        }
        // 해당 거래 취소
        public void CancelBarter(ClientSession session, C_CancelBarter packet)
        {
            BarterInfo bi = barterList[packet.barterId];

            S_CancelBarter s_CancelBarter = new S_CancelBarter();
            s_CancelBarter.barterId = packet.barterId;

            SessionManager.Instance.Find(bi.rePid).Send(s_CancelBarter.Write());
            SessionManager.Instance.Find(bi.sePid).Send(s_CancelBarter.Write());

            barterList.RemoveAt(packet.barterId);
        }

        // 전달 받은 아이템정보로 서버 값 바꾸기
        public void BarterItem(ClientSession session, C_BarterItem packet)
        {
            BarterInfo bi = barterList[packet.barterId];

            ClientSession se = null;
            if (session.SessionId == bi.sePid)
            {
                se = SessionManager.Instance.Find(bi.sePid);
            }
            else if (session.SessionId == bi.rePid)
            {
                se = SessionManager.Instance.Find(bi.rePid);
            }

            se.PlayerInfo.info.money = packet.money;
            foreach (C_BarterItem.BarterItem item in packet.barterItems)
            {
                Item t = null;
                // 있는 아이템 얻은 경우
                if (se.PlayerInfo.info.items.TryGetValue(item.itemId, out t))
                {
                    t.cnt += item.cnt;
                }
                // 없던 아이템 얻은 경우
                else
                {
                    Item item1 = new Item();
                    item1.id = item.itemId;
                    item1.cnt = item.cnt;
                    se.PlayerInfo.info.items.Add(item.itemId, item1);
                }
            }
        }

        // 거래창에 물건을 올리거나 내릴 때,
        public void DragItemInfo(ClientSession session, C_DragItemInfo packet)
        {
            BarterInfo bi = barterList[packet.barterId];

            S_SendDragItemInfo s_SendDragItemInfo = new S_SendDragItemInfo();
            s_SendDragItemInfo.barterId = packet.barterId;
            s_SendDragItemInfo.itemId = packet.itemId;
            s_SendDragItemInfo.cnt = packet.cnt;
            s_SendDragItemInfo.isSet = packet.isSet;
            s_SendDragItemInfo.money = packet.money;

            if (bi.rePid == session.SessionId)
            {
                SessionManager.Instance.Find(bi.sePid).Send(s_SendDragItemInfo.Write());
            }
            else
            {
                SessionManager.Instance.Find(bi.rePid).Send(s_SendDragItemInfo.Write());
            }
        }

        #endregion

        public void CheckLogin(ClientSession session, C_RequestLogin packet)
        {
            bool login = UserDB.Instance.CheckLogin(packet);

            S_CheckLogin p = new S_CheckLogin();
            p.successLogin = login;

            session.Send(p.Write());
            Console.WriteLine($"{session.SessionId}가 로그인 요청 => {login}");
            // 로그인 성공 시
            if (login)
            {
                // 데이터베이스에서 기존 정보 가져온다. 
                UserInfoDB s = UserDB.Instance.GetUserInfo(packet.Id);

                if (s == null)
                {
                    Console.WriteLine("CheckLogin if 오류!");
                    return;
                }

                // 세션 교체
                // 값 대입
                int imsiId = session.SessionId;

                session.SessionId = s.session.SessionId;
                session.PlayerInfo = s.session.PlayerInfo;
                Console.WriteLine($"돈 : {s.session.PlayerInfo.info.money}");
                //Console.WriteLine($"userDB 아이템개수:{s.session.PlayerInfo.info.items.Count}");
                Console.WriteLine($"처음 세팅 \n session 아이템 종류개수:{session.PlayerInfo.info.items.Count}");

                foreach (Item it in session.PlayerInfo.info.items.Values)
                {
                    Console.WriteLine($"id : {it.id}, cnt : {it.cnt} isEquip : {it.isEqiped}");
                }
                session.QuestInfos = s.session.QuestInfos;
                session.PosX = -448.87f;
                session.PosY = -30.68f;
                session.PosZ = 39.93f;
                session.DirectionX = s.session.DirectionX;
                session.DirectionY = s.session.DirectionY;
                session.DirectionZ = s.session.DirectionZ;

                // 리스트에서 임시 값 삭제 및 추가
                SessionManager.Instance._sessions.Remove(imsiId);
                SessionManager.Instance._sessions.Add(session.SessionId, session);

                if (myUserInfo == null)
                    myUserInfo = UserDB.Instance.GetUserInfo(packet.Id);

                myUserInfo.logined = true;

            }
            // 로그인 실패 시
            else
            {

            }
        }

        public void Register(ClientSession session, C_RequestRegist packet)
        {
            bool regist = UserDB.Instance.RegisterInfo(packet);

            S_SuccessRegister p = new S_SuccessRegister();
            p.Registed = regist;
            session.Send(p.Write());

            UserInfoDB user = UserDB.Instance.GetUserInfo(packet.Id);
            user.session = session;
            user.playerId = session.SessionId;
            myUserInfo = user;
        }

    }
}