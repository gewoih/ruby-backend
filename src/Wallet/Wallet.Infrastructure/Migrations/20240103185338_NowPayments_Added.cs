using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NowPayments_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Payment",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);

            migrationBuilder.AddColumn<string>(
                name: "CallbackUrl",
                table: "Payment",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayAddress",
                table: "Payment",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PayAmount",
                table: "Payment",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayCurrency",
                table: "Payment",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Payment",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriceCurrency",
                table: "Payment",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallbackUrl",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "PayAddress",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "PayAmount",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "PayCurrency",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "PriceCurrency",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Payment",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(13)",
                oldMaxLength: 13);
        }
    }
}
