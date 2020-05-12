using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eLicitatie.Api.Migrations
{
    public partial class ImageColumnMaxSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Products",
                type: "varbinary(MAX)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(4000)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Products",
                type: "varbinary(4000)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(MAX)",
                oldNullable: true);
        }
    }
}
