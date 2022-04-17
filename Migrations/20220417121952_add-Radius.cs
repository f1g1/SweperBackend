using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SweperBackend.Migrations
{
    public partial class addRadius : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "UserPreferredLocation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Radius",
                table: "UserPreferredLocation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "UserPreferredLocation");

            migrationBuilder.DropColumn(
                name: "Radius",
                table: "UserPreferredLocation");
        }
    }
}
