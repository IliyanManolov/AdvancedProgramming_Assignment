using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyRelationshipsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorMovie_actors_ActorsId",
                schema: "imdb",
                table: "ActorMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorMovie_movies_ParticipatedMoviesId",
                schema: "imdb",
                table: "ActorMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorTvShow_actors_ActorsId",
                schema: "imdb",
                table: "ActorTvShow");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorTvShow_tv_shows_ParticipatedShowsId",
                schema: "imdb",
                table: "ActorTvShow");

            migrationBuilder.DropForeignKey(
                name: "FK_DirectorTvShow_directors_DirectorsId",
                schema: "imdb",
                table: "DirectorTvShow");

            migrationBuilder.DropForeignKey(
                name: "FK_DirectorTvShow_tv_shows_DirectedShowsId",
                schema: "imdb",
                table: "DirectorTvShow");

            migrationBuilder.DropTable(
                name: "MovieWatchList",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "TvShowWatchList",
                schema: "imdb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TvShowGenre",
                schema: "imdb",
                table: "TvShowGenre");

            migrationBuilder.DropIndex(
                name: "IX_TvShowGenre_GenreId",
                schema: "imdb",
                table: "TvShowGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieGenre",
                schema: "imdb",
                table: "MovieGenre");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenre_GenreId",
                schema: "imdb",
                table: "MovieGenre");

            migrationBuilder.DropIndex(
                name: "IX_DirectorTvShow_DirectedShowsId",
                schema: "imdb",
                table: "DirectorTvShow");

            migrationBuilder.DropIndex(
                name: "IX_DirectorTvShow_DirectorsId",
                schema: "imdb",
                table: "DirectorTvShow");

            migrationBuilder.DropIndex(
                name: "IX_ActorTvShow_ActorsId",
                schema: "imdb",
                table: "ActorTvShow");

            migrationBuilder.DropIndex(
                name: "IX_ActorTvShow_ParticipatedShowsId",
                schema: "imdb",
                table: "ActorTvShow");

            migrationBuilder.DropIndex(
                name: "IX_ActorMovie_ActorsId",
                schema: "imdb",
                table: "ActorMovie");

            migrationBuilder.DropIndex(
                name: "IX_ActorMovie_ParticipatedMoviesId",
                schema: "imdb",
                table: "ActorMovie");

            migrationBuilder.DropColumn(
                name: "DirectedShowsId",
                schema: "imdb",
                table: "DirectorTvShow");

            migrationBuilder.DropColumn(
                name: "DirectorsId",
                schema: "imdb",
                table: "DirectorTvShow");

            migrationBuilder.DropColumn(
                name: "ActorsId",
                schema: "imdb",
                table: "ActorTvShow");

            migrationBuilder.DropColumn(
                name: "ParticipatedShowsId",
                schema: "imdb",
                table: "ActorTvShow");

            migrationBuilder.DropColumn(
                name: "ActorsId",
                schema: "imdb",
                table: "ActorMovie");

            migrationBuilder.DropColumn(
                name: "ParticipatedMoviesId",
                schema: "imdb",
                table: "ActorMovie");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TvShowGenre",
                schema: "imdb",
                table: "TvShowGenre",
                columns: new[] { "GenreId", "TvShowId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieGenre",
                schema: "imdb",
                table: "MovieGenre",
                columns: new[] { "GenreId", "MovieId" });

            migrationBuilder.CreateTable(
                name: "WatchlistMovie",
                schema: "imdb",
                columns: table => new
                {
                    WatchlistId = table.Column<long>(type: "bigint", nullable: false),
                    MovieId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchlistMovie", x => new { x.WatchlistId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_WatchlistMovie_movies_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "imdb",
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WatchlistMovie_watch_lists_WatchlistId",
                        column: x => x.WatchlistId,
                        principalSchema: "imdb",
                        principalTable: "watch_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WatchlistTvShow",
                schema: "imdb",
                columns: table => new
                {
                    WatchlistId = table.Column<long>(type: "bigint", nullable: false),
                    TvShowId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchlistTvShow", x => new { x.WatchlistId, x.TvShowId });
                    table.ForeignKey(
                        name: "FK_WatchlistTvShow_tv_shows_TvShowId",
                        column: x => x.TvShowId,
                        principalSchema: "imdb",
                        principalTable: "tv_shows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WatchlistTvShow_watch_lists_WatchlistId",
                        column: x => x.WatchlistId,
                        principalSchema: "imdb",
                        principalTable: "watch_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TvShowGenre_TvShowId",
                schema: "imdb",
                table: "TvShowGenre",
                column: "TvShowId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenre_MovieId",
                schema: "imdb",
                table: "MovieGenre",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistMovie_MovieId",
                schema: "imdb",
                table: "WatchlistMovie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistTvShow_TvShowId",
                schema: "imdb",
                table: "WatchlistTvShow",
                column: "TvShowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchlistMovie",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "WatchlistTvShow",
                schema: "imdb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TvShowGenre",
                schema: "imdb",
                table: "TvShowGenre");

            migrationBuilder.DropIndex(
                name: "IX_TvShowGenre_TvShowId",
                schema: "imdb",
                table: "TvShowGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieGenre",
                schema: "imdb",
                table: "MovieGenre");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenre_MovieId",
                schema: "imdb",
                table: "MovieGenre");

            migrationBuilder.AddColumn<long>(
                name: "DirectedShowsId",
                schema: "imdb",
                table: "DirectorTvShow",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DirectorsId",
                schema: "imdb",
                table: "DirectorTvShow",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ActorsId",
                schema: "imdb",
                table: "ActorTvShow",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ParticipatedShowsId",
                schema: "imdb",
                table: "ActorTvShow",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ActorsId",
                schema: "imdb",
                table: "ActorMovie",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ParticipatedMoviesId",
                schema: "imdb",
                table: "ActorMovie",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TvShowGenre",
                schema: "imdb",
                table: "TvShowGenre",
                columns: new[] { "TvShowId", "GenreId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieGenre",
                schema: "imdb",
                table: "MovieGenre",
                columns: new[] { "MovieId", "GenreId" });

            migrationBuilder.CreateTable(
                name: "MovieWatchList",
                schema: "imdb",
                columns: table => new
                {
                    MoviesId = table.Column<long>(type: "bigint", nullable: false),
                    WatchListId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieWatchList", x => new { x.MoviesId, x.WatchListId });
                    table.ForeignKey(
                        name: "FK_MovieWatchList_movies_MoviesId",
                        column: x => x.MoviesId,
                        principalSchema: "imdb",
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieWatchList_watch_lists_WatchListId",
                        column: x => x.WatchListId,
                        principalSchema: "imdb",
                        principalTable: "watch_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TvShowWatchList",
                schema: "imdb",
                columns: table => new
                {
                    ShowsId = table.Column<long>(type: "bigint", nullable: false),
                    WatchListId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvShowWatchList", x => new { x.ShowsId, x.WatchListId });
                    table.ForeignKey(
                        name: "FK_TvShowWatchList_tv_shows_ShowsId",
                        column: x => x.ShowsId,
                        principalSchema: "imdb",
                        principalTable: "tv_shows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TvShowWatchList_watch_lists_WatchListId",
                        column: x => x.WatchListId,
                        principalSchema: "imdb",
                        principalTable: "watch_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TvShowGenre_GenreId",
                schema: "imdb",
                table: "TvShowGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenre_GenreId",
                schema: "imdb",
                table: "MovieGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorTvShow_DirectedShowsId",
                schema: "imdb",
                table: "DirectorTvShow",
                column: "DirectedShowsId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorTvShow_DirectorsId",
                schema: "imdb",
                table: "DirectorTvShow",
                column: "DirectorsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorTvShow_ActorsId",
                schema: "imdb",
                table: "ActorTvShow",
                column: "ActorsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorTvShow_ParticipatedShowsId",
                schema: "imdb",
                table: "ActorTvShow",
                column: "ParticipatedShowsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovie_ActorsId",
                schema: "imdb",
                table: "ActorMovie",
                column: "ActorsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovie_ParticipatedMoviesId",
                schema: "imdb",
                table: "ActorMovie",
                column: "ParticipatedMoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieWatchList_WatchListId",
                schema: "imdb",
                table: "MovieWatchList",
                column: "WatchListId");

            migrationBuilder.CreateIndex(
                name: "IX_TvShowWatchList_WatchListId",
                schema: "imdb",
                table: "TvShowWatchList",
                column: "WatchListId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorMovie_actors_ActorsId",
                schema: "imdb",
                table: "ActorMovie",
                column: "ActorsId",
                principalSchema: "imdb",
                principalTable: "actors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorMovie_movies_ParticipatedMoviesId",
                schema: "imdb",
                table: "ActorMovie",
                column: "ParticipatedMoviesId",
                principalSchema: "imdb",
                principalTable: "movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorTvShow_actors_ActorsId",
                schema: "imdb",
                table: "ActorTvShow",
                column: "ActorsId",
                principalSchema: "imdb",
                principalTable: "actors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorTvShow_tv_shows_ParticipatedShowsId",
                schema: "imdb",
                table: "ActorTvShow",
                column: "ParticipatedShowsId",
                principalSchema: "imdb",
                principalTable: "tv_shows",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DirectorTvShow_directors_DirectorsId",
                schema: "imdb",
                table: "DirectorTvShow",
                column: "DirectorsId",
                principalSchema: "imdb",
                principalTable: "directors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DirectorTvShow_tv_shows_DirectedShowsId",
                schema: "imdb",
                table: "DirectorTvShow",
                column: "DirectedShowsId",
                principalSchema: "imdb",
                principalTable: "tv_shows",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
