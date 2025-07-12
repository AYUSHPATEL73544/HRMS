using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hrms.Infrastructure.Migrations
{
    public partial class UdpateLeave1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Leaves",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Leaves",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveFrom",
                table: "Leaves",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveTo",
                table: "Leaves",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Leaves",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Leaves",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "LeaveRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "LeaveRules",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveFrom",
                table: "LeaveRules",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveTo",
                table: "LeaveRules",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "LeaveRules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "LeaveRules",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "EffectiveFrom",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "EffectiveTo",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "LeaveRules");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "LeaveRules");

            migrationBuilder.DropColumn(
                name: "EffectiveFrom",
                table: "LeaveRules");

            migrationBuilder.DropColumn(
                name: "EffectiveTo",
                table: "LeaveRules");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "LeaveRules");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "LeaveRules");
        }
    }
}
