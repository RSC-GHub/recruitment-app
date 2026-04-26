using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recruitment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addAssignedToColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAt",
                table: "Applications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedTo",
                table: "Applications",
                type: "int",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_AssignedToUserId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_AssignedToUserId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "Applications");
        }
    }
}
