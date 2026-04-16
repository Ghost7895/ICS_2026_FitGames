using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitGames.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ExplicitForeignKeysAndJoinEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLibrary_Games_GamesId",
                table: "GameLibrary");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLibrary_Libraries_LibrariesId",
                table: "GameLibrary");

            migrationBuilder.RenameColumn(
                name: "LibrariesId",
                table: "GameLibrary",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "GameLibrary",
                newName: "LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_GameLibrary_LibrariesId",
                table: "GameLibrary",
                newName: "IX_GameLibrary_GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameLibrary_Games_GameId",
                table: "GameLibrary",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLibrary_Libraries_LibraryId",
                table: "GameLibrary",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLibrary_Games_GameId",
                table: "GameLibrary");

            migrationBuilder.DropForeignKey(
                name: "FK_GameLibrary_Libraries_LibraryId",
                table: "GameLibrary");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GameLibrary",
                newName: "LibrariesId");

            migrationBuilder.RenameColumn(
                name: "LibraryId",
                table: "GameLibrary",
                newName: "GamesId");

            migrationBuilder.RenameIndex(
                name: "IX_GameLibrary_GameId",
                table: "GameLibrary",
                newName: "IX_GameLibrary_LibrariesId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameLibrary_Games_GamesId",
                table: "GameLibrary",
                column: "GamesId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLibrary_Libraries_LibrariesId",
                table: "GameLibrary",
                column: "LibrariesId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
