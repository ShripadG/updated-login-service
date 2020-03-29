using System.ComponentModel.DataAnnotations;

namespace loginservice.Models
{
    public class LoginUsersAddRequest
    {      
        public string Email { get; set; }

        public string Name { get; set; }
        //public string Password { get; set; }
        //public string Passwordsalt { get; set; }
        public string Type { get; set; }
       
    }
}