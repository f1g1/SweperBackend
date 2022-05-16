using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SweperBackend.Migrations
{
    public partial class AddedUserRentITem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRentItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RentItemId = table.Column<int>(type: "int", nullable: false),
                    Liked = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateViewd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateInteraction = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRentItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRentItem_RentItem_RentItemId",
                        column: x => x.RentItemId,
                        principalTable: "RentItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRentItem_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRentItem_RentItemId",
                table: "UserRentItem",
                column: "RentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRentItem_UserId",
                table: "UserRentItem",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRentItem");
        }
    }
}
