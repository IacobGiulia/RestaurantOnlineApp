using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tema3_Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class CompletedConfigAppTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConfigurationApp",
                columns: new[] { "ID", "Key", "Value" },
                values: new object[,]
                {
                    { 8, "ProcentReducereComanda", "10" },
                    { 9, "ValoareMinimaPentruReducere", "150" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConfigurationApp",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ConfigurationApp",
                keyColumn: "ID",
                keyValue: 9);
        }
    }
}
