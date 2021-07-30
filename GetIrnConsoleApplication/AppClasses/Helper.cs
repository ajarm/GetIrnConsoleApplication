using GetIrnConsoleApplication.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GetIrnConsoleApplication.AppClasses
{
    public static class Helper

    {
        public static string HMAC_SHA256(string data, string EK)
        {
            UTF8Encoding enc = new UTF8Encoding();
            byte[] secretKey = enc.GetBytes(EK);
            HMACSHA256 hmac = new HMACSHA256(secretKey);
            hmac.Initialize();
            byte[] bytes = enc.GetBytes(data);
            byte[] rawHmac = hmac.ComputeHash(bytes);
            string result = Convert.ToBase64String(rawHmac);
            return result;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string openFileToString(byte[] _bytes)
        {
            string file_string = "";

            for (int i = 0; i < _bytes.Length; i++)
            {
                file_string += (char)_bytes[i];
            }

            return file_string;
        }

        public static string getHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

        public static string Encrypt(string plainText, byte[] keyBytes)
        {
            byte[] dataToEncrypt = UTF8Encoding.UTF8.GetBytes(plainText);

            AesManaged tdes = new AesManaged();

            tdes.KeySize = 256;
            tdes.BlockSize = 128;
            tdes.Key = keyBytes;// Encoding.ASCII.GetBytes(key);
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform crypt = tdes.CreateEncryptor();
            byte[] cipher = crypt.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            tdes.Clear();
            return Convert.ToBase64String(cipher, 0, cipher.Length);
        }

        public static string hmac(byte[] deCipher, string message)
        {
            string EK_result = Convert.ToBase64String(deCipher);
            Console.WriteLine(EK_result);
            var EK = Convert.FromBase64String(EK_result);

            string gen_hmac = "";
            //string message = data;

            using (var HMACSHA256 = new HMACSHA256(EK))
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                byte[] hashmessage = HMACSHA256.ComputeHash(data);
                gen_hmac = Convert.ToBase64String(hashmessage);
            }
            return gen_hmac;
        }

        public static void GSTR_DataAdapter(string strGSTINNo, out string strUsername, out string strAppKey, out string strEncryptedSek, out string strAuthToken)
        {
            string Username = "", AppKey = "", EncryptedSEK = "", AuthToken = "";
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlDataAdapter adt = new SqlDataAdapter("Select * from TBL_AUTH_KEYS where AuthorizationToken = '" + strGSTINNo + "'", con);
                DataTable dt = new DataTable();
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Username = dt.Rows[0]["Username"].ToString();
                    AppKey = dt.Rows[0]["AppKey"].ToString();
                    EncryptedSEK = dt.Rows[0]["EncryptedSEK"].ToString();
                    AuthToken = dt.Rows[0]["AuthToken"].ToString();
                }
                con.Close();
            }
            strUsername = Username;
            strAppKey = AppKey;
            strEncryptedSek = EncryptedSEK;
            strAuthToken = AuthToken;
        }

        public static void EWB_Error_DataAdapter(string strErrorCode, out string strErrorDescription)
        {
            string ErrorDescription = "";
            strErrorCode = strErrorCode.Trim().TrimEnd(',');
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                if (!string.IsNullOrEmpty(strErrorCode))
                {
                    SqlDataAdapter adt = new SqlDataAdapter("Select * from TBL_EWB_ERROR_CODES where ErrorCode in (" + strErrorCode + ")", con);
                    DataTable dt = new DataTable();
                    adt.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ErrorDescription += dt.Rows[i]["ErrorDescription"].ToString().Trim() + ",";
                        }
                    }
                    con.Close();
                }
            }
            ErrorDescription = ErrorDescription.Trim().TrimEnd(',');
            strErrorDescription = ErrorDescription;
        }

        public static void EINV_AUTH_DataAdapter(string strGSTINNo, out string strUsername, out string strEncAppKey, out string strEncpassword, out bool strForceRefreshToken)
        {
            string Username = "", EncryptedAppKey = "", EncryptedPassword = "";
            bool ForceRefreshToken = false;
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlDataAdapter adt = new SqlDataAdapter("Select * from TBL_AUTH_KEYS_EINVOICE where GSTIN = '" + strGSTINNo + "'", con);
                DataTable dt = new DataTable();
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Username = dt.Rows[0]["Username"].ToString();
                    EncryptedAppKey = dt.Rows[0]["EncryptedAppKey"].ToString();
                    EncryptedPassword = dt.Rows[0]["EncryptedPassword"].ToString();
                    //ForceRefreshToken = (bool)dt.Rows[0]["Force_Refresh_Accesstoken"];
                }
                con.Close();
            }
            strUsername = Username;
            strEncAppKey = EncryptedAppKey;
            strEncpassword = EncryptedPassword;
            strForceRefreshToken = false;
        }

        public static void EWB_DataAdapter(string strGSTINNo, out string strUsername, out string strAppKey, out string strEncryptedSek, out string strAuthToken)
        {
            string Username = "", AppKey = "", EncryptedSEK = "", AuthToken = "";
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlDataAdapter adt = new SqlDataAdapter("Select * from TBL_AUTH_KEYS_EWB where GSTIN = '" + strGSTINNo + "'", con);
                DataTable dt = new DataTable();
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Username = dt.Rows[0]["Username"].ToString();
                    AppKey = dt.Rows[0]["AppKey"].ToString();
                    EncryptedSEK = dt.Rows[0]["EncryptedSEK"].ToString();
                    AuthToken = dt.Rows[0]["AuthToken"].ToString();
                }
                con.Close();
            }
            strUsername = Username;
            strAppKey = AppKey;
            strEncryptedSek = EncryptedSEK;
            strAuthToken = AuthToken;
        }

        public static void InsertAuditLog(string strUserId, string strUsername, string strMessage, string strException)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@FK_Audit_User_ID", SqlDbType.Int).Value = strUserId;
                cmd.Parameters.Add("@FK_Audit_Username", SqlDbType.VarChar).Value = strUsername;
                cmd.Parameters.Add("@Audit_DateTime", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@Audit_Message", SqlDbType.VarChar).Value = strMessage;
                cmd.Parameters.Add("@Audit_Exception", SqlDbType.VarChar).Value = strException;
                SQLHelper.InsertIntoTable("TBL_Audit_Log", cmd, con);
                con.Close();
            }
        }

        #region "Insert and Updating Otherparty EWB-GET API"
        public static void Insert_Update_Otherpary(string ewbNo, string ewbDate, string genMode, string genGstin, string docNo, string docDate, string fromGstin, string fromTrdName, string togstin, string toTrdName, string hsncode, string hsndesc, string status, string rejectStatus, decimal totalinvvalue, int CustId, int UserId)
        {
            SqlConnection Con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            try
            {
                if (Con.State == ConnectionState.Closed)
                {
                    Con.Open();
                }

                SqlCommand cmd = new SqlCommand("usp_Insert_Update_EWB_OtherParty", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ewbNo", ewbNo));
                cmd.Parameters.Add(new SqlParameter("@ewbDate", ewbDate));
                cmd.Parameters.Add(new SqlParameter("@genMode", genMode));
                cmd.Parameters.Add(new SqlParameter("@genGstin", genGstin));
                cmd.Parameters.Add(new SqlParameter("@docNo", docNo));
                cmd.Parameters.Add(new SqlParameter("@docDate", docDate));
                cmd.Parameters.Add(new SqlParameter("@fromGstin", fromGstin));
                cmd.Parameters.Add(new SqlParameter("@fromTrdName", fromTrdName));
                cmd.Parameters.Add(new SqlParameter("@toGstin", togstin));
                cmd.Parameters.Add(new SqlParameter("@toTrdName", toTrdName));
                cmd.Parameters.Add(new SqlParameter("@totInvValue", totalinvvalue));
                cmd.Parameters.Add(new SqlParameter("@hsnCode", hsncode));
                cmd.Parameters.Add(new SqlParameter("@hsnDesc", hsndesc));
                cmd.Parameters.Add(new SqlParameter("@status", status));
                cmd.Parameters.Add(new SqlParameter("@rejectStatus", rejectStatus));
                cmd.Parameters.Add(new SqlParameter("@CreatedBy", UserId));
                cmd.Parameters.Add(new SqlParameter("@CustId", CustId));

                cmd.ExecuteNonQuery();

                Con.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion

    }
}
