using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20181004C샵deleEvent실습
{
    class AccountInput
    {
   
        public void MakeAccount()
        {
            
           
            Console.WriteLine(">> 이름을 입력하세요.");
            string name = Console.ReadLine();
            Console.WriteLine(">> 초기 잔액을 입력하세요.");
            int balance;
            int.TryParse(Console.ReadLine(),out balance);
            BankManager.Bank_Singleton.AddAccount(name, balance);
   
        }

    }
}
