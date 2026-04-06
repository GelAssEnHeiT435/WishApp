using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WishListServer.Migrations
{
    /// <inheritdoc />
    public partial class RenameLoginColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoginHash",
                table: "Credential",
                newName: "Login");

            migrationBuilder.RenameIndex(
                name: "IX_Credential_UserId_LoginHash",
                table: "Credential",
                newName: "IX_Credential_UserId_Login");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Login",
                table: "Credential",
                newName: "LoginHash");

            migrationBuilder.RenameIndex(
                name: "IX_Credential_UserId_Login",
                table: "Credential",
                newName: "IX_Credential_UserId_LoginHash");
        }
    }
}
