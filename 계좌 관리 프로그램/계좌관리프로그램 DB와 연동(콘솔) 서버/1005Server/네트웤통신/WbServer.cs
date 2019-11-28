using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Example
{
    class WbServer
    {
        private Socket server;
        private List<Socket> clientlist = new List<Socket>();

        #region 이벤트 객체 생성

        public event LogEvent logevent = null;
        public event MessageEvent messagevent = null;

        #endregion

        public void Start(int port)
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            server = new Socket(AddressFamily.InterNetwork,
                                                SocketType.Stream, ProtocolType.Tcp);
            server.Bind(ipep);
            server.Listen(20);

            Thread thread = new Thread(new ThreadStart(ListenThread));
            thread.Start();  
        }
 
        private void ListenThread()
        {
            logevent(this, new LogEventArgs(LogMsgType.START,
                "", 0, "서버 시작... 클라이언트 접속 대기중..."));

            try
            {
                while (true)
                {
                    //1.클라이언트 접속 대기
                    Socket client = server.Accept();

                    //2.접속정보출력
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;                
                    logevent(this, new LogEventArgs(LogMsgType.ACCEPT,
                     ip.Address.ToString(), ip.Port, "클라이언트 접속"));

                    //3.컨테이너저장
                    clientlist.Add(client);

                    //4.통신스레드생성
                    Thread th = new Thread(new ParameterizedThreadStart(WorkThread));
                    th.Start(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[에러] " + ex.Message);
                logevent(this, new LogEventArgs(LogMsgType.ERROR,
                     "",0, "클라이언트 접속 에러"));
                server.Close();
            }
        }
    
        private void WorkThread(object value)
        {
            Socket socket = (Socket)value;
            IPEndPoint ip = (IPEndPoint)socket.RemoteEndPoint;

            try
            {
                while (true)
                {
                    byte[] data=null;
                    ReceiveData(socket, ref data);
                    string msg =  Encoding.Default.GetString(data);
                    messagevent(this, 
                        new MessageEventArgs(socket, ip.Address.ToString(),
                            ip.Port, msg));

                    //=======================================
                    data = Encoding.Default.GetBytes(msg);
                    //=======================================
                    //SendData(socket, data);
                    //foreach (Socket sock in clientlist)
                    //{
                    //    SendData(sock, data);
                        //sock.Send(data, data.Length, SocketFlags.None); // 문자열 전송
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[에러] " + ex.Message);
                
                logevent(this, new LogEventArgs(LogMsgType.CLOSE,
                     ip.Address.ToString(), ip.Port, "클라이언트 해제"));

                //소켓 종료 처리
                clientlist.Remove(socket);
                socket.Close();
            }
        }

        public void SendMessage(Socket sock, string str)
        {
            SendData(sock, Encoding.Default.GetBytes(str));
        }

        public void SendChatMessage(Socket sock, string str)
        {
            foreach (Socket so in clientlist)
            {
                SendData(so, Encoding.Default.GetBytes(str));
            }
        }

        #region 가변형 데이터 처리하는 코드 
        private void SendData(Socket sock, byte[] data)
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
                //Console.WriteLine(ex.Message);
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
                //Console.WriteLine("[수신에러] " + ex.Message);
                throw ex;
            }
        }
        #endregion                          
    }
}
