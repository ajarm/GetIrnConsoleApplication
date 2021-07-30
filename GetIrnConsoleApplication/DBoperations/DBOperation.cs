using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using GetIrnConsoleApplication.Helper;
using GetIrnConsoleApplication.AppClasses;

namespace GetIrnConsoleApplication.DBoperations
{
    class DBOperation
    {
            public bool GetGSTINAuthenticationDetails(string GSTIN, out string strUsername, out string strAppKey, out string strEncryptedSek, out string strAuthToken)
            {
                string Username = "", AppKey = "", EncryptedSEK = "", AuthToken = "";
                try
                {
                    //string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                    //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["WePASPDBConnect"].ConnectionString);
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string queryString = "Select* from TBL_AUTH_KEYS_EINVOICE where GSTIN=@GSTIN";
                        SqlCommand command = new SqlCommand(queryString, con);
                        command.Parameters.AddWithValue("@GSTIN", GSTIN);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Username = reader["Username"].ToString();
                            AppKey = reader["AppKey"].ToString();
                            EncryptedSEK = reader["EncryptedSEK"].ToString();
                            AuthToken = reader["AuthToken"].ToString();
                        }

                        //    if (dt.Rows.Count > 0)
                        //{
                        //    Username = dt.Rows[0]["Username"].ToString();
                        //    AppKey = dt.Rows[0]["AppKey"].ToString();
                        //    EncryptedSEK = dt.Rows[0]["EncryptedSEK"].ToString();
                        //    AuthToken = dt.Rows[0]["AuthToken"].ToString();
                        //}
                        con.Close();
                    }
                    strUsername = Username;
                    strAppKey = AppKey;
                    strEncryptedSek = EncryptedSEK;
                    strAuthToken = AuthToken;
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                strUsername = Username;
                strAppKey = AppKey;
                strEncryptedSek = EncryptedSEK;
                strAuthToken = AuthToken;
                return false;
            }

            public static bool UpdateTransactionLog(int logId, string responseData)
            {
                try
                {
                    //string isEwayBill = ConfigurationManager.AppSettings["IS_EINV_ASP_EXCEPTION_LOG"];
                    string isEwayBill = ConfigurationManager.ConnectionStrings["IS_EINV_ASP_EXCEPTION_LOG"].ConnectionString;

                    if (isEwayBill == "NO")
                    {
                        return false;
                    }
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        string sqlquery = "update TBL_EINVOICE_TRANS_LOG set ResponseData=@ResponseData, ResponseDateTime=@ResponseDateTime where Id=@Id";
                        SqlCommand cmd = new SqlCommand(sqlquery, con);
                        cmd.Parameters.Clear();
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = logId;
                        cmd.Parameters.Add("@ResponseData", SqlDbType.NVarChar).Value = responseData;
                        cmd.Parameters.Add("@ResponseDateTime", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return false;
            }

            public bool UpdateSuccessResponse(string docnum, string docdt, string docType, string SupTyp, string Ackno, string Ackdate, string Irn, string SignedInvoice, string SignedQR, string Status)
            {
                try
                {
                    //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Parameters.Clear();
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@AckNo", SqlDbType.NVarChar).Value = Ackno;
                        cmd.Parameters.Add("@AckDt", SqlDbType.DateTime).Value = Ackdate;
                        cmd.Parameters.Add("@Irn", SqlDbType.NVarChar).Value = Irn.ToUpper();
                        cmd.Parameters.Add("@SignedInvoice", SqlDbType.NVarChar).Value = SignedInvoice.Trim();
                        cmd.Parameters.Add("@SignedQRCode", SqlDbType.NVarChar).Value = SignedQR.Trim();
                        cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;

                        SQLHelper.UpdateTable("TBL_EINVOICE_GENERATION", docnum, docdt, docType, SupTyp, cmd, con);
                        con.Close();
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return false;
            }

            public bool UpdateSuccessResponse(string docnum, string docdt, string docType, string SupTyp, string Ackno, string Ackdate, string Irn, long? EwbNo, string EwbDt, string EwbValidTill, string SignedInvoice, string SignedQR, string Status)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Parameters.Clear();
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@AckNo", SqlDbType.NVarChar).Value = Ackno;
                        cmd.Parameters.Add("@AckDt", SqlDbType.DateTime).Value = Ackdate;
                        cmd.Parameters.Add("@Irn", SqlDbType.NVarChar).Value = Irn.ToUpper();
                        cmd.Parameters.Add("@EwbNo", SqlDbType.NVarChar).Value = EwbNo;
                        cmd.Parameters.Add("@EwbDt", SqlDbType.DateTime).Value = EwbDt;
                        cmd.Parameters.Add("@EwbValidTill", SqlDbType.DateTime).Value = EwbValidTill;
                        cmd.Parameters.Add("@SignedInvoice", SqlDbType.NVarChar).Value = SignedInvoice.Trim();
                        cmd.Parameters.Add("@SignedQRCode", SqlDbType.NVarChar).Value = SignedQR.Trim();
                        cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;

                        SQLHelper.UpdateTable("TBL_EINVOICE_GENERATION", docnum, docdt, docType, SupTyp, cmd, con);
                        con.Close();
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return false;
            }

        public AuthTokenDetailsModel GetAuthTokenDetails(string fromGSTIN)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    string queryString = "Select TOP 1 * from TBL_AUTH_KEYS_EINVOICE where GSTIN=@FromGSTIN";
                    SqlCommand command = new SqlCommand(queryString, con);
                    command.Parameters.AddWithValue("@FromGSTIN", fromGSTIN);

                    try
                    {
                        con.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                return new AuthTokenDetailsModel { createDateTime = reader["CreatedDate"].ToString(), password = reader["Password"].ToString() };
                            }

                        }
                        //while (reader.Read())
                        //{
                        //    return true;
                        //}
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public bool UpdateAuthTokenDetails(string encSEK, string authToken, string GSTIN)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.Clear();
                    cmd.CommandTimeout = 0;
                    //cmd.Parameters.Add("@AppKey", SqlDbType.NVarChar).Value = appKey.Trim();
                    //cmd.Parameters.Add("@EncryptedAppKey", SqlDbType.NVarChar).Value = encAppKey.Trim();
                    //cmd.Parameters.Add("@EncryptedPassword", SqlDbType.NVarChar).Value = encPassword.Trim();
                    //SQLHelper.UpdateTable("TBL_AUTH_KEYS_EWB", "GSTIN", GSTIN, cmd, con);


                    cmd.Parameters.Add("@EncryptedSEK", SqlDbType.NVarChar).Value = encSEK.Trim();
                    cmd.Parameters.Add("@AuthToken", SqlDbType.NVarChar).Value = authToken.Trim();
                    cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                    SQLHelper.UpdateTable("TBL_AUTH_KEYS_EINVOICE", "GSTIN", GSTIN, cmd, con);
                    con.Close();
                    return true;
                }

            }
            catch (Exception e)
            {

            }
            return false;
        }

        public string RetrieveEinvoiceId(string DocNo, string DocDate, string docType, string SupTyp)
            {
                string einvoice;
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {
                        string queryString = "Select einvoiceid from TBL_EINVOICE_GENERATION Where DocNo=@DocNo and DocDt=@DocDt and DocType=@DocType and SupTyp=@SupTyp";
                        SqlCommand command = new SqlCommand(queryString, con);
                        command.Parameters.AddWithValue("@DocNo", DocNo);
                        command.Parameters.AddWithValue("@DocDt", DocDate);
                        command.Parameters.AddWithValue("@DocType", docType);
                        command.Parameters.AddWithValue("@SupTyp", SupTyp);
                        try
                        {
                            con.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                einvoice = reader["einvoiceid"].ToString();
                                reader.Close();
                                return einvoice;
                            }
                            //while (reader.Read())
                            //{
                            //    return true;
                            //}
                            reader.Close();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {

                }
                return null;
            }
            public bool UpdateErrorResponse(string EinvoiceID, string ErrorMessage, string ErrorCode, string ApiName)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Parameters.Clear();
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@ErrorDesc", SqlDbType.NVarChar).Value = ErrorMessage;
                        cmd.Parameters.Add("@ErrorCode", SqlDbType.NVarChar).Value = ErrorCode;
                        cmd.Parameters.Add("@EinvoiceId", SqlDbType.NVarChar).Value = EinvoiceID;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@ApiName", SqlDbType.NVarChar).Value = ApiName;

                        SQLHelper.UpdateTable("TBL_EINVOICE_GENERATION_ERRORDETAILS", EinvoiceID, cmd, con);
                        con.Close();
                        return true;
                    }
                }
                catch (Exception e )
                {
                }
                return false;
            }

            public bool UpdateErrorResponse(string EinvoiceID, string ErrorMessage, string ErrorCode, string AckNo, string AckDt, string IRN, string ApiName)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Parameters.Clear();
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@ErrorDesc", SqlDbType.NVarChar).Value = ErrorMessage;
                        cmd.Parameters.Add("@ErrorCode", SqlDbType.NVarChar).Value = ErrorCode;
                        cmd.Parameters.Add("@EinvoiceId", SqlDbType.NVarChar).Value = EinvoiceID;
                        cmd.Parameters.Add("@AckNo", SqlDbType.NVarChar).Value = AckNo;
                        cmd.Parameters.Add("@AckDt", SqlDbType.NVarChar).Value = AckDt;
                        cmd.Parameters.Add("@ErrIrn", SqlDbType.NVarChar).Value = IRN;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@ApiName", SqlDbType.NVarChar).Value = ApiName;

                        SQLHelper.UpdateTable("TBL_EINVOICE_GENERATION_ERRORDETAILS", EinvoiceID, cmd, con);
                        con.Close();
                        return true;
                    }
                }
                catch (Exception)
                {

                }
                return false;
            }
            public bool InsertExceptionIntoTable(string message, string stackTrace, int custid, int userid, string controllerName)
            {
            //try
            //{
            //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            //    {
            //        SqlCommand cmd = new SqlCommand();
            //        con.Open();
            //        cmd.Parameters.Clear();
            //        cmd.CommandTimeout = 0;
            //        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;
            //        cmd.Parameters.Add("@custid", SqlDbType.Int).Value = custid;
            //        cmd.Parameters.Add("@Exception_Message", SqlDbType.NVarChar).Value = message;
            //        cmd.Parameters.Add("@Stack_Trace", SqlDbType.NVarChar).Value = stackTrace;
            //        cmd.Parameters.Add("@Timestamp", SqlDbType.VarChar).Value = DateTime.Now;
            //        cmd.Parameters.Add("@Controller_Name", SqlDbType.NVarChar).Value = controllerName;
            //        InsertIntoTable("TBL_EWB_API_EXCEPTION_LOG", cmd, con);
            //        con.Close();
            //    }
            //}
            //catch (Exception)
            //{

            //}
            return false;
            }
        public bool UpdateGetResponse(string EinvoiceID, string Ackno, string Ackdate, string Irn, string SignedInvoice, string SignedQR, string Status)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.Clear();
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@AckNo", SqlDbType.NVarChar).Value = Ackno;
                    cmd.Parameters.Add("@AckDt", SqlDbType.DateTime).Value = Ackdate;
                    cmd.Parameters.Add("@Irn", SqlDbType.NVarChar).Value = Irn.ToUpper();
                    cmd.Parameters.Add("@SignedInvoice", SqlDbType.NVarChar).Value = SignedInvoice.Trim();
                    cmd.Parameters.Add("@SignedQRCode", SqlDbType.NVarChar).Value = SignedQR.Trim();
                    cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;
                    SQLHelper.UpdateGetTable("TBL_EINVOICE_GENERATION", "einvoiceid", EinvoiceID, cmd, con);
                    con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public DataSet GetDate(string date)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand dCmd = new SqlCommand("Usp_Retrieve_EINVOICE_GENERATION_ErrorData_Reprocess", conn);
                    dCmd.CommandType = CommandType.StoredProcedure;
                    dCmd.CommandTimeout = 0;
                    dCmd.Parameters.Add(new SqlParameter("@DocDt", date));
                    SqlDataAdapter da = new SqlDataAdapter(dCmd);
                    da.Fill(ds);
                    conn.Close();
                    return ds;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

       
        public bool UpdateAuthKeyDetails(string appKey, string encAppKey, string encPassword, string GSTIN)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.Clear();
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@AppKey", SqlDbType.NVarChar).Value = appKey.Trim();
                    cmd.Parameters.Add("@EncryptedAppKey", SqlDbType.NVarChar).Value = encAppKey.Trim();
                    cmd.Parameters.Add("@EncryptedPassword", SqlDbType.NVarChar).Value = encPassword.Trim();
                    SQLHelper.UpdateTable("TBL_AUTH_KEYS_EINVOICE", "GSTIN", GSTIN, cmd, con);
                    con.Close();
                    return true;
                }

            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}
