using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentStatusAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaxpeerItem");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Payment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InventoryAsset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    SteamGame = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    WaxpeerPaymentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAsset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAsset_Payment_WaxpeerPaymentId",
                        column: x => x.WaxpeerPaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAsset_WaxpeerPaymentId",
                table: "InventoryAsset",
                column: "WaxpeerPaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryAsset");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payment");

            migrationBuilder.CreateTable(
                name: "WaxpeerItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsInstant = table.Column<bool>(type: "boolean", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    WaxpeerPaymentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaxpeerItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaxpeerItem_Payment_WaxpeerPaymentId",
                        column: x => x.WaxpeerPaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaxpeerItem_WaxpeerPaymentId",
                table: "WaxpeerItem",
                column: "WaxpeerPaymentId");
        }
    }
}
