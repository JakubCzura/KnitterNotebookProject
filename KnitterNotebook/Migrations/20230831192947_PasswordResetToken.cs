using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace KnitterNotebook.Migrations;

/// <inheritdoc />
public partial class PasswordResetToken : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "PasswordResetToken",
            table: "Users",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "PasswordResetTokenExpiresDate",
            table: "Users",
            type: "datetime2",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PasswordResetToken",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "PasswordResetTokenExpiresDate",
            table: "Users");
    }
}