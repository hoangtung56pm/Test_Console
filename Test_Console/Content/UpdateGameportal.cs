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
    class UpdateGameportal
    {
        //static string ConnGamePortal = "server=sv56.vmgmedia.vn;database=ContentPortal;uid=cportal;pwd=cportal123cportal";
        static string ConnGamePortal = ConfigurationManager.ConnectionStrings["ConnGamePortal"].ConnectionString;

        public static void Updatedata()
        {
            DataTable dtPartner = GetAllPartner();

            if (dtPartner != null && dtPartner.Rows.Count > 0)
            {
                foreach (DataRow drPartner in dtPartner.Rows)
                {
                    string partnerId = drPartner["PartnerId"].ToString();
                    //string partnerId = "3";
                    try
                    {
                        DataTable dtDichVu = GetAllDichVuByPartner(Convert.ToInt32(partnerId));
                        if (dtDichVu != null && dtDichVu.Rows.Count > 0)
                        {
                            foreach (DataRow rowDv in dtDichVu.Rows)
                            {
                                //for(int i = 4; i >=1; i--) { 
                                try
                                {
                                    DateTime now = DateTime.Now.AddDays(-1);
                                    int day = now.Day;
                                    int month = now.Month;
                                    int year = now.Year;
                                    CDR_VMN_Gameportal_ByDay(day, month, year, Convert.ToInt32(partnerId), Convert.ToString(rowDv["Service_ID"]));

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("***** gameportal Service_eror : " + rowDv["Service_ID"].ToString() + "--" + ex.ToString());

                                }

                            }
                            //}
                        }                       
                        
                        Console.WriteLine("***** Updatedata gameportal : " + partnerId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("***** Updatedata gameportal : " + partnerId + "--" + ex.Message + "--" + ex.ToString());
                        
                    }
                }
            }


            
        }
        public static DataTable GetAllPartner()
        {
            DataSet ds = SqlHelper.ExecuteDataset(ConnGamePortal, "CDR_GetAll_Partner");
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }
        public static DataTable GetAllDichVuByPartner(int PartnerID)
        {
            DataSet ds = SqlHelper.ExecuteDataset(ConnGamePortal, "CDR_GetAllServiceByPartner", PartnerID);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }
        public static void CDR_VMN_Gameportal_ByDay(int day, int month, int year, int partnerid, string serviceid)
        {
            SqlConnection dbConn = new SqlConnection(ConnGamePortal);
            SqlCommand dbCmd = new SqlCommand("CDR_VMN_Gameportal_ByDay", dbConn);
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
