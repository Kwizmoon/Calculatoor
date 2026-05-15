using CalculatriceLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculatriceLibrary.Data
{

    /// Contexte EF Core pour la base SQLite de la calculatrice.
    /// Gère la connexion à la BD et la structure des tables.

    public class AppDbContext : DbContext
    {
        public DbSet<CalculationLog> CalculationLogs { get; set; }
        public DbSet<User> Users { get; set; }

        //This allows Program.cs to pass in the In-Memory settings
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // for local development if needed, but the constructor above is the priority
        public AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // ONLY use SQLite if nothing else was configured in Program.cs
            if (!optionsBuilder.IsConfigured)
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                var dbPath = System.IO.Path.Join(path, "calculatrice_tp1.db");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData.Configure(modelBuilder);
        }
    }
}