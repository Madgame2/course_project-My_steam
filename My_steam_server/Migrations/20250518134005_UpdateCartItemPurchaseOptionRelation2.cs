using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_steam_server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartItemPurchaseOptionRelation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_AspNetUsers_UserId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceived_PurchaseOptions_PurchaseOptionId",
                table: "GoodsReceived");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Games_GameId",
                table: "Screenshots");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_AspNetUsers_UserId",
                table: "CartItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceived_PurchaseOptions_PurchaseOptionId",
                table: "GoodsReceived",
                column: "PurchaseOptionId",
                principalTable: "PurchaseOptions",
                principalColumn: "OptionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Screenshots_Games_GameId",
                table: "Screenshots",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_AspNetUsers_UserId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceived_PurchaseOptions_PurchaseOptionId",
                table: "GoodsReceived");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Games_GameId",
                table: "Screenshots");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_AspNetUsers_UserId",
                table: "CartItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceived_PurchaseOptions_PurchaseOptionId",
                table: "GoodsReceived",
                column: "PurchaseOptionId",
                principalTable: "PurchaseOptions",
                principalColumn: "OptionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Screenshots_Games_GameId",
                table: "Screenshots",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
