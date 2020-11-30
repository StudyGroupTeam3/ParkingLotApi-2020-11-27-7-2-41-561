using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingLotApi.Migrations
{
    public partial class Delete_foreign_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ParkingLots_ParkingLotName",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ParkingLotName",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "ParkingLotName",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParkingLotEntityName",
                table: "Orders",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "ParkingLotName",
                table: "Orders",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ParkingLotName",
                table: "Orders",
                column: "ParkingLotName");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ParkingLots_ParkingLotName",
                table: "Orders",
                column: "ParkingLotName",
                principalTable: "ParkingLots",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
