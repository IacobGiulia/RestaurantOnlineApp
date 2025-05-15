using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tema3_Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class NewProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Available", "CategoryID", "Name", "PortionQuantity", "Price", "TotalQuantity" },
                values: new object[,]
                {
                    { 10, true, 3, "Pastrav afumat", 300m, 40.00m, 4000m },
                    { 11, true, 3, "Cartofi prajiti", 200m, 10.00m, 8000m },
                    { 12, true, 3, "Sarmale", 350m, 35.00m, 6000m },
                    { 13, true, 3, "Mamaliga", 200m, 15.50m, 4000m },
                    { 14, true, 4, "Inghetata", 150m, 20.00m, 3000m },
                    { 15, true, 4, "Papanasi", 250m, 35.00m, 5000m },
                    { 16, true, 4, "Tarta cu fructe", 200m, 20.00m, 4000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 16);
        }
    }
}
