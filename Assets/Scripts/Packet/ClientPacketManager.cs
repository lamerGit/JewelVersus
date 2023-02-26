using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
		
    public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }

	public void Register()
	{		
		_onRecv.Add((ushort)MsgId.SEnterGame, MakePacket<S_EnterGame>);
		_handler.Add((ushort)MsgId.SEnterGame, PacketHandler.S_EnterGameHandler);		
		_onRecv.Add((ushort)MsgId.SLeaveGame, MakePacket<S_LeaveGame>);
		_handler.Add((ushort)MsgId.SLeaveGame, PacketHandler.S_LeaveGameHandler);		
		_onRecv.Add((ushort)MsgId.SSpawn, MakePacket<S_Spawn>);
		_handler.Add((ushort)MsgId.SSpawn, PacketHandler.S_SpawnHandler);		
		_onRecv.Add((ushort)MsgId.SDespawn, MakePacket<S_Despawn>);
		_handler.Add((ushort)MsgId.SDespawn, PacketHandler.S_DespawnHandler);		
		_onRecv.Add((ushort)MsgId.SGauge, MakePacket<S_Gauge>);
		_handler.Add((ushort)MsgId.SGauge, PacketHandler.S_GaugeHandler);		
		_onRecv.Add((ushort)MsgId.STimer, MakePacket<S_Timer>);
		_handler.Add((ushort)MsgId.STimer, PacketHandler.S_TimerHandler);		
		_onRecv.Add((ushort)MsgId.SStartGame, MakePacket<S_StartGame>);
		_handler.Add((ushort)MsgId.SStartGame, PacketHandler.S_StartGameHandler);		
		_onRecv.Add((ushort)MsgId.SAttack, MakePacket<S_Attack>);
		_handler.Add((ushort)MsgId.SAttack, PacketHandler.S_AttackHandler);		
		_onRecv.Add((ushort)MsgId.SWinner, MakePacket<S_Winner>);
		_handler.Add((ushort)MsgId.SWinner, PacketHandler.S_WinnerHandler);		
		_onRecv.Add((ushort)MsgId.SLose, MakePacket<S_Lose>);
		_handler.Add((ushort)MsgId.SLose, PacketHandler.S_LoseHandler);		
		_onRecv.Add((ushort)MsgId.SDrow, MakePacket<S_Drow>);
		_handler.Add((ushort)MsgId.SDrow, PacketHandler.S_DrowHandler);		
		_onRecv.Add((ushort)MsgId.SPing, MakePacket<S_Ping>);
		_handler.Add((ushort)MsgId.SPing, PacketHandler.S_PingHandler);		
		_onRecv.Add((ushort)MsgId.SBlockList, MakePacket<S_BlockList>);
		_handler.Add((ushort)MsgId.SBlockList, PacketHandler.S_BlockListHandler);		
		_onRecv.Add((ushort)MsgId.SSpawnBlockList, MakePacket<S_SpawnBlockList>);
		_handler.Add((ushort)MsgId.SSpawnBlockList, PacketHandler.S_SpawnBlockListHandler);		
		_onRecv.Add((ushort)MsgId.SSelectBlock, MakePacket<S_SelectBlock>);
		_handler.Add((ushort)MsgId.SSelectBlock, PacketHandler.S_SelectBlockHandler);		
		_onRecv.Add((ushort)MsgId.SChangeBlock, MakePacket<S_ChangeBlock>);
		_handler.Add((ushort)MsgId.SChangeBlock, PacketHandler.S_ChangeBlockHandler);		
		_onRecv.Add((ushort)MsgId.SConnected, MakePacket<S_Connected>);
		_handler.Add((ushort)MsgId.SConnected, PacketHandler.S_ConnectedHandler);		
		_onRecv.Add((ushort)MsgId.SLogin, MakePacket<S_Login>);
		_handler.Add((ushort)MsgId.SLogin, PacketHandler.S_LoginHandler);		
		_onRecv.Add((ushort)MsgId.SCreatePlayer, MakePacket<S_CreatePlayer>);
		_handler.Add((ushort)MsgId.SCreatePlayer, PacketHandler.S_CreatePlayerHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
	{
		T pkt = new T();
		pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

		if(CustomHandler!= null)
		{
			CustomHandler.Invoke(session, pkt, id);
		}else
		{
            Action<PacketSession, IMessage> action = null;
            if (_handler.TryGetValue(id, out action))
                action.Invoke(session, pkt);
        }
	}

	public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}