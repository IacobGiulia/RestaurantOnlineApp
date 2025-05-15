using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tema3_Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class PicPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 1,
                column: "Path",
                value: "Pics/omleta.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "ID",
                keyValue: 1,
                column: "Path",
                value: "/Pics/omleta.jpg");
        }
    }
}
