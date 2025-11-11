using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recruitment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePermissionCongig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Permissions_PermissionName",
                table: "Permissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionName",
                table: "Permissions",
                column: "PermissionName",
                unique: true);
        }
    }
}
