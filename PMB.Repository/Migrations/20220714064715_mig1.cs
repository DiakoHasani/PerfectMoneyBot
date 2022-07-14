using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMB.Repository.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblPriceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreteDate = table.Column<DateTime>(nullable: false),
                    Ex4Ir = table.Column<double>(nullable: false),
                    Payfa24 = table.Column<double>(nullable: false),
                    HdPay = table.Column<double>(nullable: false),
                    IraniCard = table.Column<double>(nullable: false),
                    Nobitex = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPriceHistories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblPriceHistories");
        }
    }
}
