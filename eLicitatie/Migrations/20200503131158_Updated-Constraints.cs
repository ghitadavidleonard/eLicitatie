using Microsoft.EntityFrameworkCore.Migrations;

namespace eLicitatie.Api.Migrations
{
    public partial class UpdatedConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Users_CreatorId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Products_ProductId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_CreatorId",
                table: "Auctions");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CreatorId",
                table: "Auctions",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Users_CreatorId",
                table: "Auctions",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Products_ProductId",
                table: "Auctions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Users_CreatorId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Products_ProductId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_CreatorId",
                table: "Auctions");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CreatorId",
                table: "Auctions",
                column: "CreatorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Users_CreatorId",
                table: "Auctions",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Products_ProductId",
                table: "Auctions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
