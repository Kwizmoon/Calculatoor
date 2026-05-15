using CalculatriceLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculatriceLibrary.Data
{

    /// Classe séparée pour définir les données de départ (seeding).
    /// Ces données sont insérées automatiquement lors de la première migration.

    public static class SeedData
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalculationLog>().HasData(
                new CalculationLog
                {
                    Id = 1,
                    Expression = "10+5",
                    Result = "15",
                    CreatedAt = new DateTime(2026, 3, 13, 9, 0, 0)
                },
                new CalculationLog
                {
                    Id = 2,
                    Expression = "100-37",
                    Result = "63",
                    CreatedAt = new DateTime(2025, 1, 1, 9, 1, 0)
                },
                new CalculationLog
                {
                    Id = 3,
                    Expression = "7*8",
                    Result = "56",
                    CreatedAt = new DateTime(2025, 1, 1, 9, 2, 0)
                },
                new CalculationLog
                {
                    Id = 4,
                    Expression = "144/12",
                    Result = "12",
                    CreatedAt = new DateTime(2025, 1, 1, 9, 3, 0)
                },
                new CalculationLog
                {
                    Id = 5,
                    Expression = "9^2",
                    Result = "81",
                    CreatedAt = new DateTime(2025, 1, 1, 9, 4, 0)
                },
                new CalculationLog
                {
                    Id = 6,
                    Expression = "2^10",
                    Result = "1024",
                    CreatedAt = new DateTime(2024, 2, 1, 9, 5, 0)
                },
                new CalculationLog
                {
                    Id = 7,
                    Expression = "sqrt(256)",
                    Result = "16",
                    CreatedAt = new DateTime(2024, 5, 20, 9, 6, 0)
                }
            );
        }
    }
}