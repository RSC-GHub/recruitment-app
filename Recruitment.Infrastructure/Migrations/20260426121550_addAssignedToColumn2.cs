using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recruitment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addAssignedToColumn2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_AssignedToUserId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_AssignedToUserId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "Applications");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_AssignedTo",
                table: "Applications",
                column: "AssignedTo");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_AssignedTo",
                table: "Applications",
                column: "AssignedTo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_AssignedTo",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_AssignedTo",
                table: "Applications");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToUserId",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_AssignedToUserId",
                table: "Applications",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_AssignedToUserId",
                table: "Applications",
                column: "AssignedToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
