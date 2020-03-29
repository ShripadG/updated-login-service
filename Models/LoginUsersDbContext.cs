using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loginservice.Models
{
    public partial class LoginUsersDbContext : DbContext
    {
        public LoginUsersDbContext()
        {
        }

        public LoginUsersDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<LoginUsers> LoginUsers { get; set; }
    }
}
