using Microsoft.EntityFrameworkCore.Migrations;

namespace Crawler.Repository.Migrations
{
    public partial class ChangeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkPerformanceResults_Tests_TestId",
                table: "LinkPerformanceResults");

            migrationBuilder.RenameColumn(
                name: "HomePageUrl",
                table: "Tests",
                newName: "StartPageUrl");

            migrationBuilder.RenameColumn(
                name: "TestId",
                table: "LinkPerformanceResults",
                newName: "TestResultId");

            migrationBuilder.RenameIndex(
                name: "IX_LinkPerformanceResults_TestId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkPerformanceResults_Tests_TestResultId",
                table: "LinkPerformanceResults");

            migrationBuilder.RenameColumn(
                name: "StartPageUrl",
                table: "Tests",
                newName: "HomePageUrl");

            migrationBuilder.RenameColumn(
                name: "TestResultId",
                table: "LinkPerformanceResults",
                newName: "TestId");

            migrationBuilder.RenameIndex(
                name: "IX_LinkPerformanceResults_TestResultId",
                table: "LinkPerformanceResults",
                newName: "IX_LinkPerformanceResults_TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkPerformanceResults_Tests_TestId",
                table: "LinkPerformanceResults",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
