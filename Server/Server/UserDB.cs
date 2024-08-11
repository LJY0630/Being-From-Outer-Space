using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class UserInfoDB
    {
        public string id;
        public string password;
        public int playerId;
        public ClientSession session;
        public bool logined;

        public UserInfoDB() 
        {
            logined = false;
        }
        public UserInfoDB(string i,string p)
        {
            id = i; password=p;
            playerId = -1;
        }

    }


    class UserDB
    {
        static UserDB _instance;
        public static UserDB Instance { get { Init(); return _instance; } }

        // 아이디를 통해 검색
        Dictionary<string, UserInfoDB> _users = new Dictionary<string, UserInfoDB>();

        public UserDB()
        {

        }

        static void Init()
        {
            if(_instance == null)
            {
                _instance = new UserDB();
            }
        }

        public bool CheckLogin(C_RequestLogin packet)
        {
            // 패킷 같은 정보를 받아 판단해서 결과 리턴
            UserInfoDB userDB = null;
            Console.WriteLine("체크로그인 시작");
            if (_users.TryGetValue(packet.Id, out userDB))
            {
                if(packet.Password==userDB.password)
                {
                    Console.WriteLine($"로그인성공");
                    if (userDB.logined==false)
                        return true;
                }
                Console.WriteLine($"비밀번호 불일치 packet : {packet.Password}, DB : {userDB.password}");
                return false;
            }
            Console.WriteLine($"아이디 : {packet.Id} 정보 없음");
            return false;
        }

        public bool RegisterInfo(C_RequestRegist packet)
        {
            UserInfoDB userDB = null;
            if(_users.TryGetValue(packet.Id,out userDB))
            {
                // 이미 있는 아이디
                Console.WriteLine("이미 있는 아이디 => 회원가입 실패");
                return false;
            }
            userDB = new UserInfoDB();
            userDB.id = packet.Id;
            userDB.password = packet.Password;
            _users.Add(packet.Id,userDB);

            Console.WriteLine($"회원가입 완료 => 아이디 : {userDB.id}");
            return true;
        }

        public UserInfoDB GetUserInfo(string id)
        {
            UserInfoDB userInfoDB = null;
            if (_users.TryGetValue(id, out userInfoDB))
            {
                return userInfoDB;
            }
            else
                return null;
        }
    }
}
