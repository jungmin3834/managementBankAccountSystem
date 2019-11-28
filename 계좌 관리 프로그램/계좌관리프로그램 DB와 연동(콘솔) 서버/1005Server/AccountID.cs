using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1005Server
{
    class AccountID
    {
        #region 싱글톤 문법

        static public AccountID Singleton { get; private set; }
        static AccountID()
        {
            Singleton = new AccountID();
        }
        private AccountID()
        {
            Id = 1000;
        }

        #endregion


        public int Id { get; private set; }

        public int NextAccountID()
        {
            int temp = Id;
            Id += 10;
            return temp;
        }
    }
}
