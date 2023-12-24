using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Passport.WebApi.Migrations.PassportDb
{
    /// <inheritdoc />
    public partial class UserFieldsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SteamTradeLink",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SteamTradeLink",
                table: "AspNetUsers");
        }
    }
}
