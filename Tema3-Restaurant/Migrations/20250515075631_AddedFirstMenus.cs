using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tema3_Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class AddedFirstMenus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "ID", "Available", "CategoryID", "Name" },
                values: new object[,]
                {
                    { 1, true, 1, "Meniu Mic Dejun" },
                    { 2, true, 3, "Meniu Piept de Pui" },
                    { 3, true, 3, "Meniu Pulpe de Pui" },
                    { 4, true, 3, "Meniu Traditional" },
                    { 5, true, 3, "Meniu Pastrav" },
                    { 6, true, 3, "Meniu Burger" }
                });

            migrationBuilder.InsertData(
                table: "MenuProduct",
                columns: new[] { "MenuID", "ProductID", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 150m },
                    { 1, 2, 150m },
                    { 1, 20, 100m },
                    { 2, 7, 200m },
                    { 2, 11, 200m },
                    { 2, 17, 100m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 1, 20 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 2, 7 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 2, 11 });

            migrationBuilder.DeleteData(
                table: "MenuProduct",
                keyColumns: new[] { "MenuID", "ProductID" },
                keyValues: new object[] { 2, 17 });

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "ID",
                keyValue: 2);
        }
    }
}
