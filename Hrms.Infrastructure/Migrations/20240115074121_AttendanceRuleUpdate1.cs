using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hrms.Infrastructure.Migrations
{
    public partial class AttendanceRuleUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "AttendanceRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AttendanceRules",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveFrom",
                table: "AttendanceRules",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveTo",
                table: "AttendanceRules",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "AttendanceRules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "AttendanceRules",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AttendanceRules");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AttendanceRules");

            migrationBuilder.DropColumn(
                name: "EffectiveFrom",
                table: "AttendanceRules");

            migrationBuilder.DropColumn(
                name: "EffectiveTo",
                table: "AttendanceRules");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "AttendanceRules");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "AttendanceRules");
        }
    }
}
