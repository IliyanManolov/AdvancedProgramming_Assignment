using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "imdb");

            migrationBuilder.CreateTable(
                name: "watch_lists",
                schema: "imdb",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_watch_lists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "imdb",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "User"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    WatchListId = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_watch_lists_WatchListId",
                        column: x => x.WatchListId,
                        principalSchema: "imdb",
                        principalTable: "watch_lists",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "actors",
                schema: "imdb",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_death = table.Column<DateTime>(type: "datetime2", nullable: true),
                    nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    profile_image = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actors", x => x.id);
                    table.ForeignKey(
                        name: "FK_actors_users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "imdb",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "directors",
                schema: "imdb",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_death = table.Column<DateTime>(type: "datetime2", nullable: true),
                    nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    profile_image = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_directors", x => x.id);
                    table.ForeignKey(
                        name: "FK_directors_users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "imdb",
                        principalTable: "users",
                        principalColumn: "id");
                });

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
                name: "movies",
                schema: "imdb",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    trailer_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    length_seconds = table.Column<long>(type: "bigint", nullable: false),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    release_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rating = table.Column<double>(type: "float", nullable: true),
                    reviews = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    poster_image = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movies", x => x.id);
                    table.ForeignKey(
                        name: "FK_movies_users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "imdb",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tv_shows",
                schema: "imdb",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    seasons = table.Column<int>(type: "int", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    release_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rating = table.Column<double>(type: "float", nullable: true),
                    reviews = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    poster_image = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tv_shows", x => x.id);
                    table.ForeignKey(
                        name: "FK_tv_shows_users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "imdb",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ActorMovie",
                schema: "imdb",
                columns: table => new
                {
                    ActorId = table.Column<long>(type: "bigint", nullable: false),
                    MovieId = table.Column<long>(type: "bigint", nullable: false),
                    ActorsId = table.Column<long>(type: "bigint", nullable: false),
                    ParticipatedMoviesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorMovie", x => new { x.ActorId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_ActorMovie_actors_ActorId",
                        column: x => x.ActorId,
                        principalSchema: "imdb",
                        principalTable: "actors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActorMovie_actors_ActorsId",
                        column: x => x.ActorsId,
                        principalSchema: "imdb",
                        principalTable: "actors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorMovie_movies_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "imdb",
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActorMovie_movies_ParticipatedMoviesId",
                        column: x => x.ParticipatedMoviesId,
                        principalSchema: "imdb",
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectorMovie",
                schema: "imdb",
                columns: table => new
                {
                    DirectorId = table.Column<long>(type: "bigint", nullable: false),
                    MovieId = table.Column<long>(type: "bigint", nullable: false),
                    DirectedMoviesId = table.Column<long>(type: "bigint", nullable: false),
                    DirectorsId = table.Column<long>(type: "bigint", nullable: false)
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
                        name: "FK_DirectorMovie_directors_DirectorsId",
                        column: x => x.DirectorsId,
                        principalSchema: "imdb",
                        principalTable: "directors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectorMovie_movies_DirectedMoviesId",
                        column: x => x.DirectedMoviesId,
                        principalSchema: "imdb",
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectorMovie_movies_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "imdb",
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovieGenre",
                schema: "imdb",
                columns: table => new
                {
                    MovieId = table.Column<long>(type: "bigint", nullable: false),
                    GenreId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenre", x => new { x.MovieId, x.GenreId });
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
                name: "ActorTvShow",
                schema: "imdb",
                columns: table => new
                {
                    ActorId = table.Column<long>(type: "bigint", nullable: false),
                    TvShowId = table.Column<long>(type: "bigint", nullable: false),
                    ActorsId = table.Column<long>(type: "bigint", nullable: false),
                    ParticipatedShowsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorTvShow", x => new { x.ActorId, x.TvShowId });
                    table.ForeignKey(
                        name: "FK_ActorTvShow_actors_ActorId",
                        column: x => x.ActorId,
                        principalSchema: "imdb",
                        principalTable: "actors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActorTvShow_actors_ActorsId",
                        column: x => x.ActorsId,
                        principalSchema: "imdb",
                        principalTable: "actors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorTvShow_tv_shows_ParticipatedShowsId",
                        column: x => x.ParticipatedShowsId,
                        principalSchema: "imdb",
                        principalTable: "tv_shows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorTvShow_tv_shows_TvShowId",
                        column: x => x.TvShowId,
                        principalSchema: "imdb",
                        principalTable: "tv_shows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DirectorTvShow",
                schema: "imdb",
                columns: table => new
                {
                    DirectorId = table.Column<long>(type: "bigint", nullable: false),
                    TvShowId = table.Column<long>(type: "bigint", nullable: false),
                    DirectedShowsId = table.Column<long>(type: "bigint", nullable: false),
                    DirectorsId = table.Column<long>(type: "bigint", nullable: false)
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
                        name: "FK_DirectorTvShow_directors_DirectorsId",
                        column: x => x.DirectorsId,
                        principalSchema: "imdb",
                        principalTable: "directors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectorTvShow_tv_shows_DirectedShowsId",
                        column: x => x.DirectedShowsId,
                        principalSchema: "imdb",
                        principalTable: "tv_shows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectorTvShow_tv_shows_TvShowId",
                        column: x => x.TvShowId,
                        principalSchema: "imdb",
                        principalTable: "tv_shows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "showEpisodes",
                schema: "imdb",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    aired_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    length_seconds = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowId = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    season_number = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<double>(type: "float", nullable: true),
                    reviews_count = table.Column<long>(type: "bigint", nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_showEpisodes", x => x.id);
                    table.ForeignKey(
                        name: "FK_showEpisodes_tv_shows_ShowId",
                        column: x => x.ShowId,
                        principalSchema: "imdb",
                        principalTable: "tv_shows",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_showEpisodes_users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "imdb",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TvShowGenre",
                schema: "imdb",
                columns: table => new
                {
                    TvShowId = table.Column<long>(type: "bigint", nullable: false),
                    GenreId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvShowGenre", x => new { x.TvShowId, x.GenreId });
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
                name: "IX_ActorMovie_ActorsId",
                schema: "imdb",
                table: "ActorMovie",
                column: "ActorsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovie_MovieId",
                schema: "imdb",
                table: "ActorMovie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovie_ParticipatedMoviesId",
                schema: "imdb",
                table: "ActorMovie",
                column: "ParticipatedMoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_actors_CreatedByUserId",
                schema: "imdb",
                table: "actors",
                column: "CreatedByUserId");

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
                name: "IX_ActorTvShow_TvShowId",
                schema: "imdb",
                table: "ActorTvShow",
                column: "TvShowId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorMovie_DirectedMoviesId",
                schema: "imdb",
                table: "DirectorMovie",
                column: "DirectedMoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorMovie_DirectorsId",
                schema: "imdb",
                table: "DirectorMovie",
                column: "DirectorsId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorMovie_MovieId",
                schema: "imdb",
                table: "DirectorMovie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_directors_CreatedByUserId",
                schema: "imdb",
                table: "directors",
                column: "CreatedByUserId");

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
                name: "IX_MovieGenre_GenreId",
                schema: "imdb",
                table: "MovieGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_movies_CreatedByUserId",
                schema: "imdb",
                table: "movies",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieWatchList_WatchListId",
                schema: "imdb",
                table: "MovieWatchList",
                column: "WatchListId");

            migrationBuilder.CreateIndex(
                name: "IX_showEpisodes_CreatedByUserId",
                schema: "imdb",
                table: "showEpisodes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_showEpisodes_ShowId",
                schema: "imdb",
                table: "showEpisodes",
                column: "ShowId");

            migrationBuilder.CreateIndex(
                name: "IX_tv_shows_CreatedByUserId",
                schema: "imdb",
                table: "tv_shows",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TvShowGenre_GenreId",
                schema: "imdb",
                table: "TvShowGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_TvShowWatchList_WatchListId",
                schema: "imdb",
                table: "TvShowWatchList",
                column: "WatchListId");

            migrationBuilder.CreateIndex(
                name: "IX_users_WatchListId",
                schema: "imdb",
                table: "users",
                column: "WatchListId",
                unique: true,
                filter: "[WatchListId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorMovie",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "ActorTvShow",
                schema: "imdb");

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
                name: "MovieWatchList",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "showEpisodes",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "TvShowGenre",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "TvShowWatchList",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "actors",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "directors",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "movies",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "genres",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "tv_shows",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "users",
                schema: "imdb");

            migrationBuilder.DropTable(
                name: "watch_lists",
                schema: "imdb");
        }
    }
}
