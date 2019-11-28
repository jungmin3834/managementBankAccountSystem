using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20181004C샵deleEvent실습
{

    public class Account
    {
        public int id
        {
            get;
            private set;
        }
        public string name
        {
            get;
            private set;

        }
        public int balance
        {
            get;
             set;
        }
        public DateTime date
        {
            get;
            private set;
        }

        public Account(string _name,int _balance)
        {
          //  id = AccountID.Singleton.NextAccountID();
            name = _name;
            balance = _balance;
            date = System.DateTime.Now;
        }

        public override string ToString()
        {
            string temp = string.Format(" - [{0}]번 이름 : {1} 소지금 : {2:C} 날짜 : {3} {4}",id,name,balance,date.ToShortDateString(),date.ToShortTimeString());
            return temp;
        }

        public void Print()
        {
            Console.WriteLine("[계좌 번호] {0}", id);
            Console.WriteLine("[이      름] {0}", name);
            Console.WriteLine("[잔      액] {0}", balance);
            Console.WriteLine("[개설 일시] {0} {1}", date.ToShortDateString(),date.ToShortTimeString());
        }
    }

   
    public struct Data
    {
        public SortType type;
      //  public List<LogDate> savelog;
        public Dictionary<int, Account> savelist;
      
        public Data(SortType sort,Dictionary<int,Account>list)
        {
            type = sort;
            savelist = list;
        
        }
    }
}
