using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnitterNotebook.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordResetTokenExpiresDate",
                table: "Users",
                newName: "PasswordResetTokenExpirationDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordResetTokenExpirationDate",
                table: "Users",
                newName: "PasswordResetTokenExpiresDate");
        }
    }
}
