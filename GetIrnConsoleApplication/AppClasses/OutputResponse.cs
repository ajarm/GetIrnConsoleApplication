using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace EInvoice_ASP_Services_Document
{
    public class OutputResponse
    {
        // --------------- EWAYBILL GSP API RESPONSE PARAMETERS STARTS -------------------- //

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DocNo { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string data { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Alert { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorRecordsStatus { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object ErrorRecords { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object ErrorDetails { get; set; }



        //[DataMember]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public ErrorDetails ErrorDetails { get; set; }


        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AckNo { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AckDt { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Irn { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SignedInvoice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SignedQRCode { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string status { get; set; }





        //GSTIN FIELDS
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Gstin { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TradeName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LegalName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AddrBnm { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AddrBno { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AddrFlno { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AddrSt { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AddrLoc { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? StateCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? AddrPncd { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TxpType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BlkStatus { get; set; }
















        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VehicleNo { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VehUpdDate { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string fromGstin { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string toGstin { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string transporterId { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Success { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string authtoken { get; set; }


        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sek { get; set; }



        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string hmac { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string rek { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string error { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ReferenceId { get; set; }












        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string groupNo { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string VehAddedDate { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string createdDate { get; set; }

        // --------------- EWAYBILL GSP API RESPONSE PARAMETERS ENDS -------------------- //

        //--------------- EWAYBILL ASP API RESPONSE PARAMETERS STARTS-------------------- //

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorCodes { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ErrMsg { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorDescription { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? EwbNo { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EwbDt { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EwbValidTill { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Remarks { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cEwbNo { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cEwbDate { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ewbNo { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string extendedDate { get; set; }


        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CancelDate { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? ewayBillNo { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cancelDate { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ewbRejectedDate { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string vehUpdDate { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedDate { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string docDate { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object InfoDtls { get; set; }
        // --------------- EWAYBILL ASP API RESPONSE PARAMETERS ENDS -------------------- //
    }

    public class error
    {
        public string errorCodes { get; set; }

    }

    public class ErrorDetails
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}