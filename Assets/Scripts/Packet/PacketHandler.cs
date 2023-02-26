using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler
{
	
	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
		S_EnterGame enterGamePacket = (S_EnterGame)packet;

        Managers.Object.Add(enterGamePacket.Player, myPlayer: true);

	}

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leaveGamePacket = (S_LeaveGame)packet;

        Managers.Object.Clear();
    }

    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = (S_Spawn)packet;

        foreach (ObjectInfo obj in spawnPacket.Objects)
        {

            Managers.Object.Add(obj, myPlayer: false);

        }

    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawnPacket = (S_Despawn)packet;
        foreach (int id in despawnPacket.ObjectIds)
        {

            Managers.Object.Remove(id);

        }

    }

    public static void S_GaugeHandler(PacketSession session, IMessage packet)
    {
        S_Gauge gaugePacket = (S_Gauge)packet;
        GameObject go=Managers.Object.FindById(gaugePacket.ObjectId);
        if (go == null)
            return;

        VsPlayerGauge vpg=go.GetComponent<VsPlayerGauge>();

        if(vpg!=null)
        {
            vpg.SliderValue = gaugePacket.Info.GaugeValue;
        }
        
    }


    public static void S_TimerHandler(PacketSession session, IMessage packet)
    {
        S_Timer timerPacket = (S_Timer)packet;
        GameObject go = Managers.Object.FindById(timerPacket.ObjectId);
        if (go == null) 
            return;

        VsPlayerGameTimer vpgt=go.GetComponent<VsPlayerGameTimer>();

        if (vpgt!=null)
        {
            vpgt.SliderValue = timerPacket.TimerInfo.Time;

        }

    }

    public static void S_StartGameHandler(PacketSession session, IMessage packet)
    {
        S_StartGame startPacket = (S_StartGame)packet;
        GameObject go = Managers.Object.FindById(startPacket.ObjectId);
        if (go == null)
            return;

        VsPlayerReadyGo vprg = go.GetComponent<VsPlayerReadyGo>();
        
        if (vprg!=null)
        {
            vprg.Go();
        }

    }

    public static void S_AttackHandler(PacketSession session, IMessage packet)
    {
        S_Attack startPacket = (S_Attack)packet;
        
        VsPlayerBullet tempBullet=GameManager.Instance.VsMyPlayerBlockManager.Bullets.Dequeue();
        tempBullet.MoveBullet(GameManager.Instance.VsMyPlayerBlockManager.EnemyImageRect.anchoredPosition, false, 0);

    }

    public static void S_WinnerHandler(PacketSession session, IMessage packet)
    {
        S_Winner winnerPacket = (S_Winner)packet;

        
        GameManager.Instance.VsMyPlayerBlockManager.GameSetUI.VsWinnerPlayerOpen();
        
    }

    public static void S_LoseHandler(PacketSession session, IMessage packet)
    {
        S_Lose losePacket = (S_Lose)packet;

        GameManager.Instance.VsMyPlayerBlockManager.GameSetUI.VsLosePlayerOpen();
    }

    public static void S_DrowHandler(PacketSession session, IMessage packet)
    {
        S_Drow drowPacket = (S_Drow)packet;

        GameManager.Instance.VsMyPlayerBlockManager.GameSetUI.VsDrowPlayerOpen();
    }
    public static void S_PingHandler(PacketSession session, IMessage packet)
    {
        C_Pong pongPacket = new C_Pong();
        Debug.Log("[Server] PingCheck");
        Managers.Network.Send(pongPacket);
    }

    public static void S_BlockListHandler(PacketSession session, IMessage packet)
    {
        //내블록정보
        S_BlockList blockListPacket = (S_BlockList)packet;

        foreach(BlockInfo blockInfo in blockListPacket.Blocks)
        {
            if (0 <= blockInfo.X && blockInfo.X < GameManager.BOARDX && 0 <= blockInfo.Y && blockInfo.Y < GameManager.BOARDY)
            {
                GameManager.Instance.VsMyPlayerBlockManager.AllBlocks[blockInfo.X, blockInfo.Y].InfoChange(blockInfo);
            }
        }

        GameManager.Instance.VsMyPlayerBlockManager.CheckLiveBlock();




    }

    public static void S_SpawnBlockListHandler(PacketSession session, IMessage packet)
    {
        //상대방 블록정보
        S_SpawnBlockList spawnBlockPacket= (S_SpawnBlockList)packet;

        foreach (BlockInfo blockInfo in spawnBlockPacket.Blocks)
        {
            if (0 <= blockInfo.X && blockInfo.X < GameManager.BOARDX && 0 <= blockInfo.Y && blockInfo.Y < GameManager.BOARDY)
            {
                GameManager.Instance.VsPlayerBlockManager.AllBlocks[blockInfo.X, blockInfo.Y].InfoChange(blockInfo);
            }
        }

        GameManager.Instance.VsPlayerBlockManager.CheckLiveBlock();

    }

    public static void S_SelectBlockHandler(PacketSession session, IMessage packet)
    {
        S_SelectBlock selectBlockPacket= (S_SelectBlock)packet;

        BlockInfo info = selectBlockPacket.Block;
        if (0 <= info.X && info.X < GameManager.BOARDX && 0 <= info.Y && info.Y < GameManager.BOARDY)
        {
            GameManager.Instance.VsPlayerBlockManager.AllBlocks[info.X, info.Y].Selected = info.Selected;
            if(GameManager.Instance.VsPlayerBlockManager.BeforBlock!=null)
            {
                GameManager.Instance.VsPlayerBlockManager.BeforBlock.ConnectLine(info.X, info.Y);
            }
            GameManager.Instance.VsPlayerBlockManager.BeforBlock = GameManager.Instance.VsPlayerBlockManager.AllBlocks[info.X, info.Y];

        }
    }

    public static void S_ChangeBlockHandler(PacketSession session, IMessage packet)
    {
        S_ChangeBlock changeBlockPacket= (S_ChangeBlock)packet;

        foreach (BlockInfo blockInfo in changeBlockPacket.Blocks)
        {
            if (0 <= blockInfo.X && blockInfo.X < GameManager.BOARDX && 0 <= blockInfo.Y && blockInfo.Y < GameManager.BOARDY)
            {
                GameManager.Instance.VsPlayerBlockManager.AllBlocks[blockInfo.X, blockInfo.Y].InfoChange(blockInfo);
            }
        }

        GameManager.Instance.VsPlayerBlockManager.CheckLiveBlock();


    }

    public static void S_ConnectedHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("S_ConnectedHandler");
        C_Login loginPacket = new C_Login();
        loginPacket.UniqueId = SystemInfo.deviceUniqueIdentifier;

        Managers.Network.Send(loginPacket);
    }

    // 로그인 Ok
    public static void S_LoginHandler(PacketSession session, IMessage packet)
    {
        S_Login loginPacket = (S_Login)packet;
        Debug.Log($"LoginOk({loginPacket.LoginOk})");

        // 로비 UI
        if(loginPacket.Players==null || loginPacket.Players.Count==0)
        {
            C_CreatePlayer createPacket = new C_CreatePlayer();
            createPacket.Name = $"Player_{Random.Range(0,10000).ToString("0000")}";
            Managers.Network.Send(createPacket);
        }else
        {
            LobbyPlayerInfo info = loginPacket.Players[0];
            C_EnterGame enterGamePacket =new C_EnterGame();
            enterGamePacket.Name= info.Name;
            Managers.Network.Send(enterGamePacket);
        }
    }

    public static void S_CreatePlayerHandler(PacketSession session, IMessage packet)
    {
        S_CreatePlayer createOkPacket=(S_CreatePlayer)packet;

        if(createOkPacket.Player==null)
        {
            C_CreatePlayer createPacket = new C_CreatePlayer();
            createPacket.Name = $"Player_{Random.Range(0, 10000).ToString("0000")}";
            Managers.Network.Send(createPacket);
        }else
        {
            C_EnterGame enterGamePacket = new C_EnterGame();
            enterGamePacket.Name = createOkPacket.Player.Name;
            Managers.Network.Send(enterGamePacket);
        }
    }
}
