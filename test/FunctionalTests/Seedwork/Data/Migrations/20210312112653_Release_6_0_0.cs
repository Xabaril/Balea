using Microsoft.EntityFrameworkCore.Migrations;

namespace FunctionalTests.Seedwork.Data.Migrations
{
    public partial class Release_6_0_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Content = table.Column<string>(maxLength: 4000, nullable: false),
                    ApplicationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Policies_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Policies_ApplicationId",
                table: "Policies",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_Name_ApplicationId",
                table: "Policies",
                columns: new[] { "Name", "ApplicationId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Policies");
        }
    }
}
