using Microsoft.EntityFrameworkCore.Migrations;

namespace eLicitatie.Api.Migrations
{
    public partial class DeleteOFfersOnAuctionDeletion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Auctions_AuctionId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Users_UserId",
                table: "Offers");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Auctions_AuctionId",
                table: "Offers",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Users_UserId",
                table: "Offers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Auctions_AuctionId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Users_UserId",
                table: "Offers");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Auctions_AuctionId",
                table: "Offers",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Users_UserId",
                table: "Offers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
