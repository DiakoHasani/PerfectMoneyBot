using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMB.Repository.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblErrors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreteDate = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    InnerException = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    MethodName = table.Column<string>(nullable: true),
                    Line = table.Column<int>(nullable: false),
                    Col = table.Column<int>(nullable: false),
                    StackTrace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblErrors", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblErrors");
        }
    }
}
