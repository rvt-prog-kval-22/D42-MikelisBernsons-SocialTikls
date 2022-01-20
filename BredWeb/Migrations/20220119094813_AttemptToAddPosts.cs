using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BredWeb.Migrations
{
    public partial class AttemptToAddPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Groups_GroupId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Comments_CommentId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Posts_PostId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_CommentId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_PostId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Posts_GroupId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Upvoted",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Posts",
                newName: "TotalRating");

            migrationBuilder.AddColumn<int>(
                name: "UpvoteStatus",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posts",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

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
                name: "GroupPost",
                columns: table => new
                {
                    GroupListId = table.Column<int>(type: "int", nullable: false),
                    PostsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPost", x => new { x.GroupListId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_GroupPost_Groups_GroupListId",
                        column: x => x.GroupListId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPost_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
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
                name: "IX_GroupPost_PostsId",
                table: "GroupPost",
                column: "PostsId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonRating_RatingListId",
                table: "PersonRating",
                column: "RatingListId");

            migrationBuilder.CreateIndex(
                name: "IX_PostRating_RatingListId",
                table: "PostRating",
                column: "RatingListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentPost");

            migrationBuilder.DropTable(
                name: "CommentRating");

            migrationBuilder.DropTable(
                name: "GroupPost");

            migrationBuilder.DropTable(
                name: "PersonRating");

            migrationBuilder.DropTable(
                name: "PostRating");

            migrationBuilder.DropColumn(
                name: "UpvoteStatus",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "TotalRating",
                table: "Posts",
                newName: "Rating");

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Upvoted",
                table: "Ratings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CommentId",
                table: "Ratings",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_PostId",
                table: "Ratings",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GroupId",
                table: "Posts",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Groups_GroupId",
                table: "Posts",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Comments_CommentId",
                table: "Ratings",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Posts_PostId",
                table: "Ratings",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
