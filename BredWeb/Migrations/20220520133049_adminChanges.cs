using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BredWeb.Migrations
{
    public partial class adminChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "Admin");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ratings",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ratings",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "Admin",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
