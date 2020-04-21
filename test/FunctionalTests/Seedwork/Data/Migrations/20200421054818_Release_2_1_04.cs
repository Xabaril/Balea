using Microsoft.EntityFrameworkCore.Migrations;

namespace FunctionalTests.Seedwork.Data.Migrations
{
    public partial class Release_2_1_04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delegations_Applications_ApplicationEntityId",
                table: "Delegations");

            migrationBuilder.DropIndex(
                name: "IX_Delegations_ApplicationEntityId",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "ApplicationEntityId",
                table: "Delegations");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Delegations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_ApplicationId",
                table: "Delegations",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delegations_Applications_ApplicationId",
                table: "Delegations",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delegations_Applications_ApplicationId",
                table: "Delegations");

            migrationBuilder.DropIndex(
                name: "IX_Delegations_ApplicationId",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Delegations");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationEntityId",
                table: "Delegations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_ApplicationEntityId",
                table: "Delegations",
                column: "ApplicationEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delegations_Applications_ApplicationEntityId",
                table: "Delegations",
                column: "ApplicationEntityId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
