using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	ServerSession _session = new ServerSession();

	public void Send(ArraySegment<byte> sendBuff)
	{
		_session.Send(sendBuff);
	}

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		// DNS (Domain Name System)
		string host = Dns.GetHostName();
		IPHostEntry ipHost = Dns.GetHostEntry(host);
		IPAddress ipAddr = ipHost.AddressList[0];
		// 서버 ip와 포트 설정 후 exe파일 전송
		//IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

		IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.45.83"), 7777);
        //IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 7777);

        Connector connector = new Connector();

		connector.Connect(endPoint,
			() => { Debug.Log($"주소 : {endPoint.Address}"); return _session; },
			1);
	}

	void Update()
	{
		List<IPacket> list = PacketQueue.Instance.PopAll();
		foreach (IPacket packet in list)
			PacketManager.Instance.HandlePacket(_session, packet);
	}
}
