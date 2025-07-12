import { NgModule } from '@angular/core';
import { EmployeeGuard } from 'src/app/guards';
import { SharedModule } from 'src/app/shared/shared.module';
import { EmployeeRoutingModule } from 'src/app/employee/employee-routing.module';
import { EmployeeComponent } from 'src/app/employee/employee.component';
import { EmployeeHeaderComponent } from './components/header/employee-header.component';
import { EmployeeDashboardComponent } from './components/dashboard/employee-dashboard.component';
import { CompanyAddressComponent, CompanyManagerComponent, CompanyOverviewComponent } from './company/components';
import { CompanyService, CourseTypeService, DepartmentService, DesignationService, EmployeeService, FamilyServices, LeaveLogService, LeaveService, QualificationTypeService, TeamService, WorkHistoryService, CandidateService, CalendarService, IntervieweeService, SkillService } from './services';


import { AttendanceService } from './services/attendance.service';
import { AttendanceLogService } from './services/attendance-log.service';
import {
  LeaveLogsComponent,
  LeaveRuleComponent, LeavesManagerComponent, StatsDetailsComponent,
  UpsertLeavesComponent
} from './leave/components';
import { LeaveRuleService } from './services/leave-rule.service';
import { LeaveRuleDetailComponent } from './leave/components/rule/detail/leave-rule-detail.component';
import { EducationService } from './services/education.service';
import { AdminNotesViewComponent, AttendanceHistoryComponent, AttendanceLogDetailComponent, AttendanceManageComponent, AttendanceRuleManageComponent, AttendanceRulesDetailComponent, CalendarEventComponent } from './attendance/components';
import { EmployeeDirectoryComponent } from './directory/employee/employee-directory.component';
import { CommonModule } from '@angular/common';
import { LeaveLogRejectComponent } from './leave/components/log-reject-details/leave-log-reject.component';
import { LeaveLogReportingComponent } from './leave/components';
import { EmployeeAttendanceHistoryComponent } from './attendance/components/employee-attendance-history/employee-attendance-history.component';
import { EducationDetailsComponent } from './profile/component/education/detail/education-detail.component';
import { UpsertEducationComponent } from './profile/component/education/upsert/upsert-education.component';
import { ProfileManageComponent } from './profile/component/manage/profile-manage.component';
import { PersonalInfoComponent } from './profile/component/personal-info/personal-info.component';
import { TeamDetailComponent } from './profile/component/team/detail/team-detail.component';
import { WorkInfoComponent } from './profile/component/work-info/work-info.component';
import { EducationViewDetailComponent } from './profile/component/education/view-detail/education-view-detail.component';
import { FamilyDetailComponent } from './profile/component/family/detail/family-detail.component';
import { UpsertFamilyComponent } from './profile/component/family/upsert/upsert-family.component';
import { IntervieweeComponent, IntervieweeDetailComponent, UpsertIntervieweeInterviewComponent } from './job-recruitment/component';
import { ReimbursementComponent } from './reimbursement/components/manage/reimbursement-manage.component';

import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { UpsertReimbursementComponent } from './reimbursement/components';
import { ReimbursementService } from './services/reimbursement.service';
import { ProfileImageComponent } from './profile/profile-image/profile-image.component';
import { CandidateComponent, CandidateDetailComponent, UpsertCandidateComponent, UpsertHireComponent, UpsertInterviewComponent } from './job-application/components';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    EmployeeRoutingModule,

    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  providers: [
    EmployeeGuard,
    CompanyService,
    DepartmentService,
    DesignationService,
    EmployeeService,
    AttendanceService,
    AttendanceLogService,
    LeaveLogService,
    LeaveRuleService,
    LeaveService,
    FamilyServices,
    TeamService,
    EducationService,
    CourseTypeService,
    QualificationTypeService,
    WorkHistoryService,
    IntervieweeService,
    ReimbursementService,
    CalendarService,
    CandidateService,
    SkillService
  ],
  declarations: [
    EmployeeComponent,
    EmployeeHeaderComponent,
    EmployeeDashboardComponent,
    CompanyAddressComponent,
    CompanyManagerComponent,
    CompanyOverviewComponent,
    EducationDetailsComponent,
    UpsertEducationComponent,
    ProfileManageComponent,
    PersonalInfoComponent,
    ProfileImageComponent,
    TeamDetailComponent,
    WorkInfoComponent,
    EmployeeDirectoryComponent,
    EducationViewDetailComponent,
    EmployeeAttendanceHistoryComponent,


    AttendanceManageComponent,
    AttendanceHistoryComponent,
    AttendanceLogDetailComponent,
    AttendanceRulesDetailComponent,
    AttendanceRuleManageComponent,
    CalendarEventComponent,

    LeavesManagerComponent,
    UpsertLeavesComponent,
    LeaveRuleComponent,
    StatsDetailsComponent,
    LeaveRuleDetailComponent,
    LeaveLogsComponent,
    FamilyDetailComponent,
    UpsertFamilyComponent,
    LeaveLogRejectComponent,
    LeaveLogReportingComponent,
    AdminNotesViewComponent,
    IntervieweeComponent,
    IntervieweeDetailComponent,
    UpsertIntervieweeInterviewComponent,

    ReimbursementComponent,
    UpsertReimbursementComponent,

    CandidateComponent,
    CandidateDetailComponent,
    UpsertCandidateComponent,
    UpsertHireComponent,
    UpsertInterviewComponent
  ]
})

export class EmployeeModule { }