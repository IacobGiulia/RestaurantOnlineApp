﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tema3_Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class FirstAllergens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductAllergen",
                columns: new[] { "AllergenID", "ProductID" },
                values: new object[,]
                {
                    { 3, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "ProductAllergen",
                keyColumns: new[] { "AllergenID", "ProductID" },
                keyValues: new object[] { 3, 2 });
        }
    }
}
