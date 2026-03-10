using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentsystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyToJobPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "JobPositions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JobPositions_CompanyId",
                table: "JobPositions",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPositions_Company_CompanyId",
                table: "JobPositions",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPositions_Company_CompanyId",
                table: "JobPositions");

            migrationBuilder.DropIndex(
                name: "IX_JobPositions_CompanyId",
                table: "JobPositions");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "JobPositions");
        }
    }
}
