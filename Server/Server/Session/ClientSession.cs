using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using System.Net;

namespace Server
{
    #region 인벤토리 관련 클래스
    public class Item
	{
		public string id;
		public int cnt;
		public bool isEqiped=false;
		//public int equipId;
		//public List<bool> isEquipped=new List<bool>();
	}


	class InvenInfo
	{
		public int money=3000;
		public Dictionary<string,Item> items=new Dictionary<string, Item>();
	}

	class PlayerInfo
	{
		public InvenInfo info=new InvenInfo();
		public int hp;
		public int mp;
		public int exp=0;
		public int level=1;
		
	}
	#endregion

	#region 퀘스트 관련 클래스

	

	public enum QuestState
	{
        Incomplete = 0,
		Inprogress = 1,
        completed = 2,
    }

	public class QuestInfo
	{
		public int questId;
		public QuestState state=QuestState.Incomplete;
	}

    #endregion

    class ClientSession : PacketSession
	{
        public const int QUEST_CNT = 7;

        public int SessionId { get; set; }
		public GameRoom Room { get; set; }
		public float PosX { get; set; } = -448.87f;
		public float PosY { get; set; } = -30.68f;
		public float PosZ { get; set; } = 39.93f;
		public float DirectionX { get; set; }
		public float DirectionY { get; set; }
		public float DirectionZ { get; set; }
		public bool IsMoving { get; set; }

		public PlayerInfo PlayerInfo { get; set; } = new PlayerInfo();
		public QuestInfo[] QuestInfos = new QuestInfo[QUEST_CNT];

		public ClientSession()
		{
			for(int i=0;i<QUEST_CNT; i++)
			{
                QuestInfos[i] = new QuestInfo();
				QuestInfos[i].questId = 100 + i;
				QuestInfos[i].state = QuestState.Incomplete;
            }
			
			
		}


		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}");

			// 클라에서 로그인후 확인 눌렀을 때, 연결이 될거임
			// 연결되었을 때, 아이디확인 
			//Program.Room.Push(() => Program.Room.Enter(this));
			Program.Room.Push(() => Program.Room.Init(this));
		}

		public override void OnRecvPacket(ArraySegment<byte> buffer)
		{
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			SessionManager.Instance.Remove(this);
			if (Room != null)
			{
				GameRoom room = Room;
				room.Push(() => room.Leave(this));
				Room = null;
			}

			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes)
		{
			//Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}
