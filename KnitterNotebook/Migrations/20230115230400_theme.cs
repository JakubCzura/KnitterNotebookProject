using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnitterNotebook.Migrations
{
    /// <inheritdoc />
    public partial class theme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Dark");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Default");
        }
    }
}