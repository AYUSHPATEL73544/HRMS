using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hrms.Infrastructure.Migrations
{
    public partial class UpdateFamily1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Families_RelationshipId",
                table: "Families");

            migrationBuilder.CreateIndex(
                name: "IX_Families_RelationshipId",
                table: "Families",
                column: "RelationshipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Families_RelationshipId",
                table: "Families");

            migrationBuilder.CreateIndex(
                name: "IX_Families_RelationshipId",
                table: "Families",
                column: "RelationshipId",
                unique: true);
        }
    }
}
