using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AttemptDirectorMovieFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DirectorMovie_directors_DirectorsId",
                schema: "imdb",
                table: "DirectorMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_DirectorMovie_movies_DirectedMoviesId",
                schema: "imdb",
                table: "DirectorMovie");

            migrationBuilder.DropIndex(
                name: "IX_DirectorMovie_DirectedMoviesId",
                schema: "imdb",
                table: "DirectorMovie");

            migrationBuilder.DropIndex(
                name: "IX_DirectorMovie_DirectorsId",
                schema: "imdb",
                table: "DirectorMovie");

            migrationBuilder.DropColumn(
                name: "DirectedMoviesId",
                schema: "imdb",
                table: "DirectorMovie");

            migrationBuilder.DropColumn(
                name: "DirectorsId",
                schema: "imdb",
                table: "DirectorMovie");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DirectedMoviesId",
                schema: "imdb",
                table: "DirectorMovie",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DirectorsId",
                schema: "imdb",
                table: "DirectorMovie",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

            migrationBuilder.AddForeignKey(
                name: "FK_DirectorMovie_directors_DirectorsId",
                schema: "imdb",
                table: "DirectorMovie",
                column: "DirectorsId",
                principalSchema: "imdb",
                principalTable: "directors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DirectorMovie_movies_DirectedMoviesId",
                schema: "imdb",
                table: "DirectorMovie",
                column: "DirectedMoviesId",
                principalSchema: "imdb",
                principalTable: "movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
