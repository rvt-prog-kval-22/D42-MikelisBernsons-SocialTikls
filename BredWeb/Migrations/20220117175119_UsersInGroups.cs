using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BredWeb.Migrations
{
    public partial class UsersInGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserIdLists");

            migrationBuilder.CreateTable(
                name: "GroupPerson",
                columns: table => new
                {
                    GroupUserListId = table.Column<int>(type: "int", nullable: false),
                    UserListId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPerson", x => new { x.GroupUserListId, x.UserListId });
                    table.ForeignKey(
                        name: "FK_GroupPerson_AspNetUsers_UserListId",
                        column: x => x.UserListId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPerson_Groups_GroupUserListId",
                        column: x => x.GroupUserListId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupPerson_UserListId",
                table: "GroupPerson",
                column: "UserListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupPerson");

            migrationBuilder.CreateTable(
                name: "UserIdLists",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdLists", x => new { x.GroupId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_UserIdLists_AspNetUsers_PersonId",
                        column: x => x.PersonId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserIdLists_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserIdLists_Groups_GroupId1",
                        column: x => x.GroupId1,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserIdLists_GroupId1",
                table: "UserIdLists",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserIdLists_PersonId",
                table: "UserIdLists",
                column: "PersonId");
        }
    }
}
