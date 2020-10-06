using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data.Migrations
{
    public partial class Release_4_1_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Delegations_WhoId_WhomId_ApplicationId",
                table: "Delegations");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionTags",
                columns: table => new
                {
                    PermissionId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionTags", x => new { x.PermissionId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PermissionTags_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleTags",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTags", x => new { x.RoleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_RoleTags_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_WhoId",
                table: "Delegations",
                column: "WhoId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionTags_TagId",
                table: "PermissionTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleTags_TagId",
                table: "RoleTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionTags");

            migrationBuilder.DropTable(
                name: "RoleTags");

            migrationBuilder.DropTable(
                name: "Tags");

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
