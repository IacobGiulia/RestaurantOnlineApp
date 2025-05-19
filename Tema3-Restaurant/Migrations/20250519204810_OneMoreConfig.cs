using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tema3_Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class OneMoreConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConfigurationApp",
                columns: new[] { "ID", "Key", "Value" },
                values: new object[] { 10, "PragMinimStoc", "1000" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConfigurationApp",
                keyColumn: "ID",
                keyValue: 10);
        }
    }
}
