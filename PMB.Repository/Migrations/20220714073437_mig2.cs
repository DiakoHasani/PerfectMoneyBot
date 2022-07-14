using Microsoft.EntityFrameworkCore.Migrations;

namespace PMB.Repository.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AvvalMoney",
                table: "TblPriceHistories",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvvalMoney",
                table: "TblPriceHistories");
        }
    }
}
