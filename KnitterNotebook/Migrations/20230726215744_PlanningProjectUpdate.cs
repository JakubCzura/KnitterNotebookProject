using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnitterNotebook.Migrations
{
    /// <inheritdoc />
    public partial class PlanningProjectUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Needle_Projects_ProjectId",
                table: "Needle");

            migrationBuilder.DropForeignKey(
                name: "FK_Yarn_Projects_ProjectId",
                table: "Yarn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Yarn",
                table: "Yarn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Needle",
                table: "Needle");

            migrationBuilder.DropColumn(
                name: "PatternPdfPath",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "Yarn",
                newName: "Yarns");

            migrationBuilder.RenameTable(
                name: "Needle",
                newName: "Needles");

            migrationBuilder.RenameIndex(
                name: "IX_Yarn_ProjectId",
                table: "Yarns",
                newName: "IX_Yarns_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Needle_ProjectId",
                table: "Needles",
                newName: "IX_Needles_ProjectId");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Yarns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Yarns",
                table: "Yarns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Needles",
                table: "Needles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PatternPdfs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternPdfs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatternPdfs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatternPdfs_ProjectId",
                table: "PatternPdfs",
                column: "ProjectId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Needles_Projects_ProjectId",
                table: "Needles",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Yarns_Projects_ProjectId",
                table: "Yarns",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Needles_Projects_ProjectId",
                table: "Needles");

            migrationBuilder.DropForeignKey(
                name: "FK_Yarns_Projects_ProjectId",
                table: "Yarns");

            migrationBuilder.DropTable(
                name: "PatternPdfs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Yarns",
                table: "Yarns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Needles",
                table: "Needles");

            migrationBuilder.RenameTable(
                name: "Yarns",
                newName: "Yarn");

            migrationBuilder.RenameTable(
                name: "Needles",
                newName: "Needle");

            migrationBuilder.RenameIndex(
                name: "IX_Yarns_ProjectId",
                table: "Yarn",
                newName: "IX_Yarn_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Needles_ProjectId",
                table: "Needle",
                newName: "IX_Needle_ProjectId");

            migrationBuilder.AddColumn<string>(
                name: "PatternPdfPath",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Yarn",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Yarn",
                table: "Yarn",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Needle",
                table: "Needle",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Needle_Projects_ProjectId",
                table: "Needle",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Yarn_Projects_ProjectId",
                table: "Yarn",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
