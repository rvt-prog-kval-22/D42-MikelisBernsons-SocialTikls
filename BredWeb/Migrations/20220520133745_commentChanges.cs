using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BredWeb.Migrations
{
    public partial class commentChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEdited",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
