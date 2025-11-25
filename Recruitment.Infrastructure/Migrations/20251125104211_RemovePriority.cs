using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recruitment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectVacancies_Vacancies_VacancyId",
                table: "ProjectVacancies");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Titles_TitleId",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ProjectVacancies");

            migrationBuilder.AlterColumn<int>(
                name: "PositionCount",
                table: "Vacancies",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectVacancies_Vacancies_VacancyId",
                table: "ProjectVacancies",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Titles_TitleId",
                table: "Vacancies",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectVacancies_Vacancies_VacancyId",
                table: "ProjectVacancies");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Titles_TitleId",
                table: "Vacancies");

            migrationBuilder.AlterColumn<int>(
                name: "PositionCount",
                table: "Vacancies",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "ProjectVacancies",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectVacancies_Vacancies_VacancyId",
                table: "ProjectVacancies",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Titles_TitleId",
                table: "Vacancies",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
