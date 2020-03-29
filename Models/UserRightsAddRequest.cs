using System.ComponentModel.DataAnnotations;

namespace loginservice.Models
{
    public class UserRightsAddRequest
    {
        public string logintypes { get; set; }
        public string EmployeeMaster { get; set; }
        public string ForcastMaster { get; set; }
        public string Reports { get; set; }
        public string ILCMaster { get; set; }

        public string Financials { get; set; }
        public string LoginMaster { get; set; }
    }
}