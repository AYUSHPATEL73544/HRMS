import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminGuard } from 'src/app/guards';
import { AccountLogoutComponent, AuthenticationComponent, AzureAdLoginComponent } from './components';
import { EmployeeGuard } from './guards/employee.guard';

const routes: Routes = [
  { path: '', redirectTo: '/account/login', pathMatch: 'full' },
  { path: 'account/login', component: AuthenticationComponent },
  { path: 'account/logout', component: AccountLogoutComponent },
  { path: 'account/aad-login', component: AzureAdLoginComponent },
  {
    path: 'admin',
    canActivate: [AdminGuard],
    loadChildren: () =>
      import('src/app/admin/admin.module')
      .then((m) => m.AdminModule),
  },
  {
    path: 'employee',
    canActivate: [EmployeeGuard],
    loadChildren: () => import('src/app/employee/employee.module')
    .then((m) => m.EmployeeModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })],
  exports: [RouterModule]
})

export class AppRoutingModule { }
