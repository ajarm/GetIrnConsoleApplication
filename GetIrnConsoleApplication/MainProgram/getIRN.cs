using GetIrnConsoleApplication.AppClasses;
using GetIrnConsoleApplication.DBoperations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GetIrnConsoleApplication.MainProgram
{
    public class getIRN
    {
        public void GET_EINVOICEV103(string strIRNNo, string GSTINNo, string DocNo, string Docdt, string Doctype, string Suptyp, out string OErrorCode, out string OErrorMsg)
        {
            string Username = "";
            string AuthToken = "";
            string AppKey = "";
            string EncryptedSEK = "";
            string GETResponse = "";
            string URL = "";
            var headervalue = "";
            var headercode = "";
            object strResponse = "";
            object Inv = "", QRCODE = "";
            string strAckNo = "", strIRN = "", strAckDate = "", strSignedInvoice = "", strSignedQR = "", strstatus = "", strStatus = "0", strResponsemsg = "";
            EWB_Response_Attributes EWB_Data = null;
            var errmsg = "";
            object Oinfodtls = "";
            try
            {
                DBUtility.GetGSTINAuthenticationDetails(GSTINNo, out Username, out AppKey, out EncryptedSEK, out AuthToken);

                string decrypted_appkey = AppKey;
                string sek = EncryptionUtils.DecryptBySymmerticKey(EncryptedSEK, Convert.FromBase64String(decrypted_appkey));

                URL = ConfigurationManager.AppSettings["EINVOICE_GETIRN_V103"] + "?IRN=" + strIRNNo;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "GET";
                httpWebRequest.Accept = "application/json; charset=utf-8";
                httpWebRequest.Headers.Add("AuthToken", AuthToken);
                httpWebRequest.Headers.Add("gstin", GSTINNo);
                httpWebRequest.Headers.Add("user_name", Username);
                httpWebRequest.Headers.Add("client_id", ConfigurationManager.AppSettings["Client_Id"]);
                httpWebRequest.Headers.Add("client_secret", ConfigurationManager.AppSettings["Client_Secret"]);
                httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", ConfigurationManager.AppSettings["Azure_SubscriptionKey_EINV"]);
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        GETResponse = result.ToString();
                    }
                }
                catch (WebException ex)
                {
                    string str = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    OErrorCode = "";
                    OErrorMsg =ex.Message;
 
                    return;
                }
                EWB_Data = JsonConvert.DeserializeObject<EWB_Response_Attributes>(GETResponse);
                string status_response = EWB_Data.Status.ToString();
                strStatus = status_response;
                string json1 = JsonConvert.SerializeObject(EWB_Data.ErrorDetails);
                var arr = EWB_Data.ErrorDetails;
                var infodtls = EWB_Data.InfoDtls;
                Oinfodtls = infodtls;
                if (arr != null)
                {
                    errmsg = arr[0]["ErrorMessage"];
                }

                if (status_response == "1")
                {

                    string data_response = EWB_Data.data.ToString();
                    string Decryptjson = EncryptionUtils.DecryptBySymmerticKey(data_response, Convert.FromBase64String(sek));
                    Decryptjson = Helper.Helper.Base64Decode(Decryptjson);
                    var finaldata = JsonConvert.DeserializeObject<EWB_Response_Attributes>(Decryptjson);
                    string Ackno = finaldata.AckNo;
                    strAckNo = Ackno;
                    string Ackdate = finaldata.AckDt;
                    strAckDate = Ackdate;
                    strAckDate = strAckDate.Replace('.', ':');
                    string IRN = finaldata.Irn;
                    strIRN = IRN;
                    string Signedinvoice = finaldata.SignedInvoice;
                    strSignedInvoice = Signedinvoice;
                    string SignedQR = finaldata.SignedQRCode;
                    strSignedQR = SignedQR;
                    string status = finaldata.Status;
                    strstatus = status;

                    DBOperation dob = new DBOperation();

                    string einvid = dob.RetrieveEinvoiceId(DocNo, Docdt, Doctype, Suptyp);

                    dob.UpdateGetResponse(einvid, strAckNo, strAckDate, IRN, Signedinvoice, SignedQR, status);
                    strResponse = "IRN Generated Successfully.  No - " + strIRN;
                }
                else if (status_response == "0" && errmsg == "Invalid Token")
                {
                    strResponsemsg = "WeP1000";

                }
                else
                {
                    if (json1 != "null")
                    {
                        JArray errormsg = JArray.Parse(json1);
                        int a = errormsg.Count;
                        for (int i = 0; i < a; i++)
                        {
                            var header = JObject.Parse(errormsg[i].ToString());
                            headervalue = header["ErrorMessage"].ToString();
                            headercode = header["ErrorCode"].ToString();
                            DBOperation dob = new DBOperation();
                            string result1 = dob.RetrieveEinvoiceId(DocNo, Docdt, Doctype, Suptyp);

                            dob.UpdateErrorResponse(result1, headervalue, headercode, "GetIRNV103");
                        }
                    }
                    var str = json1;
                    var str1 = JsonConvert.DeserializeObject(str);
                    strResponse = str1;
                    strResponsemsg = headercode;

                }


            }
            catch (Exception ex)
            {
                strResponse = ex.Message;
                Common.InsertExceptionIntoTable("Error" + ex.Message + "& gstin=" + GSTINNo + "&ewbno=" + strIRNNo + "&webexception", ex.StackTrace, 0, 0, "api/EWBGETEWAYBILL/SendRequest");
                
            }

            OErrorMsg = strResponsemsg;
            OErrorCode = strResponsemsg;

        }
    }
}
