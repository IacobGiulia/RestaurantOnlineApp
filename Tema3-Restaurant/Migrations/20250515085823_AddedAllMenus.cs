using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tema3_Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class AddedAllMenus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MenuProduct",
                columns: new[] { "MenuID", "ProductID", "Quantity" },
                values: new object[,]
                {
                    { 3, 8, 200m },
                    { 3, 11, 200m },
                    { 3, 18, 100m },
                    { 4, 4, 100m },
                    { 4, 12, 150m },
                    { 4, 13, 150m },
                    { 4, 19, 100m },
                    { 5, 10, 200m },
                    { 5, 11, 200m },
                    { 5, 17, 100m },
                    { 6, 9, 350m },
                    { 6, 11, 200m },
                    { 6, 18, 100m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 3, 8 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 3, 11 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 3, 18 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 4, 12 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 4, 13 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 4, 19 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 5, 10 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 5, 11 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 5, 17 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 6, 9 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 6, 11 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 6, 18 });
        }
    }
}
