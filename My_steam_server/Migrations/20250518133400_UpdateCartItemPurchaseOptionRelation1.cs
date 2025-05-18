using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_steam_server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartItemPurchaseOptionRelation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_PurchaseOptions_PurchaseOptionId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_PurchaseOptions_PurchaseOptionId",
                table: "CartItems",
                column: "PurchaseOptionId",
                principalTable: "PurchaseOptions",
                principalColumn: "OptionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_PurchaseOptions_PurchaseOptionId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_PurchaseOptions_PurchaseOptionId",
                table: "CartItems",
                column: "PurchaseOptionId",
                principalTable: "PurchaseOptions",
                principalColumn: "OptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
