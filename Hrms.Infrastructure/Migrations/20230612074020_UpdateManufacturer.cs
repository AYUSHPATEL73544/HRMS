using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hrms.Infrastructure.Migrations
{
    public partial class UpdateManufacturer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssetTypeId",
                table: "Manufacturers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ManufacturerId",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_AssetTypeId",
                table: "Manufacturers",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ManufacturerId",
                table: "Assets",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Manufacturers_ManufacturerId",
                table: "Assets",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manufacturers_AssetTypes_AssetTypeId",
                table: "Manufacturers",
                column: "AssetTypeId",
                principalTable: "AssetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Manufacturers_ManufacturerId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Manufacturers_AssetTypes_AssetTypeId",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_AssetTypeId",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Assets_ManufacturerId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "AssetTypeId",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "Assets");
        }
    }
}
