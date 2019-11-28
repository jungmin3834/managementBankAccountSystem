using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using _20181004C샵deleEvent실습;
namespace Example
{
    
    class WbClient
    {
        #region 이벤트 객체 생성
        public event LogEvent logevent = null;
        public event MessageEvent messgevent = null;
        #endregion



        public WbClient()
        {
           logevent += new LogEvent(RecvLog);
           messgevent += new MessageEvent(RecvMsg);
        }
        #region 네트웤 ==> 해당 코드(이벤트 핸들러)
        public void RecvLog(object obj, LogEventArgs e)
        {
            if (e.LogType == LogMsgType.ERROR)
            {
                Console.WriteLine("[에러] {0} ({1},{2})", e.Message, e.EventDate.ToShortDateString(), e.EventDate.ToShortTimeString());
            }

            else if (e.LogType == LogMsgType.CLOSE)
            {
                Console.WriteLine("[종료] {0} ({1},{2})", e.Message, e.EventDate.ToShortDateString(), e.EventDate.ToShortTimeString());
            }
            else if (e.LogType == LogMsgType.CONNECT)
            {
                Console.WriteLine("[시작] {0} ({1},{2})", e.Message, e.EventDate.ToShortDateString(), e.EventDate.ToShortTimeString());

            }

        }
        public void RecvMsg(object obj, MessageEventArgs e)
        {
           // Console.WriteLine(">>메시지 수신 [{0}:{1}] {2} ({3},{4})", e.Ip, e.Port, e.Message, e.EventDate.ToShortDateString(), e.EventDate.ToShortTimeString());

             string[] token = e.Message.Split('\a');

         
             switch (token[0].Trim())
             {
                 case "ACK_ADDACCOUNT": BankManager.Bank_Singleton.AddAccountResult(token[1]); break;
                 case "ACK_PRINTALLACCOUNT": BankManager.Bank_Singleton.PrintAllAccountResult(token[1]); break;
                 case "ACK_UPDATEACCOUNT": BankManager.Bank_Singleton.UpdateAccountResult(token[1]); break;
                 case "ACK_DELETEACCOUNT": BankManager.Bank_Singleton.DeletAccountResult(token[1]); break;
                 case "ACK_SELECTACCOUNT": BankManager.Bank_Singleton.SelectAccountResult(token[1]); break;
                 case "ACK_SHORTMESSAGE": BankManager.Bank_Singleton.SendChatMessageResult(token[1]); break;
                 case "ACK_LOGPRINT": BankManager.Bank_Singleton.PrintAllLog(token[1]); break;
             }

     
        }
        #endregion
        private Socket server;
        public bool Start(string ip,int port)
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), port);

                server = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Stream, ProtocolType.Tcp);

                server.Connect(ipep);  // 127.0.0.1 서버 7000번 포트에 접속시도

                logevent(this, new LogEventArgs(LogMsgType.CONNECT, ipep.Address.ToString(), ipep.Port, "서버 접속 성공"));

                Thread thread = new Thread(new ParameterizedThreadStart(WorkThread));
                thread.Start(server);
            }
            catch(Exception ex)
            {
              //logevent(this, new LogEventArgs(LogMsgType.ERROR, "", 0, "서버 접속 에러"));
                Console.WriteLine("[에러] "+ex.Message);
                return false;
            }


            return true;
        }

        public void Close()
        {
            logevent(this, new LogEventArgs(LogMsgType.CLOSE, "",0, "서버 접속 해제"));
            server.Close();
        }

        public void SendData(string msg)
        {
            
            SendData(server, System.Text.Encoding.Default.GetBytes(msg));
        }

        private void SendData(Socket sock,byte[] data)
        {
            try
            {
                int total = 0;
                int size = data.Length;
                int left_data = size;
                int send_data = 0;

                // 전송할 데이터의 크기 전달
                byte[] data_size = new byte[4];
                data_size = BitConverter.GetBytes(size);
                send_data = sock.Send(data_size);
              
                // 실제 데이터 전송
                while (total < size)
                {
                    send_data = sock.Send(data, total, left_data, SocketFlags.None);
                    total += send_data;
                    left_data -= send_data;
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private void ReceiveData(Socket sock, ref byte[] data)
        {
            try
            {
                int total = 0;
                int size = 0;
                int left_data = 0;
                int recv_data = 0;

                // 수신할 데이터 크기 알아내기 
                byte[] data_size = new byte[4];
                recv_data = sock.Receive(data_size, 0, 4, SocketFlags.None);
                size = BitConverter.ToInt32(data_size, 0);
                left_data = size;

                data = new byte[size];

                // 실제 데이터 수신
                while (total < size)
                {
                    recv_data = sock.Receive(data, total, left_data, 0);
                    if (recv_data == 0) break;
                    total += recv_data;
                    left_data -= recv_data;
                }
            }
            catch (Exception ex)
            {
                //  Console.WriteLine("[수신에러]" + ex.Message);
                throw ex;
            }
        }

        private void WorkThread(object value)
        {
        
            Socket socket = (Socket)value;
            IPEndPoint ip = (IPEndPoint)socket.RemoteEndPoint;
            byte[] data = new byte[1024];
  
            while (true)
            {
                try
                {
                    while (true)
                    {
                     
                                ReceiveData(socket, ref data);
                                string msg = Encoding.Default.GetString(data);

                                messgevent(msg, new MessageEventArgs(ip.Address.ToString(), ip.Port, msg));
                           
                            
                            // Cosole.WriteLine("수신 메세지 : " + Encoding.Default.GetString(data, 0, size));

                    }

                    }
                catch (Exception ex)
                {
                    //logevent(this, new LogEventArgs(LogMsgType.ERROR, "", 0, "클라이언트 접속 에러"));
                    Console.WriteLine(ex.Message);
                    
                }
            }
        }


    }
}
