using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ================================================
 1) ACcount Update 입력정보 계좌번호 입금 or 출금 금ㅇㄱ
 * 체크 계좌 번호 올바른가 잔액 충분한가
 * enum IoType {NON,INPUT,OUTPUT};
 * 2) accountdelete
 *  값 보내고 체크 한번
 * 3) accountsort
 * 소팅 타입
 * 
 * 2단계 : 이벤트 기반 로그 처리
 =================================================
 *3단계 : 파일 IO
 */


namespace _20181004C샵deleEvent실습
{


    class Program
   {

        ChatClass chat = new ChatClass();
        AccountInput accinput = new AccountInput();
        AccountSelect accselect = new AccountSelect();
        AccountSort accsort = new AccountSort();
        AccountUpdate accupdate = new AccountUpdate();
        AccountDelete accdelete = new AccountDelete();
      
        public  void Init()
        {
            //파일 Loading 뱅크 매니져에 딕션 가지고 와서 저장 한거 불러오기
            //BankManager.Bank_Singleton.LoadFile(log);
            BankManager.Bank_Singleton.Run("127.0.0.1", 7000);
        }
        public void PrintLogAll()
        {
           
        }

        private void Enter()
        {
            Console.WriteLine("아무키나 누르세요....\n");
            Console.ReadKey();
        }


        public void Run()
        {
            while (true)
            {
               // Console.Clear();
             //   BankManager.Bank_Singleton.PrintAllAccount();
                PrintMenu();

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1: accinput.MakeAccount(); break;
                    case ConsoleKey.D2: accupdate.Update(); break;
                    case ConsoleKey.D3: accdelete.Delete(); break;
                    case ConsoleKey.D4: accselect.SelectAccount(); break;
                    case ConsoleKey.D5: BankManager.Bank_Singleton.PrintAllAccount(); break;
                    case ConsoleKey.D6: chat.ChatRun(); break;
                    case ConsoleKey.D7: BankManager.Bank_Singleton.PrintLog(); break;
            
                    case ConsoleKey.Escape: Environment.Exit(0); return;
                    default:
                        Console.WriteLine("잘못된 메뉴 입력 ");
                        break;
                }

                Enter();
            }

        }

        private void PrintMenu()
        {
            Console.WriteLine("===========================================================");
            Console.WriteLine("[ 1] 계좌 생성");
            Console.WriteLine("[ 2] 계좌 수정");
            Console.WriteLine("[ 3] 계좌 삭제");
            Console.WriteLine("[ 4] 계좌 검색");
            Console.WriteLine("[ 5] 계좌 전체 리스트 출력");
            Console.WriteLine("[ 6] 체팅 기능");
            Console.WriteLine("ESC 종료");
          //  Console.WriteLine("[ 5] 계좌 정렬");
           //
            Console.WriteLine("===========================================================");
        }



        public void Exit()
        {
           // BankManager.Bank_Singleton.SaveFile(log);
          
        }
       

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Init();
            p.Run();
            p.Exit();
        }
    }
}
