using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20181004C샵deleEvent실습
{
    class ChatClass
    {
        public void ChatRun()
        {
            ChatPrint();
            SendChat();
        }
        void ChatPrint()
        {
            Console.Clear();
            Console.WriteLine("[체팅 기능]");
        }

        void SendChat()
        {
            BankManager.Bank_Singleton.SendChatMessage();
        }

    }
}
