using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BredWeb.Migrations
{
    public partial class UniqueTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Groups_Title",
                table: "Groups",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_Title",
                table: "Groups");
        }
    }
}
