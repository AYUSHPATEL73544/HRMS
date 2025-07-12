import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { AppUtils, Constants } from 'src/app/utilities';
import { MatDialog } from '@angular/material/dialog';
import { SelectListItemModel } from 'src/app/shared/models';
import { EmployeeModel } from 'src/app/employee/profile/models/index';
import { AddressModel } from 'src/app/shared/models/address.model';
import { EmployeeService } from 'src/app/employee/services';
import { BaseService, CityService, DocumentService, ListenerService, StateService } from 'src/app/shared/services';
import { ProfileImageComponent } from '../../profile-image/profile-image.component';

@Component({
  selector: 'app-personal-info',
  templateUrl: './personal-info.component.html',
})
export class PersonalInfoComponent {
  @BlockUI('employee-blockui') blockUI: NgBlockUI;

  @Input() model: EmployeeModel;
  @Output() reloadProfile = new EventEmitter();
  addressModel = new AddressModel();
  countries = new Array<SelectListItemModel>();
  states = new Array<SelectListItemModel>();
  cities = new Array<SelectListItemModel>();

  isProfileImageEditable = false;
  isPersonalInfoEditable = false;
  isContactInfoEditable = false;
  isCurrentAddressEditable = false;
  isPermanentAddressEditable = false;
  genders = AppUtils.getGenderForDropDown();
  maritalStatuses = AppUtils.getMaritalStatusForDropDown();
  bloodGroups = AppUtils.getBloodGroupForDropDown();
  bloodGroup: string;
  status: string;
  employeeType: string;
  selectedTabIndex = 0;
  statusDropDown = AppUtils.statusDropDown();
  typeDropDown = AppUtils.employeeType();
  selectedImage: any;

  get constants(): typeof Constants {
    return Constants;
  }

  constructor(
    public appUtils: AppUtils,
    private dialog: MatDialog,
    private employeeService: EmployeeService,
    private baseService: BaseService,
    private stateService: StateService,
    private cityService: CityService,
    private documentService: DocumentService,
    private listenerService: ListenerService
  ) {

  }

  ngOnInit() {
  }


  currentAddressEditable(): void {
    this.isCurrentAddressEditable = true;
    if (this.model.currentAddress !== null) {
      this.addressModel = this.model.currentAddress;
    }
    this.addressModel.addressType = Constants.addressType.current;
    this.loadStates();
  }

  permanentAddressEditable(): void {
    this.isPermanentAddressEditable = true;
    if (this.model.permanentAddress) {
      this.addressModel = this.model.permanentAddress;
    }
    this.addressModel.addressType = Constants.addressType.permanent;
    this.loadStates();
  }

  loadStates(): void {
    const selectedCountry = this.countries.find(x => x.key === Constants.defaultCountryId);
    if (selectedCountry) {
      this.addressModel.countryName = selectedCountry.value;
    }
    this.blockUI.start();
    this.stateService.getSelectListItem(Constants.defaultCountryId).subscribe({
      next: (response) => {
        Object.assign(this.states, response);
        this.blockUI.stop();
        if (this.addressModel.stateId === -1) {
          this.addressModel.stateId = null;
          this.addressModel.stateName = '';
          this.addressModel.cityId = null;
          this.addressModel.cityName = '';
        }
        if (this.addressModel.stateId) {
          this.loadCities();
        }
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  loadCities(): void {
    const selectedState = this.states.find(x => x.key === this.addressModel.stateId);
    if (selectedState) {
      this.addressModel.stateName = selectedState.value;
      this.blockUI.start();
      this.cityService.getSelectListItem(this.addressModel.stateId).subscribe({
        next: (response) => {
          Object.assign(this.cities, response);
          this.blockUI.stop();
        },
        error: (error: any) => {
          this.blockUI.stop();
          this.baseService.processErrorResponse(error);
        }
      });
    }
  }



  add() {
    const dialogRef = this.dialog.open(ProfileImageComponent, {
      data: {
        title: "Upload Image",
        model: this.model,
        id: this.model.id,
      },
      width: Constants.dialogSize.medium,
      disableClose: true
    });
    dialogRef.afterClosed().subscribe(() => {
      this.reloadProfile.emit();
    });
  }


  deleteImage(id: any) {
    this.blockUI.start();
    this.documentService.deleteImage(id).subscribe({
      next: () => {
        this.blockUI.stop();
        this.baseService.successNotification("Image removed.");
        this.listenerService.profileUpdateListener.next(null);
        this.reloadProfile.emit();
      },
      error: (error) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
        this.reloadProfile.emit();
      }
    })
  }


  submit(): void {
    this.model.dateOfBirth = AppUtils.getFormattedDate(this.model.dateOfBirth, null);
    this.model.dateOfBirth = AppUtils.getDate(this.model.dateOfBirth);
    if (this.addressModel.addressType === Constants.addressType.current) {
      this.model.currentAddress = this.addressModel;
    }
    else if (this.addressModel.addressType === Constants.addressType.permanent) {
      this.model.permanentAddress = this.addressModel;
    }
    this.blockUI.start();
    this.employeeService.update(this.model).subscribe({
      next: () => {
        this.blockUI.stop();
        this.baseService.successNotification('Employee details has been updated successfully.');
        this.listenerService.profileUpdateListener.next(null);
        this.isProfileImageEditable = false;
        this.isPersonalInfoEditable = false;
        this.isContactInfoEditable = false;
        this.isCurrentAddressEditable = false;
        this.isPermanentAddressEditable = false;
        this.reloadProfile.emit();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
        this.reloadProfile.emit();
      }
    });
  }

  cancel(): void {
    this.isProfileImageEditable = false;
    this.isPersonalInfoEditable = false;
    this.isContactInfoEditable = false;
    this.isCurrentAddressEditable = false;
    this.isPermanentAddressEditable = false;
    this.addressModel = new AddressModel();
    this.reloadProfile.emit();
  }


}