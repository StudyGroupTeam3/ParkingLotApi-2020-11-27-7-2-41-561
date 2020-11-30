using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingLotApi.Migrations
{
    public partial class Set_name_as_primarykey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingLots",
                table: "ParkingLots");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ParkingLots");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ParkingLots",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingLots",
                table: "ParkingLots",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingLots",
                table: "ParkingLots");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ParkingLots",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ParkingLots",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingLots",
                table: "ParkingLots",
                column: "Id");
        }
    }
}
