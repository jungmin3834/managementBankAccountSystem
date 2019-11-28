using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace _1005Server
{
    class WbDB
    {
        MySqlConnection conn = new MySqlConnection();
        string connstring = @"Server=127.0.0.1;Port=8888;Database=mydb;Uid=root;Pwd=qudwns12";
        #region 서버접속
        public void Connect()
        {
            conn.ConnectionString = connstring;
            conn.Open();
        }

        public void DisConnect()
        {
            conn.Close();
        }
        #endregion


        #region 로그 작성
        public int GetAccId(string name)
        {
            string comtext = "select AccId from Account where name = @Name";

            Dictionary<int, Account> accountlist = new Dictionary<int, Account>();
            MySqlCommand command = new MySqlCommand(comtext, conn);

            MySqlParameter param_time = new MySqlParameter("@Name", name);
            command.Parameters.Add(param_time);

            MySqlDataReader myDataReader;

            myDataReader = command.ExecuteReader();
             
            int idx = -1;
            while (myDataReader.Read())
            {
                idx = int.Parse(myDataReader["AccId"].ToString());
            }
            myDataReader.Close();

            return idx;

        }

        public int GetBalance(int id)
        {
            string comtext = "select balance from Account where accid = @Name";

            Dictionary<int, Account> accountlist = new Dictionary<int, Account>();
            MySqlCommand command = new MySqlCommand(comtext, conn);

            MySqlParameter param_time = new MySqlParameter("@Name", id);
            command.Parameters.Add(param_time);

            MySqlDataReader myDataReader;

            myDataReader = command.ExecuteReader();

            int idx = -1;
            while (myDataReader.Read())
            {
                idx = int.Parse(myDataReader["Balance"].ToString());
            }
            myDataReader.Close();

            return idx;

        }

        public string WbLogPrintAll(int id)
        {
            string comtext = "select * from account a1 ,tradeinfo t1 where a1.accid = t1.accid and t1.accid = @ACC";
            
            Dictionary<int, Account> accountlist = new Dictionary<int, Account>();
            List<Log> log = new List<Log>();
            MySqlCommand command = new MySqlCommand(comtext, conn);

            MySqlParameter accnum = new MySqlParameter();
            accnum.ParameterName = "@ACC";
            accnum.MySqlDbType = MySqlDbType.Int32;
            accnum.Value = id;
            command.Parameters.Add(accnum);

            MySqlDataReader myDataReader;

            myDataReader = command.ExecuteReader();
           
            string str=null;
            List<string> msg = new List<string>();
            msg.Clear();
            while (myDataReader.Read())
            {
               DateTime date = DateTime.Parse(myDataReader["Date"].ToString());
                str = null;
                str += int.Parse(myDataReader["AccId"].ToString()) + "@";
                str += myDataReader["Name"].ToString() + "@";
                str += myDataReader["TYPE"].ToString() + "@";
              //  str += GetBalance(date).ToString() + "@";
                str += myDataReader["Balance"].ToString() + "@";
                str += int.Parse(myDataReader["Money"].ToString()) + "@";
                str += DateTime.Parse(myDataReader["Date"].ToString()) + "#";
                msg.Add(str);

            }
            Console.WriteLine("옴" + id);
            myDataReader.Close();
            string pack = Packet.PrintLogAll(msg);
            return pack;
        }

        #endregion

        #region 기본 기능

        public string WbAddLogDate(int accid,string msg,int balance,int money)
        {
            string comtext = "insert into TradeInfo (accid,type,BALANCE,money,Date) values (@ACC,@TYPE,@Balance,@Money,@date)";
            MySqlCommand command = new MySqlCommand(comtext, conn);

            MySqlParameter accnum = new MySqlParameter();
            accnum.ParameterName = "@ACC";
            accnum.MySqlDbType = MySqlDbType.Int32;
            accnum.Value = accid;
            command.Parameters.Add(accnum);

            MySqlParameter param_time = new MySqlParameter("@TYPE", msg);
            command.Parameters.Add(param_time);

            MySqlParameter Balan = new MySqlParameter();
            Balan.ParameterName = "@Balance";
            Balan.MySqlDbType = MySqlDbType.Int32;
            Balan.Value = balance;
            command.Parameters.Add(Balan);

            MySqlParameter param_price = new MySqlParameter();
            param_price.ParameterName = "@Money";
            param_price.MySqlDbType = MySqlDbType.Int32;
            param_price.Value = money;
            command.Parameters.Add(param_price);


            MySqlParameter time = new MySqlParameter("@date", DateTime.Now);
            command.Parameters.Add(time);

            if (command.ExecuteNonQuery() == 1)
                return "";

            return "";
        }
   
        public string WbAddAccountMessage(string str)
        {
            //1. 토큰 분리
            string[] token = str.Split('#');

            //2. 기능 연산
            Account acc = new Account(token[0], int.Parse(token[1]));
            //token[0] = 이름 1 = 돈
            int balance = int.Parse(token[1]);

            string comtext = "insert into Account (Name,Balance,Date) values (@Name,@Balance,@date)";
            MySqlCommand command = new MySqlCommand(comtext, conn);


            MySqlParameter param_time = new MySqlParameter("@Name", token[0]);
            command.Parameters.Add(param_time);

            MySqlParameter param_price = new MySqlParameter();
            param_price.ParameterName = "@Balance";
            param_price.MySqlDbType = MySqlDbType.Int32;
            param_price.Value = balance;
            command.Parameters.Add(param_price);

            MySqlParameter time = new MySqlParameter("@date", DateTime.Now);
            command.Parameters.Add(time);

            bool result;
            if (command.ExecuteNonQuery() == 1)
                result = true;
            else
                result = false;

            Console.WriteLine("오긴온다..");
            int idx;
            idx = GetAccId(token[0]);
    

          
            WbAddLogDate(idx, "계좌 생성", int.Parse(token[1]), int.Parse(token[1]));

              string pack = Packet.AddAccountPacket(result, acc.Id);

              return pack;
  
        }

    
        public string WbPrintAllAccountMessage()
        {


            string comtext = "select * from Account";
            Dictionary<int, Account> accountlist = new Dictionary<int, Account>();
            MySqlCommand command = new MySqlCommand(comtext, conn);

            MySqlDataReader myDataReader;

            myDataReader = command.ExecuteReader();
         
       
            while (myDataReader.Read())
            {
                Account acc = new Account(int.Parse(myDataReader["AccId"].ToString()), myDataReader["Name"].ToString(),
                    int.Parse(myDataReader["Balance"].ToString()), DateTime.Parse(myDataReader["Date"].ToString()));

                 accountlist.Add(acc.Id,acc);
            }
            myDataReader.Close();
            string pack = Packet.PrintAllAccountPacket(accountlist);
            return pack;
        }

        public string WbSelectAccount(string str)
        {
            //1. 토큰 분리
            int id = int.Parse(str);
            string pack = null;


            string comtext = "select * from Account where @Id = AccId";
            Dictionary<int, Account> accountlist = new Dictionary<int, Account>();
            MySqlCommand command = new MySqlCommand(comtext, conn);
            MySqlDataReader myDataReader;


            MySqlParameter param_price = new MySqlParameter();
            param_price.ParameterName = "@Id";
            param_price.MySqlDbType = MySqlDbType.Int32;
            param_price.Value = id;
            command.Parameters.Add(param_price);
        

            myDataReader = command.ExecuteReader();


            while (myDataReader.Read())
            {
                Account acc = new Account(int.Parse(myDataReader["AccId"].ToString()), myDataReader["Name"].ToString(),
                    int.Parse(myDataReader["Balance"].ToString()), DateTime.Parse(myDataReader["a1.Date"].ToString()));

                accountlist.Add(acc.Id, acc);
            }
            myDataReader.Close();

            pack = Packet.PrintAllAccountPacket(accountlist);
            return pack;
     
        }

        public string WbUpdateAccount(string str)
        {
            //1. 토큰 분리
            string[] token = str.Split('#');

            //2. 기능 연산
            int id = int.Parse(token[0]); //검색ID
            IoType iotype = (IoType)int.Parse(token[1]);//입출금체크
            int balance = int.Parse(token[2]); //입출금액

            bool result;
            string text=null;
       
            string comtext;
            if (iotype == IoType.INPUT)
            {

                comtext = "Update Account SET Balance = (Balance + @Price) where Accid = @Accid and @Price > -1";
                text = "입금";
            }
            else if (iotype == IoType.OUTPUT)
            {
                comtext = "Update Account SET Balance = (Balance - @Price) where Accid = @Accid and  Balance >= @Price and @price > -1";
                text = "출금";
            }
            else
               return "";

     
            MySqlCommand command = new MySqlCommand(comtext, conn);
            MySqlParameter accID = new MySqlParameter();
            accID.ParameterName = "@Price";
            accID.MySqlDbType = MySqlDbType.Int32;
            accID.Value = balance;
            command.Parameters.Add(accID);
      

            MySqlParameter param_price = new MySqlParameter();
            param_price.ParameterName = "@Accid";
            param_price.MySqlDbType = MySqlDbType.Int32;
            param_price.Value = id;
            command.Parameters.Add(param_price);
            int money = GetBalance(id);
            WbAddLogDate(id, text, money, balance);
            if (command.ExecuteNonQuery() == 1)
                result = true;
            else
                result = false;

          

            //3. 클라이언트에 전송
           string pack = Packet.UpdateAccountPacket(result, id);
           return pack;
        }

       public string WbLogDelete(int  id)
        {
 
            string comtext = "Delete from Tradeinfo where AccId = @Accid";
            MySqlCommand command = new MySqlCommand(comtext, conn);
            MySqlParameter param_price = new MySqlParameter();
            param_price.ParameterName = "@Accid";
            param_price.MySqlDbType = MySqlDbType.Int32;
            param_price.Value = id;
            command.Parameters.Add(param_price);

            bool result;
            if (command.ExecuteNonQuery() == 1)
                result = true;
            else
                result = false;

            string pack = Packet.DeleteAccountPacket(result, id);
            //1. 토큰 분리

            return pack;
        }
        public string WbDeleteAccount(string str)
        {
               int id = int.Parse(str);

               string comtext = "Delete from Account where AccId = @Accid";
               MySqlCommand command = new MySqlCommand(comtext, conn);
            MySqlParameter param_price = new MySqlParameter();
            param_price.ParameterName = "@Accid";
            param_price.MySqlDbType = MySqlDbType.Int32;
            param_price.Value = id;
            command.Parameters.Add(param_price);



                   bool result;
                   if (command.ExecuteNonQuery() == 1)
                       result = true;
                   else
                       result = false;
                   WbLogDelete(id);
            string pack = Packet.DeleteAccountPacket(result, id);
            //1. 토큰 분리

            return pack;
            //2. 기능 연산


        }

        #endregion
    }
}
