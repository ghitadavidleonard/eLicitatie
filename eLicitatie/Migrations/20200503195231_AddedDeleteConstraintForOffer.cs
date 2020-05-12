using Microsoft.EntityFrameworkCore.Migrations;

namespace eLicitatie.Api.Migrations
{
    public partial class AddedDeleteConstraintForOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Auctions_AuctionId",
                table: "Offers");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Auctions_AuctionId",
                table: "Offers",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Auctions_AuctionId",
                table: "Offers");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Auctions_AuctionId",
                table: "Offers",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
