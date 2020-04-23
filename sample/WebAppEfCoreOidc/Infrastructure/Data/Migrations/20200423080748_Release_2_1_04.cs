using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppEfCoreOidc.Infrastructure.Data.Migrations
{
    public partial class Release_2_1_04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Delegations_WhoId",
                table: "Delegations");

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_WhoId_WhomId_ApplicationId",
                table: "Delegations",
                columns: new[] { "WhoId", "WhomId", "ApplicationId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Delegations_WhoId_WhomId_ApplicationId",
                table: "Delegations");

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_WhoId",
                table: "Delegations",
                column: "WhoId");
        }
    }
}
