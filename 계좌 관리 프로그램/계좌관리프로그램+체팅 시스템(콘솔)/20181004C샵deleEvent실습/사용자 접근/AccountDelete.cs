using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20181004C샵deleEvent실습
{
    class AccountDelete
    {
        public void Delete()
        {
            Console.WriteLine(">> 검색 계좌 입력하세요.");
            int id;
            int.TryParse(Console.ReadLine(), out id);

            BankManager.Bank_Singleton.DeletAccount(id);
            

        }
    }
}
