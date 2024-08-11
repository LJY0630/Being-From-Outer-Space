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
		_makeFunc.Add((ushort)PacketID.S_BroadcastEnterGame, MakePacket<S_BroadcastEnterGame>);
		_handler.Add((ushort)PacketID.S_BroadcastEnterGame, PacketHandler.S_BroadcastEnterGameHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastLeaveGame, MakePacket<S_BroadcastLeaveGame>);
		_handler.Add((ushort)PacketID.S_BroadcastLeaveGame, PacketHandler.S_BroadcastLeaveGameHandler);
		_makeFunc.Add((ushort)PacketID.S_PlayerList, MakePacket<S_PlayerList>);
		_handler.Add((ushort)PacketID.S_PlayerList, PacketHandler.S_PlayerListHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastDead, MakePacket<S_BroadcastDead>);
		_handler.Add((ushort)PacketID.S_BroadcastDead, PacketHandler.S_BroadcastDeadHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastRespawnPlayer, MakePacket<S_BroadcastRespawnPlayer>);
		_handler.Add((ushort)PacketID.S_BroadcastRespawnPlayer, PacketHandler.S_BroadcastRespawnPlayerHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastMove, MakePacket<S_BroadcastMove>);
		_handler.Add((ushort)PacketID.S_BroadcastMove, PacketHandler.S_BroadcastMoveHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastJump, MakePacket<S_BroadcastJump>);
		_handler.Add((ushort)PacketID.S_BroadcastJump, PacketHandler.S_BroadcastJumpHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastAttack, MakePacket<S_BroadcastAttack>);
		_handler.Add((ushort)PacketID.S_BroadcastAttack, PacketHandler.S_BroadcastAttackHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastSkill, MakePacket<S_BroadcastSkill>);
		_handler.Add((ushort)PacketID.S_BroadcastSkill, PacketHandler.S_BroadcastSkillHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastDecideTargetMonster, MakePacket<S_BroadcastDecideTargetMonster>);
		_handler.Add((ushort)PacketID.S_BroadcastDecideTargetMonster, PacketHandler.S_BroadcastDecideTargetMonsterHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastPlayerStat, MakePacket<S_BroadcastPlayerStat>);
		_handler.Add((ushort)PacketID.S_BroadcastPlayerStat, PacketHandler.S_BroadcastPlayerStatHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastMonsterAttack, MakePacket<S_BroadcastMonsterAttack>);
		_handler.Add((ushort)PacketID.S_BroadcastMonsterAttack, PacketHandler.S_BroadcastMonsterAttackHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastMonsterMove, MakePacket<S_BroadcastMonsterMove>);
		_handler.Add((ushort)PacketID.S_BroadcastMonsterMove, PacketHandler.S_BroadcastMonsterMoveHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastMonsterDamaged, MakePacket<S_BroadcastMonsterDamaged>);
		_handler.Add((ushort)PacketID.S_BroadcastMonsterDamaged, PacketHandler.S_BroadcastMonsterDamagedHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastMonsterDead, MakePacket<S_BroadcastMonsterDead>);
		_handler.Add((ushort)PacketID.S_BroadcastMonsterDead, PacketHandler.S_BroadcastMonsterDeadHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastBossDamaged, MakePacket<S_BroadcastBossDamaged>);
		_handler.Add((ushort)PacketID.S_BroadcastBossDamaged, PacketHandler.S_BroadcastBossDamagedHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastBossDead, MakePacket<S_BroadcastBossDead>);
		_handler.Add((ushort)PacketID.S_BroadcastBossDead, PacketHandler.S_BroadcastBossDeadHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastMonsterSpawn, MakePacket<S_BroadcastMonsterSpawn>);
		_handler.Add((ushort)PacketID.S_BroadcastMonsterSpawn, PacketHandler.S_BroadcastMonsterSpawnHandler);
		_makeFunc.Add((ushort)PacketID.S_EnterSignBoss, MakePacket<S_EnterSignBoss>);
		_handler.Add((ushort)PacketID.S_EnterSignBoss, PacketHandler.S_EnterSignBossHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastRandBossPatern, MakePacket<S_BroadcastRandBossPatern>);
		_handler.Add((ushort)PacketID.S_BroadcastRandBossPatern, PacketHandler.S_BroadcastRandBossPaternHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastChat, MakePacket<S_BroadcastChat>);
		_handler.Add((ushort)PacketID.S_BroadcastChat, PacketHandler.S_BroadcastChatHandler);
		_makeFunc.Add((ushort)PacketID.S_PlayerInfo, MakePacket<S_PlayerInfo>);
		_handler.Add((ushort)PacketID.S_PlayerInfo, PacketHandler.S_PlayerInfoHandler);
		_makeFunc.Add((ushort)PacketID.S_QuestInfo, MakePacket<S_QuestInfo>);
		_handler.Add((ushort)PacketID.S_QuestInfo, PacketHandler.S_QuestInfoHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastEquipped, MakePacket<S_BroadcastEquipped>);
		_handler.Add((ushort)PacketID.S_BroadcastEquipped, PacketHandler.S_BroadcastEquippedHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastUnEquipped, MakePacket<S_BroadcastUnEquipped>);
		_handler.Add((ushort)PacketID.S_BroadcastUnEquipped, PacketHandler.S_BroadcastUnEquippedHandler);
		_makeFunc.Add((ushort)PacketID.S_CheckLogin, MakePacket<S_CheckLogin>);
		_handler.Add((ushort)PacketID.S_CheckLogin, PacketHandler.S_CheckLoginHandler);
		_makeFunc.Add((ushort)PacketID.S_SelectHost, MakePacket<S_SelectHost>);
		_handler.Add((ushort)PacketID.S_SelectHost, PacketHandler.S_SelectHostHandler);
		_makeFunc.Add((ushort)PacketID.S_SetServerSession, MakePacket<S_SetServerSession>);
		_handler.Add((ushort)PacketID.S_SetServerSession, PacketHandler.S_SetServerSessionHandler);
		_makeFunc.Add((ushort)PacketID.S_SuccessRegister, MakePacket<S_SuccessRegister>);
		_handler.Add((ushort)PacketID.S_SuccessRegister, PacketHandler.S_SuccessRegisterHandler);
		_makeFunc.Add((ushort)PacketID.S_SendLevel, MakePacket<S_SendLevel>);
		_handler.Add((ushort)PacketID.S_SendLevel, PacketHandler.S_SendLevelHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastPotionEat, MakePacket<S_BroadcastPotionEat>);
		_handler.Add((ushort)PacketID.S_BroadcastPotionEat, PacketHandler.S_BroadcastPotionEatHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastExp, MakePacket<S_BroadcastExp>);
		_handler.Add((ushort)PacketID.S_BroadcastExp, PacketHandler.S_BroadcastExpHandler);
		_makeFunc.Add((ushort)PacketID.S_RequestBarter, MakePacket<S_RequestBarter>);
		_handler.Add((ushort)PacketID.S_RequestBarter, PacketHandler.S_RequestBarterHandler);
		_makeFunc.Add((ushort)PacketID.S_PlayerBarter, MakePacket<S_PlayerBarter>);
		_handler.Add((ushort)PacketID.S_PlayerBarter, PacketHandler.S_PlayerBarterHandler);
		_makeFunc.Add((ushort)PacketID.S_CancelBarter, MakePacket<S_CancelBarter>);
		_handler.Add((ushort)PacketID.S_CancelBarter, PacketHandler.S_CancelBarterHandler);
		_makeFunc.Add((ushort)PacketID.S_FinishBarter, MakePacket<S_FinishBarter>);
		_handler.Add((ushort)PacketID.S_FinishBarter, PacketHandler.S_FinishBarterHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastRespawnMonster, MakePacket<S_BroadcastRespawnMonster>);
		_handler.Add((ushort)PacketID.S_BroadcastRespawnMonster, PacketHandler.S_BroadcastRespawnMonsterHandler);
		_makeFunc.Add((ushort)PacketID.S_SendDragItemInfo, MakePacket<S_SendDragItemInfo>);
		_handler.Add((ushort)PacketID.S_SendDragItemInfo, PacketHandler.S_SendDragItemInfoHandler);

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