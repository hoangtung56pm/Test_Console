using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test_Console.Entity
{
    public class Sport_Game_BetOnlineInfo
    {

        //ID of betonline
        private int _betID;
        public int BetID
        {
            get { return _betID; }
            set { _betID = value; }
        }
        //TeamID of team1
        private string _team1_ID;
        public string Team1_ID
        {
            get { return _team1_ID; }
            set { _team1_ID = value; }
        }
        //Team Name of team 1
        private string _teamName1;
        public string TeamName1
        {
            get { return _teamName1; }
            set { _teamName1 = value; }
        }
        //TeamID of team2
        private string _team2_ID;
        public string Team2_ID
        {
            get { return _team2_ID; }
            set { _team2_ID = value; }
        }
        //Team Name of team2
        private string _teamName2;
        public string TeamName2
        {
            get { return _teamName2; }
            set { _teamName2 = value; }
        }
        //GameID of Sport Game
        private string _game_ID;
        public string Game_ID
        {
            get { return _game_ID; }
            set { _game_ID = value; }
        }
        //Tong so ban thang team 1
        private int _team1_Score;
        public int Team1_Score
        {
            get { return _team1_Score; }
            set { _team1_Score = value; }
        }
        //Tong so ban thang team 2
        private int _team2_Score;
        public int Team2_Score
        {
            get { return _team2_Score; }
            set { _team2_Score = value; }
        }
        //Ti le thang cho team 1
        private double _tLCATeam1;
        public double TLCATeam1
        {
            get { return _tLCATeam1; }
            set { _tLCATeam1 = value; }
        }
        //ti le chau a 1 (x 0:1 1/4)
        private string _tLCA1;
        public string TLCA1
        {
            get { return _tLCA1; }
            set { _tLCA1 = value; }
        }
        //ti le chau a 2 (1 x:1 1/4)
        private string _tLCA2;
        public string TLCA2
        {
            get { return _tLCA2; }
            set { _tLCA2 = value; }
        }
        //ti le chau a 3(1 1/4:x 1/4)
        private string _tLCA3;
        public string TLCA3
        {
            get { return _tLCA3; }
            set { _tLCA3 = value; }
        }
        //ti le chau a 4(1 1/4:1 x)
        private string _tLCA4;
        public string TLCA4
        {
            get { return _tLCA4; }
            set { _tLCA4 = value; }
        }
        // ti le thang neu dat cuoc cho team 2
        private double _tLCATeam2;
        public double TLCATeam2
        {
            get { return _tLCATeam2; }
            set { _tLCATeam2 = value; }
        }
        //Ti le thang cho ta`i
        private double _tXOver;
        public double TXOver
        {
            get { return _tXOver; }
            set { _tXOver = value; }
        }
        //ti le tai xiu 1 (x 1/4)
        private string _tX1;
        public string TX1
        {
            get { return _tX1; }
            set { _tX1 = value; }
        }
        //ti le tai xiu 2 (1 x)
        private string _tX2;
        public string TX2
        {
            get { return _tX2; }
            set { _tX2 = value; }
        }
        //Ti le thang chi Xiu
        private double _tXUnder;
        public double TXUnder
        {
            get { return _tXUnder; }
            set { _tXUnder = value; }
        }
        //Loai bet: theo ti le chau a': 1, theo ti le ta`i xiu:2
        private int _bet_Type;
        public int Bet_Type
        {
            get { return _bet_Type; }
            set { _bet_Type = value; }
        }
        //Doi nguoi choi chon doi voi ti le chau a'
        private string _team_Select;
        public string Team_Select
        {
            get { return _team_Select; }
            set { _team_Select = value; }
        }
        //Cua tai hay xiu nguoi choi chon doi voi ti le tai xiu
        private int _tX_Select;
        public int TX_Select
        {
            get { return _tX_Select; }
            set { _tX_Select = value; }
        }
        //So tien nguoi choi dat cuoc
        private double _bet_Amount;
        public double Bet_Amount
        {
            get { return _bet_Amount; }
            set { _bet_Amount = value; }
        }
        //So tien nguoi choi co neu thang
        private double _win_Money;
        public double Win_Money
        {
            get { return _win_Money; }
            set { _win_Money = value; }
        }
        //So tien nguoi choi an khi thang dam
        private double _win_Money_Full;
        public double Win_Money_Full
        {
            get { return _win_Money_Full; }
            set { _win_Money_Full = value; }
        }
        //So tien nguoi choi co neu hoa
        private double _draw_Money;
        public double Draw_Money
        {
            get { return _draw_Money; }
            set { _draw_Money = value; }
        }
        //So tien nguoi choi mat neu thua
        private double _loss_Money;
        public double Loss_Money
        {
            get { return _loss_Money; }
            set { _loss_Money = value; }
        }
        //so tien nguoi choi mat neu thua dam
        private double _loss_Money_Full;
        public double Loss_Money_Full
        {
            get { return _loss_Money_Full; }
            set { _loss_Money_Full = value; }
        }
        //Trang thai giao dich (ket thuc hay chua ket thuc)
        private bool _bet_Status;
        public bool Bet_Status
        {
            get { return _bet_Status; }
            set { _bet_Status = value; }
        }
        //So tien mat va nhan duoc sau tran dau
        private double _bet_Money;
        public double Bet_Money
        {
            get { return _bet_Money; }
            set { _bet_Money = value; }
        }
        //Ket qua dat cuoc (thang, thua hay hoa`)
        private int _bet_Result;
        public int Bet_Result
        {
            get { return _bet_Result; }
            set { _bet_Result = value; }
        }
        //ID cua thanh vien tham gia dat cuoc
        private int _bet_MemberID;
        public int Bet_MemberID
        {
            get { return _bet_MemberID; }
            set { _bet_MemberID = value; }
        }
        //user name cua thanh vien tham gia dat cuoc
        private string _bet_MemberName;
        public string Bet_MemberName
        {
            get { return _bet_MemberName; }
            set { _bet_MemberName = value; }
        }
        //so tai khoan cua nguoi tham gia dat cuoc
        private int _bet_MemberAccount;
        public int Bet_MemberAccount
        {
            get { return _bet_MemberAccount; }
            set { _bet_MemberAccount = value; }
        }
        //Thoi gian tham gia dat cuoc
        private DateTime _bet_CreatedOn;
        public DateTime Bet_CreatedOn
        {
            get { return _bet_CreatedOn; }
            set { _bet_CreatedOn = value; }
        }
        //Thoi gian thay doi cuoc
        private DateTime _bet_ModifiedOn;
        public DateTime Bet_ModifiedOn
        {
            get { return _bet_ModifiedOn; }
            set { _bet_ModifiedOn = value; }
        }

        public double Euro_Rate { get; set; }
        public double Euro_Team1 { get; set; }
        public double Euro_Team2 { get; set; }

    }

    public class BetProcessInfo
    {
        private double _bet_Money;
        public double Bet_Money
        {
            get { return _bet_Money; }
            set { _bet_Money = value; }
        }
        private int _bet_Result;
        public int Bet_Result
        {
            get { return _bet_Result; }
            set { _bet_Result = value; }
        }

    }

    public class BetFinishInfo
    {
        private int _h_ID;
        public int H_ID
        {
            get { return _h_ID; }
            set { _h_ID = value; }
        }

        private string _team1_ID;
        public string Team1_ID
        {
            get { return _team1_ID; }
            set { _team1_ID = value; }
        }

        private string _teamName1;
        public string TeamName1
        {
            get { return _teamName1; }
            set { _teamName1 = value; }
        }

        private string _team2_ID;
        public string Team2_ID
        {
            get { return _team2_ID; }
            set { _team2_ID = value; }
        }

        private string _teamName2;
        public string TeamName2
        {
            get { return _teamName2; }
            set { _teamName2 = value; }
        }

        private string _game_ID;
        public string Game_ID
        {
            get { return _game_ID; }
            set { _game_ID = value; }
        }

        private int _team1_Score;
        public int Team1_Score
        {
            get { return _team1_Score; }
            set { _team1_Score = value; }
        }

        private int _team2_Score;
        public int Team2_Score
        {
            get { return _team2_Score; }
            set { _team2_Score = value; }
        }

        private double _tLCATeam1;
        public double TLCATeam1
        {
            get { return _tLCATeam1; }
            set { _tLCATeam1 = value; }
        }

        private string _tLCA1;
        public string TLCA1
        {
            get { return _tLCA1; }
            set { _tLCA1 = value; }
        }

        private string _tLCA2;
        public string TLCA2
        {
            get { return _tLCA2; }
            set { _tLCA2 = value; }
        }

        private string _tLCA3;
        public string TLCA3
        {
            get { return _tLCA3; }
            set { _tLCA3 = value; }
        }

        private string _tLCA4;
        public string TLCA4
        {
            get { return _tLCA4; }
            set { _tLCA4 = value; }
        }

        private double _tLCATeam2;
        public double TLCATeam2
        {
            get { return _tLCATeam2; }
            set { _tLCATeam2 = value; }
        }

        private double _tXOver;
        public double TXOver
        {
            get { return _tXOver; }
            set { _tXOver = value; }
        }

        private string _tX1;
        public string TX1
        {
            get { return _tX1; }
            set { _tX1 = value; }
        }

        private string _tX2;
        public string TX2
        {
            get { return _tX2; }
            set { _tX2 = value; }
        }

        private double _tXUnder;
        public double TXUnder
        {
            get { return _tXUnder; }
            set { _tXUnder = value; }
        }

        private int _bet_Type;
        public int Bet_Type
        {
            get { return _bet_Type; }
            set { _bet_Type = value; }
        }

        private string _team_Select;
        public string Team_Select
        {
            get { return _team_Select; }
            set { _team_Select = value; }
        }

        private int _tX_Select;
        public int TX_Select
        {
            get { return _tX_Select; }
            set { _tX_Select = value; }
        }

        private double _bet_Amount;
        public double Bet_Amount
        {
            get { return _bet_Amount; }
            set { _bet_Amount = value; }
        }

        private int _betID_M;
        public int BetID_M
        {
            get { return _betID_M; }
            set { _betID_M = value; }
        }

        private int _betID_T;
        public int BetID_T
        {
            get { return _betID_T; }
            set { _betID_T = value; }
        }

        private bool _bet_Status;
        public bool Bet_Status
        {
            get { return _bet_Status; }
            set { _bet_Status = value; }
        }

        private double _bet_Money;
        public double Bet_Money
        {
            get { return _bet_Money; }
            set { _bet_Money = value; }
        }

        private int _bet_Result;
        public int Bet_Result
        {
            get { return _bet_Result; }
            set { _bet_Result = value; }
        }

        private int _bet_MemberID;
        public int Bet_MemberID
        {
            get { return _bet_MemberID; }
            set { _bet_MemberID = value; }
        }

        private string _bet_MemberName;
        public string Bet_MemberName
        {
            get { return _bet_MemberName; }
            set { _bet_MemberName = value; }
        }

        private int _bet_MemberAccount;
        public int Bet_MemberAccount
        {
            get { return _bet_MemberAccount; }
            set { _bet_MemberAccount = value; }
        }

        private DateTime _bet_CreatedOn;
        public DateTime Bet_CreatedOn
        {
            get { return _bet_CreatedOn; }
            set { _bet_CreatedOn = value; }
        }

        private DateTime _bet_ModifiedOn;
        public DateTime Bet_ModifiedOn
        {
            get { return _bet_ModifiedOn; }
            set { _bet_ModifiedOn = value; }
        }

        private int _bet_Reference;
        public int Bet_Reference
        {
            get { return _bet_Reference; }
            set { _bet_Reference = value; }
        }

        public double Euro_Rate { get; set; }
        public double Euro_Team1 { get; set; }
        public double Euro_Team2 { get; set; }

    }

}
