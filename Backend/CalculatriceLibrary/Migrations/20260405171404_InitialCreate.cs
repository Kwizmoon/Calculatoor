using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalculatriceLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalculationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Expression = table.Column<string>(type: "TEXT", nullable: true),
                    Operand1 = table.Column<double>(type: "REAL", nullable: true),
                    Operand2 = table.Column<double>(type: "REAL", nullable: true),
                    Operator = table.Column<string>(type: "TEXT", nullable: false),
                    Result = table.Column<double>(type: "REAL", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculationLogs_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "CalculationLogs",
                columns: new[] { "Id", "CreatedAt", "Expression", "Operand1", "Operand2", "Operator", "Result", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 13, 9, 0, 0, 0, DateTimeKind.Unspecified), "10+5", 10.0, 5.0, "+", 15.0, null },
                    { 2, new DateTime(2025, 1, 1, 9, 1, 0, 0, DateTimeKind.Unspecified), "100-37", 100.0, 37.0, "-", 63.0, null },
                    { 3, new DateTime(2025, 1, 1, 9, 2, 0, 0, DateTimeKind.Unspecified), "7*8", 7.0, 8.0, "*", 56.0, null },
                    { 4, new DateTime(2025, 1, 1, 9, 3, 0, 0, DateTimeKind.Unspecified), "144/12", 144.0, 12.0, "/", 12.0, null },
                    { 5, new DateTime(2025, 1, 1, 9, 4, 0, 0, DateTimeKind.Unspecified), "9^2", 9.0, null, "pow2", 81.0, null },
                    { 6, new DateTime(2024, 2, 1, 9, 5, 0, 0, DateTimeKind.Unspecified), "2^10", 2.0, 10.0, "powN", 1024.0, null },
                    { 7, new DateTime(2024, 5, 20, 9, 6, 0, 0, DateTimeKind.Unspecified), "sqrt(256)", 256.0, null, "sqrt", 16.0, null }
                });

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
                name: "User");
        }
    }
}
