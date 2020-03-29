using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loginservice.Models
{
    public class ChangePassword
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Passwordsalt { get; set; }
    }
}
