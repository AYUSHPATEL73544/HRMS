import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminGuard } from 'src/app/guards';
import { AdminComponent } from './admin.component';
import { AttendanceHistoryComponent, AttendanceManageComponent, AttendanceRuleDetailComponent } from './attendance/components';
import { CompanyManageComponent } from './company/components';
import {
    DashboardComponent,
} from './components';
import { EmployeeProfileManageComponent, PersonalDetailComponent } from './directory/components';
import { EmployeesDirectoryComponent } from './directory/components/employees-directory/detail/employees-directory.component';
import { LeaveRuleDetailComponent, LeavesManageComponent } from './leave/components';
import { LeaveHistoryComponent } from './leave/components/leave-history/leave-history.component';
import { AssetManageComponent } from './asset/component/manage/asset-manage.component';
import { CandidateComponent, CandidateDetailComponent } from './jobApplication/component';
import { ReimbursementComponent } from './reimbursement/components';
import { ReimbursementHistoryComponent } from './reimbursement/components/history/reimbursement-history.component';

const routes: Routes = [
    {
        path: '', component: AdminComponent, runGuardsAndResolvers: 'always',
        children: [
            { path: 'dashboard', component: DashboardComponent, canActivate: [AdminGuard] },
            { path: 'company', component: CompanyManageComponent },
            { path: 'leave', component: LeavesManageComponent },
            { path: 'leave-history/:id', component: LeaveHistoryComponent },
            { path: 'rule-detail/:id', component: LeaveRuleDetailComponent },
            { path: 'attendance', component: AttendanceManageComponent },
            { path: 'attendance-history/:id', component: AttendanceHistoryComponent },
            { path: 'attendance-rule-detail/:id', component: AttendanceRuleDetailComponent },
            { path: 'directory', component: EmployeesDirectoryComponent },
            { path: 'manager/:id', component: EmployeeProfileManageComponent },
            { path: 'personal', component: PersonalDetailComponent },
            { path: 'asset', component: AssetManageComponent },
            { path: 'candidate', component: CandidateComponent },
            { path: 'candidate-detail/:id', component: CandidateDetailComponent },
            { path: 'reimbursement', component: ReimbursementComponent },
            { path: 'reimbursement-history/:id', component: ReimbursementHistoryComponent },
            { path: '', redirectTo: '/admin/dashboard', pathMatch: 'full' },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class AdminRoutingModule { }
