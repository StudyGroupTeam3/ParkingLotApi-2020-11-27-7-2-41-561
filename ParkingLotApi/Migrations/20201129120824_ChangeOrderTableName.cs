using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingLotApi.Migrations
{
    public partial class ChangeOrderTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderEntity_Parkinglots_ParkinglotEntityID",
                table: "OrderEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderEntity",
                table: "OrderEntity");

            migrationBuilder.RenameTable(
                name: "OrderEntity",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_OrderEntity_ParkinglotEntityID",
                table: "Orders",
                newName: "IX_Orders_ParkinglotEntityID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Parkinglots_ParkinglotEntityID",
                table: "Orders",
                column: "ParkinglotEntityID",
                principalTable: "Parkinglots",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Parkinglots_ParkinglotEntityID",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "OrderEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ParkinglotEntityID",
                table: "OrderEntity",
                newName: "IX_OrderEntity_ParkinglotEntityID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderEntity",
                table: "OrderEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderEntity_Parkinglots_ParkinglotEntityID",
                table: "OrderEntity",
                column: "ParkinglotEntityID",
                principalTable: "Parkinglots",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
