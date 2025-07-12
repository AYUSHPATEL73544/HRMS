import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeGuard } from 'src/app/guards/employee.guard';
import { EmployeeComponent } from 'src/app/employee/employee.component';
import { EmployeeDashboardComponent } from './components/dashboard/employee-dashboard.component';
import { CompanyManagerComponent } from './company/components';
import { AttendanceHistoryComponent, AttendanceManageComponent, AttendanceRulesDetailComponent } from './attendance/components';
import { LeaveRuleDetailComponent, LeavesManagerComponent, UpsertLeavesComponent } from './leave/components';
import { EmployeeDirectoryComponent } from './directory/employee/employee-directory.component';
import { ProfileManageComponent } from './profile/component/manage/profile-manage.component';
import { FamilyDetailComponent } from './profile/component';
import { IntervieweeComponent, IntervieweeDetailComponent } from './job-recruitment/component';
import { ReimbursementComponent } from './reimbursement/components/manage/reimbursement-manage.component';
import { CandidateComponent, CandidateDetailComponent } from './job-application/components';

const routes: Routes = [
  {
    path: '', component: EmployeeComponent, runGuardsAndResolvers: 'always',
    children: [
      { path: 'dashboard', component: EmployeeDashboardComponent, canActivate: [EmployeeGuard] },
      { path: 'company', component: CompanyManagerComponent },
      { path: 'profile', component: ProfileManageComponent },
      { path: 'directory', component: EmployeeDirectoryComponent },
      { path: 'attendance', component: AttendanceManageComponent },
      { path: 'attendance-rule-detail/:id', component: AttendanceRulesDetailComponent },
      { path: 'attendance-history/:id', component: AttendanceHistoryComponent },
      { path: 'leave', component: LeavesManagerComponent },
      { path: 'apply-leave', component: UpsertLeavesComponent },
      { path: 'rule-detail/:id', component: LeaveRuleDetailComponent },
      { path: 'family', component: FamilyDetailComponent },
      { path: 'interviewee', component: IntervieweeComponent },
      { path: 'interviewee-detail/:id', component: IntervieweeDetailComponent },
      { path: 'candidate', component: CandidateComponent},
      { path: 'candidate-detail/:id', component: CandidateDetailComponent },
      { path: 'reimbursement', component: ReimbursementComponent },
      { path: 'reimbursement/:id', component: ReimbursementComponent },
    ]
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class EmployeeRoutingModule { }