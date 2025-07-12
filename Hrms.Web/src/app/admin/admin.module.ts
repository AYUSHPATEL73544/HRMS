import { NgModule } from '@angular/core';
import { AdminGuard } from 'src/app/guards';
import { SharedModule } from 'src/app/shared/shared.module';
import { AdminComponent } from './admin.component';

import {
  HeaderComponent, DashboardComponent,
} from './components';

import { AdminRoutingModule } from './admin-routing.module';
import {
  CompanyAddressComponent, CompanyManageComponent, CompanyOverviewComponent,
  DepartmentDetailComponent, DesignationDetailComponent, UpsertCompanyAddressComponent, UpsertCompanyOverviewComponent, UpsertDepartmentComponent,
  UpsertDesignationComponent
} from './company/components';
import {
  CompanyService,
  DepartmentService,
  DesignationService,
  LeaveService,
  AttendanceService,
  AttendanceLogService,
  EmployeeService,
  LeaveRuleService,
  FamilyService,
  TeamService,
  CourseTypeService,
  QualificationTypeService,
  AttendanceRuleService,
  WorkHistoryService,
  AssetService,
  AssetTypeService,
  ManufacturerService,
  VariantService,
  AssetAssignService,
  AssignHistoryService,
  CandidateService,
  SkillService,
  NoteService,
  UserRoleService,
  CalendarService

} from 'src/app/admin/services';


import { LeaveLogService } from './services/leave-log.services';

import { EducationService } from './services/education.service';
import { EmployeeAttendanceRuleService } from './services/employee-attendance.rule.service';
import { DocumentDetailComponent, EducationDetailComponent, EmployeeProfileManageComponent, EmployeesDirectoryComponent, FamilyDetailComponent, NoteDetailComponent, PersonalDetailComponent, ResetPasswordComponent, TeamDetailComponent, UpdateWorkDetailComponent, UpsertEmployeesDirectoryComponent, UpsertNoteComponent, UpsertTeamComponent, WorkDetailComponent } from './directory/components';
import { AddLeaveRuleComponent, AssignLeaveRuleDetailComponent, LeaveApproveDialogComponent, LeaveBalanceComponent, LeaveLogsComponent, LeaveRuleDetailComponent, LeaveRuleManageComponent, LeavesManageComponent, PendingLeavesComponent, UpsertLeaveCountComponent } from './leave/components';
import { AssignRuleComponent, AssignRuleDetailComponent, AttendanceHistoryComponent, AttendanceLogDetailComponent, AttendanceManageComponent, AttendanceRuleDetailComponent, AttendanceRuleManageComponent, CalendarEventComponent, EmployeeAttendanceHistoryComponent, UpsertAttendanceLogComponent, UpsertAttendanceRuleDetailComponent } from './attendance/components';
import { LeaveLogDetailsComponent } from './leave/components/leave-log-details/leave-log-details.component';
import { CommonModule } from '@angular/common';

import { UpsertHolidayComponent } from './holidays/Components/upsert/upsert-holiday.component';
import { HolidayService } from './services/holiday.service';
import { EmployeeStatusComponent } from './directory/components/work/Status-dailog/employee-status.component';
import { EducationViewDetailComponent } from './directory/components/education/detail/detail/education-view-detail.component';
import { AttendanceViewNoteComponent } from './attendance/components/attendance-history/view-note/attendance-view-note.component';
import { LeaveHistoryComponent } from './leave/components/leave-history/leave-history.component';
import { AssetManageComponent } from './asset/component/manage/asset-manage.component';
import { UpsertAssetComponent } from './asset/component/upsert/upsert-asset.component';
import { AssetAssignComponent, AssignHistoryComponent } from './asset/component';
import { CandidateComponent, CandidateDetailComponent,  UpsertCandidateComponent, UpsertInterviewComponent , UpsertHireComponent } from './jobApplication/component';
import { PendingReimbursementComponent, ReimbursementComponent, ReimbursementLogComponent } from './reimbursement/components';
import { ReimbursementService } from './services/reimbursement.service';
import { ReimbursementHistoryComponent } from './reimbursement/components/history/reimbursement-history.component';
import { ApproveReimbursementComponent } from './reimbursement/components/approve-reimbursement/approve-reimbursement.component';


@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    AdminRoutingModule
  ],
  providers: [
    AdminGuard,
    CompanyService,
    DepartmentService,
    DesignationService,
    LeaveService,
    AttendanceService,
    LeaveLogService,
    LeaveRuleService,
    AttendanceLogService,
    EmployeeService,
    FamilyService,
    TeamService,
    EducationService,
    QualificationTypeService,
    CourseTypeService,
    AttendanceRuleService,
    EmployeeAttendanceRuleService,
    HolidayService,
    WorkHistoryService,
    AssetService,
    AssetTypeService,
    ManufacturerService,
    VariantService,
    AssetAssignService,
    AssignHistoryService,
    CandidateService,
    SkillService,
    NoteService,
    UserRoleService,
    ReimbursementService,
    CalendarService
  ],
  declarations: [
    AdminComponent,
    DocumentDetailComponent,
    EducationDetailComponent,
    EmployeesDirectoryComponent,
    UpsertEmployeesDirectoryComponent,
    FamilyDetailComponent,
    EmployeeProfileManageComponent,
    PersonalDetailComponent,
    TeamDetailComponent,
    UpsertTeamComponent,
    WorkDetailComponent,
    EmployeeStatusComponent,
    EducationViewDetailComponent,
    AttendanceViewNoteComponent,
    AssetManageComponent,
    UpsertAssetComponent,
    AssetAssignComponent,
    AssignHistoryComponent,

    UpsertHolidayComponent,
    UpsertNoteComponent,
    NoteDetailComponent,
    AssignLeaveRuleDetailComponent,
    LeaveBalanceComponent,
    LeaveRuleManageComponent,
    AddLeaveRuleComponent,
    LeaveLogsComponent,
    LeaveLogDetailsComponent,
    LeavesManageComponent,
    LeaveRuleDetailComponent,
    UpsertLeaveCountComponent,
    PendingLeavesComponent,
    LeaveHistoryComponent,

    DashboardComponent,
    HeaderComponent,

    CompanyAddressComponent,
    UpsertCompanyAddressComponent,
    DepartmentDetailComponent,
    UpsertDepartmentComponent,
    DesignationDetailComponent,
    UpsertDesignationComponent,
    CompanyManageComponent,
    CompanyOverviewComponent,
    UpsertCompanyOverviewComponent,

    AssignRuleDetailComponent,
    AssignRuleComponent,
    AttendanceHistoryComponent,
    AttendanceLogDetailComponent,
    UpsertAttendanceLogComponent,
    AttendanceRuleDetailComponent,
    AttendanceRuleDetailComponent,
    AttendanceRuleManageComponent,
    UpsertAttendanceRuleDetailComponent,
    AttendanceManageComponent,
    EmployeeAttendanceHistoryComponent,
    UpdateWorkDetailComponent,

    ResetPasswordComponent,
    CandidateComponent,
    UpsertCandidateComponent,
    CandidateDetailComponent,
    UpsertInterviewComponent,
    UpsertHireComponent,
    LeaveApproveDialogComponent,
    ReimbursementComponent,
    ReimbursementHistoryComponent,
    ApproveReimbursementComponent,
    PendingReimbursementComponent,
    ReimbursementLogComponent,
    CalendarEventComponent
  ],
  entryComponents: []
})

export class AdminModule { }
