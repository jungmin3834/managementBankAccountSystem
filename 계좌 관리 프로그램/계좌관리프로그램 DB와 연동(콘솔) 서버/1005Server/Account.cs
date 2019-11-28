using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1005Server
{
    public class Log
    {
        public string type;
        public int balance;
        public int money;

        public Log(string t,int b,int m)
        {
            type = t;
            balance = b;
            money = m;
        }

    }

    public class Account
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Balance { get; set; }
        public DateTime Date { get; private set; }

        public Account(int id ,string name, int balance,DateTime da)
        {
            Name = name;
            Balance = balance;
            Date = da;
            Id = id;
        }
        public Account(string name, int balance)
        {
            Name = name;
            Balance = balance;
            Date = DateTime.Now;
            Id = AccountID.Singleton.NextAccountID();
        }

        public override string ToString()
        {
            string temp = string.Format(" - [{0}] {1} {2:C} {3} {4}",
                Id, Name, Balance, Date.ToShortDateString(),
                Date.ToShortTimeString());

            return temp;
        }
    
        public void Print()
        {
            Console.WriteLine("[계좌번호] {0}", Id);
            Console.WriteLine("[이    름] {0}", Name);
            Console.WriteLine("[잔    액] {0}원", Balance);
            Console.WriteLine("[개설일시] {0} {1}",
                Date.ToShortDateString(), Date.ToShortTimeString());
        }
    }
}
