using CalculatriceLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculatriceLibrary.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CalculationLog> CalculationLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // We leave this empty or call the base. 
            // The configuration should happen in Program.cs for maximum flexibility.
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Professional Touch: Define the Relationship
            // One User has Many CalculationLogs
            modelBuilder.Entity<CalculationLog>()
                .HasOne(c => c.User)
                .WithMany(u => u.Calculations)
                .HasForeignKey(c => c.UserId);

            SeedData.Configure(modelBuilder);
        }
    }
}