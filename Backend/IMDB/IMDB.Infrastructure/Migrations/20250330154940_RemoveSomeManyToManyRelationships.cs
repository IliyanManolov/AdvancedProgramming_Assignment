using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSomeManyToManyRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirectorMovie",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "DirectorTvShow",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "MovieGenre",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "TvShowGenre",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "genres",
                schema: "imdb");

            migrationBuilder.AddColumn<long>(
                name: "DirectorId",
                schema: "imdb",
                table: "tv_shows",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "genres",
                schema: "imdb",
                table: "tv_shows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "DirectorId",
                schema: "imdb",
                table: "movies",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "genres",
                schema: "imdb",
                table: "movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_tv_shows_DirectorId",
                schema: "imdb",
                table: "tv_shows",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_movies_DirectorId",
                schema: "imdb",
                table: "movies",
                column: "DirectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_movies_directors_DirectorId",
                schema: "imdb",
                table: "movies",
                column: "DirectorId",
                principalSchema: "imdb",
                principalTable: "directors",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tv_shows_directors_DirectorId",
                schema: "imdb",
                table: "tv_shows",
                column: "DirectorId",
                principalSchema: "imdb",
                principalTable: "directors",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movies_directors_DirectorId",
                schema: "imdb",
                table: "movies");

            migrationBuilder.DropForeignKey(
                name: "FK_tv_shows_directors_DirectorId",
                schema: "imdb",
                table: "tv_shows");

            migrationBuilder.DropIndex(
                name: "IX_tv_shows_DirectorId",
                schema: "imdb",
                table: "tv_shows");

            migrationBuilder.DropIndex(
                name: "IX_movies_DirectorId",
                schema: "imdb",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "DirectorId",
                schema: "imdb",
                table: "tv_shows");

            migrationBuilder.DropColumn(
                name: "genres",
                schema: "imdb",
                table: "tv_shows");

            migrationBuilder.DropColumn(
                name: "DirectorId",
                schema: "imdb",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "genres",
                schema: "imdb",
                table: "movies");

            migrationBuilder.CreateTable(
                name: "DirectorMovie",
                schema: "imdb",
                columns: table => new
                {
                    DirectorId = table.Column<long>(type: "bigint", nullable: false),
                    MovieId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectorMovie", x => new { x.DirectorId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_DirectorMovie_directors_DirectorId",
                        column: x => x.DirectorId,
                        principalSchema: "imdb",
                        principalTable: "directors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirectorMovie_movies_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "imdb",
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DirectorTvShow",
                schema: "imdb",
                columns: table => new
                {
                    DirectorId = table.Column<long>(type: "bigint", nullable: false),
                    TvShowId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectorTvShow", x => new { x.DirectorId, x.TvShowId });
                    table.ForeignKey(
                        name: "FK_DirectorTvShow_directors_DirectorId",
                        column: x => x.DirectorId,
                        principalSchema: "imdb",
                        principalTable: "directors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirectorTvShow_tv_shows_TvShowId",
                        column: x => x.TvShowId,
                        principalSchema: "imdb",
                        principalTable: "tv_shows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                schema: "imdb",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    genre_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "IX_DirectorMovie_MovieId",
                schema: "imdb",
                table: "DirectorMovie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorTvShow_TvShowId",
                schema: "imdb",
                table: "DirectorTvShow",
                column: "TvShowId");

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
    }
}
