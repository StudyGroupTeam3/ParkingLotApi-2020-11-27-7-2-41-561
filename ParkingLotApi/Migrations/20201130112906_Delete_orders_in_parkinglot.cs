using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingLotApi.Migrations
{
    public partial class Delete_orders_in_parkinglot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ParkingLots_ParkingLotEntityName",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ParkingLotEntityName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ParkingLotEntityName",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParkingLotEntityName",
                table: "Orders",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ParkingLotEntityName",
                table: "Orders",
                column: "ParkingLotEntityName");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ParkingLots_ParkingLotEntityName",
                table: "Orders",
                column: "ParkingLotEntityName",
                principalTable: "ParkingLots",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
