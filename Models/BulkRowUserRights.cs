using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loginservice.Models
{
    public class BulkRowUserRights
    {
        public string id { get; set; }
        public string rev { get; set; }
        public string ok { get; set; }
        public string key { get; set; }
        public BulkRowRevision value { get; set; }
        public UserRights doc { get; set; }
    }
}
