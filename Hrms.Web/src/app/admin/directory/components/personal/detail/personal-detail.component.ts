import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { EmployeeService } from 'src/app/admin/services';
import { BaseService, CityService, StateService } from 'src/app/shared/services';
import { AddressModel } from 'src/app/shared/models/address.model';
import { EmployeeModel } from 'src/app/admin/directory/models';
import { SelectListItemModel } from 'src/app/shared/models';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
  selector: 'app-personal-detail',
  templateUrl: './personal-detail.component.html',
})
export class PersonalDetailComponent implements OnInit {
  @BlockUI('employee-blockui') blockUI: NgBlockUI;

  model = new EmployeeModel();
  addressModel = new AddressModel();
  countries = new Array<SelectListItemModel>();
  states = new Array<SelectListItemModel>();
  cities = new Array<SelectListItemModel>();

  isPersonalInfoEditable = false;
  isContactInfoEditable = false;
  isCurrentAddressEditable = false;
  isPermanentAddressEditable = false;
  isModelLoaded: boolean;

  gender: string;
  dateOfBirth: string;
  martialStatus: string;
  bloodGroup: string;
  genderDropDown = AppUtils.getGenderForDropDown();
  maritalStatusDropDown = AppUtils.getMaritalStatusForDropDown();
  bloodGroupDropDown = AppUtils.getBloodGroupForDropDown();

  get constants(): typeof Constants {
    return Constants;
  }

  constructor(private dialog: MatDialog,
    public appUtils: AppUtils,
    private route: ActivatedRoute,
    private employeeService: EmployeeService,
    private baseService: BaseService,
    private stateService: StateService,
    private cityService: CityService
  ) {
    this.isModelLoaded = false;
    this.route.params.subscribe((params) => {
      this.model.id = params['id'];
    });
  }

  ngOnInit(): void {
    this.getEmployeeDetail(this.model.id);
  }

  getEmployeeDetail(id: number): void {
    this.blockUI.start();
    this.isModelLoaded = false;

    this.employeeService.getEmployee(this.model.id).subscribe({
      next: (response) => {
        this.model = response;
        if (this.model.dateOfBirth !== null) {
          this.dateOfBirth = AppUtils.getLocalFormattedDate(this.model.dateOfBirth);
        }
        if (this.model.gender > 0) {
          this.gender = this.genderDropDown.find(x => x.key == this.model.gender).value;
        }
        if (this.model.maritalStatus > 0) {
          this.martialStatus = this.maritalStatusDropDown.find(x => x.key == this.model.maritalStatus).value;
        }

        if (this.model.bloodGroup > 0) {
          this.bloodGroup = this.bloodGroupDropDown.find(x => x.key == this.model.bloodGroup).value;
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

  currentAddressEditable(): void {
    this.isCurrentAddressEditable = true;
    if (this.model.currentAddress !== null) {
      this.addressModel = this.model.currentAddress;
    }
    this.addressModel.addressType = Constants.addressType.current;
    this.loadStates();
    this.loadCities();
  }

  permanentAddressEditable(): void {
    this.isPermanentAddressEditable = true;
    if (this.model.permanentAddress !== null) {
      this.addressModel = this.model.permanentAddress;
    }
    this.addressModel.addressType = Constants.addressType.permanent;
    this.loadStates();
    this.loadCities();
  }

  loadStates(): void {
    const selectedCountry = this.countries.find(x => x.key === Constants.defaultCountryId);
    if (selectedCountry) {
      this.addressModel.countryName = selectedCountry.value;
    }
    this.blockUI.start();
    this.isModelLoaded = false;

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
      this.blockUI.start();
      this.isModelLoaded = false;

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

  submit(): void {
    if (this.model.dateOfBirth) {
      this.model.dateOfBirth = AppUtils.getFormattedDate(this.model.dateOfBirth, null);
      this.model.dateOfBirth = AppUtils.getDate(this.model.dateOfBirth);
    }

    if (this.addressModel.addressType === Constants.addressType.current) {
      this.model.currentAddress = this.addressModel;
    }
    else if (this.addressModel.addressType === Constants.addressType.permanent) {
      this.model.permanentAddress = this.addressModel;
    }
    if (this.model.id) {
      this.blockUI.start();
      this.isModelLoaded = false;

      this.employeeService.update(this.model).subscribe({
        next: () => {
          this.baseService.successNotification('Employee detail has been updated successfully.');
          this.isPersonalInfoEditable = false;
          this.isContactInfoEditable = false;
          this.addressModel = new AddressModel();
          this.isCurrentAddressEditable = false;
          this.isPermanentAddressEditable = false;
          this.getEmployeeDetail(this.model.id);
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
    else {
      this.blockUI.start();
      this.isModelLoaded = false;

      this.employeeService.add(this.model).subscribe({
        next: () => {
          this.baseService.successNotification('Employee details has been added successfully.');
          this.isPersonalInfoEditable = false;
          this.isContactInfoEditable = false;
          this.isCurrentAddressEditable = false;
          this.isPermanentAddressEditable = false;
          this.addressModel = new AddressModel();
          this.getEmployeeDetail(this.model.id);
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
    this.getEmployeeDetail(this.model.id);
    this.addressModel = new AddressModel();
    this.isPersonalInfoEditable = false;
    this.isContactInfoEditable = false;
    this.isCurrentAddressEditable = false;
    this.isPermanentAddressEditable = false;
  }

  reloadDetails(): void {
    this.getEmployeeDetail(this.model.id);
  }
}