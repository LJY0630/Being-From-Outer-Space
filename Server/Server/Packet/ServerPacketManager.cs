using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
	Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();
		
	public void Register()
	{
		_makeFunc.Add((ushort)PacketID.C_LeaveGame, MakePacket<C_LeaveGame>);
		_handler.Add((ushort)PacketID.C_LeaveGame, PacketHandler.C_LeaveGameHandler);
		_makeFunc.Add((ushort)PacketID.C_Move, MakePacket<C_Move>);
		_handler.Add((ushort)PacketID.C_Move, PacketHandler.C_MoveHandler);
		_makeFunc.Add((ushort)PacketID.C_Dead, MakePacket<C_Dead>);
		_handler.Add((ushort)PacketID.C_Dead, PacketHandler.C_DeadHandler);
		_makeFunc.Add((ushort)PacketID.C_RespawnPlayer, MakePacket<C_RespawnPlayer>);
		_handler.Add((ushort)PacketID.C_RespawnPlayer, PacketHandler.C_RespawnPlayerHandler);
		_makeFunc.Add((ushort)PacketID.C_Jump, MakePacket<C_Jump>);
		_handler.Add((ushort)PacketID.C_Jump, PacketHandler.C_JumpHandler);
		_makeFunc.Add((ushort)PacketID.C_Attack, MakePacket<C_Attack>);
		_handler.Add((ushort)PacketID.C_Attack, PacketHandler.C_AttackHandler);
		_makeFunc.Add((ushort)PacketID.C_Skill, MakePacket<C_Skill>);
		_handler.Add((ushort)PacketID.C_Skill, PacketHandler.C_SkillHandler);
		_makeFunc.Add((ushort)PacketID.C_DecideTargetMonster, MakePacket<C_DecideTargetMonster>);
		_handler.Add((ushort)PacketID.C_DecideTargetMonster, PacketHandler.C_DecideTargetMonsterHandler);
		_makeFunc.Add((ushort)PacketID.C_PlayerStat, MakePacket<C_PlayerStat>);
		_handler.Add((ushort)PacketID.C_PlayerStat, PacketHandler.C_PlayerStatHandler);
		_makeFunc.Add((ushort)PacketID.C_MonsterAttack, MakePacket<C_MonsterAttack>);
		_handler.Add((ushort)PacketID.C_MonsterAttack, PacketHandler.C_MonsterAttackHandler);
		_makeFunc.Add((ushort)PacketID.C_MonsterMove, MakePacket<C_MonsterMove>);
		_handler.Add((ushort)PacketID.C_MonsterMove, PacketHandler.C_MonsterMoveHandler);
		_makeFunc.Add((ushort)PacketID.C_MonsterDamaged, MakePacket<C_MonsterDamaged>);
		_handler.Add((ushort)PacketID.C_MonsterDamaged, PacketHandler.C_MonsterDamagedHandler);
		_makeFunc.Add((ushort)PacketID.C_MonsterDead, MakePacket<C_MonsterDead>);
		_handler.Add((ushort)PacketID.C_MonsterDead, PacketHandler.C_MonsterDeadHandler);
		_makeFunc.Add((ushort)PacketID.C_BossDamaged, MakePacket<C_BossDamaged>);
		_handler.Add((ushort)PacketID.C_BossDamaged, PacketHandler.C_BossDamagedHandler);
		_makeFunc.Add((ushort)PacketID.C_BossDead, MakePacket<C_BossDead>);
		_handler.Add((ushort)PacketID.C_BossDead, PacketHandler.C_BossDeadHandler);
		_makeFunc.Add((ushort)PacketID.C_MonsterSpawn, MakePacket<C_MonsterSpawn>);
		_handler.Add((ushort)PacketID.C_MonsterSpawn, PacketHandler.C_MonsterSpawnHandler);
		_makeFunc.Add((ushort)PacketID.C_EnterBoss, MakePacket<C_EnterBoss>);
		_handler.Add((ushort)PacketID.C_EnterBoss, PacketHandler.C_EnterBossHandler);
		_makeFunc.Add((ushort)PacketID.C_RandBossPatern, MakePacket<C_RandBossPatern>);
		_handler.Add((ushort)PacketID.C_RandBossPatern, PacketHandler.C_RandBossPaternHandler);
		_makeFunc.Add((ushort)PacketID.C_Chat, MakePacket<C_Chat>);
		_handler.Add((ushort)PacketID.C_Chat, PacketHandler.C_ChatHandler);
		_makeFunc.Add((ushort)PacketID.C_InvenInfo, MakePacket<C_InvenInfo>);
		_handler.Add((ushort)PacketID.C_InvenInfo, PacketHandler.C_InvenInfoHandler);
		_makeFunc.Add((ushort)PacketID.C_RequestPlayerInfo, MakePacket<C_RequestPlayerInfo>);
		_handler.Add((ushort)PacketID.C_RequestPlayerInfo, PacketHandler.C_RequestPlayerInfoHandler);
		_makeFunc.Add((ushort)PacketID.C_RequestQuestInfo, MakePacket<C_RequestQuestInfo>);
		_handler.Add((ushort)PacketID.C_RequestQuestInfo, PacketHandler.C_RequestQuestInfoHandler);
		_makeFunc.Add((ushort)PacketID.C_UpdateQuestInfo, MakePacket<C_UpdateQuestInfo>);
		_handler.Add((ushort)PacketID.C_UpdateQuestInfo, PacketHandler.C_UpdateQuestInfoHandler);
		_makeFunc.Add((ushort)PacketID.C_Equipped, MakePacket<C_Equipped>);
		_handler.Add((ushort)PacketID.C_Equipped, PacketHandler.C_EquippedHandler);
		_makeFunc.Add((ushort)PacketID.C_UnEquipped, MakePacket<C_UnEquipped>);
		_handler.Add((ushort)PacketID.C_UnEquipped, PacketHandler.C_UnEquippedHandler);
		_makeFunc.Add((ushort)PacketID.C_RequestLogin, MakePacket<C_RequestLogin>);
		_handler.Add((ushort)PacketID.C_RequestLogin, PacketHandler.C_RequestLoginHandler);
		_makeFunc.Add((ushort)PacketID.C_RequestEnterGame, MakePacket<C_RequestEnterGame>);
		_handler.Add((ushort)PacketID.C_RequestEnterGame, PacketHandler.C_RequestEnterGameHandler);
		_makeFunc.Add((ushort)PacketID.C_RequestRegist, MakePacket<C_RequestRegist>);
		_handler.Add((ushort)PacketID.C_RequestRegist, PacketHandler.C_RequestRegistHandler);
		_makeFunc.Add((ushort)PacketID.C_SendMoney, MakePacket<C_SendMoney>);
		_handler.Add((ushort)PacketID.C_SendMoney, PacketHandler.C_SendMoneyHandler);
		_makeFunc.Add((ushort)PacketID.C_SendExp, MakePacket<C_SendExp>);
		_handler.Add((ushort)PacketID.C_SendExp, PacketHandler.C_SendExpHandler);
		_makeFunc.Add((ushort)PacketID.C_SendLevel, MakePacket<C_SendLevel>);
		_handler.Add((ushort)PacketID.C_SendLevel, PacketHandler.C_SendLevelHandler);
		_makeFunc.Add((ushort)PacketID.C_SendPotionEat, MakePacket<C_SendPotionEat>);
		_handler.Add((ushort)PacketID.C_SendPotionEat, PacketHandler.C_SendPotionEatHandler);
		_makeFunc.Add((ushort)PacketID.C_RequestBarter, MakePacket<C_RequestBarter>);
		_handler.Add((ushort)PacketID.C_RequestBarter, PacketHandler.C_RequestBarterHandler);
		_makeFunc.Add((ushort)PacketID.C_OkBarter, MakePacket<C_OkBarter>);
		_handler.Add((ushort)PacketID.C_OkBarter, PacketHandler.C_OkBarterHandler);
		_makeFunc.Add((ushort)PacketID.C_OkFinishBarter, MakePacket<C_OkFinishBarter>);
		_handler.Add((ushort)PacketID.C_OkFinishBarter, PacketHandler.C_OkFinishBarterHandler);
		_makeFunc.Add((ushort)PacketID.C_CancelBarter, MakePacket<C_CancelBarter>);
		_handler.Add((ushort)PacketID.C_CancelBarter, PacketHandler.C_CancelBarterHandler);
		_makeFunc.Add((ushort)PacketID.C_BarterItem, MakePacket<C_BarterItem>);
		_handler.Add((ushort)PacketID.C_BarterItem, PacketHandler.C_BarterItemHandler);
		_makeFunc.Add((ushort)PacketID.C_RespawnMonster, MakePacket<C_RespawnMonster>);
		_handler.Add((ushort)PacketID.C_RespawnMonster, PacketHandler.C_RespawnMonsterHandler);
		_makeFunc.Add((ushort)PacketID.C_DragItemInfo, MakePacket<C_DragItemInfo>);
		_handler.Add((ushort)PacketID.C_DragItemInfo, PacketHandler.C_DragItemInfoHandler);

	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
		if (_makeFunc.TryGetValue(id, out func))
		{
			IPacket packet = func.Invoke(session, buffer);
			if (onRecvCallback != null)
				onRecvCallback.Invoke(session, packet);
			else
				HandlePacket(session, packet);
		}
	}

	T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
	{
		T pkt = new T();
		pkt.Read(buffer);
		return pkt;
	}

	public void HandlePacket(PacketSession session, IPacket packet)
	{
		Action<PacketSession, IPacket> action = null;
		if (_handler.TryGetValue(packet.Protocol, out action))
			action.Invoke(session, packet);
	}
}