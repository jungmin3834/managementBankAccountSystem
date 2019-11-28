using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using Example;

namespace _20181004C샵deleEvent실습
{

    public enum IoType { NON, INPUT, OUTPUT };
    public  enum SortType { NON,ID,NAME};
    class BankManager 
    {
        public bool chat =false;
        public IoType iotype = IoType.NON;
        public SortType sorttype = SortType.NON;
        public Dictionary<int, Account> memberlist = new Dictionary<int, Account>();
        public WbClient client = new WbClient();
        private int Id;

   
        #region 이벤트 객체

        #endregion

        #region 매니셔 => 네트웤
        public void SendData(string msg)
        {
            client.SendData(msg);
        }

        public void Run(string ip, int port)
        {
            if (client.Start(ip, port) == false)
            {
                Environment.Exit(0);
            }
        }


        public void RecvData(object obj,MessageEventArgs e)
        {
            string[] token = e.Message.Split('\a');

            switch (token[0].Trim())
            {
                case "PACK_ADDACCOUNT":
                    AddAccountResult(token[1]);
                    break;
                case "PACK_PRINTALL": break;
                case "PACK_DELETEACCOUNT": break;
            }
        }
        #endregion
        #region 딕셔너리 데이터 저장 및 불러오기
        //public bool LoadFile(AccLog log)
        //{
        //    try
        //    {
        //        Stream rs = new FileStream("a.dat", FileMode.Open);
        //        BinaryFormatter deserializer = new BinaryFormatter();

        //        Data data;
        //        data = (Data)deserializer.Deserialize(rs);

        //        log.logdata = data.savelog;
        //        memberlist = data.savelist;
        //        sorttype = data.type;
        //        AccountID.Singleton.FileOpen(memberlist.Keys.Max()+10);

        //        rs.Close();
        //    }
        //    catch(Exception ex)
        //    {
        //       Console.WriteLine(ex.Message);
        //        return false;
        //    }
        //    return true;
        //}

        //public bool SaveFile(AccLog log)
        //{

        //    Stream ws = new FileStream("a.dat", FileMode.Create);
        //    BinaryFormatter serializer = new BinaryFormatter();

        //    Data data = new Data(sorttype, log.logdata, memberlist);
        //    serializer.Serialize(ws, data);
        //    ws.Close();
        //    return true;
        //}
        #endregion

        #region 싱글톤 문법
        static public BankManager Bank_Singleton { get; private set; }
         static BankManager()
        {
            Bank_Singleton = new BankManager();

        }

        private BankManager()
        {
       
        }
     
        
        static public BankManager Singleton{ get; private set; }


    
    
        #endregion

        #region 전체 출력
        public void PrintAllLog(string msg)
        {
            Console.WriteLine(msg);
            string[] token = msg.Split('#');
            string[] text;
            Console.WriteLine(token.Length);
            foreach (string filter in token)
            {
                text = filter.Split('@');
                Console.WriteLine(text.Length);
                    Console.WriteLine("[계좌번호] {0}   [이름] {1}   [선택 종류] {2}   [잔액] {3} [수정 잔액] {4} [날짜] ({5} )", 
                        text[0], text[1], text[2], text[3],text[4],text[5]);
            }
        }

        public void PrintLog()
        {
            Console.Write("검색할 계좌 >> ");
            
            SendData(Packet.PrintLog(int.Parse(Console.ReadLine())));
        }
        public void PrintAllAccount()
        {
            //if (sorttype == SortType.NAME)
            //{
            //    Console.WriteLine(">> [저장개수] :", memberlist.Count);
            //    var sortedDict = from entry in memberlist orderby entry.Value.name ascending select entry;
            //    // memberlist.Clear();
            //    foreach (var c in sortedDict)
            //    {
            //        Console.WriteLine(c.Value);
            //    }
            //}
            //else
            //{

            SendData(Packet.PrintAll());
                //Console.WriteLine(">> [저장개수] :", memberlist.Count);
                //foreach (KeyValuePair<int, Account> acc in memberlist)
                //{
                //    Console.WriteLine(acc.Value);
                //}
            
        }

        public void PrintAllAccountResult(string msg)
        {
         //   Console.WriteLine("dfqewit jpiqwbithowhp");
            string [] token = msg.Split('#');
            string[] text;
            foreach(string filter in token)
            {
                text = filter.Split('@');
                if(text.Length == 5)
                Console.WriteLine("[계좌번호] {0}   [이름] {1}  [잔액] {2}   [날짜] {3}",text[0],text[1],text[2],text[3]);
            }
        }
        #endregion

        #region 저장
   

        public bool AddAccount(string name, int balance)
        {
            try
            {
                //1)패킷 생성
                string packet = Packet.AddAccountPacket(name, balance);
    
                //2 전송
                SendData(packet);

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("[AddAccount에러] " + ex.Message);
                return false;
            }
         

        }

        public void AddAccountResult(string msg)
        {
            string[] token = msg.Split('#');

            if (token[0].Equals("True"))
           {
               Console.WriteLine(">> 회원 가입 요청 결과 : 성공");
               int.TryParse(token[1],out Id);
           }
           else 
           {
               Console.WriteLine(">> 회원 가입 요청 결과 : 실패");
           }
   
        }
        #endregion


        #region 검색
        public bool SelectAccount(int id)
        {
            try
            {
                //1)패킷 생성
                string packet = Packet.SelectAccountPacket(id);
                //2 전송
                SendData(packet);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[AddAccount에러] " + ex.Message);
                return false;
            }
        }

        public void SelectAccountResult(string msg)
        {

            string[] token = msg.Split('#');

            Console.WriteLine(">> 회원 검색 요청 결과");
            Console.WriteLine("[계좌번호] {0}   [이름] {1}  [잔액] {2}   [날짜] ({3} {4})", token[0], token[1], token[2], token[3],token[4]);

        }
        #endregion
        #region 수정
        public bool UpdateAccount(int id, IoType type, int balance)
        {
            //if (balance <= 0)
            //    return false;

            try
            {
                //1)패킷 생성
                string packet = Packet.UpdateAccountPacket(id, type, balance);
                //2 전송
                SendData(packet);
    
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[AddAccount에러] " + ex.Message);
                return false;
            }
        }


        public void UpdateAccountResult(string msg)
        {
            Console.WriteLine(msg);
            string[] token = msg.Split('#');
         
            if (token[0].Equals("True"))
            {
                Console.WriteLine(">> 회원 업데이트 요청 결과 : 성공");
            }
            else
            {
                Console.WriteLine(">> 회원 업데이트 요청 결과 : 실패");
            }
        }
        #endregion
        #region 삭제
        public bool DeletAccount(int id)
        {
            try
            {
                //1)패킷 생성
                string packet= Packet.DeleteAccountPacket(id);
                //2 전송
                SendData(packet);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[AddAccount에러] " + ex.Message);
                return false;
            }
        }

        public void DeletAccountResult(string msg)
        {
            Console.WriteLine(msg);
            string[] token = msg.Split('#'); 
            if (token[0].Equals("True"))
            {
                Console.WriteLine(">> 회원 삭제 요청 결과 : 성공");
            }
            else
            {
                Console.WriteLine(">> 회원 삭제 요청 결과 : 실패");
            }

        }
        #endregion
        #region 정렬(기본 정렬 -id,서브 정렬 -name)
        //Map 정렬가능?
        //가능 시 정렬
        //불가능하면 List 로 정렬
        public void SortId()
        {
       
         
        }

        public void SortName()
        {
        
      
        }

     

        #endregion

        #region 체팅
        public void SendChatMessage()   
        {
            chat = true;
            while (true)
            {
                string temp = Console.ReadLine();
                if (temp == "")
                {
                    Console.Clear();
                    chat = false;
                    return;
                }
          

                string str = Packet.SendChatMessage(Id, temp);

                SendData(str);        

            }
        }

        public void SendChatMessageResult(string msg)
        {
            if (chat == false)
                return;

            Console.WriteLine(msg);


        }

        #endregion

        private void Enter()
        {
            Console.WriteLine("아무키나 누르세요....\n");
            Console.ReadKey();
        }
    }
}
