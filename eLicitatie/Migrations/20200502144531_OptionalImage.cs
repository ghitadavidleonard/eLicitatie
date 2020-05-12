using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eLicitatie.Api.Migrations
{
    public partial class OptionalImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Products",
                type: "varbinary(4000)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(4000)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Products",
                type: "varbinary(4000)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(4000)",
                oldNullable: true);
        }
    }
}
