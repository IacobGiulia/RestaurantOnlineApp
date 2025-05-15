using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tema3_Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class AddedAllPics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "ID", "Path", "ProductID" },
                values: new object[,]
                {
                    { 3, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\cascavalpane.jpg", 3 },
                    { 4, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\ciorbaperisoare.jpg", 4 },
                    { 5, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\supagaluste.jpg", 5 },
                    { 6, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\ciorbaburta.jpg", 6 },
                    { 7, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\pieptpui.jpg", 7 },
                    { 8, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\pulpepui.jpg", 8 },
                    { 9, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\burger.jpg", 9 },
                    { 10, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\pastrav.jpg", 10 },
                    { 11, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\cartofiprajiti.jpg", 11 },
                    { 12, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\sarmale.jpg", 12 },
                    { 13, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\mamaliga.jpg", 13 },
                    { 14, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\inghetata.jpg", 14 },
                    { 15, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\papanasi.jpg", 15 },
                    { 16, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\tarta.jpg", 16 },
                    { 17, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\fanta.jpg", 17 },
                    { 18, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\pepsi.jpg", 18 },
                    { 19, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\apa.jpg", 19 },
                    { 20, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\espresso.jpg", 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 20);
        }
    }
}
