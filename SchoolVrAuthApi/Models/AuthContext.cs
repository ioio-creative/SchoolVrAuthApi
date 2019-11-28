using Microsoft.EntityFrameworkCore;

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