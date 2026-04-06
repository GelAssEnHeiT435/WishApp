using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WishListServer.Migrations
{
    /// <inheritdoc />
    public partial class AuthAndWishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WishlistId",
                table: "Wishes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Credential",
                columns: table => new
                {
                    CredId = table.Column<Guid>(type: "uuid", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credential", x => x.CredId);
                    table.ForeignKey(
                        name: "FK_Credential_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    WishlistId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShareToken = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.WishlistId);
                    table.ForeignKey(
                        name: "FK_Wishlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wishes_WishlistId",
                table: "Wishes",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishes_WishlistId_IsReceived",
                table: "Wishes",
                columns: new[] { "WishlistId", "IsReceived" });

            migrationBuilder.CreateIndex(
                name: "IX_Credential_UserId",
                table: "Credential",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_ShareToken",
                table: "Wishlists",
                column: "ShareToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishes_Wishlists_WishlistId",
                table: "Wishes",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "WishlistId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishes_Wishlists_WishlistId",
                table: "Wishes");

            migrationBuilder.DropTable(
                name: "Credential");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Wishes_WishlistId",
                table: "Wishes");

            migrationBuilder.DropIndex(
                name: "IX_Wishes_WishlistId_IsReceived",
                table: "Wishes");

            migrationBuilder.DropColumn(
                name: "WishlistId",
                table: "Wishes");
        }
    }
}
