using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_movies_MovieId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_showEpisodes_EpisodeId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_tv_shows_ShowId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_users_UserId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "media_reviews",
                newSchema: "imdb");

            migrationBuilder.RenameColumn(
                name: "Rating",
                schema: "imdb",
                table: "media_reviews",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "imdb",
                table: "media_reviews",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdateTimeStamp",
                schema: "imdb",
                table: "media_reviews",
                newName: "update_date");

            migrationBuilder.RenameColumn(
                name: "ReviewText",
                schema: "imdb",
                table: "media_reviews",
                newName: "review_text");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "imdb",
                table: "media_reviews",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreateTimeStamp",
                schema: "imdb",
                table: "media_reviews",
                newName: "create_date");

            migrationBuilder.RenameIndex(
                name: "IX_Review_UserId",
                schema: "imdb",
                table: "media_reviews",
                newName: "IX_media_reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ShowId",
                schema: "imdb",
                table: "media_reviews",
                newName: "IX_media_reviews_ShowId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_MovieId",
                schema: "imdb",
                table: "media_reviews",
                newName: "IX_media_reviews_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_EpisodeId",
                schema: "imdb",
                table: "media_reviews",
                newName: "IX_media_reviews_EpisodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_media_reviews",
                schema: "imdb",
                table: "media_reviews",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_media_reviews_movies_MovieId",
                schema: "imdb",
                table: "media_reviews",
                column: "MovieId",
                principalSchema: "imdb",
                principalTable: "movies",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_media_reviews_showEpisodes_EpisodeId",
                schema: "imdb",
                table: "media_reviews",
                column: "EpisodeId",
                principalSchema: "imdb",
                principalTable: "showEpisodes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_media_reviews_tv_shows_ShowId",
                schema: "imdb",
                table: "media_reviews",
                column: "ShowId",
                principalSchema: "imdb",
                principalTable: "tv_shows",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_media_reviews_users_UserId",
                schema: "imdb",
                table: "media_reviews",
                column: "UserId",
                principalSchema: "imdb",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_media_reviews_movies_MovieId",
                schema: "imdb",
                table: "media_reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_media_reviews_showEpisodes_EpisodeId",
                schema: "imdb",
                table: "media_reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_media_reviews_tv_shows_ShowId",
                schema: "imdb",
                table: "media_reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_media_reviews_users_UserId",
                schema: "imdb",
                table: "media_reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_media_reviews",
                schema: "imdb",
                table: "media_reviews");

            migrationBuilder.RenameTable(
                name: "media_reviews",
                schema: "imdb",
                newName: "Review");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Review",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Review",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "update_date",
                table: "Review",
                newName: "UpdateTimeStamp");

            migrationBuilder.RenameColumn(
                name: "review_text",
                table: "Review",
                newName: "ReviewText");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Review",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "create_date",
                table: "Review",
                newName: "CreateTimeStamp");

            migrationBuilder.RenameIndex(
                name: "IX_media_reviews_UserId",
                table: "Review",
                newName: "IX_Review_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_media_reviews_ShowId",
                table: "Review",
                newName: "IX_Review_ShowId");

            migrationBuilder.RenameIndex(
                name: "IX_media_reviews_MovieId",
                table: "Review",
                newName: "IX_Review_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_media_reviews_EpisodeId",
                table: "Review",
                newName: "IX_Review_EpisodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_movies_MovieId",
                table: "Review",
                column: "MovieId",
                principalSchema: "imdb",
                principalTable: "movies",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_showEpisodes_EpisodeId",
                table: "Review",
                column: "EpisodeId",
                principalSchema: "imdb",
                principalTable: "showEpisodes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_tv_shows_ShowId",
                table: "Review",
                column: "ShowId",
                principalSchema: "imdb",
                principalTable: "tv_shows",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_users_UserId",
                table: "Review",
                column: "UserId",
                principalSchema: "imdb",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
