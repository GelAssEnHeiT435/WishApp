using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WishListServer.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkColumnToWishes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Wishes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "Wishes");
        }
    }
}
