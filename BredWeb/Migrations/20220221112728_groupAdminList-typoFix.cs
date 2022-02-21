using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BredWeb.Migrations
{
    public partial class groupAdminListtypoFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Groups_GroupId1",
                table: "Admin");

            migrationBuilder.DropIndex(
                name: "IX_Admin_GroupId1",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "Admin");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Admin",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Admin",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Admin_GroupId",
                table: "Admin",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Groups_GroupId",
                table: "Admin",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Groups_GroupId",
                table: "Admin");

            migrationBuilder.DropIndex(
                name: "IX_Admin_GroupId",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Admin");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "Admin",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId1",
                table: "Admin",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admin_GroupId1",
                table: "Admin",
                column: "GroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Groups_GroupId1",
                table: "Admin",
                column: "GroupId1",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
