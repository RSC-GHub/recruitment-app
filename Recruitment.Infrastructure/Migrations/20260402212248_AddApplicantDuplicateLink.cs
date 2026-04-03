using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recruitment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicantDuplicateLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MasterApplicantId",
                table: "Applicants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_MasterApplicantId",
                table: "Applicants",
                column: "MasterApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Applicants_MasterApplicantId",
                table: "Applicants",
                column: "MasterApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Applicants_MasterApplicantId",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_MasterApplicantId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "MasterApplicantId",
                table: "Applicants");
        }
    }
}
