using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Test_Console.Entity;
using Test_Console.Library;

namespace Test_Console.Content
{
    class BetFolk
    {
        public static readonly ILog _log = LogManager.GetLogger(typeof(BetFolk));
        static string connectString = ConfigurationManager.ConnectionStrings["ConnTTND"].ConnectionString;
        public static void process()
        {
            try
            {

                DataTable dt = GameBetOnlineNotProcess();
                int count = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    count = count + 1;
                    DataTable dtMatch = GameMatchPlayedGetInfo(ConvertUtility.ToInt32(dr["Game_Id"]));
                    _log.Debug("Game_Id :" + dr["Game_Id"] + "dtMatch :" + dtMatch.Rows.Count);
                    if (dtMatch.Rows[0]["Status"].ToString() == "Played")//CHI XU LY KHI TRAN DAU DA KET THUC
                    {
                        DataRow drMatch = dtMatch.Rows[0];
                        string team1Id = dr["Team1_ID"].ToString();
                        string team2Id = dr["Team2_ID"].ToString();
                        int team1Score = ConvertUtility.ToInt32(drMatch["Score_a"]);
                        int team2Score = ConvertUtility.ToInt32(drMatch["Score_b"]);
                        double tlcateam1 = ConvertUtility.ToDouble(dr["TLCATeam1"]);
                        double tlcateam2 = ConvertUtility.ToDouble(dr["TLCATeam2"]);
                        string tlca1 = dr["TLCA1"].ToString();
                        string tlca2 = dr["TLCA2"].ToString();
                        string tlca3 = dr["TLCA3"].ToString();
                        string tlca4 = dr["TLCA4"].ToString();
                        string teamselect = dr["Team_Select"].ToString();
                        double betamount = ConvertUtility.ToDouble(dr["Bet_Amount"]);
                        double tltai = ConvertUtility.ToDouble(dr["TXOver"]);
                        string tltaixiu1 = dr["TX1"].ToString();
                        string tltaixiu2 = dr["TX2"].ToString();
                        double tlxiu = ConvertUtility.ToDouble(dr["TXUnder"]);
                        int tlselect = ConvertUtility.ToInt32(dr["TX_Select"]);
                        int bettype = ConvertUtility.ToInt32(dr["Bet_Type"]);
                        double euroRate = ConvertUtility.ToDouble(dr["Euro_Rate"]);
                        double euroTeam1 = ConvertUtility.ToDouble(dr["Euro_Team1"]);
                        double euroTeam2 = ConvertUtility.ToDouble(dr["Euro_Team2"]);

                        //var betinfo = new Sport_Game_BetOnlineInfo();

                        BetProcessInfo processinfo;

                        //Lay ra thong tin ve cua bet nay
                        DataTable dtBetInfo = GameBetOnlineGetInfo(ConvertUtility.ToInt32(dr["BetID"]));
                        string Bet_MemberID, Match_ID;
                        Bet_MemberID = dtBetInfo.Rows[0]["Bet_MemberID"].ToString();
                        Match_ID = dtBetInfo.Rows[0]["Game_ID"].ToString();
                        if (ConvertUtility.ToInt32(dr["Bet_Type"]) == 1)
                        {
                            #region Xu Ly Theo Ty Le Chau A

                            processinfo = ReturnBetResultByTLCA(team1Id, team2Id, team1Score, team2Score, tlcateam1, tlca1, tlca2, tlca3, tlca4, tlcateam2, teamselect, betamount, bettype);

                            if (processinfo.Bet_Money > 0)//THANG KEO
                            {
                                //Neu nguoi choi thang thi cong tien vao tai khoan cua thanh vien

                                int betResult = processinfo.Bet_Result;

                                _log.Info("do_bet_process Log Chau-A WIN: " + "betId:" + dr["BetId"] + "|betMoney before : " + processinfo.Bet_Money + " |betAmount : " + ConvertUtility.ToDouble(dtBetInfo.Rows[0]["Bet_Amount"]));

                                DateTime betModifiedOn = DateTime.Now;
                                //BetOnlineController.GameBetOnlineUpdate(ConvertUtility.ToInt32(dr["BetID"]), team1Score, team2Score, processinfo.Bet_Money, betResult, true, betModifiedOn);

                                if (betResult == 2)
                                {
                                    //PostDataToApi(Bet_MemberID, processinfo.Bet_Money.ToString(), "hoa tl chau a match_id = " + Match_ID);
                                }
                                else
                                {
                                    //PostDataToApi(Bet_MemberID, processinfo.Bet_Money.ToString(), "thang tl chau a match_id = " + Match_ID);
                                }




                            }
                            else// THUA KEO
                            {
                                //Cap nhat lai bet nay thoi
                                double betMoney = (processinfo.Bet_Money - ConvertUtility.ToDouble(dtBetInfo.Rows[0]["Bet_Amount"]));
                                int betResult = processinfo.Bet_Result;

                                _log.Info("do_bet_process Log Chau-A LOSS: " + "betId:" + dr["BetId"] + "|betMoney before : " + processinfo.Bet_Money + " |betAmount : " + ConvertUtility.ToDouble(dtBetInfo.Rows[0]["Bet_Amount"]));

                                DateTime betModifiedOn = DateTime.Now;
                                //BetOnlineController.GameBetOnlineUpdate(ConvertUtility.ToInt32(dr["BetID"]), team1Score, team2Score, betMoney, betResult, true, betModifiedOn);
                                //PostDataToApi(Bet_MemberID, betMoney.ToString(), "thua tl chau a match_id = " + Match_ID);
                            }

                            #endregion
                        }
                        else if (ConvertUtility.ToInt32(dr["Bet_Type"]) == 2)
                        {
                            #region Xu Ly Theo TAI-XIU

                            processinfo = ReturnBetResultByTX(tltai, tltaixiu1, tltaixiu2, tlxiu, tlselect, betamount, (team1Score + team2Score), bettype);
                            if (processinfo.Bet_Money > 0)
                            {
                                ////Neu nguoi choi thang thi cong tien vao tai khoan cua thanh vien
                                //BetOnlineController.GameAdmoneyAccountAfterProcess(processinfo.Bet_Money, ConvertUtility.ToInt32(dtBetInfo.Rows[0]["bet_memberId"]));

                                int betResult = processinfo.Bet_Result;
                                DateTime betModifiedOn = DateTime.Now;
                                //BetOnlineController.GameBetOnlineUpdate(ConvertUtility.ToInt32(dr["BetID"]), team1Score, team2Score, processinfo.Bet_Money, betResult, true, betModifiedOn);
                                if (betResult == 2)
                                {

                                    //PostDataToApi(Bet_MemberID, processinfo.Bet_Money.ToString(), "hoa tai xiu match_id = " + Match_ID);
                                }
                                else
                                {

                                    //PostDataToApi(Bet_MemberID, processinfo.Bet_Money.ToString(), "thang tai xiu match_id = " + Match_ID);
                                }

                                _log.Info("do_bet_process Log Tai-Xiu WIN: " + "betId:" + dr["BetId"] + "|betMoney before : " + processinfo.Bet_Money + " |betAmount : " + ConvertUtility.ToDouble(dtBetInfo.Rows[0]["Bet_Amount"]));

                            }
                            else
                            {
                                //Cap nhat lai bet nay thoi
                                double betMoney = (processinfo.Bet_Money - ConvertUtility.ToDouble(dtBetInfo.Rows[0]["Bet_Amount"]));
                                int betResult = processinfo.Bet_Result;

                                _log.Info("do_bet_process Log Tai-Xiu LOST: " + "betId:" + dr["BetId"] + "|betMoney before : " + processinfo.Bet_Money + " |betAmount : " + ConvertUtility.ToDouble(dtBetInfo.Rows[0]["Bet_Amount"]));

                                DateTime betModifiedOn = DateTime.Now;
                                //BetOnlineController.GameBetOnlineUpdate(ConvertUtility.ToInt32(dr["BetID"]), team1Score, team2Score, betMoney, betResult, true, betModifiedOn);
                                //PostDataToApi(Bet_MemberID, betMoney.ToString(), "thua tai xiu match_id = " + Match_ID);
                            }

                            #endregion
                        }
                        else if (ConvertUtility.ToInt32(dr["Bet_Type"]) == 0)
                        {
                            #region Xu Ly Theo Ty Le Chau Au

                            processinfo = ReturnBetResultByTLCAu(team1Id, team2Id, euroRate, euroTeam1, euroTeam2, betamount, teamselect, team1Score, team2Score);
                            if (processinfo.Bet_Money > 0)//THANG KEO
                            {
                                ////Neu nguoi choi thang thi cong tien vao tai khoan cua thanh vien

                                int betResult = processinfo.Bet_Result;
                                DateTime betModifiedOn = DateTime.Now;
                                // BetOnlineController.GameBetOnlineUpdate(ConvertUtility.ToInt32(dr["BetID"]), team1Score, team2Score, processinfo.Bet_Money, betResult, true, betModifiedOn);
                                if (betResult == 2)
                                {
                                    //PostDataToApi(Bet_MemberID, processinfo.Bet_Money.ToString(), "hoa tl chau au match_id = " + Match_ID);
                                }
                                else
                                {
                                    //PostDataToApi(Bet_MemberID, processinfo.Bet_Money.ToString(), "thang tl chau au match_id = " + Match_ID);
                                }

                            }
                            else//THUA KEO
                            {
                                //double betMoney = (processinfo.Bet_Money - ConvertUtility.ToDouble(dtBetInfo.Rows[0]["Bet_Amount"]));
                                double betMoney = processinfo.Bet_Money;

                                int betResult = processinfo.Bet_Result;

                                //_log.Info("do_bet_process Log Chau-au LOST: " + "betId:" + dr["BetId"] + "|betMoney before : " + processinfo.Bet_Money + " |betAmount : " + ConvertUtility.ToDouble(dtBetInfo.Rows[0]["Bet_Amount"]));

                                DateTime betModifiedOn = DateTime.Now;
                                //BetOnlineController.GameBetOnlineUpdate(ConvertUtility.ToInt32(dr["BetID"]), team1Score, team2Score, betMoney, betResult, true, betModifiedOn);
                                //PostDataToApi(Bet_MemberID, betMoney.ToString(), "thua tl chau au match_id = " + Match_ID);
                            }

                            #endregion
                        }

                    }

                }

                //_log.Info("do_bet_process Total Processed: " + count );
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("do_bet_process Error: " + ex.Message);
            }

            //string tlca1;
            //string tlca2;
            //string tlca3;
            //string tlca4;
            //string tl = "0:1 1/4";
            //string[] tlsplit = tl.Split(':');
            //if (tlsplit[0].IndexOf(" ") <= 0)
            //{
            //    tlca2 = tlsplit[0].Replace(" ", "");
            //    tlca1 = "0";
            //}
            //else
            //{
            //    string[] tlleft = tlsplit[0].Split(' ');
            //    tlca1 = tlleft[0];
            //    tlca2 = tlleft[1];
            //}
            //if (tlsplit[1].IndexOf(" ") <= 0)
            //{
            //    tlca4 = tlsplit[1].Replace(" ", "");
            //    tlca3 = "0";
            //}
            //else
            //{
            //    string[] tlright = tlsplit[1].Split(' ');
            //    tlca3 = tlright[0];
            //    tlca4 = tlright[1];
            //}
        }
        #region BET RESULT PROCESS

        //Process bet online here
        //1.process by asia rate
        public static BetProcessInfo ReturnBetResultByTLCA(string team1id, string team2id, int team1score, int team2score, double tlcateam1, string tlca1, string tlca2, string tlca3, string tlca4, double tlcateam2, string teamselected, double betamount, int bettype)
        {
            BetProcessInfo retval = new BetProcessInfo();
            //
            double tl1;
            double tl2;
            double tl3;
            double tl4;
            string[] temp;
            if (tlca1.IndexOf("/") > 0)
            {
                temp = tlca1.Split('/');
                tl1 = ConvertUtility.ToDouble(temp[0].Replace(" ", "")) / ConvertUtility.ToDouble(temp[1].Replace(" ", ""));
            }
            else
            {
                tl1 = ConvertUtility.ToDouble(tlca1);
            }
            if (tlca2.IndexOf("/") > 0)
            {
                temp = tlca2.Split('/');
                tl2 = ConvertUtility.ToDouble(temp[0].Replace(" ", "")) / ConvertUtility.ToDouble(temp[1].Replace(" ", ""));
            }
            else
            {
                tl2 = ConvertUtility.ToDouble(tlca2);
            }
            if (tlca3.IndexOf("/") > 0)
            {
                temp = tlca3.Split('/');
                tl3 = ConvertUtility.ToDouble(temp[0].Replace(" ", "")) / ConvertUtility.ToDouble(temp[1].Replace(" ", ""));
            }
            else
            {
                tl3 = ConvertUtility.ToDouble(tlca3);
            }
            if (tlca4.IndexOf("/") > 0)
            {
                temp = tlca4.Split('/');
                tl4 = ConvertUtility.ToDouble(temp[0].Replace(" ", "")) / ConvertUtility.ToDouble(temp[1].Replace(" ", ""));
            }
            else
            {
                tl4 = ConvertUtility.ToDouble(tlca4);
            }
            //Kiem tra xem co phai cuoc theo ti le chau a
            if (bettype == 1)
            {
                //truong hop ti le co dinh dang (x:y)
                if (tl1 == 0 && tl3 == 0)
                {       
                    //truong hop ti le la 0:0 (thang an,hoa tra lai tien, thua mat dut )
                    if (tl2 == 0 && tl4 == 0)
                    {
                        #region tl2 == 0 && tl4 == 0
                        //thang thi nhan chia theo ti le ma doi minh chon;
                        //neu doi chon la` doi 1 nhan theo ti le duoc cua doi 1;
                        if (teamselected == team1id)
                        {
                            //Kiem tra ti so 
                            //Neu doi dat cuoc thang thi` an du theo ti le
                            if (team1score > team2score)
                            {
                                //kiem tra ti le
                                if (tlcateam1 > 0)
                                {
                                    retval.Bet_Money = betamount + betamount * tlcateam1;
                                }
                                else
                                {
                                    retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                }
                                //cap nhat trang thai cua bet la` thang
                                retval.Bet_Result = 1;
                            }
                            else
                            {
                                //Neu hoa` thi` nhan lai du tien
                                if (team1score == team2score)
                                {
                                    retval.Bet_Money = betamount;
                                    //Cap nhat trang tha'i la` hoa`
                                    retval.Bet_Result = 2;
                                }//Thua thi` mat dut
                                else
                                {
                                    //Cap nhat trang tha'i la` thua
                                    retval.Bet_Money = 0;
                                    retval.Bet_Result = 3;
                                }
                            }

                        }
                        else
                        {
                            //Neu doi chon cuoc la` doi 2 thi nhan theo ti le thang cua doi 2
                            if (teamselected == team2id)
                            {
                                //Kiem tra ti so
                                //Neu team 2 thang thi an du theo dung ti le
                                if (team2score > team1score)
                                {
                                    //Kiem tra ti le
                                    if (tlcateam2 > 0)
                                    {
                                        retval.Bet_Money = betamount + betamount * tlcateam2;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                    }
                                    //Cap nhat trang thai cua bet la` tha'ng
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //Hoa nha nlai du tien
                                    if (team2score == team1score)
                                    {
                                        retval.Bet_Money = betamount;
                                        //Cap nhat trang thai bet la` hoa`
                                        retval.Bet_Result = 2;
                                    }
                                    else//thua mat dut
                                    {
                                        retval.Bet_Money = 0;
                                        retval.Bet_Result = 3;
                                    }

                                }

                            }
                        }
                        #endregion
                    }
                    else
                    {
                        //truong hop ti le co dang (0:1/4)
                        if (tl2 == 0 && tl4 == (double)1 / 4)
                        {
                            #region tl2 == 0 && tl4 == (double)1 / 4
                            //Neu cuoc cho doi 1 
                            if (teamselected == team1id)
                            {
                                //Kiem tra ti so 
                                //Neu doi dat cuoc thang thi` an dung theo ti le
                                if (team1score > team2score)
                                {
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //Cap nhat trang thai bet la` thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //Neu ti so la` hoa thi` lay lai nua tien
                                    if (team1score == team2score)
                                    {
                                        retval.Bet_Money = betamount / 2;
                                        //cap nhat trang thai bet la` thua
                                        retval.Bet_Result = 3;
                                    }//Neu ti so team 1 thua team 2 thi thua dut
                                    else
                                    {
                                        retval.Bet_Money = 0;
                                        //Cap nhat trang thai bet la` thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                            else
                            {
                                //neu cuoc cho doi 2
                                if (teamselected == team2id)
                                {
                                    //Kiem tra ti so 
                                    if (team2score > team1score)
                                    {
                                        //neu thang thi an tien theo dung ti le cuoc
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;

                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang thai bet la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //Neu ti so hoa` thi an nua + von
                                        if (team2score == team1score)
                                        {
                                            //retval.Bet_Money = betamount + betamount / 2;

                                            //UPDATE 14/12/2015
                                            retval.Bet_Money = betamount + (betamount * tlcateam2 / 2);

                                            //cap nhat trang thai cua bet la` thang
                                            retval.Bet_Result = 1;
                                        }
                                        else //Neu thua thi mat dut
                                        {
                                            retval.Bet_Money = 0;
                                            //Cap nhat trang thai cua bet la` thua
                                            retval.Bet_Result = 3;
                                        }
                                    }

                                }
                            }
                            #endregion
                        }
                        //truong hop ti le co dang (0:1/2)
                        if (tl2 == 0 && tl4 == (double)1 / 2)
                        {
                            #region tl2 == 0 && tl4 == (double)1 / 2
                            //Neu cuoc cho doi 1
                            if (teamselected == team1id)
                            {
                                //Kiem tra ti so 
                                if (team1score > team2score)
                                {
                                    //Neu thang thi an du cho theo dung ti le
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //Cap nhat trang thai cua bet la` thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    if (team1score <= team2score)
                                    {
                                        retval.Bet_Money = 0;
                                        //Cap nhat trang thai la thua;
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                            else
                            {
                                //Neu nguoi choi cuoc doi 2
                                if (teamselected == team2id)
                                {
                                    //Kiem tra ti so
                                    if (team2score >= team1score)
                                    {
                                        //Thang va hoa thi` an du
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang tha'i la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //Neu thua thi mat dut
                                        retval.Bet_Money = 0;
                                        //Cap nhat trang thai bet la` thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                            #endregion
                        }
                        //truong hop ti le co dang (0:3/4)
                        if (tl2 == 0 && tl4 == (double)3 / 4)
                        {
                            #region tl2 == 0 && tl4 == (double)3 / 4
                            //Neu dat cuoc cho doi 1
                            if (teamselected == team1id)
                            {
                                if (team1score > team2score)
                                {
                                    //Neu thang sat nu't thi an nua
                                    if ((team1score - team2score) == 1)
                                    {
                                        //retval.Bet_Money = betamount + betamount / 2;
                                        //UPDATE 14/12/2015
                                        retval.Bet_Money = betamount + (betamount * tlcateam1 / 2);

                                        //Cap nhat tinh trang bet la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //neu thang >1  thi` an theo ti le bt
                                        if (tlcateam1 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam1 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                        }
                                        //Cap nhat tinh trang cua bet la` thang
                                        retval.Bet_Result = 1;
                                    }
                                }
                                else
                                {
                                    //Hoa` va thua thi` mat dut
                                    retval.Bet_Money = 0;
                                    //Cap nhat trang tha'i bet la` thua
                                    retval.Bet_Result = 3;
                                }
                            }
                            else
                            {
                                if (teamselected == team2id)
                                {
                                    //Kiem tra ti so
                                    if (team2score >= team1score)
                                    {
                                        //Neu hoa va thang la an du theo ti le
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang thai bet la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //Thua 1 ban thi mat nua tien (nhan lai 1 nua tien)
                                        if ((team1score - team2score) == 1)
                                        {
                                            retval.Bet_Money = betamount / 2;

                                        }
                                        else
                                        {
                                            retval.Bet_Money = 0;
                                        }
                                        //cap nhat trang tha'i la` thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                            #endregion
                        }

                        //truong hop ti le co dang (1/4:0)
                        if (tl2 == (double)1 / 4 && tl4 == 0)
                        {
                            #region tl2 == (double)1 / 4 && tl4 == 0
                            //Neu nguoi choi dat cuoc cho doi 1
                            if (teamselected == team1id)
                            {
                                //Kiem tra ti so
                                if (team1score > team2score)
                                {
                                    //Neu thang an du
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //Cap nhat trang tha'i thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    if (team1score == team2score)
                                    {
                                        //neu hoa thi duoc nua tien + von
                                        //retval.Bet_Money = betamount + betamount / 2;

                                        //UPDATE 14/12/2015
                                        retval.Bet_Money = betamount + (betamount * tlcateam1 / 2);


                                        //Cap nhat trang tha'i la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //neu thua thi mat dut
                                        retval.Bet_Money = 0;
                                        //Cap nhat trang tha'i la` thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                            else
                            {
                                //Neu nguoi choi dat cuoc cho doi 2
                                if (teamselected == team2id)
                                {
                                    //Kiem tra ti so
                                    if (team2score > team1score)
                                    {
                                        //Neu thang an du
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang tha'i bet la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        if (team2score == team1score)
                                        {
                                            //neu hoa nhan lai nua tien
                                            retval.Bet_Money = betamount / 2;
                                            //Cap nhat trang thai bet la` thua
                                            retval.Bet_Result = 3;
                                        }
                                        else
                                        {
                                            //neu thua thi` mat dut
                                            retval.Bet_Money = 0;
                                            //Cap nhat trang thai la` thua
                                            retval.Bet_Result = 3;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        //truong hop ti le co dang (1/2:0)
                        if (tl2 == (double)1 / 2 && tl4 == 0)
                        {
                            #region tl2 == (double)1 / 2 && tl4 == 0
                            //Neu nguoi choi dat cuoc cho doi 1
                            if (teamselected == team1id)
                            {
                                //Kiem tra ti so
                                if (team1score >= team2score)
                                {
                                    //Neu thang va hoa van an du
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //Cap nhat trang tha'i cua bet la` thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //Neu thua thi mat dut
                                    retval.Bet_Money = 0;
                                    //Cap nhat trang tha'i cua bet la` thua
                                    retval.Bet_Result = 3;
                                }
                            }
                            else
                            {
                                //Neu nguoi choi dat cuoc cho doi 2
                                if (teamselected == team2id)
                                {
                                    //Kiem tra ti so
                                    if (team2score > team1score)
                                    {
                                        //Neu thang van an du
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + betamount * tlcateam2;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang tha'i la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //Neu hoa va thua mat dut
                                        retval.Bet_Money = 0;
                                        //cap nhat trang tha'i bet la` thua
                                        retval.Bet_Result = 3;
                                    }
                                }

                            }
                            #endregion
                        }
                        //truong hop ti le co dang (3/4:0)
                        if (tl2 == (double)3 / 4 && tl4 == 0)
                        {
                            #region tl2 == (double)3 / 4 && tl4 == 0
                            //Neu nguoi choi dat cuoc cho doi 1
                            if (teamselected == team1id)
                            {
                                //Kiem tra ti so
                                if (team1score >= team2score)
                                {
                                    //Neu hoa hay thang an du tien theo ti le
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + betamount * tlcateam1;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //Cap nhat trang tha'i bet la` thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    if ((team2score - team1score) == 1)
                                    {
                                        //thua 1 ban mat nua
                                        retval.Bet_Money = betamount / 2;
                                    }
                                    else
                                    {
                                        //thua >1 ban mat dut
                                        retval.Bet_Money = 0;
                                    }
                                    //Cap nhat trang thai bet la` thua
                                    retval.Bet_Result = 3;
                                }
                            }
                            else
                            {
                                //Neu nguoi choi dat cuoc cho doi 2
                                if (teamselected == team2id)
                                {
                                    //Kiem tra ti so
                                    if (team2score > team1score)
                                    {
                                        if ((team2score - team1score) == 1)
                                        {
                                            //Neu thang 1 ban thi an nua tien;
                                            //retval.Bet_Money = betamount + betamount / 2;

                                            //UPDATE 14/12/2015
                                            retval.Bet_Money = betamount + (betamount * tlcateam2 / 2);
                                        }
                                        else
                                        {
                                            //Neu thang >1 ban thi an du theo ti le
                                            if (tlcateam2 > 0)
                                            {
                                                retval.Bet_Money = betamount + betamount * tlcateam2;
                                            }
                                            else
                                            {
                                                retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                            }
                                        }
                                        //Cap nhat trang thai bet la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //Neu hoa va thua thi mat dut tien
                                        retval.Bet_Money = 0;
                                        //Cap nhat trang thai bet la` thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                            #endregion
                        }
                        //truong hop ti le co dang (n:0)
                        if (tl2 >= 1 && tl4 == 0)
                        {
                            #region tl2 == 1 && tl4 == 0
                            //Neu nguoi choi dat cuoc cho doi 1
                            if (teamselected == team1id)
                            {
                                //Kiem tra ti so
                                if (team1score >= team2score)
                                {
                                    //Neu hoa hay thang nguoi choi an du theo ti le
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //Cap nhat trang thai bet la` thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    if ((team2score - team1score) == tl2)
                                    {
                                        //Neu thua 1 ban thi nhan lai tien
                                        retval.Bet_Money = betamount;
                                        //Cap nhat trang thai bet la` hoa`
                                        retval.Bet_Result = 2;
                                    }
                                    else
                                    {
                                        //neu thua >1 ban thi mat dut
                                        retval.Bet_Money = 0;
                                        //Cap nhat trang thai bet la` thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                            else
                            {
                                if (teamselected == team2id)
                                {
                                    //Kiem tra ti so
                                    if (team2score > team1score)
                                    {
                                        if ((team2score - team1score) == tl2)
                                        {
                                            //Neu thang 1 ban thi nhan lai tien
                                            retval.Bet_Money = betamount;
                                            //Cap nhat trang thai bet la` hoa`
                                            retval.Bet_Result = 2;
                                        }
                                        else
                                        {
                                            //Neu thang >1 ban thi an du tien theo ti le
                                            if (tlcateam2 > 0)
                                            {
                                                retval.Bet_Money = betamount + tlcateam2 * betamount;
                                            }
                                            else
                                            {
                                                retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                            }
                                            //Cap nhat trang thai bet la` thang 
                                            retval.Bet_Result = 1;
                                        }
                                    }
                                    else
                                    {
                                        //Neu hoa hay thua thi mat dut
                                        retval.Bet_Money = 0;
                                        //Cap nhat lai trang thai be't la` thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                            #endregion
                        }

                        //Neu ti le co dang (0:n)
                        if (tl2 == 0 && tl4 >= 1)
                        {
                            #region tl2 == 0 && tl4 == 1
                            //Neu nguoi choi dat cuoc doi 1
                            if (teamselected == team1id)
                            {
                                //Kiem tra ti so
                                if (team1score > team2score)
                                {
                                    //Neu thang cach biet 1 ban thi nhan lai du tien
                                    if ((team1score - team2score) == tl4)
                                    {
                                        retval.Bet_Money = betamount;
                                        //Cap nhat trang tha'i la` hoa
                                        retval.Bet_Result = 2;
                                    }
                                    else
                                    { //Neu thang cach biet > 1 ban thi an du theo ti le
                                        //neu thang >= 2 qua thi an theo ti le dat ra
                                        if (tlcateam1 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam1 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                        }
                                        //Cap nhat trang thai cua bet la` thang 
                                        retval.Bet_Result = 1;
                                    }
                                }//Hoa` va thua thi` mat dut
                                else
                                {
                                    //Neu thua va hoa thi` mat dut tien
                                    retval.Bet_Money = 0;
                                    retval.Bet_Result = 3;
                                }
                            }
                            else
                            {
                                //Neu nguoi choi dat cuoc doi 2
                                if (teamselected == team2id)
                                {
                                    //Kiem tra ti so
                                    //Neu hoa va thang thi an du theo ti le
                                    if (team2score >= team1score)
                                    {
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                    }
                                    else
                                    {
                                        //Neu thu 1 ba`n thi hoa tien
                                        if ((team1score - team2score) == tl4)
                                        {
                                            //Neu thua 1 ban thi an = tien;
                                            retval.Bet_Money = betamount;
                                            //Cap nhat trang thai bet la` hoa
                                            retval.Bet_Result = 2;
                                        }
                                        else
                                        {
                                            //Neu thua >1 ban thi mat dut tien
                                            retval.Bet_Money = 0;
                                            retval.Bet_Result = 3;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        ////truong hop ti le co dang (1:0)
                        //if (tl2 == 1 && tl4 == 0)
                        //{
                        //    #region tl2 == 1 && tl4 == 0
                        //    //Neu nguoi choi dat cuoc cho doi 1
                        //    if (teamselected == team1id)
                        //    {
                        //        //Kiem tra ti so
                        //        if (team1score >= team2score)
                        //        {
                        //            //Neu hoa hay thang nguoi choi an du theo ti le
                        //            if (tlcateam1 > 0)
                        //            {
                        //                retval.Bet_Money = betamount + tlcateam1 * betamount;
                        //            }
                        //            else
                        //            {
                        //                retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                        //            }
                        //            //Cap nhat trang thai bet la` thang
                        //            retval.Bet_Result = 1;
                        //        }
                        //        else
                        //        {
                        //            if ((team2score - team1score) == 1)
                        //            {
                        //                //Neu thua 1 ban thi nhan lai tien
                        //                retval.Bet_Money = betamount;
                        //                //Cap nhat trang thai bet la` hoa`
                        //                retval.Bet_Result = 2;
                        //            }
                        //            else
                        //            {
                        //                //neu thua >1 ban thi mat dut
                        //                retval.Bet_Money = 0;
                        //                //Cap nhat trang thai bet la` thua
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (teamselected == team2id)
                        //        {
                        //            //Kiem tra ti so
                        //            if (team2score > team1score)
                        //            {
                        //                if ((team2score - team1score) == 1)
                        //                {
                        //                    //Neu thang 1 ban thi nhan lai tien
                        //                    retval.Bet_Money = betamount;
                        //                    //Cap nhat trang thai bet la` hoa`
                        //                    retval.Bet_Result = 2;
                        //                }
                        //                else
                        //                {
                        //                    //Neu thang >1 ban thi an du tien theo ti le
                        //                    if (tlcateam2 > 0)
                        //                    {
                        //                        retval.Bet_Money = betamount + tlcateam2 * betamount;
                        //                    }
                        //                    else
                        //                    {
                        //                        retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                        //                    }
                        //                    //Cap nhat trang thai bet la` thang 
                        //                    retval.Bet_Result = 1;
                        //                }
                        //            }
                        //            else
                        //            {
                        //                //Neu hoa hay thua thi mat dut
                        //                retval.Bet_Money = 0;
                        //                //Cap nhat lai trang thai be't la` thua
                        //                retval.Bet_Result = 3;
                        //            }
                        //        }
                        //    }
                        //    #endregion
                        //}

                        ////Neu ti le co dang (0:1)
                        //if (tl2 == 0 && tl4 == 1)
                        //{
                        //    #region tl2 == 0 && tl4 == 1
                        //    //Neu nguoi choi dat cuoc doi 1
                        //    if (teamselected == team1id)
                        //    {
                        //        //Kiem tra ti so
                        //        if (team1score > team2score)
                        //        {
                        //            //Neu thang cach biet 1 ban thi nhan lai du tien
                        //            if ((team1score - team2score) == 1)
                        //            {
                        //                retval.Bet_Money = betamount;
                        //                //Cap nhat trang tha'i la` hoa
                        //                retval.Bet_Result = 2;
                        //            }
                        //            else
                        //            { //Neu thang cach biet > 1 ban thi an du theo ti le
                        //                //neu thang >= 2 qua thi an theo ti le dat ra
                        //                if (tlcateam1 > 0)
                        //                {
                        //                    retval.Bet_Money = betamount + tlcateam1 * betamount;
                        //                }
                        //                else
                        //                {
                        //                    retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                        //                }
                        //                //Cap nhat trang thai cua bet la` thang 
                        //                retval.Bet_Result = 1;
                        //            }
                        //        }//Hoa` va thua thi` mat dut
                        //        else
                        //        {
                        //            //Neu thua va hoa thi` mat dut tien
                        //            retval.Bet_Money = 0;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        //Neu nguoi choi dat cuoc doi 2
                        //        if (teamselected == team2id)
                        //        {
                        //            //Kiem tra ti so
                        //            //Neu hoa va thang thi an du theo ti le
                        //            if (team2score >= team1score)
                        //            {
                        //                if (tlcateam2 > 0)
                        //                {
                        //                    retval.Bet_Money = betamount + tlcateam2 * betamount;
                        //                }
                        //                else
                        //                {
                        //                    retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                        //                }
                        //            }
                        //            else
                        //            {
                        //                //Neu thu 1 ba`n thi hoa tien
                        //                if ((team1score - team2score) == 1)
                        //                {
                        //                    //Neu thua 1 ban thi an = tien;
                        //                    retval.Bet_Money = betamount;
                        //                    //Cap nhat trang thai bet la` hoa
                        //                    retval.Bet_Result = 2;
                        //                }
                        //                else
                        //                {
                        //                    //Neu thua >1 ban thi mat dut tien
                        //                    retval.Bet_Money = 0;
                        //                    retval.Bet_Result = 3;
                        //                }
                        //            }
                        //        }
                        //    }
                        //    #endregion
                        //}
                    }
                }//truong hop ti le 1 va ti le 3 khac 0 (vd: 1 1/4:2 1/4)
                else
                {
                    //truong hop ti le co dang (n 1/4:0)
                    if (tl1 > 0 && tl2 == (double)1 / 4 && tl3 == 0 && tl4 == 0)
                    {
                        #region tl1 > 0 && tl2 == (double)1 / 4 && tl3 == 0 && tl4 == 0
                        //Neu nguoi choi dat cuoc choi doi 1
                        if (teamselected == team1id)
                        {
                            //Kiem tra ti so
                            if (team1score >= team2score)
                            {
                                //Neu hoa va thang thi an du theo ti le
                                if (tlcateam1 > 0)
                                {
                                    retval.Bet_Money = betamount + tlcateam1 * betamount;
                                }
                                else
                                {
                                    retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                }
                                //cap nhat trang thai bet la` thang
                                retval.Bet_Result = 1;
                            }
                            else
                            {
                                if ((team2score - team1score) < ConvertUtility.ToInt32(tl1))
                                {
                                    //Neu thua < n ban thi an du theo ti le
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                    }
                                    //cap nhat trang thai bet la` thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    if ((team2score - team1score) == ConvertUtility.ToInt32(tl1))
                                    {
                                        //Neu thua n ban thi an nua tien + von
                                        retval.Bet_Money = betamount + betamount / 2;
                                        //cap nhat trang thai bet la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //neu thua >n ban thi mat dut
                                        retval.Bet_Money = 0;
                                        //Cap nhat trang thai la` thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Neu nguoi choi dat cuoc cho doi 2
                            if (teamselected == team2id)
                            {
                                //Kiem tra ti so
                                if (team2score > team1score)
                                {
                                    if ((team2score - team1score) > ConvertUtility.ToInt32(tl1))
                                    {
                                        //Neu thang tren >n ban thi an du theo ti le
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang thai bet la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        if ((team2score - team1score) == ConvertUtility.ToInt32(tl1))
                                        {
                                            //Neu thang n ban thi nhan lai nua tien
                                            retval.Bet_Money = betamount / 2;
                                            //Cap nhat trang thai bet la` thua
                                            retval.Bet_Result = 3;
                                        }
                                        else
                                        {
                                            //neu thang < n ban thi mat dut
                                            retval.Bet_Money = 0;
                                            //cap nhat trang thai bet la` thua
                                            retval.Bet_Result = 3;
                                        }
                                    }
                                }
                                else
                                {
                                    //Neu hoa, thang < n ban, hoac thua thi mat dut
                                    retval.Bet_Money = 0;
                                    //Cap nhat trang thai bet la` thua

                                    //retval.Bet_Result = 0;(Không hiểu sao hệ thống cũ lại = 0)
                                    retval.Bet_Result = 3;
                                }
                            }
                        }
                        #endregion
                    }
                    //truong hop ti le co dang (n 1/2:0)
                    if (tl1 > 0 && tl2 == (double)1 / 2 && tl3 == 0 && tl4 == 0)
                    {
                        #region tl1 > 0 && tl2 == (double)1 / 2 && tl3 == 0 && tl4 == 0
                        //Neu nguoi choi dat cuoc cho doi 1
                        if (teamselected == team1id)
                        {
                            //Kiem tra ti so
                            if (team1score >= team2score)
                            {
                                //Neu thang hoac hoa an du theo ti le
                                if (tlcateam1 > 0)
                                {
                                    retval.Bet_Money = betamount + tlcateam1 * betamount;
                                }
                                else
                                {
                                    retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                }
                                //Cap nhat trang thai bet la thang
                                retval.Bet_Result = 1;
                            }
                            else
                            {
                                if ((team2score - team1score) <= ConvertUtility.ToInt32(tl1))
                                {
                                    //Neu thua < n ban thi van an du theo ti le
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //Cap nhat trang thai bet la thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //Neu thua > n ban thi mat dut
                                    retval.Bet_Money = 0;
                                    //cap nhat trang thai bet la` thua
                                    retval.Bet_Result = 3;
                                }
                            }
                        }
                        else
                        {
                            //Neu nguoi choi dat cuoc cho doi 2
                            if (teamselected == team2id)
                            {
                                //Kiem tra ti so
                                if (team2score > team1score)
                                {
                                    //Neu thang tren n ban thi an du theo ti le
                                    if ((team2score - team1score) > ConvertUtility.ToInt32(tl1))
                                    {
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang thai cua bet la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //Neu thang <=n ban thua dut
                                        retval.Bet_Money = 0;
                                        //cap nhat trang thai bet la` thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                                else
                                {
                                    //Hoa vao thua thi` dut 
                                    retval.Bet_Money = 0;
                                    retval.Bet_Result = 3;
                                }
                            }
                        }
                        #endregion
                    }
                    //truong hop ti le co dang (n 3/4:0)
                    if (tl1 > 0 && tl2 == (double)3 / 4 && tl3 == 0 && tl4 == 0)
                    {
                        #region tl1 > 0 && tl2 == (double)3 / 4 && tl3 == 0 && tl4 == 0
                        //Neu nguoi choi dat cuoc cho doi 1
                        if (teamselected == team1id)
                        {
                            //Kiem tra ti so
                            if (team1score >= team2score)
                            {
                                //Neu hoa hoac thang thi an du theo ti le 
                                if (tlcateam1 > 0)
                                {
                                    retval.Bet_Money = betamount + tlcateam1 * betamount;
                                }
                                else
                                {
                                    retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                }
                                //Cap nhat trang thai cho bet la` thang
                                retval.Bet_Result = 1;
                            }
                            else
                            {
                                // Neu thua <= n ban thang thi van an du theo ti le
                                if ((team2score - team1score) <= ConvertUtility.ToInt32(tl1))
                                {
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //Cap nhat trang thai cho bet la` thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //neu thua n+1 ban thi nhan lai nua tien
                                    if ((team2score - team1score) == ConvertUtility.ToInt32(tl1) + 1)
                                    {
                                        retval.Bet_Money = betamount / 2;
                                    }
                                    else
                                    {
                                        //Neu thua >n+1 ban thi mat dut
                                        retval.Bet_Money = 0;
                                    }
                                    //Cap nhat trang tha'i bet la thua
                                    retval.Bet_Result = 3;
                                }
                            }
                        }
                        else
                        {
                            //Neu nguoi choi dat cuoc cho doi 2
                            if (teamselected == team2id)
                            {
                                if (team2score > team1score)
                                {
                                    if ((team2score - team1score) > ConvertUtility.ToInt32(tl1) + 1)
                                    {
                                        //Neu nguoi choi thang > n ban thi an du theo ti le
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang thai bet la thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //Neu thang = n+1 ban thi an nua
                                        if ((team2score - team1score) == ConvertUtility.ToInt32(tl1) + 1)
                                        {
                                            //retval.Bet_Money = betamount + betamount / 2;

                                            //UPDATE 14/12/2015
                                            retval.Bet_Money = betamount + (betamount * tlcateam2 / 2);

                                            //Cap nhat trang thai bet la` thang
                                            retval.Bet_Result = 1;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = 0;
                                            //cap nhat trang thai bet la` thua
                                            retval.Bet_Result = 3;
                                        }
                                    }

                                }
                                else
                                {
                                    //Neu nguoi choi thang <= n ban, hoa hoac thua thi mat dut
                                    retval.Bet_Money = 0;
                                    //Cap nhat trang thai cho bet la` thua
                                    retval.Bet_Result = 3;
                                }
                            }
                        }
                        #endregion
                    }
                    //truong hop ti le co dang (n:0)
                    if (tl1 > 0 && tl2 == 0 && tl3 == 0 && tl4 == 0)
                    {
                        #region tl1 > 0 && tl2 == 0 && tl3 == 0 && tl4 == 0
                        //Neu nguoi choi dat cuoc cho doi 1
                        if (teamselected == team1id)
                        {
                            //Kiem tra ti so
                            if (team1score >= team2score)
                            {
                                //Neu nguoi choi hoa
                                if (tlcateam1 > 0)
                                {
                                    retval.Bet_Money = betamount + tlcateam1 * betamount;
                                }
                                else
                                {
                                    retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                }
                                //Cap nhat trang thai bet la` thang
                                retval.Bet_Result = 1;
                            }
                            else
                            {
                                //Neu thua <n ban an dung theo ti le
                                if ((team2score - team1score) < ConvertUtility.ToInt32(tl1))
                                {
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //Cap nhat trang thai bet la` thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //Neu thua n ban thi hoan lai tien
                                    if ((team2score - team1score) == ConvertUtility.ToInt32(tl1))
                                    {
                                        retval.Bet_Money = betamount;
                                        //cap nhat trang thai bet la hoa`
                                        retval.Bet_Result = 2;
                                    }
                                    else
                                    {
                                        //Neu nguoi choi thua > n ban thi mat dut
                                        retval.Bet_Money = 0;
                                        //Cap nhat trang thai thua cho bet na`y
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }

                        }
                        else
                        {
                            //Neu nguoi choi chon team2
                            if (teamselected == team2id)
                            {
                                //Kiem tra ti so
                                if (team2score > team1score)
                                {
                                    //Neu thang > n ban thi an dung theo ti le
                                    if ((team2score - team1score) > ConvertUtility.ToInt32(tl1))
                                    {
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang thai bet la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //neu thang dung n ban thi nhan lai tien
                                        if ((team2score - team1score) == ConvertUtility.ToInt32(tl1))
                                        {
                                            retval.Bet_Money = betamount;
                                            //Cap nhat trang thai bet la hoa
                                            retval.Bet_Result = 2;
                                        }
                                        else
                                        {
                                            //Neu thang <n ban thi thua dut
                                            retval.Bet_Money = 0;
                                            retval.Bet_Result = 3;
                                        }
                                    }

                                }
                                else
                                {
                                    //Neu thua hoac hoa thi mat dut
                                    retval.Bet_Money = 0;
                                    //Cap nhat trang thai cua bet la` thua
                                    retval.Bet_Result = 3;
                                }
                            }
                        }
                        #endregion
                    }
                    //truong hop ti le co dang ( 0:n 1/4)
                    if (tl1 == 0 && tl2 == 0 && tl3 > 0 && tl4 == (double)1 / 4)
                    {
                        #region tl1 == 0 && tl2 == 0 && tl3 > 0 && tl4 == (double)1 / 4
                        //Neu nguoi choi dat cuoc doi 1
                        if (teamselected == team1id)
                        {
                            //Kiem tra ti so
                            if (team1score > team2score)
                            {
                                if ((team1score - team2score) > ConvertUtility.ToInt32(tl3))
                                {
                                    //Neu thang hon > n ban thi an dung theo ti le
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + betamount * tlcateam1;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //Cap nhat trang thai cua bet la thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    if ((team1score - team2score) == ConvertUtility.ToInt32(tl3))
                                    {
                                        //Neu thang n ban thi nhan lai 1/2 tien
                                        retval.Bet_Money = betamount / 2;
                                        //cap nhat lai trang thai cua bet la thua
                                        retval.Bet_Result = 3;
                                    }
                                    else
                                    {
                                        //Neu thang duoi n ban thi thua dut
                                        retval.Bet_Money = 0;
                                        //Cap nhat trang thai cua bet la thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                            else
                            {
                                //hoa hay thua thi mat dut
                                retval.Bet_Money = 0;
                                //Cap nhat trang thai bet la` thua
                                retval.Bet_Result = 3;
                            }
                        }
                        else
                        {
                            if (teamselected == team2id)
                            {
                                //Kiem tra ti so
                                if (team2score >= team1score)
                                {
                                    //Neu thang hoac hoa thi an du theo ti le
                                    if (tlcateam2 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam2 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                    }
                                    //Cap nhat trang thai bet la thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    if ((team1score - team2score) < ConvertUtility.ToInt32(tl3))
                                    {
                                        //Neu thua < n ban thi an dung theo ti le
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang thai bet la thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        if ((team1score - team2score) == ConvertUtility.ToInt32(tl3))
                                        {
                                            //Neu thua duoi n ban an ca von + nua tien
                                            //retval.Bet_Money = betamount + betamount / 2;

                                            //UPDATE 14/12/2015
                                            retval.Bet_Money = betamount + (betamount * tlcateam2 / 2);

                                            //Cap nhat trang thai cua bet la` thang
                                            retval.Bet_Result = 1;
                                        }
                                        else
                                        {
                                            //Neu thua >n ban thi mat dut
                                            retval.Bet_Money = 0;

                                            retval.Bet_Result = 3;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }
                    //truong hop ti le co dang ( 0:n 1/2)
                    if (tl1 == 0 && tl2 == 0 && tl3 > 0 && tl4 == (double)1 / 2)
                    {
                        #region tl1 == 0 && tl2 == 0 && tl3 > 0 && tl4 == (double)1 / 2
                        //Neu nguoi choi dat cuoc cho doi 1
                        if (teamselected == team1id)
                        {
                            //Kiem tra ti so
                            if (team1score > team2score)
                            {
                                if ((team1score - team2score) > ConvertUtility.ToInt32(tl3))
                                {
                                    //Neu thang > n ban thang thi an du tien theo ti le
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //Cap nhat trang thai cua bet la thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //Neu thang <= n ban thi mat dut tien
                                    retval.Bet_Money = 0;
                                    //Cap nhat trang thai cua bet la` thua
                                    retval.Bet_Result = 3;
                                }
                            }
                            else
                            {
                                //Neu hoa thi mat dut
                                retval.Bet_Money = 0;
                                //Cap nhat trang thai cua bet la` thang
                                retval.Bet_Result = 3;
                            }
                        }
                        else
                        {
                            //Neu nguoi choi dat cuoc doi 2
                            if (teamselected == team2id)
                            {
                                //Kiem tra ti so
                                //Neu thang hoac hoa thi an du theo ti le
                                if (team2score >= team1score)
                                {
                                    if (tlcateam2 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam2 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                    }
                                    //Cap nhat trang thai bet la thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //Neu thua <=n ban an theo dung ti le
                                    if ((team1score - team2score) <= ConvertUtility.ToInt32(tl3))
                                    {
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang thai bet la thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //Neu thua > n ban thi mat dut
                                        retval.Bet_Money = 0;
                                        //Cap nhat trang thai cua bet la thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    //truong hop ti le co dang ( 0:n 3/4)
                    if (tl1 == 0 && tl2 == 0 && tl3 > 0 && tl4 == (double)3 / 4)
                    {
                        #region tl1 == 0 && tl2 == 0 && tl3 > 0 && tl4 == (double)3 / 4
                        //Neu nguoi choi dat cuoc doi 1
                        if (teamselected == team1id)
                        {
                            //Kiem tra ti so
                            if (team1score > team2score)
                            {
                                if ((team1score - team2score) > ConvertUtility.ToInt32(tl3) + 1)
                                {
                                    //Neu thang > n+1 ban thi an du tien theo dung ti le
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //Neu thang = n+1 ban thi` an nua tien
                                    if ((team1score - team2score) == ConvertUtility.ToInt32(tl3) + 1)
                                    {
                                        //retval.Bet_Money = betamount + betamount / 2;

                                        //UPDATE 14/12/2015
                                        retval.Bet_Money = betamount + (betamount * tlcateam1 / 2);

                                        //Cap nhat trang thai cua bet na`y la` thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //neu thang <=n ban thi` mat dut
                                        retval.Bet_Money = 0;
                                        //Cap nhat lai trang thai bet la` thua
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                            else
                            {
                                //truong hop hoa hoc thua thi mat dut
                                retval.Bet_Money = 0;
                                //Cap nhat lai trang thai cua bet la` thua
                                retval.Bet_Result = 3;
                            }
                        }
                        else
                        {
                            if (teamselected == team2id)
                            {
                                //Kiem tra ti so
                                if (team2score >= team1score)
                                {
                                    //Neu hoa va thang ba`n thi an du theo dung ti le 
                                    if (tlcateam2 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam2 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                    }
                                    //Cap nhat trang thai cua bet la thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //Neu thua <=n ban thi an du tien theo dung ti le
                                    if ((team1score - team2score) <= ConvertUtility.ToInt32(tl3))
                                    {
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang thai cua bet la thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        if ((team1score - team2score) == ConvertUtility.ToInt32(tl3) + 1)
                                        {
                                            retval.Bet_Money = betamount / 2;
                                            //Cap nhat trang thai bet la thua
                                            retval.Bet_Result = 3;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = 0;
                                            retval.Bet_Result = 3;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    //truong hop ti le co dang ( 0:n)
                    if (tl1 == 0 && tl2 == 0 && tl3 > 0 && tl4 == 0)
                    {
                        #region tl1 == 0 && tl2 == 0 && tl3 > 0 && tl4 == 0
                        //Neu nguoi choi dat cuoc cho doi 1
                        if (teamselected == team1id)
                        {
                            if (team1score > team2score)
                            {
                                if ((team1score - team2score) > ConvertUtility.ToInt32(tl3))
                                {
                                    //Neu thang tren n ban thi an du theo ti le
                                    if (tlcateam1 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam1 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam1) * betamount;
                                    }
                                    //cap nhat trang thai bet la` thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    if ((team1score - team2score) == ConvertUtility.ToInt32(tl3))
                                    {
                                        retval.Bet_Money = betamount;
                                        //Cap nhat trang thai bet la` hoa
                                        retval.Bet_Result = 2;
                                    }
                                    else
                                    {
                                        //Thang <n ban thua dut
                                        retval.Bet_Money = 0;
                                        retval.Bet_Result = 3;
                                    }
                                }
                            }
                            else
                            {
                                //neu hoa hoac thua thi mat dut
                                retval.Bet_Money = 0;
                                retval.Bet_Result = 3;
                            }
                        }
                        else
                        {
                            //Neu nguoi choi dat cuoc cho doi 2
                            if (teamselected == team2id)
                            {
                                if (tlcateam2 >= tlcateam1)
                                {
                                    //Neu thang va hoa thi an du tien theo ti le
                                    if (tlcateam2 > 0)
                                    {
                                        retval.Bet_Money = betamount + tlcateam2 * betamount;
                                    }
                                    else
                                    {
                                        retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                    }
                                    //Cap nhat trang thai cho bet la thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //Neu thua < n ban thi an du theo ti le
                                    if ((team1score - team2score) < ConvertUtility.ToInt32(tl3))
                                    {
                                        if (tlcateam2 > 0)
                                        {
                                            retval.Bet_Money = betamount + tlcateam2 * betamount;
                                        }
                                        else
                                        {
                                            retval.Bet_Money = betamount + (2 + tlcateam2) * betamount;
                                        }
                                        //Cap nhat trang thai cho bet la thang
                                        retval.Bet_Result = 1;
                                    }
                                    else
                                    {
                                        //Neu thua dung n ban thi nhan lai du
                                        if ((team1score - team2score) == ConvertUtility.ToInt32(tl3))
                                        {
                                            retval.Bet_Money = betamount;
                                            //Cap nhat trang thai bet la` hoa`
                                            retval.Bet_Result = 2;
                                        }
                                        else
                                        {
                                            //Neu thua tren n ban thi` thua dut
                                            retval.Bet_Money = 0;
                                            retval.Bet_Result = 3;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            else
            {
                retval = null;
            }
            return retval;
        }

        //2.process by under over rate
        public static BetProcessInfo ReturnBetResultByTX(double tltai, string tltaixiu1, string tltaixiu2, double tlxiu, int tlselected, double betamount, int sumscore, int bettype)
        {
            BetProcessInfo retval = new BetProcessInfo();

            double tltx1;
            double tltx2;

            string[] temp;
            if (tltaixiu1.IndexOf("/") > 0)
            {
                temp = tltaixiu1.Split('/');
                tltx1 = ConvertUtility.ToDouble(temp[0].Replace(" ", "")) / ConvertUtility.ToDouble(temp[1].Replace(" ", ""));
            }
            else
            {
                tltx1 = ConvertUtility.ToDouble(tltaixiu1);
            }
            if (tltaixiu2.IndexOf("/") > 0)
            {
                temp = tltaixiu2.Split('/');
                tltx2 = ConvertUtility.ToDouble(temp[0].Replace(" ", "")) / ConvertUtility.ToDouble(temp[1].Replace(" ", ""));
            }
            else
            {
                tltx2 = ConvertUtility.ToDouble(tltaixiu2);
            }

            //Kiem tra loai ca do la` theo ti le tai xiu
            if (bettype == 2)
            {
                //truong hop ti le tai xiu (n)
                #region Truong hop Ty le Tai-Xiu (n)

                if (tltx1 > 0 && tltx2 == 0)
                {
                    //truong hop neu nguoi choi chon ta`i
                    if (tlselected == 1)
                    {
                        if (sumscore > ConvertUtility.ToInt32(tltx1))
                        {
                            //Neu tong ti so lon hon n thi an du theo ti le
                            if (tltai > 0)
                            {
                                retval.Bet_Money = betamount + tltai * betamount;
                            }
                            else
                            {
                                retval.Bet_Money = betamount + (2 + tltai) * betamount;
                            }
                            //Cap nhat lai trang thai bet la` thang
                            retval.Bet_Result = 1;
                        }
                        else
                        {
                            if (sumscore == ConvertUtility.ToInt32(tltaixiu1))
                            {
                                //Neu tong ti so = n thi nhan lai tien 
                                retval.Bet_Money = betamount;
                                //Cap nhat trang thai cua bet la` hoa
                                retval.Bet_Result = 2;
                            }
                            else
                            {
                                //Neu tong ti so < n thi mat dut
                                retval.Bet_Money = 0;
                                //Cap nhat trang thai cua bet la` thua
                                retval.Bet_Result = 3;
                            }
                        }
                    }
                    else
                    {
                        //truong hop nguoi choi chon xiu
                        if (tlselected == 2)
                        {
                            //Neu tong ti so < n thi nguoi choi an du theo ti le
                            if (sumscore < ConvertUtility.ToInt32(tltx1))
                            {
                                if (tlxiu > 0)
                                {
                                    retval.Bet_Money = betamount + tlxiu * betamount;
                                }
                                else
                                {
                                    retval.Bet_Money = betamount + (2 + tlxiu) * betamount;
                                }
                                //Cap nhat trang thai bet la` thang
                                retval.Bet_Result = 1;
                            }
                            else
                            {
                                if (sumscore == ConvertUtility.ToInt32(tltx1))
                                {
                                    //Neu tong ti so = n thi nguoi choi nhan lai du tien
                                    retval.Bet_Money = betamount;
                                    //Cap nhat trang thai bet la hoa
                                    retval.Bet_Result = 2;

                                }
                                else
                                {
                                    //Neu tong ti so >n thi nguoi choi mat het
                                    retval.Bet_Money = 0;
                                    //cap nhat trang thai bet la thua
                                    retval.Bet_Result = 3;
                                }
                            }
                        }
                    }
                }

                #endregion

                //truong hop ti le tai xiu co' dang (n 1/4)
                #region Truong hop Ty le Tai-Xiu co dang (n 1/4)

                if (tltx1 > 0 && tltx2 == (double)1 / 4)
                {
                    //Neu nguoi choi dat cuoc ti le ta`i
                    if (tlselected == 1)
                    {
                        if (sumscore > ConvertUtility.ToInt32(tltx1))
                        {
                            //Neu tong so ban thang > n thi an du theo ti le
                            if (tltai > 0)
                            {
                                retval.Bet_Money = betamount + tltai * betamount;
                            }
                            else
                            {
                                retval.Bet_Money = betamount + (2 + tltai) * betamount;
                            }
                            //Cap nhat trang thai cua bet la thang
                            retval.Bet_Result = 1;
                        }
                        else
                        {
                            if (sumscore == ConvertUtility.ToInt32(tltx1))
                            {
                                //Neu tong so ban thang n ban thi nhan lai nua tien
                                retval.Bet_Money = betamount / 2;
                                //Cap nhat trang thai cua bet la` thua
                                retval.Bet_Result = 3;
                            }
                            else
                            {
                                //Neu tong so ban thang < n thi mat dut
                                retval.Bet_Money = 0;
                                //Cap nhat trang thai cua bet la` thua
                                retval.Bet_Result = 3;
                            }
                        }
                    }
                    else
                    {
                        // Neu nguoi choi dat cuoc ti le xiu
                        if (tlselected == 2)
                        {
                            if (sumscore < ConvertUtility.ToInt32(tltx1))
                            {
                                //Neu tong so ban thang <n thi an du theo ti le
                                if (tlxiu > 0)
                                {
                                    retval.Bet_Money = betamount + tlxiu * betamount;
                                }
                                else
                                {
                                    retval.Bet_Money = betamount + (2 + tlxiu) * betamount;
                                }
                                //Cap nhat trang thai cua bet la` thang
                                retval.Bet_Result = 1;
                            }
                            else
                            {
                                if (sumscore == ConvertUtility.ToInt32(tltx1))
                                {
                                    //Neu tong so ban thang =n thi an nua tien
                                    //retval.Bet_Money = betamount + betamount / 2;

                                    //UPDATE 14/12/2015
                                    retval.Bet_Money = betamount + (betamount * tlxiu / 2);

                                    //Cap nhat trang thai bet la thang
                                    retval.Bet_Result = 1;
                                }
                                else
                                {
                                    //Neu tong so ban thang > n thi mat dut
                                    retval.Bet_Money = 0;
                                    //Cap nhat trang thai bet la thua
                                    retval.Bet_Result = 3;
                                }
                            }
                        }
                    }
                }

                #endregion

                //truong hop ti le tai xiu co' dang (n 1/2)
                #region Truong hop Ty le Tai-Xiu co dang (n 1/2)

                if (tltx1 > 0 && tltx2 == (double)1 / 2)
                {
                    //Neu nguoi choi chon cua ta`i
                    if (tlselected == 1)
                    {
                        if (sumscore > ConvertUtility.ToInt32(tltx1))
                        {
                            //neu tong so ban thang >n thi an du theo ti le
                            if (tltai > 0)
                            {
                                retval.Bet_Money = betamount + tltai * betamount;
                            }
                            else
                            {
                                retval.Bet_Money = betamount + (2 + tltai) * betamount;
                            }
                            //Cap nhat tinh trang cua bet la` thang
                            retval.Bet_Result = 1;
                        }
                        else
                        {
                            //Neu tong so bang thang <= n thi mat dut
                            retval.Bet_Money = 0;
                            //Cap nhat trang thai bet la` thua
                            retval.Bet_Result = 3;
                        }
                    }
                    else
                    {
                        //Neu nguoi choi dat cua xiu
                        if (tlselected == 2)
                        {
                            if (sumscore <= ConvertUtility.ToInt32(tltx1))
                            {
                                //Neu tong so ban thang <=n thi an du theo ti le
                                if (tlxiu > 0)
                                {
                                    retval.Bet_Money = betamount + betamount * tlxiu;
                                }
                                else
                                {
                                    retval.Bet_Money = betamount + (2 + tlxiu) * betamount;
                                }
                                //Cap nhat trang thai bet la` thang
                                retval.Bet_Result = 1;
                            }
                            else
                            {
                                //Neu tong so ban thang > n thi ma dut
                                retval.Bet_Money = 0;
                                //Cap nhat trang thai bet la` thua
                                retval.Bet_Result = 3;
                            }
                        }
                    }
                }

                #endregion

                //truong hop ti le tai xiu co' dang (n 3/4)
                #region Truong hop ti le Tai-Xiu co dang (n 3/4)

                if (tltx1 > 0 && tltx2 == (double)3 / 4)
                {
                    //Neu nguoi choi dat cua tai
                    if (tlselected == 1)
                    {
                        if (sumscore > ConvertUtility.ToInt32(tltx1))
                        {
                            if (sumscore == ConvertUtility.ToInt32(tltx1) + 1)
                            {
                                //neu tong so ban thang = n+1 thi an nua tien
                                retval.Bet_Money = betamount + betamount / 2;
                            }
                            else
                            {
                                //Neu tong so ban thang lon hon n+1 thi an du theo ti le
                                if (tltai > 0)
                                {
                                    retval.Bet_Money = betamount + tltai * betamount;
                                }
                                else
                                {
                                    retval.Bet_Money = betamount + (2 + tltai) * betamount;
                                }
                            }
                            //cap nhat trang thai bet la` thang
                            retval.Bet_Result = 1;
                        }
                        else
                        {
                            //Neu tong so bang thang =<n hoac thua thi mat dut tien
                            retval.Bet_Money = 0;
                            //Cap nhat trang thai bet la` thua
                            retval.Bet_Result = 3;
                        }
                    }
                    else
                    {
                        //Neu nguoi choi dat cua xiu
                        if (tlselected == 2)
                        {
                            if (sumscore <= ConvertUtility.ToInt32(tltx1))
                            {
                                //Neu tong so ban thang <=n thi an du theo ti le
                                if (tltai > 0)
                                {
                                    retval.Bet_Money = betamount + tltai * betamount;
                                }
                                else
                                {
                                    retval.Bet_Money = betamount + (2 + tltai) * betamount;
                                }
                                retval.Bet_Result = 1;
                            }
                            else
                            {
                                if (sumscore == ConvertUtility.ToInt32(tltx1) + 1)
                                {
                                    //Neu tong so ban thang = n +1 thi nhan lai nua tien
                                    retval.Bet_Money = betamount / 2;
                                    //Cap nhat trang thai cua bet la` thua
                                    retval.Bet_Result = 3;
                                }
                                else
                                {
                                    //Neu tong so bang thang > n+1 thi mat dut
                                    retval.Bet_Money = 0;
                                    //Cap nhat trang thai bet la` thua
                                    retval.Bet_Result = 3;
                                }
                            }
                        }
                    }
                }

                #endregion

            }
            else
            {
                retval = null;
            }
            return retval;
        }

        //3.process by euro rate
        public static BetProcessInfo ReturnBetResultByTLCAu(string team1Id, string team2Id, double euroRate, double team1Rate, double team2Rate, double betamount, string teamSelected, int team1Score, int team2Score)
        {
            BetProcessInfo info = new BetProcessInfo();

            //THANG = 1,
            //HOA = 2,
            //THUA = 3,

            _log.Info("team1Id : " + team1Id + "|team2Id : " + team2Id + "| euroRate :" + euroRate + "|team1Rate:" + team1Rate + "|team2Rate:" + team2Rate + "|betamount:" + betamount + "|teamselected:" + teamSelected + "|team1Score:" + team1Score + "|team2Score:" + team2Score);

            team1Id = team1Id.Trim();
            team2Id = team2Id.Trim();
            teamSelected = teamSelected.Trim();


            if (team1Score > team2Score)// TEAM1 THANG
            {
                if (team1Id == teamSelected)//THANG KEO
                {
                    info.Bet_Result = 1;
                    info.Bet_Money = (team1Rate * betamount) + betamount;
                    //info.Bet_Money = (team1Rate * betamount);
                }
                else
                {
                    info.Bet_Result = 3;
                    info.Bet_Money = -betamount;
                }
            }
            else if (team1Score < team2Score)// TEAM2 THANG
            {
                if (team2Id == teamSelected)//THANG KEO
                {
                    info.Bet_Result = 1;
                    info.Bet_Money = (team2Rate * betamount) + betamount;
                    //info.Bet_Money = (team2Rate * betamount);
                }
                else
                {
                    info.Bet_Result = 3;
                    info.Bet_Money = -betamount;
                }
            }
            else if (team1Score == team2Score)//2 DOI HOA NHAU
            {
                if (teamSelected != team1Id && teamSelected != team2Id)
                {
                    info.Bet_Result = 1;
                    info.Bet_Money = (euroRate * betamount) + betamount;
                    //info.Bet_Money = (euroRate * betamount);
                }
                else
                {
                    info.Bet_Result = 3;
                    info.Bet_Money = -betamount;
                }
            }

            return info;
        }
        public static DataTable GameBetOnlineNotProcess()
        {
            //DataSet ds = SqlHelper.ExecuteDataset(connectString, CommandType.StoredProcedure, "dsg_BetOnline_FolkGetAllByStatusAndNotProcess");
            string cmdText = "select * from [dsg_BetOnline_Folk] where betid = 563";
            DataSet ds = SqlHelper.ExecuteDataset(connectString, CommandType.Text, cmdText);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return new DataTable();
        }
        public static DataTable GameMatchPlayedGetInfo(int matchId)
        {
            DataSet ds = SqlHelper.ExecuteDataset(connectString, "dsg_BetOnline_GetPlayedMatchInfo", matchId);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return new DataTable();
        }
        public static DataTable GameBetOnlineGetInfo(int betId)
        {
            DataSet ds = SqlHelper.ExecuteDataset(connectString, "dsg_BetOnline_Folk_GetInfo", betId);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return new DataTable();
        }
        #endregion
    }
}
