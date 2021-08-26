using Microsoft.EntityFrameworkCore.Migrations;

namespace Crawler.Repository.Migrations
{
    public partial class RenameTableColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponseTime",
                table: "LinkPerformanceResults",
                newName: "ResponseTimeMs");

            migrationBuilder.AlterColumn<string>(
                name: "HomePageUrl",
                table: "Tests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponseTimeMs",
                table: "LinkPerformanceResults",
                newName: "ResponseTime");

            migrationBuilder.AlterColumn<string>(
                name: "HomePageUrl",
                table: "Tests",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
