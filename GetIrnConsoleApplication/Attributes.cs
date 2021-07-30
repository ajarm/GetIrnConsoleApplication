using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace GetIrnConsoleApplication
{
    [Serializable]
    [DataContract]
    public class Attributes
    {
        public Attributes()
        {

        }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Data data { get; set; }
        //[DataMember]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public string UserName { get; set; }

        //[DataMember]
        //public string Password { get; set; }

        //[DataMember]
        //public string Appkey { get; set; }

        //[DataMember]
        //public bool ForceRefreshAccessToken { get; set; }


    }
    [Serializable]
    [DataContract]
    public class Data
    {

        //[DataMember]
        //public string action { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Appkey { get; set; }

        [DataMember]
        public bool ForceRefreshAccessToken { get; set; }
        //[DataMember]
        //public string hmac { get; set; }

        //[DataMember]
        //public string rek { get; set; }
    }


    public class EINV_Response_Attributes
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public data Data { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
    }
    public class data
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ClientId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AuthToken { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Sek { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TokenExpiry { get; set; }
    }
}