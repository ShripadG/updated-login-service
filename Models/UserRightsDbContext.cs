using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loginservice.Models
{
    public partial class UserRightsDbContext : DbContext
    {
        public UserRightsDbContext()
        {
        }

        public UserRightsDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<UserRights> userRights { get; set; }
    }
}
