using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    public enum LogMsgType { ERROR, START, STOP, ACCEPT, CLOSE, MSG}

    #region 로그메시지 
    public class LogEventArgs : EventArgs
    {
        public LogMsgType LogType   { get; private set; }
        public string Ip            { get; private set; }
        public int Port             { get; private set; }
        public string Message       {get; private set; }

        //이벤트 발생 일시
        public DateTime EventDate   { get; private set; }

        public LogEventArgs(LogMsgType logtype, string ip, int port, string message)
        {
            LogType     = logtype;
            Ip          = ip;
            Port        = port;
            Message     = message;
            EventDate   = DateTime.Now;
        }
    }

    public delegate void LogEvent(object obj, LogEventArgs e);

    #endregion


    #region 수신 메시지 
    public class MessageEventArgs : EventArgs
    {
        public Socket Sock { get; private set; }

        public string Ip { get; private set; }
        public int Port { get; private set; }
        public string Message { get; private set; }

        //이벤트 발생 일시
        public DateTime EventDate { get; private set; }

        public MessageEventArgs(Socket sock, string ip, int port, string message)
        {
            Sock = sock;
            Ip = ip;
            Port = port;
            Message = message;
            EventDate = DateTime.Now;
        }
    }

    public delegate void MessageEvent(object obj, MessageEventArgs e);
    #endregion

}
