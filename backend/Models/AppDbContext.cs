using Microsoft.EntityFrameworkCore;
using InsuranceApi.Models;

namespace InsuranceApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserPolicy> UserPolicies { get; set; }
        public DbSet<Policy> Policies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPolicy>()
                .ToTable("UserPolicy")
                .HasKey(up => new { up.UserID, up.PolicyID });

            modelBuilder.Entity<Policy>()
                .ToTable("Policy")
                .HasKey(p => p.PolicyID);  // Assuming string as key

            modelBuilder.Entity<UserProfile>()
                .ToTable("UserProfile")
                .HasKey(u => u.UserID);

            base.OnModelCreating(modelBuilder);

        }

    }
}
