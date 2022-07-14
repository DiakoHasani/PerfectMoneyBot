using Microsoft.EntityFrameworkCore.Migrations;

namespace PMB.Repository.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvvalMoney",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "Ex4Ir",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "HdPay",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "IraniCard",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "Nobitex",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "Payfa24",
                table: "TblPriceHistories");

            migrationBuilder.AddColumn<double>(
                name: "AvvalMoneyBuyPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AvvalMoneySellPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ex4IrBuyPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ex4IrSellPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HdPayBuyPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HdPaySellPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "IraniCardBuyPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "IraniCardSellPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NobitexBuyPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NobitexSellPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Payfa24BuyPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Payfa24SellPrice",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvvalMoneyBuyPrice",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "AvvalMoneySellPrice",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "Ex4IrBuyPrice",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "Ex4IrSellPrice",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "HdPayBuyPrice",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "HdPaySellPrice",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "IraniCardBuyPrice",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "IraniCardSellPrice",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "NobitexBuyPrice",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "NobitexSellPrice",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "Payfa24BuyPrice",
                table: "TblPriceHistories");

            migrationBuilder.DropColumn(
                name: "Payfa24SellPrice",
                table: "TblPriceHistories");

            migrationBuilder.AddColumn<double>(
                name: "AvvalMoney",
                table: "TblPriceHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ex4Ir",
                table: "TblPriceHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HdPay",
                table: "TblPriceHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "IraniCard",
                table: "TblPriceHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Nobitex",
                table: "TblPriceHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Payfa24",
                table: "TblPriceHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
