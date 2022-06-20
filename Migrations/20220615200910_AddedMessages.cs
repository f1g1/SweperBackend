using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace SweperBackend.Migrations
{
    public partial class AddedMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GivenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateLastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InitialForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PriceCategory = table.Column<int>(type: "int", nullable: false),
                    SpaceCategory = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InitialForm_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RentItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Rooms = table.Column<int>(type: "int", nullable: false),
                    Surface = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<Point>(type: "geography", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Radius = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateLastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentItem_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPreferredLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Location = table.Column<Point>(type: "geography", nullable: true),
                    Radius = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferredLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreferredLocation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFromId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserToId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RentItemId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Media = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateServer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateSentFromServer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateViewed = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_RentItem_RentItemId",
                        column: x => x.RentItemId,
                        principalTable: "RentItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_UserFromId",
                        column: x => x.UserFromId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_User_UserToId",
                        column: x => x.UserToId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RentItemImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RentItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentItemImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentItemImage_RentItem_RentItemId",
                        column: x => x.RentItemId,
                        principalTable: "RentItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRentItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RentItemId = table.Column<int>(type: "int", nullable: false),
                    Liked = table.Column<bool>(type: "bit", nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateViewd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateInteraction = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRemoved = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "IX_InitialForm_UserId",
                table: "InitialForm",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Message_RentItemId",
                table: "Message",
                column: "RentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserFromId",
                table: "Message",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserToId",
                table: "Message",
                column: "UserToId");

            migrationBuilder.CreateIndex(
                name: "IX_RentItem_UserId",
                table: "RentItem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentItemImage_RentItemId",
                table: "RentItemImage",
                column: "RentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferredLocation_UserId",
                table: "UserPreferredLocation",
                column: "UserId");

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
                name: "InitialForm");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "RentItemImage");

            migrationBuilder.DropTable(
                name: "UserPreferredLocation");

            migrationBuilder.DropTable(
                name: "UserRentItem");

            migrationBuilder.DropTable(
                name: "RentItem");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
