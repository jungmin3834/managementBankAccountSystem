using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20181004C샵deleEvent실습
{
    class AccountSort
    {
        public void Sort()
        {
            Console.WriteLine("[1] ID정렬 [2] 이름 정렬");
            int sort;
            int.TryParse(Console.ReadLine(), out sort);

            switch(sort)
            {
                case 1: BankManager.Bank_Singleton.SortId(); break;
                case 2: BankManager.Bank_Singleton.SortName(); break;
            }
        }
    }

}
