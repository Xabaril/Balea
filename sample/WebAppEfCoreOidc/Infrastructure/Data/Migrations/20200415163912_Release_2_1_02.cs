using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppEfCoreOidc.Infrastructure.Data.Migrations
{
    public partial class Release_2_1_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                table: "Roles");

            migrationBuilder.AlterColumn<bool>(
                name: "Enabled",
                table: "Roles",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_Name_ApplicationId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_Name_ApplicationId",
                table: "Permissions");

            migrationBuilder.AlterColumn<bool>(
                name: "Enabled",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);
        }
    }
}
