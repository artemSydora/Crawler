using Microsoft.EntityFrameworkCore.Migrations;

namespace Crawler.Repository.Migrations
{
    public partial class ChangeTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkPerformanceResults_Tests_TestDTOId",
                table: "LinkPerformanceResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinkPerformanceResults",
                table: "LinkPerformanceResults");

            migrationBuilder.RenameTable(
                name: "LinkPerformanceResults",
                newName: "Details");

            migrationBuilder.RenameIndex(
                name: "IX_LinkPerformanceResults_TestDTOId",
                table: "Details",
                newName: "IX_Details_TestDTOId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Details",
                table: "Details",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Tests_TestDTOId",
                table: "Details",
                column: "TestDTOId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Details_Tests_TestDTOId",
                table: "Details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Details",
                table: "Details");

            migrationBuilder.RenameTable(
                name: "Details",
                newName: "LinkPerformanceResults");

            migrationBuilder.RenameIndex(
                name: "IX_Details_TestDTOId",
                table: "LinkPerformanceResults",
                newName: "IX_LinkPerformanceResults_TestDTOId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinkPerformanceResults",
                table: "LinkPerformanceResults",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkPerformanceResults_Tests_TestDTOId",
                table: "LinkPerformanceResults",
                column: "TestDTOId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
