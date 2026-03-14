using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnBridge_E_LearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class tup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherName",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherName",
                table: "Teachers");
        }
    }
}
