import { Component, Input } from '@angular/core';
import { CompanyModel } from 'src/app/employee/company/models/index';

@Component({
  selector: 'app-company-overview',
  templateUrl: './company-overview.component.html',
})

export class CompanyOverviewComponent{
  @Input() model: CompanyModel;
}