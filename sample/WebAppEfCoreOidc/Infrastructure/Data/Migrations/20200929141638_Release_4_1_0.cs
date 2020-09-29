using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppEfCoreOidc.Infrastructure.Data.Migrations
{
    public partial class Release_4_1_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Delegations_WhoId_WhomId_ApplicationId",
                table: "Delegations");

            migrationBuilder.CreateTable(
                name: "TagEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionTagEntity",
                columns: table => new
                {
                    PermissionId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionTagEntity", x => new { x.PermissionId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PermissionTagEntity_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionTagEntity_TagEntity_TagId",
                        column: x => x.TagId,
                        principalTable: "TagEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleTagEntity",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTagEntity", x => new { x.RoleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_RoleTagEntity_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleTagEntity_TagEntity_TagId",
                        column: x => x.TagId,
                        principalTable: "TagEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_WhoId",
                table: "Delegations",
                column: "WhoId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionTagEntity_TagId",
                table: "PermissionTagEntity",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleTagEntity_TagId",
                table: "RoleTagEntity",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionTagEntity");

            migrationBuilder.DropTable(
                name: "RoleTagEntity");

            migrationBuilder.DropTable(
                name: "TagEntity");

            migrationBuilder.DropIndex(
                name: "IX_Delegations_WhoId",
                table: "Delegations");

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_WhoId_WhomId_ApplicationId",
                table: "Delegations",
                columns: new[] { "WhoId", "WhomId", "ApplicationId" },
                unique: true);
        }
    }
}
