using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transactions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BalanceTransaction_Reworked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "BalanceTransactions",
                newName: "TriggerId");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "BalanceTransactions",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "AdjustmentAmount",
                table: "BalanceTransactions",
                newName: "Amount");

            migrationBuilder.AddColumn<int>(
                name: "TriggerType",
                table: "BalanceTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggerType",
                table: "BalanceTransactions");

            migrationBuilder.RenameColumn(
                name: "TriggerId",
                table: "BalanceTransactions",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "BalanceTransactions",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "BalanceTransactions",
                newName: "AdjustmentAmount");
        }
    }
}
