using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
    public static void C_LeaveGameHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.Leave(clientSession));
    }

    public static void C_MoveHandler(PacketSession session, IPacket packet)
    {
        C_Move movePacket = packet as C_Move;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        //Console.WriteLine($"{movePacket.posX}, {movePacket.posY}, {movePacket.posZ}");

        GameRoom room = clientSession.Room;
        room.Push(() => room.Move(clientSession, movePacket));
    }

    public static void C_SkillHandler(PacketSession session, IPacket packet)
    {
        C_Skill skillPacket = packet as C_Skill;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.Skill(clientSession, skillPacket));
    }

    public static void C_JumpHandler(PacketSession session, IPacket packet)
    {
        C_Jump jump = packet as C_Jump;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.Jump(clientSession, jump));
    }

    public static void C_DeadHandler(PacketSession session, IPacket packet)
    {
        C_Dead handler = packet as C_Dead;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.DeadPlayer(clientSession, handler));
    }

    public static void C_RespawnPlayerHandler(PacketSession session, IPacket packet)
    {
        C_RespawnPlayer handler = packet as C_RespawnPlayer;
        ClientSession clientSession = session as ClientSession;

        if ((clientSession.Room == null))
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.RespawnPlayer(clientSession, handler));
    }


    public static void C_AttackHandler(PacketSession session, IPacket packet)
    {
        C_Attack attackPacket = packet as C_Attack;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.Attack(clientSession, attackPacket));
    }

    public static void C_PlayerStatHandler(PacketSession session, IPacket packet)
    {
        C_PlayerStat c_PlayerStat = packet as C_PlayerStat;
        ClientSession clientSession = session as ClientSession;

        if ((clientSession.Room == null))
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.PlayerStat(clientSession, c_PlayerStat));

    }


    public static void C_SendPotionEatHandler(PacketSession session, IPacket packet)
    {
        C_SendPotionEat c_SendPotionEat = packet as C_SendPotionEat;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.PotionEat(clientSession, c_SendPotionEat));

    }

    public static void C_DecideTargetMonsterHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_DecideTargetMonster p = packet as C_DecideTargetMonster;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
    }

    public static void C_MonsterAttackHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_MonsterAttack p = packet as C_MonsterAttack;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.MonsterAttack(clientSession, p));

    }

    public static void C_MonsterMoveHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_MonsterMove p = packet as C_MonsterMove;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.MonsterMove(clientSession, p));
    }


    public static void C_MonsterDamagedHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_MonsterDamaged p = packet as C_MonsterDamaged;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.MonsterDamaged(clientSession, p));
    }

    public static void C_MonsterDeadHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_MonsterDead p = packet as C_MonsterDead;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.MonsterDead(clientSession, p));
    }

    public static void C_MonsterSpawnHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_MonsterSpawn p = packet as C_MonsterSpawn;
        if (clientSession.Room == null)
            return;

        Console.WriteLine("C_MonsterSpawn 패킷 받음");
        GameRoom room = clientSession.Room;
        room.Push(() => room.MonsterList(clientSession, p));
    }

    public static void C_EnterBossHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_EnterBoss p = packet as C_EnterBoss;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.BossAddUser(clientSession, p));
    }

    public static void C_RandBossPaternHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_RandBossPatern p = packet as C_RandBossPatern;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.BossPatern(clientSession, p));
    }

    public static void C_BossDamagedHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_BossDamaged p = packet as C_BossDamaged;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.BossDamaged(clientSession, p));

    }

    public static void C_BossDeadHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_BossDead p = packet as C_BossDead;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.BossDead(clientSession, p));

    }



    public static void C_ChatHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_Chat p = packet as C_Chat;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.Chat(clientSession, p));
    }

    public static void C_InvenInfoHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_InvenInfo p = packet as C_InvenInfo;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.UpdateInvenInfo(clientSession, p));
    }

    public static void C_RequestPlayerInfoHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_RequestPlayerInfo p = packet as C_RequestPlayerInfo;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.SendPlayerInfo(clientSession, p));
    }

    public static void C_RequestQuestInfoHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_RequestQuestInfo p = packet as C_RequestQuestInfo;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.SendQuestInfo(clientSession, p));
    }

    public static void C_UpdateQuestInfoHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_UpdateQuestInfo p = packet as C_UpdateQuestInfo;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.UpdateQuestInfo(clientSession, p));
    }

    public static void C_EquippedHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_Equipped p = packet as C_Equipped;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.UpdateEquipped(clientSession, p));
    }

    public static void C_UnEquippedHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_UnEquipped p = packet as C_UnEquipped;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.UpdateUnEquipped(clientSession, p));
    }

    public static void C_RequestLoginHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_RequestLogin p = packet as C_RequestLogin;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.CheckLogin(clientSession, p));
    }

    public static void C_RequestEnterGameHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_RequestEnterGame p = packet as C_RequestEnterGame;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.Enter(clientSession));
    }

    public static void C_RequestRegistHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_RequestRegist p = packet as C_RequestRegist;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.Register(clientSession, p));

    }

    public static void C_SendMoneyHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_SendMoney p = packet as C_SendMoney;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.UpdateMoney(clientSession, p));
    }

    public static void C_SendExpHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_SendExp p = packet as C_SendExp;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.UpdateExp(clientSession, p));
    }

    public static void C_SendLevelHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_SendLevel p = packet as C_SendLevel;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.UpdateLevel(clientSession, p));
    }

    public static void C_RequestBarterHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_RequestBarter p = packet as C_RequestBarter;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.RequestBarter(clientSession, p));
    }

    public static void C_RespawnMonsterHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_RespawnMonster p = packet as C_RespawnMonster;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.RespawnMonster(clientSession, p));
    }

    public static void C_OkBarterHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_OkBarter p = packet as C_OkBarter;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.OkBarter(clientSession, p));
    }

    public static void C_OkFinishBarterHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_OkFinishBarter p = packet as C_OkFinishBarter;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.OkFinish(clientSession, p));
    }

    public static void C_CancelBarterHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_CancelBarter p = packet as C_CancelBarter;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.CancelBarter(clientSession, p));
    }

    public static void C_BarterItemHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_BarterItem p = packet as C_BarterItem;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.BarterItem(clientSession, p));
    }

    public static void C_DragItemInfoHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_DragItemInfo p = packet as C_DragItemInfo;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.DragItemInfo(clientSession, p));
    }
}