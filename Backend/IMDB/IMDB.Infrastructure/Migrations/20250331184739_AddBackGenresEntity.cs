using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBackGenresEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "genres",
                schema: "imdb",
                table: "tv_shows");

            migrationBuilder.DropColumn(
                name: "genres",
                schema: "imdb",
                table: "movies");

            migrationBuilder.CreateTable(
                name: "genres",
                schema: "imdb",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genre_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.id);
                    table.ForeignKey(
                        name: "FK_genres_users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "imdb",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MovieGenre",
                schema: "imdb",
                columns: table => new
                {
                    GenreId = table.Column<long>(type: "bigint", nullable: false),
                    MovieId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenre", x => new { x.GenreId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_MovieGenre_genres_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "imdb",
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovieGenre_movies_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "imdb",
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TvShowGenre",
                schema: "imdb",
                columns: table => new
                {
                    GenreId = table.Column<long>(type: "bigint", nullable: false),
                    TvShowId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvShowGenre", x => new { x.GenreId, x.TvShowId });
                    table.ForeignKey(
                        name: "FK_TvShowGenre_genres_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "imdb",
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TvShowGenre_tv_shows_TvShowId",
                        column: x => x.TvShowId,
                        principalSchema: "imdb",
                        principalTable: "tv_shows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_genres_CreatedByUserId",
                schema: "imdb",
                table: "genres",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenre_MovieId",
                schema: "imdb",
                table: "MovieGenre",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_TvShowGenre_TvShowId",
                schema: "imdb",
                table: "TvShowGenre",
                column: "TvShowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieGenre",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "TvShowGenre",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "genres",
                schema: "imdb");

            migrationBuilder.AddColumn<string>(
                name: "genres",
                schema: "imdb",
                table: "tv_shows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "genres",
                schema: "imdb",
                table: "movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
