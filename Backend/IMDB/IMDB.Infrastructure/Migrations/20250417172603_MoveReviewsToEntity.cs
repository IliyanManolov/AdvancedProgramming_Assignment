using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveReviewsToEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reviews",
                schema: "imdb",
                table: "tv_shows");

            migrationBuilder.DropColumn(
                name: "reviews_count",
                schema: "imdb",
                table: "showEpisodes");

            migrationBuilder.DropColumn(
                name: "reviews",
                schema: "imdb",
                table: "movies");

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MovieId = table.Column<long>(type: "bigint", nullable: true),
                    ShowId = table.Column<long>(type: "bigint", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EpisodeId = table.Column<long>(type: "bigint", nullable: true),
                    CreateTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_movies_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "imdb",
                        principalTable: "movies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Review_showEpisodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalSchema: "imdb",
                        principalTable: "showEpisodes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Review_tv_shows_ShowId",
                        column: x => x.ShowId,
                        principalSchema: "imdb",
                        principalTable: "tv_shows",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Review_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "imdb",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Review_EpisodeId",
                table: "Review",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_MovieId",
                table: "Review",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ShowId",
                table: "Review",
                column: "ShowId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_UserId",
                table: "Review",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.AddColumn<long>(
                name: "reviews",
                schema: "imdb",
                table: "tv_shows",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "reviews_count",
                schema: "imdb",
                table: "showEpisodes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "reviews",
                schema: "imdb",
                table: "movies",
                type: "bigint",
                nullable: true);
        }
    }
}
