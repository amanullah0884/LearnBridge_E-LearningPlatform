using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnBridge_E_LearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Enrollments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_ApplicationUserId",
                table: "Enrollments",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_AspNetUsers_ApplicationUserId",
                table: "Enrollments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_AspNetUsers_ApplicationUserId",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_ApplicationUserId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Enrollments");
        }
    }
}
