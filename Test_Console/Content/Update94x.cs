using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Test_Console.Library;

namespace Test_Console.Content
{
    class Update94x
    {
        //static string connectString = "server=dbttnd.vmgmedia.vn;database=TTND_Services;uid=ttndservices;pwd=ttND53r7vice$";
        static string connectString = ConfigurationManager.ConnectionStrings["Conn949"].ConnectionString;
        public static void Updatedata()
        {
            DataTable dtPartner = GetAllPartner();

            if (dtPartner != null && dtPartner.Rows.Count > 0)
            {
                foreach (DataRow drPartner in dtPartner.Rows)
                {
                    string partnerId = drPartner["PartnerId"].ToString();
                    //string partnerId = "1";
                    Console.WriteLine("***** Updatedata 94x partnerId : " + partnerId);
                    //string partnerId = "3";
                    try
                    {
                        DataTable dtDichVu = GetAllDichVuByPartner(Convert.ToInt32(partnerId));
                        if (dtDichVu != null && dtDichVu.Rows.Count > 0)
                        {
                            foreach (DataRow rowDv in dtDichVu.Rows)
                            {
                                for(int i = 4; i >=1; i--) { 
                                try
                                {
                                    DateTime now = DateTime.Now.AddDays(-i);
                                    int day = now.Day;
                                    int month = now.Month;
                                    int year = now.Year;
                                    Insert_CDR_VMN_94x_ByDay(day, month, year, Convert.ToInt32(partnerId), Convert.ToString(rowDv["Service_ID"]));
                                        Console.WriteLine(now + "___***** 94x update service : " + rowDv["Service_ID"].ToString());
                                    }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("***** 94x Service_eror : " + rowDv["Service_ID"].ToString() + "--" + ex.ToString());

                                }

                            }
                            }
                        }

                        Console.WriteLine("***** Updatedata 94x : " + partnerId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("***** Updatedata 94x : " + partnerId + "--" + ex.Message + "--" + ex.ToString());

                    }
                }
            }
            
        }
        public static void Updatedata_WithpartnerID(int partnerId)
        {
                                   
                    try
                    {
                        DataTable dtDichVu = GetAllDichVuByPartner(partnerId);
                        if (dtDichVu != null && dtDichVu.Rows.Count > 0)
                        {
                            foreach (DataRow rowDv in dtDichVu.Rows)
                            {
                                for(int i=4;i<=14;i++) { 
                                try
                                {
                                    DateTime now = DateTime.Now.AddDays(-i);
                                    int day = now.Day;
                                    int month = now.Month;
                                    int year = now.Year;
                                    Insert_CDR_VMN_94x_ByDay(day, month, year,partnerId, Convert.ToString(rowDv["Service_ID"]));
                                    Console.WriteLine(now+"___***** gameportal update service : " + rowDv["Service_ID"].ToString());
                        }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("***** gameportal Service_eror : " + rowDv["Service_ID"].ToString() + "--" + ex.ToString());

                                }

                            }
                            }
                        }

                        //Console.WriteLine("***** Updatedata gameportal : " + partnerId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("***** Updatedata gameportal : " + partnerId + "--" + ex.Message + "--" + ex.ToString());

                    }
               

        }
        public static DataTable GetAllPartner()
        {
            DataSet ds = SqlHelper.ExecuteDataset(connectString, "CDR_GetAllPartner");
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }
        public static DataTable GetAllDichVuByPartner(int PartnerID)
        {
            //CDR_VNM949_GetAllDichVuByPartner
            //CDR_VNM949_GetAllDichVuByPartner_AndServiceType
            DataSet ds = SqlHelper.ExecuteDataset(connectString, "CDR_VNM949_GetAllDichVuByPartner", PartnerID);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }
        //public static void Insert_CDR_VMN_94x_ByDay(int day, int month, int year, int partnerid, string serviceid)
        //{
        //    //SqlHelper.ExecuteNonQuery(connectString, "CDR_VMN_94x_ByDay_UpdateLuyKe",
        //    SqlHelper.ExecuteNonQuery(connectString, "CDR_VMN_94x_ByDay",
        //                     day, month, year, partnerid, serviceid
        //                        );
        //}
        public static void Insert_CDR_VMN_94x_ByDay(int day, int month, int year, int partnerid, string serviceid)
        {
            SqlConnection dbConn = new SqlConnection(connectString);
            SqlCommand dbCmd = new SqlCommand("CDR_VMN_94x_ByDay", dbConn);
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.CommandTimeout = 300;
            dbCmd.Parameters.AddWithValue("@DAY", day);
            dbCmd.Parameters.AddWithValue("@MONTH", month);
            dbCmd.Parameters.AddWithValue("@YEAR", year);
            dbCmd.Parameters.AddWithValue("@PartnerID", partnerid);
            dbCmd.Parameters.AddWithValue("@Service_ID", serviceid);


            try
            {
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
            }
            finally
            {




                dbConn.Close();
            }

        }
    }
}
