using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingLotApi.Migrations
{
    public partial class AddOrdersInParkinglotEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderNumber = table.Column<Guid>(nullable: false),
                    NameOfParkingLot = table.Column<string>(nullable: true),
                    PlateNumber = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    ClosedTime = table.Column<DateTime>(nullable: false),
                    OrderStatus = table.Column<string>(nullable: true),
                    ParkinglotEntityID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderEntity_Parkinglots_ParkinglotEntityID",
                        column: x => x.ParkinglotEntityID,
                        principalTable: "Parkinglots",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderEntity_ParkinglotEntityID",
                table: "OrderEntity",
                column: "ParkinglotEntityID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderEntity");
        }
    }
}
