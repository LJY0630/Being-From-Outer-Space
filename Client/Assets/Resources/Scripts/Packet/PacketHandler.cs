using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class PacketHandler
{
    // 누군가 들어왔을 때
    public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;
        NetPlayerManager.Instance.EnterGame(pkt);
    }

    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;
        NetPlayerManager.Instance.LeaveGame(pkt);
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        S_PlayerList pkt = packet as S_PlayerList;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;
        NetPlayerManager.Instance.Add(pkt);
    }

    public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMove pkt = packet as S_BroadcastMove;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;
        NetPlayerManager.Instance.Move(pkt);
    }

    public static void S_BroadcastJumpHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastJump pkt = packet as S_BroadcastJump;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;
        NetPlayerManager.Instance.Jump(pkt);
    }

    public static void S_BroadcastDeadHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastDead pkt = packet as S_BroadcastDead;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.DeadPlayer(pkt);
    }

    public static void S_BroadcastRespawnPlayerHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastRespawnPlayer pkt = packet as S_BroadcastRespawnPlayer;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.RespawnPlayer(pkt);
    }

    public static void S_BroadcastAttackHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastAttack pkt = packet as S_BroadcastAttack;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;
        NetPlayerManager.Instance.Attack(pkt);
    }

    public static void S_BroadcastSkillHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastSkill pkt = packet as S_BroadcastSkill;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;
        NetPlayerManager.Instance.Skill(pkt);
    }

    public static void S_BroadcastPlayerStatHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastPlayerStat pkt = packet as S_BroadcastPlayerStat;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.PlayerStat(pkt);
    }

    public static void S_BroadcastPotionEatHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastPotionEat pkt = packet as S_BroadcastPotionEat;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.PotionEat(pkt);
    }

    public static void S_BroadcastDecideTargetMonsterHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastDecideTargetMonster pkt = packet as S_BroadcastDecideTargetMonster;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        // TODO
    }

    public static void S_BroadcastMonsterSpawnHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMonsterSpawn pkt = packet as S_BroadcastMonsterSpawn;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;


        if (NetPlayerManager.Instance.isHost == false)
        {
            Debug.Log("몬스터리스트 init 실행");
            SpawnManager.instance.Init(pkt);
        }
    }

    public static void S_BroadcastMonsterAttackHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMonsterAttack pkt = packet as S_BroadcastMonsterAttack;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;


        if (pkt.playerId != NetPlayerManager.Instance._playerManager.PlayerId)
        {
            // 몬스터 공격시키기
            SpawnManager.instance.AttackByMonsterId(pkt);
            Debug.Log("몬스터 공격 패킷 받음");
        }
    }

    public static void S_BroadcastMonsterMoveHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMonsterMove pkt = packet as S_BroadcastMonsterMove;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;


        if (NetPlayerManager.Instance.isHost == false &&
           SpawnManager.instance.isSpawn == true)
        {
            SpawnManager.instance.SetTargetPosById(pkt);
            Debug.Log("몬스터무브 패킷받음");
        }
    }

    public static void S_BroadcastMonsterDamagedHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMonsterDamaged pkt = packet as S_BroadcastMonsterDamaged;
        ServerSession serverSession = session as ServerSession;
        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;
        // TODO:
        if (pkt.playerId == NetPlayerManager.Instance._playerManager.PlayerId)
            return;
        SpawnManager.instance.Damaged(pkt);
        //MonsterManager.Instance.Damaged(pkt); - 해당 id의 몬스터 찾아서 피격 애니메이션 및 정보 변경
    }

    public static void S_BroadcastMonsterDeadHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMonsterDead pkt = packet as S_BroadcastMonsterDead;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.MonsterDead(pkt);
        Debug.Log("몬스터 죽음 패킷");
    }


    // 첫번째 플레이어가 넘겨준 몬스터들의 위치정보로 몬스터들 생성하면됨.


    public static void S_EnterSignBossHandler(PacketSession session, IPacket packet)
    {
        S_EnterSignBoss pkt = packet as S_EnterSignBoss;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;


        NetPlayerManager.Instance.LoadBossScene();
    }

    public static void S_BroadcastRandBossPaternHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastRandBossPatern pkt = packet as S_BroadcastRandBossPatern;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.BossPaternRecv(pkt);
    }

    public static void S_BroadcastBossDamagedHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastBossDamaged pkt = packet as S_BroadcastBossDamaged;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.BossDamaged(pkt);

    }

    public static void S_BroadcastRespawnMonsterHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastRespawnMonster pkt = packet as S_BroadcastRespawnMonster;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        //TODO
        if (!NetPlayerManager.Instance.isHost)
        {
            SpawnManager.instance.RespawnMonsterServer(pkt);
        }
    }

    public static void S_BroadcastBossDeadHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastBossDead pkt = packet as S_BroadcastBossDead;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.BossDead();
    }

    public static void S_BroadcastChatHandler(PacketSession session, IPacket packet)
    {
        Debug.Log("S_BroadcastChat 패킷 받음");
        S_BroadcastChat pkt = packet as S_BroadcastChat;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.UpdateChat(pkt);
    }

    public static void S_PlayerInfoHandler(PacketSession session, IPacket packet)
    {
        S_PlayerInfo pkt = packet as S_PlayerInfo;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.ReceivePlayerInfo(pkt);
    }

    public static void S_BroadcastExpHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastExp pkt = packet as S_BroadcastExp;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        if (NetPlayerManager.Instance._playerManager.PlayerId == pkt.playerId)
            return;

        // playerStat에 접근해서 경험치 올리기
        Debug.Log("S_BroadcastExp패킷 확인");
        NetPlayerManager.Instance.UpdateExp(pkt);
    }

    public static void S_SendLevelHandler(PacketSession session, IPacket packet)
    {
        S_SendLevel pkt = packet as S_SendLevel;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        if (NetPlayerManager.Instance._playerManager.PlayerId == pkt.playerId)
            return;

        // playerStat에 접근해서 경험치 올리기
        Debug.Log("S_SendLevel패킷 확인");
        NetPlayerManager.Instance.UpdateLevel(pkt);
    }

    // 거래 관련-------------------------------------------------------------------------

    // 상대방이 요청했을 경우 실행되는 함수. -> 밑에 알림창처럼 o,x 있는거 뜨기
    public static void S_RequestBarterHandler(PacketSession session, IPacket packet)
    {
        S_RequestBarter pkt = packet as S_RequestBarter;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        // TODO
        // UI창 알림창같은거나 띄우기 뭔지알지 
    }

    // 거래 의지에 따라 다른 값을 bool 변수로 받게 된다. 
    // true면 거래 시작-> 거래창 띄우면됨. false면 거래취소 알림?
    public static void S_PlayerBarterHandler(PacketSession session, IPacket packet)
    {
        S_PlayerBarter pkt = packet as S_PlayerBarter;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        // TODO
        //pkt.isOk 이 값이 true면 거래가 시작된것. 거래창 띄우면 됨 프리팹->barter 
        // pkt.isOk 가 false면 상대가 취소한 것. 거래 관련 창 다 끄면됨.
    }

    // 거래 도중 한쪽이 취소했을 때
    public static void S_CancelBarterHandler(PacketSession session, IPacket packet)
    {
        S_PlayerBarter pkt = packet as S_PlayerBarter;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        // TODO
        // 거래창이 떠있던 상태에서 중간에 상대가 x버튼 누른거로 거래 관련 창 끄면됨.
    }

    // 거래가 완료되었고 상대에가 주는 물건정보를 보낸다. C_BarterItem 패킷 
    public static void S_FinishBarterHandler(PacketSession session, IPacket packet)
    {
        S_PlayerBarter pkt = packet as S_PlayerBarter;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        // TODO
        // 거래창에 올렸던 물건들,돈 정보 넣어서 보내면됨. 자기꺼만
    }

    public static void S_SendDragItemInfoHandler(PacketSession session, IPacket packet)
    {
        S_SendDragItemInfo pkt = packet as S_SendDragItemInfo;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        // TODO
        // 이건 거래창에 상대가 뭐 올렸을 때 다른 클라에서도 띄우려고 올린 물건 정보 보내주는 것.
    }

    // 거래관련 끝------------------------------------------------------------------
    public static void S_QuestInfoHandler(PacketSession session, IPacket packet)
    {
        S_QuestInfo pkt = packet as S_QuestInfo;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.ReceiveQuestInfo(pkt);
    }

    public static void S_BroadcastEquippedHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEquipped pkt = packet as S_BroadcastEquipped;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.UpdateEquipped(pkt);
        Debug.Log("패킷핸들러 이큅");
    }

    public static void S_BroadcastUnEquippedHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastUnEquipped pkt = packet as S_BroadcastUnEquipped;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.UpdateUnEquipped(pkt);
        Debug.Log("패킷핸들러 언이큅");
    }

    public static void S_CheckLoginHandler(PacketSession session, IPacket packet)
    {
        S_CheckLogin pkt = packet as S_CheckLogin;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.AuthenticationLogin(pkt);
    }

    public static void S_SelectHostHandler(PacketSession session, IPacket packet)
    {
        S_SelectHost pkt = packet as S_SelectHost;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        NetPlayerManager.Instance.SetHost(pkt);
    }

    public static void S_SetServerSessionHandler(PacketSession session, IPacket packet)
    {
        S_SelectHost pkt = packet as S_SelectHost;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        Debug.Log("서버 연결완료");
    }

    public static void S_SuccessRegisterHandler(PacketSession session, IPacket packet)
    {
        S_SuccessRegister pkt = packet as S_SuccessRegister;
        ServerSession serverSession = session as ServerSession;

        if (NetPlayerManager.Instance.Session == null)
            NetPlayerManager.Instance.Session = serverSession;

        // TODO
        NetPlayerManager.Instance.ResultRegister(pkt);
    }
}