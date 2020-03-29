using System.ComponentModel.DataAnnotations;

namespace loginservice.Models
{
         public class UserRights
        {
            public string _id { get; set; }
            public string _rev { get; set; }
            public string ok { get; set; }
            public string logintypes { get; set; }
            public string EmployeeMaster { get; set; }
            public string ForcastMaster { get; set; }
            public string Reports { get; set; }
            public string ILCMaster { get; set; }
        
            public string Financials { get; set; }        
            public string LoginMaster { get; set; }
       

       

    }
}
