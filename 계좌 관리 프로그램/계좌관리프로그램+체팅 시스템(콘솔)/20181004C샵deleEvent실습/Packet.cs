using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20181004C샵deleEvent실습
{
    class Packet
    {
        public static string AddAccountPacket(string name,int balance)
        {
            string msg = null;
            msg += "PACK_ADDACCOUNT\a";
            msg += name.Trim() + "#";
            msg += balance.ToString().Trim();

            return msg;
        }
        public static string LogInPacket(string id, IoType type, string pw)
        {
            string msg = null;
            msg += "ACK_LOGIN\a";
            msg += id.Trim() + "#";
       
            msg += pw.Trim();
            return msg;
        }

        public static string SelectAccountPacket(int id)
        {
            string msg = null;
            msg += "PACK_SELECTACCOUNT\a";
            msg += id.ToString().Trim();

            return msg;
        }
        public static string UpdateAccountPacket(int id,IoType type,int balance)
        {
            string msg = null;
            msg += "PACK_UPDATEACCOUNT\a";
            msg += id.ToString().Trim() + "#";
            msg += ((int)type).ToString().Trim() + "#";
            msg += balance.ToString();
     

            return msg;
        }
        public static string DeleteAccountPacket(int id)
        {
            string msg = null;
            msg += "PACK_DELETEACCOUNT\a";
            msg += id.ToString().Trim();

            return msg;
        }
        public static string PrintLog(int idx)
        {
            string msg = null;
            msg += "PACK_PRINTLOG\a";
            msg += idx.ToString().Trim();
            return msg;
        }


        public static string PrintAll()
        {
            string msg = null;
            msg += "PACK_PRINTALLACCOUNT\a";
            return msg;
        }

        public static string SendChatMessage(int id,string str)
        {
            string msg = null;
            msg += "PACK_SHORTMESSAGE\a";
            msg += id.ToString().Trim() +"#";
            msg += str;

            return msg;
        }
    }
}
