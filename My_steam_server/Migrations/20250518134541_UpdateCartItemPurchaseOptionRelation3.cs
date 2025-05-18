using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_steam_server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartItemPurchaseOptionRelation3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLibraryEntries_AspNetUsers_UserId",
                table: "UserLibraryEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLibraryEntries_Games_GameId",
                table: "UserLibraryEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLibraryEntries_AspNetUsers_UserId",
                table: "UserLibraryEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLibraryEntries_Games_GameId",
                table: "UserLibraryEntries",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLibraryEntries_AspNetUsers_UserId",
                table: "UserLibraryEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLibraryEntries_Games_GameId",
                table: "UserLibraryEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLibraryEntries_AspNetUsers_UserId",
                table: "UserLibraryEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLibraryEntries_Games_GameId",
                table: "UserLibraryEntries",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
