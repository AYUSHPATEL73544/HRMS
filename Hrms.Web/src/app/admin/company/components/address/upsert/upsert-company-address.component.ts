import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { CompanyService } from "src/app/admin/services";
import { BaseService, CityService, StateService } from "src/app/shared/services";
import { AddressModel } from "src/app/shared/models/address.model";
import { CompanyModel } from "src/app/admin/company/models";
import { SelectListItemModel } from "src/app/shared/models";
import { Constants } from "src/app/utilities";

@Component({
  selector: 'app-upsert-company-address',
  templateUrl: './upsert-company-address.component.html',
})

export class UpsertCompanyAddressComponent implements OnInit {
  @BlockUI('company-blockui') blockUI: NgBlockUI;
  model = new CompanyModel();
  addressModel = new AddressModel();
  line1:string;
  line2:string;
  countries = new Array<SelectListItemModel>();
  states = new Array<SelectListItemModel>();
  cities = new Array<SelectListItemModel>();
  isModelLoaded: boolean;
  addressType: any;

  constructor(@Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<UpsertCompanyAddressComponent>,
    private companyService: CompanyService,
    private stateService: StateService,
    private cityService: CityService,
    private baseService: BaseService,
  ) {
    this.isModelLoaded = false;
    if (data) {
      this.model = data.model;
      this.addressModel.addressType = data.addressType;
      this.addressType = data.addressType;
    }
    
    if (this.addressModel.addressType == Constants.addressType.corporate) {
      if (this.model.corporateOffice !== null) {
        this.addressModel = this.model.corporateOffice;
        this.line1 = this.addressModel.line1;
        this.line2 = this.addressModel.line2;
      
      }
    }
    else if (this.addressModel.addressType === Constants.addressType.registered) {
      if (this.model.registeredOffice !== null) {
        this.addressModel = this.model.registeredOffice;
        this.line1 = this.addressModel.line1;
        this.line2 = this.addressModel.line2;
      }
    }

  }

  ngOnInit(): void {
    this.loadStates();
  }

  loadStates(): void {
    const selectedCountry = this.countries.find(x => x.key === Constants.defaultCountryId);
    if (selectedCountry) {
      this.addressModel.countryName = selectedCountry.value;
    }
    this.isModelLoaded = false;
    this.blockUI.start();
    this.stateService.getSelectListItem(Constants.defaultCountryId).subscribe({
      next: (response) => {
        Object.assign(this.states, response);
        
        if (this.addressModel.stateId === -1) {
          this.addressModel.stateId = null;
          this.addressModel.stateName = '';
          this.addressModel.cityId = null;
          this.addressModel.cityName = '';
        }
        if (this.addressModel.stateId) {
          this.loadCities();
        }
        this.blockUI.stop();
        this.isModelLoaded = true;
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.isModelLoaded = true;
        this.baseService.processErrorResponse(error);
      }
    });
  }

  loadCities(): void {
    const selectedState = this.states.find(x => x.key === this.addressModel.stateId);
    if (selectedState) {
      this.addressModel.stateName = selectedState.value;
      this.isModelLoaded = false;
      this.blockUI.start();
      this.cityService.getSelectListItem(this.addressModel.stateId).subscribe({
        next: (response) => {
          Object.assign(this.cities, response);
          this.blockUI.stop();
          this.isModelLoaded = true;
        },
        error: (error: any) => {
          this.blockUI.stop();
          this.isModelLoaded = true;
          this.baseService.processErrorResponse(error);
        }
      });
    }
  }


  cancel(): void {
    this.dialogRef.close();
  }

  updateAddressLine(): void{
    if (this.addressType === Constants.addressType.registered) {
      this.model.registeredOffice.line1 = this.line1;
      this.model.registeredOffice.line2 = this.line2;
    }
    else if (this.addressType === Constants.addressType.corporate) {
      this.model.corporateOffice.line1 = this.line1;
      this.model.corporateOffice.line2 = this.line2;
    }
  }

  submit(): void {
    if (this.addressModel.addressType === Constants.addressType.registered) {
      this.model.registeredOffice = this.addressModel;
    }
    else if (this.addressModel.addressType === Constants.addressType.corporate) {
      this.model.corporateOffice = this.addressModel;
    }
    this.updateAddressLine();
   
    this.isModelLoaded = false;
    this.blockUI.start();
    this.companyService.update(this.model).subscribe({
      next: () => {
        this.baseService.successNotification('Details has been updated successfully');
        this.dialogRef.close();
        this.blockUI.stop();
        this.isModelLoaded = true;
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.isModelLoaded = true;
        this.baseService.processErrorResponse(error);
      }
    });
  }
}