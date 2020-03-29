using System.ComponentModel.DataAnnotations;

namespace loginservice.Models
{
    public class LoginUsers
    {
        [Key]
        public string Id { get; set; }
        public string _id { get; set; }
        public string _rev { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        //public string Password { get; set; }
        //public string Passwordsalt { get; set; }
        public string Type { get; set; }
        
    }
}