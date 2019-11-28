using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20181004C샵deleEvent실습
{
    public class AccountUpdate
    {
        public void Update()
        {
            Console.WriteLine(">> 계좌번호 입력하세요.");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine(">> [1] 입금 [2]출금 선택하세요");
             IoType type = (IoType)int.Parse(Console.ReadLine());
      
             Console.WriteLine(">> 수정 금액");
            int balance;
            int.TryParse(Console.ReadLine(), out balance);

            BankManager.Bank_Singleton.UpdateAccount(id, type, balance);
       
        }

    }
}
