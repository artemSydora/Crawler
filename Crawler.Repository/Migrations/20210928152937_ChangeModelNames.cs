using Microsoft.EntityFrameworkCore.Migrations;

namespace Crawler.Repository.Migrations
{
    public partial class ChangeModelNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkPerformanceResults_Tests_TestResultId",
                table: "LinkPerformanceResults");

            migrationBuilder.RenameColumn(
                name: "TestResultId",
                table: "LinkPerformanceResults",
                newName: "TestDTOId");

            migrationBuilder.RenameIndex(
                name: "IX_LinkPerformanceResults_TestResultId",
                table: "LinkPerformanceResults",
                newName: "IX_LinkPerformanceResults_TestDTOId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkPerformanceResults_Tests_TestDTOId",
                table: "LinkPerformanceResults",
                column: "TestDTOId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkPerformanceResults_Tests_TestDTOId",
                table: "LinkPerformanceResults");

            migrationBuilder.RenameColumn(
                name: "TestDTOId",
                table: "LinkPerformanceResults",
                newName: "TestResultId");

            migrationBuilder.RenameIndex(
                name: "IX_LinkPerformanceResults_TestDTOId",
                table: "LinkPerformanceResults",
                newName: "IX_LinkPerformanceResults_TestResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkPerformanceResults_Tests_TestResultId",
                table: "LinkPerformanceResults",
                column: "TestResultId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
