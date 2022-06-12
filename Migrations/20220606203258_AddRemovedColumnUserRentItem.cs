using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SweperBackend.Migrations
{
    public partial class AddRemovedColumnUserRentItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateRemoved",
                table: "UserRentItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Removed",
                table: "UserRentItem",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRemoved",
                table: "UserRentItem");

            migrationBuilder.DropColumn(
                name: "Removed",
                table: "UserRentItem");
        }
    }
}
