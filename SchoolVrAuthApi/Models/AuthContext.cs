using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolVrAuthApi.Models
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<LicenseKey> LicenseKeys { get; set; }
        public DbSet<MacAddress> MacAddresses { get; set; }
    }
}
