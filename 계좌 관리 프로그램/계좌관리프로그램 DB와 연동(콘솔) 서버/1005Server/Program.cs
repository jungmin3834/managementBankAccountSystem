using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example;
using System.Net.Sockets;      //<============1)

namespace _1005Server
{ 


    class Program
    {
        WbServer server = new WbServer();       
   
        #region 네트웤 ==> 해당 코드(이벤트 핸들러)
        
        private void RecvLog(object obj, LogEventArgs e)
        {
            if(e.LogType == LogMsgType.ERROR)
            {
                Console.WriteLine("[에러] {0} ({1},{2})", 
                    e.Message, e.EventDate.ToShortDateString(),
                    e.EventDate.ToShortTimeString());
            }
            else if(e.LogType == LogMsgType.ACCEPT)
            {
                Console.WriteLine("[연결] {0}:{1} ({2},{3})",
                   e.Ip, e.Port, e.EventDate.ToShortDateString(),
                    e.EventDate.ToShortTimeString());
            }
            else if(e.LogType == LogMsgType.CLOSE)
            {
                Console.WriteLine("[해제] {0}:{1} ({2},{3})",
                   e.Ip, e.Port, e.EventDate.ToShortDateString(),
                    e.EventDate.ToShortTimeString());
            }
            else if(e.LogType == LogMsgType.START)
            {
                Console.WriteLine("[시작] {0} ({1},{2})",
                    e.Message, e.EventDate.ToShortDateString(),
                    e.EventDate.ToShortTimeString());
            }
            else if(e.LogType == LogMsgType.STOP)
            {
                Console.WriteLine("[종료] {0} ({1},{2})",
                    e.Message, e.EventDate.ToShortDateString(),
                    e.EventDate.ToShortTimeString());
            }
        }

        private void RecvMsg(object obj, MessageEventArgs e)
        { 
            string[] token = e.Message.Split('\a');
            switch (token[0].Trim())
            {
                case "PACK_ADDACCOUNT": //아이디 중복조회 결과 사용가능
                    BankManager.Singleton.AddAccountMessage(e.Sock, token[1]);
                    break;

                case "PACK_PRINTALLACCOUNT":  //아이디 중복조회 결과 사용불가
                    BankManager.Singleton.PrintAllAccountMessage(e.Sock, token[1]);
                    break;

                case "PACK_SELECTACCOUNT":
                    BankManager.Singleton.SelectAccount(e.Sock, token[1]);
                    break;

                case "PACK_UPDATEACCOUNT":
                    BankManager.Singleton.UpdateAccount(e.Sock, token[1]);
                    break;

                case "PACK_DELETEACCOUNT":
                    BankManager.Singleton.DeleteAccount(e.Sock, token[1]);
                    break;
                case "PACK_SHORTMESSAGE":
                    BankManager.Singleton.SendChat(e.Sock, token[1]);
                    break;
                case "PACK_PRINTLOG":
                    BankManager.Singleton.PrintLog(e.Sock, token[1]);
                    break;
              }
        }

        #endregion
     

        public void Run()
        {
            server.logevent += new LogEvent(RecvLog);
            server.messagevent += new MessageEvent(RecvMsg);

            BankManager.Singleton.Server = server;//<=======

            server.Start(7000);
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }
    }
}
