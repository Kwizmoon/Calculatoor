using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalculatriceLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgresFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalculationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Expression = table.Column<string>(type: "text", nullable: false),
                    Result = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculationLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            /*
            migrationBuilder.InsertData(
                table: "CalculationLogs",
                columns: new[] { "Id", "CreatedAt", "Expression", "Result", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 13, 9, 0, 0, 0, DateTimeKind.Utc), "10+5", "15", 0 },
                    { 2, new DateTime(2025, 1, 1, 9, 1, 0, 0, DateTimeKind.Utc), "100-37", "63", 0 },
                    { 3, new DateTime(2025, 1, 1, 9, 2, 0, 0, DateTimeKind.Utc), "7*8", "56", 0 },
                    { 4, new DateTime(2025, 1, 1, 9, 3, 0, 0, DateTimeKind.Utc), "144/12", "12", 0 },
                    { 5, new DateTime(2025, 1, 1, 9, 4, 0, 0, DateTimeKind.Utc), "9^2", "81", 0 },
                    { 6, new DateTime(2024, 2, 1, 9, 5, 0, 0, DateTimeKind.Utc), "2^10", "1024", 0 },
                    { 7, new DateTime(2024, 5, 20, 9, 6, 0, 0, DateTimeKind.Utc), "sqrt(256)", "16", 0 }
                });
            */
            migrationBuilder.CreateIndex(
                name: "IX_CalculationLogs_UserId",
                table: "CalculationLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculationLogs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
