using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Test_Console.Library;

namespace Test_Console.Content
{
    class UpdateVisport
    {
        static string ConnTTND = "server=db168.vmgmedia.vn; database=TTND; uid=ttndacc; pwd=Swe8234Ue3ND";


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
                        Console.WriteLine("***** Updatedata visport partnerId : " + partnerId);
                        if (dtDichVu != null && dtDichVu.Rows.Count > 0)
                        {
                            foreach (DataRow rowDv in dtDichVu.Rows)
                            {
                                //for(int i = 1; i <=17; i++) { 
                                try
                                {
                                    DateTime now = DateTime.Now.AddDays(-1);
                                    int day = now.Day;
                                    int month = now.Month;
                                    int year = now.Year;
                                    Insert_CDR_VMN_Visport_ByDay(day, month, year, Convert.ToInt32(partnerId), Convert.ToString(rowDv["Service_ID"]));
                                    Console.WriteLine(now + "___***** visport update service : " + rowDv["Service_ID"].ToString());
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("***** visport Service_eror : " + rowDv["Service_ID"].ToString() + "--" + ex.ToString());

                                }

                            //}
                            }
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("***** Updatedata visport : " + partnerId + "--" + ex.Message + "--" + ex.ToString());

                    }
                }
            }



        }
        public static DataTable GetAllPartner()
        {
            DataSet ds = SqlHelper.ExecuteDataset(ConnTTND, "CDR_GetAll_Service");
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }
        public static DataTable GetAllDichVuByPartner(int PartnerID)
        {
            DataSet ds = SqlHelper.ExecuteDataset(ConnTTND, "CDR_Visport_GetAllDichVuByPartner", PartnerID);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }
        public static void Insert_CDR_VMN_Visport_ByDay(int day, int month, int year, int partnerid, string serviceid)
        {
            SqlConnection dbConn = new SqlConnection(ConnTTND);
            SqlCommand dbCmd = new SqlCommand("CDR_VMN_Visport_ByDay_New", dbConn);
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
