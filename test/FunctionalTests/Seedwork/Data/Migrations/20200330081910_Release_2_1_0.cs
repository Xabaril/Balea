using Microsoft.EntityFrameworkCore.Migrations;

namespace FunctionalTests.Seedwork.Data.Migrations
{
    public partial class Release_2_1_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Applications",
                maxLength: 2048,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Applications");
        }
    }
}
