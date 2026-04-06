using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WishListServer.Migrations
{
    /// <inheritdoc />
    public partial class AddLoginToCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoginHash",
                table: "Credential",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Credential_UserId_LoginHash",
                table: "Credential",
                columns: new[] { "UserId", "LoginHash" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Credential_UserId_LoginHash",
                table: "Credential");

            migrationBuilder.DropColumn(
                name: "LoginHash",
                table: "Credential");
        }
    }
}
