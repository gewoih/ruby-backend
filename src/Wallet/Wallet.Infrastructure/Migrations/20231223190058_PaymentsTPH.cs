using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentsTPH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaxpeerItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsInstant = table.Column<bool>(type: "boolean", nullable: false),
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaxpeerItem");

            migrationBuilder.DropTable(
                name: "Payment");
        }
    }
}
