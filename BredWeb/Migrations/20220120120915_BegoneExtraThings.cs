using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BredWeb.Migrations
{
    public partial class BegoneExtraThings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentPost");

            migrationBuilder.DropTable(
                name: "CommentRating");

            migrationBuilder.DropTable(
                name: "PersonRating");

            migrationBuilder.DropTable(
                name: "PostRating");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorName",
                table: "Posts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AuthorName",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CommentPost",
                columns: table => new
                {
                    CommentListId = table.Column<int>(type: "int", nullable: false),
                    PostListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentPost", x => new { x.CommentListId, x.PostListId });
                    table.ForeignKey(
                        name: "FK_CommentPost_Comments_CommentListId",
                        column: x => x.CommentListId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentPost_Posts_PostListId",
                        column: x => x.PostListId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentRating",
                columns: table => new
                {
                    CommentListId = table.Column<int>(type: "int", nullable: false),
                    RatingListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentRating", x => new { x.CommentListId, x.RatingListId });
                    table.ForeignKey(
                        name: "FK_CommentRating_Comments_CommentListId",
                        column: x => x.CommentListId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentRating_Ratings_RatingListId",
                        column: x => x.RatingListId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonRating",
                columns: table => new
                {
                    PersonListId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RatingListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRating", x => new { x.PersonListId, x.RatingListId });
                    table.ForeignKey(
                        name: "FK_PersonRating_AspNetUsers_PersonListId",
                        column: x => x.PersonListId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonRating_Ratings_RatingListId",
                        column: x => x.RatingListId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostRating",
                columns: table => new
                {
                    PostListId = table.Column<int>(type: "int", nullable: false),
                    RatingListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostRating", x => new { x.PostListId, x.RatingListId });
                    table.ForeignKey(
                        name: "FK_PostRating_Posts_PostListId",
                        column: x => x.PostListId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostRating_Ratings_RatingListId",
                        column: x => x.RatingListId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentPost_PostListId",
                table: "CommentPost",
                column: "PostListId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentRating_RatingListId",
                table: "CommentRating",
                column: "RatingListId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonRating_RatingListId",
                table: "PersonRating",
                column: "RatingListId");

            migrationBuilder.CreateIndex(
                name: "IX_PostRating_RatingListId",
                table: "PostRating",
                column: "RatingListId");
        }
    }
}
