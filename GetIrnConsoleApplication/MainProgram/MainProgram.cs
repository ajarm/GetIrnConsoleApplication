using EInvoice_ASP_Services_Document;
using GetIrnConsoleApplication.DBoperations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetIrnConsoleApplication.MainProgram
{
    class MainProgram
    {

        public static bool Execute()
        {
            string _gstin = "";
            string DocTyp = "";
            string DocNo = "";
            string DocDt = "";
            string SupTyp = "";
            object _OInvoice = "";
            object _OQRCode = "";
            object _info = "";
            string Errirn = "";
            string OErrorCode = "";
            string OErrorMsg = "", OAuthStatus="", OAuthError="";


            var objResponse = new OutputResponse();
            try
            {
                string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
           
                DataSet ds = new DataSet();
                DBOperation dBOperation = new DBOperation();
                ds = dBOperation.GetDate(date);

                int count = ds.Tables[0].Rows.Count;
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Errirn = item.IsNull("ErrIrn") ? "" : item["ErrIrn"].ToString();
                    _gstin = item.IsNull("SGstin") ? "" : item["SGstin"].ToString();
                    DocTyp = item.IsNull("Doctype") ? "" : item["Doctype"].ToString();
                    DocNo = item.IsNull("DocNo") ? "" : item["DocNo"].ToString();
                    DocDt = item.IsNull("DocDt") ? "" : item["DocDt"].ToString();
                    SupTyp = item.IsNull("SupTyp") ? "" : item["SupTyp"].ToString();
                    try
                    {
                        //EWB_GeneratingKeys.AuthenticationV103(_gstin, _subscriptionkey, out OAuthStatus, out OAuthError, cModel.cust_id, cModel.user_id);
                    }
                    catch (Exception ex)
                    {
                        //objResponse.Status = "0";
                        //objResponse.data = "";
                        //objResponse.DocNo = "";
                        //objResponse.ErrorRecordsStatus = "0";
                        //objResponse.ErrorRecords = "";
                        //string j = "[{\"ErrorCode\":\"WeP300\",\"ErrorMessage\":\"Authentication Error\"}]";
                        //var json = JsonConvert.DeserializeObject(j);
                        //JArray jarr = new JArray(json);
                        //var header = JArray.Parse(jarr[0].ToString());
                        //objResponse.ErrorDetails = header;
                        //objResponse.InfoDtls = "";
                        ////Common.InsertExceptionIntoTable("Error" + ex.Message + "& gstin=" + _gstin + "", ex.StackTrace, 0, 0, "api/GetEwayGSPAPI/SendRequest/Generate Auth Token - 1");
                        //Common.InsertExceptionIntoTable("Error" + ex.Message + "& gstin=" + _gstin + "&ewbno=" + _IRN, ex.StackTrace, cModel.cust_id, cModel.user_id, "api/GetEwayGSPAPI/SendRequest/Generate Auth Token - 1");
                        //return Request.CreateResponse(HttpStatusCode.OK, new { OutputResponse = objResponse }, Configuration.Formatters.JsonFormatter);
                    }
                    getIRN get = new getIRN();
                    get.GET_EINVOICEV103(Errirn, _gstin, DocNo, DocDt, DocTyp, SupTyp, out  OErrorCode, out OErrorMsg);
                    if (OErrorCode.Contains("108") || OErrorCode.Contains("238") || OErrorCode.Contains("WeP1000") || OErrorCode.Equals("WeP1000") || OErrorMsg.Equals("The remote server returned an error: (500) Internal Server Error."))
                    {
                        try
                        {
                            EWB_GeneratingKeys.GeneratingEncryptedKeys(_gstin);
                            //EWB_GeneratingKeys.AuthenticationError(_gstin, _subscriptionkey, out OAuthStatus, out OAuthError);
                            EWB_GeneratingKeys.AuthenticateV103(_gstin,  out OAuthStatus, out OAuthError);

                        }
                        catch (Exception ex)
                        {
                            //Common.InsertExceptionIntoTable("Error" + ex.Message + "& gstin=" + _gstin + "", ex.StackTrace, 0, 0, "api/GetEwayGSPAPI/SendRequest/Generate Auth Token - 2");
                        }

                        //EWB_GetAPI.GET_EWAYBILL(_ewayNumber, _gstin, strUserId, strCustId,
                        //         strUserName, out OStatus, out OResponse, out OError);
                        get.GET_EINVOICEV103(Errirn, _gstin, DocNo, DocDt, DocTyp, SupTyp, out OErrorCode, out OErrorMsg);

                    }
                    count--;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}
