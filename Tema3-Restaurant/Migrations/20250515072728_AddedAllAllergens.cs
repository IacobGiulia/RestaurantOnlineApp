using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tema3_Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class AddedAllAllergens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductAllergen",
                columns: new[] { "AllergenID", "ProductID" },
                values: new object[,]
                {
                    { 2, 3 },
                    { 3, 3 },
                    { 4, 4 },
                    { 1, 5 },
                    { 2, 6 },
                    { 1, 9 },
                    { 5, 10 },
                    { 2, 14 },
                    { 1, 15 },
                    { 2, 15 },
                    { 3, 15 },
                    { 1, 16 },
                    { 2, 16 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 1, 9 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 5, 10 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 2, 14 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 1, 15 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 2, 15 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 3, 15 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 1, 16 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 2, 16 });
        }
    }
}
