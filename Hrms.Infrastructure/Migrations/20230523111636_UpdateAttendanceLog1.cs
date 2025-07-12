using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hrms.Infrastructure.Migrations
{
    public partial class UpdateAttendanceLog1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "AttendancesLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AttendancesLogs",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveFrom",
                table: "AttendancesLogs",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveTo",
                table: "AttendancesLogs",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "AttendancesLogs",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "AttendancesLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "AttendancesLogs",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AttendancesLogs");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AttendancesLogs");

            migrationBuilder.DropColumn(
                name: "EffectiveFrom",
                table: "AttendancesLogs");

            migrationBuilder.DropColumn(
                name: "EffectiveTo",
                table: "AttendancesLogs");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "AttendancesLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "AttendancesLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "AttendancesLogs");
        }
    }
}
