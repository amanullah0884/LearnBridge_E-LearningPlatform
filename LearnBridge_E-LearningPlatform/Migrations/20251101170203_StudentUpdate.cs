using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnBridge_E_LearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class StudentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "Date");

            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Users",
                newName: "CreatedAt");
        }
    }
}
