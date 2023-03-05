using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager 
{
    public string Token { get; set; } = "";
    public StatInfo MyStatInfo { get; set; } = new StatInfo();

    public bool DoubleConnect { get; set; }

    public LobbyPlayerInfo LobbyPlayerInfo { get; set;} = new LobbyPlayerInfo();

    ServerSession _session = new ServerSession();

    public void Send(IMessage packet)
    {
        _session.Send(packet);
    }
    // Start is called before the first frame update
    public void ConnectToGame()
    {
        // DNS (Domain Name System)
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[1];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        Connector connector = new Connector();

        connector.Connect(endPoint,
            () => { return _session; },
            1);
    }

    // Update is called once per frame
    public void Update()
    {
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        foreach (PacketMessage packet in list)
        {
            Action<PacketSession, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.Id);
            if (handler != null)
                handler.Invoke(_session, packet.Message);
        }

    }
}
