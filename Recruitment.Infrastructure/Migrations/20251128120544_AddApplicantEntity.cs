using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recruitment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicantEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CurrentJob = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CurrentEmployer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CurrentSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpectedSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    MilitaryStatus = table.Column<int>(type: "int", nullable: false),
                    EducationDegree = table.Column<int>(type: "int", nullable: false),
                    GraduationYear = table.Column<short>(type: "smallint", nullable: true),
                    Major = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NoticePeriod = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ExtraCertificate = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CVFilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applicants_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applicants_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_CountryId",
                table: "Applicants",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_CurrencyId",
                table: "Applicants",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applicants");
        }
    }
}
