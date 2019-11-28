using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Example;

namespace _1005Server
{
    enum IoType { NON, INPUT, OUTPUT };

    class BankManager
    {
        WbDB wbdb = new WbDB();
        public WbServer Server { get; set; }

         #region 싱글톤 문법

        static public BankManager Singleton { get; private set; }
        static BankManager()
        {
            Singleton = new BankManager();
        }
        private BankManager()
        {
            wbdb.Connect();
        }

        #endregion

     
        #region 수신메시지 처리

        public void PrintLog(Socket sock, string str)
        {
            int id = int.Parse(str);
            string pack = wbdb.WbLogPrintAll(id);
            Server.SendMessage(sock, pack);
        }
        public void AddAccountMessage(Socket sock, string str)
        {

            string pack = wbdb.WbAddAccountMessage(str);
            Server.SendMessage(sock, pack);
        }

        public void PrintAllAccountMessage(Socket sock, string str)
        {
            Console.WriteLine("전체 회원정보 출력 요청");

            string pack = wbdb.WbPrintAllAccountMessage();
            Server.SendMessage(sock, pack);
        }

        public void SelectAccount(Socket sock, string str)
        {
       
            string pack = wbdb.WbSelectAccount(str);
            Server.SendMessage(sock, pack);
        }
   
        public void UpdateAccount(Socket sock, string str)
        {

            //3. 클라이언트에 전송
            string pack = wbdb.WbUpdateAccount(str);
            Server.SendMessage(sock, pack);
        }
        
        public void SendChat(Socket sock, string str)
        {
            string[] temp = str.Split('#');
            string pack = Packet.MessagePacket(int.Parse(temp[0]), temp[1]);
            Server.SendChatMessage(sock, pack);
        }

        public void DeleteAccount(Socket sock, string str)
        {
        
            //3. 클라이언트에 전송
            string pack = wbdb.WbDeleteAccount(str);
            Server.SendMessage(sock, pack);
        }
        #endregion
    }
}
