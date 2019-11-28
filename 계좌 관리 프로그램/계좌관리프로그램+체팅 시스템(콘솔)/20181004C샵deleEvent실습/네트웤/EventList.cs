using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{

    public enum LogMsgType { ERROR, CLOSE, CONNECT }
    #region 로그 메시지
    public class LogEventArgs : EventArgs
    {
        
        public LogMsgType LogType { get; private set; }
        public string Ip { get; private set; }
        public int Port { get; private set; }
        public string Message { get; private set; }

        public DateTime EventDate { get; private set; }

        public LogEventArgs(LogMsgType logtype, string ip, int port, string msg)
        {
            LogType = logtype;
            Ip = ip;
            Port = port;
            Message = msg;
            EventDate = System.DateTime.Now;
        }

    }

    public delegate void LogEvent(object obj, LogEventArgs e);
    #endregion

    #region 수신 메시지
    public class MessageEventArgs : EventArgs
    {

        public string Ip { get; private set; }
        public int Port { get; private set; }
        public string Message { get; private set; }

        public DateTime EventDate { get; private set; }

        public MessageEventArgs(string ip, int port, string msg)
        {

            Ip = ip;
            Port = port;
            Message = msg;
            EventDate = System.DateTime.Now;
        }

    }

    public delegate void MessageEvent(object obj, MessageEventArgs e);
    #endregion




}
