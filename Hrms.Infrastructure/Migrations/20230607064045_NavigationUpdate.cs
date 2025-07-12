using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hrms.Infrastructure.Migrations
{
    public partial class NavigationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkHistories_DepartmentId",
                table: "WorkHistories",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHistories_DesignationId",
                table: "WorkHistories",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHistories_EmployeeId",
                table: "WorkHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_EmployeeId",
                table: "Leaves",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_RuleId",
                table: "Leaves",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRules_CompanyId",
                table: "LeaveRules",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveLogs_EmployeeId",
                table: "LeaveLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveLogs_RuleId",
                table: "LeaveLogs",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_EmployeeId",
                table: "Families",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_RelationshipId",
                table: "Families",
                column: "RelationshipId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DesignationId",
                table: "Employees",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EduactionDetails_CourseTypeId",
                table: "EduactionDetails",
                column: "CourseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EduactionDetails_EmployeeId",
                table: "EduactionDetails",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EduactionDetails_QualificationTypeId",
                table: "EduactionDetails",
                column: "QualificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeId",
                table: "Attendances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRules_CompanyId",
                table: "AttendanceRules",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRules_Companies_CompanyId",
                table: "AttendanceRules",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Employees_EmployeeId",
                table: "Attendances",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EduactionDetails_CourseTypes_CourseTypeId",
                table: "EduactionDetails",
                column: "CourseTypeId",
                principalTable: "CourseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EduactionDetails_Employees_EmployeeId",
                table: "EduactionDetails",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EduactionDetails_QualificationTypes_QualificationTypeId",
                table: "EduactionDetails",
                column: "QualificationTypeId",
                principalTable: "QualificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                table: "Employees",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Designations_DesignationId",
                table: "Employees",
                column: "DesignationId",
                principalTable: "Designations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_UserId",
                table: "Employees",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Employees_EmployeeId",
                table: "Families",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Relationships_RelationshipId",
                table: "Families",
                column: "RelationshipId",
                principalTable: "Relationships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveLogs_Employees_EmployeeId",
                table: "LeaveLogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveLogs_LeaveRules_RuleId",
                table: "LeaveLogs",
                column: "RuleId",
                principalTable: "LeaveRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRules_Companies_CompanyId",
                table: "LeaveRules",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Employees_EmployeeId",
                table: "Leaves",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_LeaveRules_RuleId",
                table: "Leaves",
                column: "RuleId",
                principalTable: "LeaveRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHistories_Departments_DepartmentId",
                table: "WorkHistories",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHistories_Designations_DesignationId",
                table: "WorkHistories",
                column: "DesignationId",
                principalTable: "Designations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHistories_Employees_EmployeeId",
                table: "WorkHistories",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRules_Companies_CompanyId",
                table: "AttendanceRules");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Employees_EmployeeId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_EduactionDetails_CourseTypes_CourseTypeId",
                table: "EduactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EduactionDetails_Employees_EmployeeId",
                table: "EduactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EduactionDetails_QualificationTypes_QualificationTypeId",
                table: "EduactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Designations_DesignationId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_UserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_Employees_EmployeeId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_Relationships_RelationshipId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveLogs_Employees_EmployeeId",
                table: "LeaveLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveLogs_LeaveRules_RuleId",
                table: "LeaveLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRules_Companies_CompanyId",
                table: "LeaveRules");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Employees_EmployeeId",
                table: "Leaves");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_LeaveRules_RuleId",
                table: "Leaves");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkHistories_Departments_DepartmentId",
                table: "WorkHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkHistories_Designations_DesignationId",
                table: "WorkHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkHistories_Employees_EmployeeId",
                table: "WorkHistories");

            migrationBuilder.DropIndex(
                name: "IX_WorkHistories_DepartmentId",
                table: "WorkHistories");

            migrationBuilder.DropIndex(
                name: "IX_WorkHistories_DesignationId",
                table: "WorkHistories");

            migrationBuilder.DropIndex(
                name: "IX_WorkHistories_EmployeeId",
                table: "WorkHistories");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_EmployeeId",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_RuleId",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRules_CompanyId",
                table: "LeaveRules");

            migrationBuilder.DropIndex(
                name: "IX_LeaveLogs_EmployeeId",
                table: "LeaveLogs");

            migrationBuilder.DropIndex(
                name: "IX_LeaveLogs_RuleId",
                table: "LeaveLogs");

            migrationBuilder.DropIndex(
                name: "IX_Families_EmployeeId",
                table: "Families");

            migrationBuilder.DropIndex(
                name: "IX_Families_RelationshipId",
                table: "Families");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DesignationId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_EduactionDetails_CourseTypeId",
                table: "EduactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_EduactionDetails_EmployeeId",
                table: "EduactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_EduactionDetails_QualificationTypeId",
                table: "EduactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_EmployeeId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRules_CompanyId",
                table: "AttendanceRules");
        }
    }
}
