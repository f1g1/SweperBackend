using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SweperBackend.Migrations
{
    public partial class AddChatCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatCount",
                table: "UserRentItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastChat",
                table: "UserRentItem",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatCount",
                table: "UserRentItem");

            migrationBuilder.DropColumn(
                name: "DateLastChat",
                table: "UserRentItem");
        }
    }
}
