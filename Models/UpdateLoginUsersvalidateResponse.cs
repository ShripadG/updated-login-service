using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace loginservice.Models
{
    public class UpdateLoginUsersvalidateResponse
    {
        [JsonProperty("ok")]
        public string ok { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("rev")]
        public string Rev { get; set; }
        

        [JsonProperty("logintypes")]
        public string logintypes { get; set; }

        [JsonProperty("EmployeeMaster")]
        public string EmployeeMaster { get; set; }

        [JsonProperty("ILCMaster")]
        public string ILCMaster { get; set; }

        [JsonProperty("ForcastMaster")]
        public string ForcastMaster { get; set; }

        [JsonProperty("Financials")]
        public string Financials { get; set; }

        [JsonProperty("Reports")]
        public string Reports { get; set; }

        [JsonProperty("LoginMaster")]
        public string LoginMaster { get; set; }
       
       
    }
}
