using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SweperBackend.Migrations
{
    public partial class ImprovedMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_UserFromId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_UserToId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "UserToId",
                table: "Message",
                newName: "UserRenterId");

            migrationBuilder.RenameColumn(
                name: "UserFromId",
                table: "Message",
                newName: "UserOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_UserToId",
                table: "Message",
                newName: "IX_Message_UserRenterId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_UserFromId",
                table: "Message",
                newName: "IX_Message_UserOwnerId");

            migrationBuilder.AddColumn<bool>(
                name: "IsFromOwner",
                table: "Message",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_UserOwnerId",
                table: "Message",
                column: "UserOwnerId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_UserRenterId",
                table: "Message",
                column: "UserRenterId",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_UserOwnerId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_UserRenterId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IsFromOwner",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "UserRenterId",
                table: "Message",
                newName: "UserToId");

            migrationBuilder.RenameColumn(
                name: "UserOwnerId",
                table: "Message",
                newName: "UserFromId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_UserRenterId",
                table: "Message",
                newName: "IX_Message_UserToId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_UserOwnerId",
                table: "Message",
                newName: "IX_Message_UserFromId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_UserFromId",
                table: "Message",
                column: "UserFromId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_UserToId",
                table: "Message",
                column: "UserToId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
