using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingLotApi.Migrations
{
    public partial class ChangeTableNameToParkinglots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Parkinglots");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parkinglots",
                table: "Parkinglots",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Parkinglots",
                table: "Parkinglots");

            migrationBuilder.RenameTable(
                name: "Parkinglots",
                newName: "Companies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "ID");
        }
    }
}
