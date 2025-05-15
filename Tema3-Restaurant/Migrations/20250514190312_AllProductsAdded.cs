using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tema3_Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class AllProductsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "ID", "Path", "ProductID" },
                values: new object[] { 1, "/Pics/omleta.jpg", 1 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Available", "CategoryID", "Name", "PortionQuantity", "Price", "TotalQuantity" },
                values: new object[,]
                {
                    { 17, true, 5, "Fanta", 100m, 5.00m, 3000m },
                    { 18, true, 5, "Pepsi", 100m, 5.00m, 3000m },
                    { 19, true, 5, "Apa plata", 100m, 3.00m, 3000m },
                    { 20, true, 5, "Espresso", 100m, 10.00m, 3000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 20);
        }
    }
}
