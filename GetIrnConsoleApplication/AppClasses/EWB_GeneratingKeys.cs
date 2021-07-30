using System;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GetIrnConsoleApplication.DBoperations;

namespace GetIrnConsoleApplication.AppClasses
{
    public class EWB_GeneratingKeys
    {
        private static SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        private static string binPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        static string Certificate_Path = "", AppKey = "", public_key = "", strPassword = "";

        //public static void GeneratingEncryptedKeys(string strGSTINNo)



        public static bool GeneratingEncryptedKeys(string strGSTINNo)
        {
            try
            {
                Certificate_Path = binPath + "\\" + ConfigurationManager.AppSettings["EINV_Certificate"].ToString();
                // reading public key string from file
                using (var reader = File.OpenText(Certificate_Path))
                {
                    public_key = reader.ReadToEnd().Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\n", "");
                }

                //Generation of app key.
                byte[] _aeskey = EncryptionUtils.generateSecureKey();
                AppKey = Convert.ToBase64String(_aeskey);

                //Encrypt AppKey with EInvoice public key
                string encAppKey = EncryptionUtils.Encrypt(_aeskey, public_key);

                DBOperation dbop = new DBOperation();
                AuthTokenDetailsModel authTknDetails = dbop.GetAuthTokenDetails(strGSTINNo);
                if (authTknDetails != null)
                {
                    if (String.IsNullOrEmpty(authTknDetails.password))
                    {

                    }
                }

                //    if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}
                //SqlDataAdapter adt = new SqlDataAdapter("Select TOP 1 * from TBL_AUTH_KEYS_EWB where GSTIN = '" + strGSTINNo + "'", con);
                //DataTable dt = new DataTable();
                //adt.Fill(dt);
                //if (dt.Rows.Count > 0)
                //{
                //    strPassword = dt.Rows[0]["Password"].ToString();
                //}

                //Encrypt Password with EWayBill public key
                string encPassword = EncryptionUtils.Encrypt(authTknDetails.password, public_key);

                //SqlCommand cmd = new SqlCommand();
                //cmd.Parameters.Clear();
                //cmd.CommandTimeout = 0;
                //cmd.Parameters.Add("@AppKey", SqlDbType.NVarChar).Value = AppKey.Trim();
                //cmd.Parameters.Add("@EncryptedAppKey", SqlDbType.NVarChar).Value = encAppKey.Trim();
                //cmd.Parameters.Add("@EncryptedPassword", SqlDbType.NVarChar).Value = encPassword.Trim();
                //SQLHelper.UpdateTable("TBL_AUTH_KEYS_EWB", "GSTIN", strGSTINNo, cmd, con);
                //con.Close();

                //Update Keys and password to database.
                DBOperation dbo = new DBOperation();

                dbo.UpdateAuthKeyDetails(AppKey, encAppKey, encPassword, strGSTINNo);
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

    }
}
