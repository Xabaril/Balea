using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data.Migrations
{
    public partial class Release_4_2_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Subjects",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Subjects");
        }
    }
}
