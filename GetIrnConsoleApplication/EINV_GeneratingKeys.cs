using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GetIrnConsoleApplication.DBoperations;
using Newtonsoft.Json;
using GetIrnConsoleApplication.Helper;
using GetIrnConsoleApplication.AppClasses;
using EInvoice_ASP_Services_Document;

namespace GetIrnConsoleApplication
{
    class EWB_GeneratingKeys
    {
        private static string EWBAuthResponse = "";
        private static SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        private static string binPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        static string Certificate_Path = "", AppKey = "", public_key = "", strPassword = "";


        private static string EncryptedAppKey = "", Username = "", EncryptedPassword = "", ErrorDescription = "";
        private static bool accesstoken;
        //public static void GeneratingEncryptedKeys(string strGSTINNo)
        public static void AuthenticateV103(string strGSTINNo,  out string OAuthStatus, out string OAuthError)
        {
            try
            {
                SQLHelper.EINV_AUTH_DataAdapter(strGSTINNo, out Username, out EncryptedAppKey, out EncryptedPassword, out accesstoken);


                //Attributes objEINVAuth = new Attributes();
                //objEINVAuth.data = new Data();
                //objEINVAuth.data.UserName = Username;
                //objEINVAuth.data.Password = EncryptedPassword;
                //objEINVAuth.data.Appkey = EncryptedAppKey;
                //objEINVAuth.data.ForceRefreshAccessToken = accesstoken;

                //string jsondata = JsonConvert.SerializeObject(objEINVAuth);

                Certificate_Path = binPath + "\\" + ConfigurationManager.AppSettings["EINV_Certificate"].ToString();
                // reading public key string from file
                using (var reader = File.OpenText(Certificate_Path))
                {
                    public_key = reader.ReadToEnd().Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\n", "");
                }

                byte[] _aeskey = EncryptionUtils.generateSecureKey();
                AppKey = Convert.ToBase64String(_aeskey);

                Attributes objEINVAuth = new Attributes();
                objEINVAuth.UserName = Username;
                objEINVAuth.Password = EncryptedPassword;
                objEINVAuth.Appkey = EncryptedAppKey;
                objEINVAuth.ForceRefreshAccessToken = accesstoken;
                string JsonData = JsonConvert.SerializeObject(objEINVAuth);

                byte[] encodejson = UTF8Encoding.UTF8.GetBytes(JsonData);
                string json = Convert.ToBase64String(encodejson);
                string payload = EncryptionUtils.Encrypt(json, public_key);
                AuthTokenAttr objEINV = new AuthTokenAttr();
                objEINV.Data = payload;

                string ReqData = JsonConvert.SerializeObject(objEINV);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["EINVOICE_AUTHENTICATE_V103"]);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("client_id", ConfigurationManager.AppSettings["Client_Id"]);
                httpWebRequest.Headers.Add("client_secret", ConfigurationManager.AppSettings["Client_Secret"]);
                httpWebRequest.Headers.Add("Gstin", strGSTINNo);
                httpWebRequest.Headers.Add("user_name", Username);
                httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", ConfigurationManager.AppSettings["Azure_SubscriptionKey_EINV"]);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(ReqData);
                    streamWriter.Flush();
                    streamWriter.Close();
                    try
                    {
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            EWBAuthResponse = result.ToString();
                        }
                    }
                    catch (WebException ex)
                    {
                        string str = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        Common.InsertExceptionIntoTable("Error" + str + "& gstin=" + strGSTINNo + "&webexception-accesstoken", ex.StackTrace, 0, 0, "api/Authentication");
                        OAuthStatus = "0";
                        OAuthError = ex.Message;
                    }
                }
                var odata = JsonConvert.DeserializeObject<EINV_Response_Attributes>(EWBAuthResponse);
                string status_response = odata.Status.ToString();
                string ErrorCode = "";
                if (status_response == "1")
                {
                    string authToken_response = odata.Data.AuthToken.ToString();
                    string sek_response = odata.Data.Sek.ToString();                   
                    DBOperation dob = new DBOperation();
                    dob.UpdateAuthTokenDetails(sek_response, authToken_response, strGSTINNo);

                }
                else
                {
                    var root = JsonConvert.DeserializeObject<EWB_Response_Attributes>(EWBAuthResponse);
                    string statusresponse = root.Status.ToString();
                    //strStatus = status_response;
                    string json1 = JsonConvert.SerializeObject(root.ErrorDetails);
                    ErrorCode = json1;
                    //var root = JsonConvert.DeserializeObject<EWB_Response_Attributes>(EWBAuthResponse);
                    ////var root_error = JsonConvert.DeserializeObject<EWB_Response_Attributes>(Helper.Base64Decode(root.error));
                    //ErrorDescription = DBUtility.GetErrorDescription(root_error.ErrorDetails);
                    ////Helper.EWB_Error_DataAdapter(root_error.errorcodes, out ErrorDescription);
                    //ErrorCode = root_error.errorcodes.Trim();
                }
                OAuthStatus = status_response;
                OAuthError = ErrorCode;
            }
            catch (Exception ex)
            {
                Common.InsertExceptionIntoTable("Error" + ex.Message, ex.StackTrace, 0, 0, "EInvoicing_ASPAPIController_AUTHENTICATE");
                OAuthStatus = "0";
                OAuthError = ex.Message;
            }
        }



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
                byte[] _aeskey = EncryptionUtil.generateSecureKey();
                AppKey = Convert.ToBase64String(_aeskey);

                //Encrypt AppKey with EInvoice public key
                string encAppKey = EncryptionUtil.Encrypt(_aeskey, public_key);

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
                string encPassword = EncryptionUtil.Encrypt(authTknDetails.password, public_key);

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
            catch (Exception e)
            {

            }
            return false;
        }
    }

 
}
