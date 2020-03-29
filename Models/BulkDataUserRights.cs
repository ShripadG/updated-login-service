using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loginservice.Models
{
    public class BulkDataUserRights
    {
        public string total_rows { get; set; }
        public string offset { get; set; }
        public BulkRowUserRights[] rows { get; set; }
    }
}
