using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data.Migrations
{
    public partial class Release_2_1_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delegations_Applications_ApplicationEntityId",
                table: "Delegations");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Delegations_ApplicationEntityId",
                table: "Delegations");

            migrationBuilder.DropIndex(
                name: "IX_Delegations_WhoId",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "ApplicationEntityId",
                table: "Delegations");

            migrationBuilder.AlterColumn<bool>(
                name: "Enabled",
                table: "Roles",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Delegations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Applications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name_ApplicationId",
                table: "Roles",
                columns: new[] { "Name", "ApplicationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name_ApplicationId",
                table: "Permissions",
                columns: new[] { "Name", "ApplicationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_ApplicationId",
                table: "Delegations",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_WhoId_WhomId_ApplicationId",
                table: "Delegations",
                columns: new[] { "WhoId", "WhomId", "ApplicationId" },
                unique: true);

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
                name: "IX_Roles_Name_ApplicationId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_Name_ApplicationId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Delegations_ApplicationId",
                table: "Delegations");

            migrationBuilder.DropIndex(
                name: "IX_Delegations_WhoId_WhomId_ApplicationId",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Applications");

            migrationBuilder.AlterColumn<bool>(
                name: "Enabled",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<int>(
                name: "ApplicationEntityId",
                table: "Delegations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_ApplicationEntityId",
                table: "Delegations",
                column: "ApplicationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_WhoId",
                table: "Delegations",
                column: "WhoId");

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
