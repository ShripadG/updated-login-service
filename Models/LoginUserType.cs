using System.ComponentModel.DataAnnotations;

namespace loginservice.Models
{
    public class LoginUserType
    {
        public string _id { get; set; }
        public string _rev { get; set; }
        public string LoginType { get; set; }      
    }
}