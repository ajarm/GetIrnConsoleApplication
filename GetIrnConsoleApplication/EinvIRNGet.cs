using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetIrnConsoleApplication
{
    class EinvIRNGet
    {
        public static string GetJsonIRNGeneration(string strGSTINNo, string Doctype, string strDocNo, string strDocDate, string SupTyp)
        {
            string returnJson = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                try
                {
                    StringBuilder MyStringBuilder = new StringBuilder();
                    #region commented
                    conn.Open();
                    SqlCommand dCmd = new SqlCommand("usp_Construct_JSON_EINVOICE_Generation", conn);
                    dCmd.CommandTimeout = 7200;
                    dCmd.CommandType = CommandType.StoredProcedure;
                    dCmd.Parameters.Add(new SqlParameter("@SGSTIN", strGSTINNo));
                    dCmd.Parameters.Add(new SqlParameter("@DocType", Doctype));
                    dCmd.Parameters.Add(new SqlParameter("@DocNo", strDocNo));
                    dCmd.Parameters.Add(new SqlParameter("@DocDt", strDocDate));
                    dCmd.Parameters.Add(new SqlParameter("@SupTyp", SupTyp));
                    var reader = dCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MyStringBuilder.Append(reader.GetValue(0));
                        }
                    }
                    reader.Close();
                    conn.Close();
                    returnJson = MyStringBuilder.ToString();
                    
                    #endregion
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return returnJson;
        }

        public static string GetJsonCONSEWBGeneration(string strGSTINNo, string strtransDocNo, string strtransDocDate)
        {
            DataSet ds = new DataSet();
            string returnJson = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                try
                {
                    StringBuilder MyStringBuilder = new StringBuilder();
                    #region commented
                    conn.Open();
                    SqlCommand dCmd = new SqlCommand("usp_Construct_JSON_CONS_EWB_Generation", conn);
                    dCmd.CommandTimeout = 7200;
                    dCmd.CommandType = CommandType.StoredProcedure;
                    dCmd.Parameters.Add(new SqlParameter("@userGSTIN", strGSTINNo));
                    dCmd.Parameters.Add(new SqlParameter("@TransDocNo", strtransDocNo));
                    dCmd.Parameters.Add(new SqlParameter("@TransDocDate", strtransDocDate));
                    //dCmd.Parameters.Add(new SqlParameter("@JsonResult", SqlDbType.NVarChar,1000000000)).Direction = ParameterDirection.Output;

                    var reader = dCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MyStringBuilder.Append(reader.GetValue(0));
                        }
                    }
                    reader.Close();
                    conn.Close();
                    returnJson = MyStringBuilder.ToString();
                    #endregion
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return returnJson;
        }

        public static string GetJsonEWBUpdateVehicleNo(string strEwbNo)
        {
            DataSet ds = new DataSet();
            string returnJson = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                try
                {
                    StringBuilder MyStringBuilder = new StringBuilder();
                    #region commented
                    conn.Open();
                    SqlCommand dCmd = new SqlCommand("usp_Construct_JSON_EWB_UPDATE_VEHICLENO", conn);
                    dCmd.CommandTimeout = 7200;
                    dCmd.CommandType = CommandType.StoredProcedure;
                    //dCmd.Parameters.Add(new SqlParameter("@Usergstin", struserGstin));
                    dCmd.Parameters.Add(new SqlParameter("@EwbNo", strEwbNo));
                    //dCmd.Parameters.Add(new SqlParameter("@JsonResult", SqlDbType.NVarChar,1000000000)).Direction = ParameterDirection.Output;

                    var reader = dCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MyStringBuilder.Append(reader.GetValue(0));
                        }
                    }
                    reader.Close();
                    conn.Close();
                    returnJson = MyStringBuilder.ToString();
                    #endregion
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return returnJson;
        }


        public static string GetJsonInitiateMultiVeh(string strGSTINNo, string ewbNo, string strDocDate)
        {
            DataSet ds = new DataSet();
            string returnJson = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                try
                {
                    StringBuilder MyStringBuilder = new StringBuilder();
                    #region commented
                    conn.Open();
                    SqlCommand dCmd = new SqlCommand("usp_Construct_JSON_EWB_INITIATE_MULTIVEHICLE_MOVEMENT", conn);
                    dCmd.CommandTimeout = 7200;
                    dCmd.CommandType = CommandType.StoredProcedure;
                    dCmd.Parameters.Add(new SqlParameter("@userGSTIN", strGSTINNo));
                    dCmd.Parameters.Add(new SqlParameter("@ewbNo", ewbNo));
                    var reader = dCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MyStringBuilder.Append(reader.GetValue(0));
                        }
                    }
                    reader.Close();
                    conn.Close();
                    returnJson = MyStringBuilder.ToString();
                    #endregion
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return returnJson;
        }

        public static string GetJsonAddMultiVeh(string strGSTINNo, string ewbNo, string strDocDate)
        {
            DataSet ds = new DataSet();
            string returnJson = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                try
                {
                    StringBuilder MyStringBuilder = new StringBuilder();
                    #region commented
                    conn.Open();
                    SqlCommand dCmd = new SqlCommand("usp_Construct_JSON_EWB_ADDMULTIVEHICLE_MOVEMENT", conn);
                    dCmd.CommandTimeout = 7200;
                    dCmd.CommandType = CommandType.StoredProcedure;
                    dCmd.Parameters.Add(new SqlParameter("@userGSTIN", strGSTINNo));
                    dCmd.Parameters.Add(new SqlParameter("@ewbNo", ewbNo));
                    var reader = dCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MyStringBuilder.Append(reader.GetValue(0));
                        }
                    }
                    reader.Close();
                    conn.Close();
                    returnJson = MyStringBuilder.ToString();
                    #endregion
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return returnJson;
        }

        public static string GetJsonUpdMultiVeh(string strGSTINNo, string ewbNo, string strDocDate)
        {
            DataSet ds = new DataSet();
            string returnJson = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                try
                {
                    StringBuilder MyStringBuilder = new StringBuilder();
                    #region commented
                    conn.Open();
                    SqlCommand dCmd = new SqlCommand("usp_Construct_JSON_EWB_UPDATE_MULTIVEHICLE_MOVEMENT", conn);
                    dCmd.CommandTimeout = 7200;
                    dCmd.CommandType = CommandType.StoredProcedure;
                    dCmd.Parameters.Add(new SqlParameter("@userGSTIN", strGSTINNo));
                    dCmd.Parameters.Add(new SqlParameter("@ewbNo", ewbNo));
                    var reader = dCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MyStringBuilder.Append(reader.GetValue(0));
                        }
                    }
                    reader.Close();
                    conn.Close();
                    returnJson = MyStringBuilder.ToString();
                    #endregion
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return returnJson;
        }
    }
}
