using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20181004C샵deleEvent실습
{
     class AccountSelect
    {

        public void SelectAccount()
        {

            Console.WriteLine(">> 검색 계좌 입력하세요.");
            int balance;
            int.TryParse(Console.ReadLine(), out balance);
            BankManager.Bank_Singleton.SelectAccount(balance);
   
      
        }
    }
}
