using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filmiy.Migrations
{
    /// <inheritdoc />
    public partial class FixTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movieActors_actors_ActorId",
                table: "movieActors");

            migrationBuilder.DropForeignKey(
                name: "FK_movieActors_movies_MovieId",
                table: "movieActors");

            migrationBuilder.DropForeignKey(
                name: "FK_movieImages_movies_MovieId",
                table: "movieImages");

            migrationBuilder.DropForeignKey(
                name: "FK_movies_categories_CategoryId",
                table: "movies");

            migrationBuilder.DropForeignKey(
                name: "FK_movies_cinema_CinemaId",
                table: "movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movies",
                table: "movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movieImages",
                table: "movieImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movieActors",
                table: "movieActors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cinema",
                table: "cinema");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                table: "categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_actors",
                table: "actors");

            migrationBuilder.RenameTable(
                name: "movies",
                newName: "Movies");

            migrationBuilder.RenameTable(
                name: "movieImages",
                newName: "MovieImages");

            migrationBuilder.RenameTable(
                name: "movieActors",
                newName: "MovieActors");

            migrationBuilder.RenameTable(
                name: "cinema",
                newName: "Cinema");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "actors",
                newName: "Actors");

            migrationBuilder.RenameIndex(
                name: "IX_movies_CinemaId",
                table: "Movies",
                newName: "IX_Movies_CinemaId");

            migrationBuilder.RenameIndex(
                name: "IX_movies_CategoryId",
                table: "Movies",
                newName: "IX_Movies_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_movieImages_MovieId",
                table: "MovieImages",
                newName: "IX_MovieImages_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_movieActors_ActorId",
                table: "MovieActors",
                newName: "IX_MovieActors_ActorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieImages",
                table: "MovieImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieActors",
                table: "MovieActors",
                columns: new[] { "MovieId", "ActorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cinema",
                table: "Cinema",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Actors",
                table: "Actors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActors_Actors_ActorId",
                table: "MovieActors",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActors_Movies_MovieId",
                table: "MovieActors",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieImages_Movies_MovieId",
                table: "MovieImages",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Categories_CategoryId",
                table: "Movies",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Cinema_CinemaId",
                table: "Movies",
                column: "CinemaId",
                principalTable: "Cinema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActors_Actors_ActorId",
                table: "MovieActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieActors_Movies_MovieId",
                table: "MovieActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieImages_Movies_MovieId",
                table: "MovieImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Categories_CategoryId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Cinema_CinemaId",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieImages",
                table: "MovieImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieActors",
                table: "MovieActors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cinema",
                table: "Cinema");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Actors",
                table: "Actors");

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "movies");

            migrationBuilder.RenameTable(
                name: "MovieImages",
                newName: "movieImages");

            migrationBuilder.RenameTable(
                name: "MovieActors",
                newName: "movieActors");

            migrationBuilder.RenameTable(
                name: "Cinema",
                newName: "cinema");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "categories");

            migrationBuilder.RenameTable(
                name: "Actors",
                newName: "actors");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_CinemaId",
                table: "movies",
                newName: "IX_movies_CinemaId");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_CategoryId",
                table: "movies",
                newName: "IX_movies_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieImages_MovieId",
                table: "movieImages",
                newName: "IX_movieImages_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieActors_ActorId",
                table: "movieActors",
                newName: "IX_movieActors_ActorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_movies",
                table: "movies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_movieImages",
                table: "movieImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_movieActors",
                table: "movieActors",
                columns: new[] { "MovieId", "ActorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_cinema",
                table: "cinema",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                table: "categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_actors",
                table: "actors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_movieActors_actors_ActorId",
                table: "movieActors",
                column: "ActorId",
                principalTable: "actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movieActors_movies_MovieId",
                table: "movieActors",
                column: "MovieId",
                principalTable: "movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movieImages_movies_MovieId",
                table: "movieImages",
                column: "MovieId",
                principalTable: "movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movies_categories_CategoryId",
                table: "movies",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movies_cinema_CinemaId",
                table: "movies",
                column: "CinemaId",
                principalTable: "cinema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
