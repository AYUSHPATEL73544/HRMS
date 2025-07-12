import { Component, Input } from '@angular/core';
import { CompanyModel } from 'src/app/employee/company/models/index';

@Component({
  selector: 'app-company-address',
  templateUrl: './company-address.component.html',
})

export class CompanyAddressComponent {
  @Input() model: CompanyModel;
} 
