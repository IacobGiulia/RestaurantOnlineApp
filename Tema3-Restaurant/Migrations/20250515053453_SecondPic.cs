using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tema3_Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class SecondPic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "ID", "Path", "ProductID" },
                values: new object[] { 2, "C:\\Users\\giuli\\Documents\\MVP\\Tema3-Restaurant\\Tema3-Restaurant\\Pics\\clatita.jpg", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 2);
        }
    }
}
