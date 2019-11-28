using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace _1005Server
{
    class Packet
    {


        static public string AddAccountPacket(bool result, int id)
        {
            string msg = null;
            msg += "ACK_ADDACCOUNT\a";         // 회원 가입 요청 메시지
            msg += result.ToString().Trim() + "#";                  // 아이디
            msg += id.ToString().Trim();
            return msg;
        }

        static public string PrintAllAccountPacket(
                        Dictionary<int, Account> accountlist)
        {
            string msg = null;
            msg += "ACK_PRINTALLACCOUNT\a";         // 회원 가입 요청 메시지
            foreach(KeyValuePair<int, Account> keyvalue in accountlist)
            {
                Account acc = keyvalue.Value;
                msg += acc.Id + "@";
                msg += acc.Name + "@";
                msg += acc.Balance + "@";
                msg += acc.Date.ToShortDateString() + "@";
                msg += acc.Date.ToShortTimeString() + "#";
            }
            return msg;
        }

        static public string PrintLogAll(
                        List<string> str)
        {


            string msg = null;
            msg += "ACK_LOGPRINT\a";
            foreach (string cc in str)
            {
                msg += cc;
            }
        
            return msg;
        }

        static public string SelectAccountPacket(bool result, Account acc)
        {
            string msg = null;
            msg += "ACK_SELECTACCOUNT\a";         // 회원 가입 요청 메시지
            msg += acc.Id.ToString().Trim() + "#";
            msg += acc.Name.Trim() + "#";
            msg += acc.Balance.ToString().Trim() + "#";
            msg += acc.Date.ToShortDateString().Trim() + "#";
            msg += acc.Date.ToShortTimeString().Trim() + "#";
            return msg;
        }

        static public string UpdateAccountPacket(bool result, int id)
        {
            string msg = null;
            msg += "ACK_UPDATEACCOUNT\a";         // 회원 가입 요청 메시지
            msg += result.ToString().Trim() + "#";                  // 아이디
            msg += id.ToString().Trim();

            return msg;
        }
        static public string MessagePacket(int  ID,string str)
        {
            string msg = null;

  
            msg += "ACK_SHORTMESSAGE\a";
            msg += "[" + ID.ToString().Trim() +"]";
            msg += str + "("+DateTime.Now.ToShortDateString().Trim() +" : "+DateTime.Now.ToShortTimeString().Trim() +")";


            return msg;
        }
        static public string DeleteAccountPacket(bool result, int id)
        {
            string msg = null;
            msg += "ACK_DELETEACCOUNT\a";         // 회원 가입 요청 메시지
            msg += result.ToString().Trim() + "#"; 
            msg += id.ToString().Trim();
            return msg;
        }

    }
}
