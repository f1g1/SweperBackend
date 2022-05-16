using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SweperBackend.Migrations
{
    public partial class CascadeImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentItemImage_RentItem_RentItemId",
                table: "RentItemImage");

            migrationBuilder.AddForeignKey(
                name: "FK_RentItemImage_RentItem_RentItemId",
                table: "RentItemImage",
                column: "RentItemId",
                principalTable: "RentItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentItemImage_RentItem_RentItemId",
                table: "RentItemImage");

            migrationBuilder.AddForeignKey(
                name: "FK_RentItemImage_RentItem_RentItemId",
                table: "RentItemImage",
                column: "RentItemId",
                principalTable: "RentItem",
                principalColumn: "Id");
        }
    }
}
