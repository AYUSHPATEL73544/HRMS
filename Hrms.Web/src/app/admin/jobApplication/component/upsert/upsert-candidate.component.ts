import { Component, EventEmitter, OnInit, Output, ViewChild } from "@angular/core";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { BaseService, StorageService } from "src/app/shared/services";
import { AppUtils } from "src/app/utilities";
import { Guid } from 'guid-typescript';
import { CandidateModel } from "src/app/admin/jobApplication/model";
import { CandidateService, CourseTypeService, QualificationTypeService, SkillService } from "src/app/admin/services";
import { SelectListItemModel } from "src/app/shared/models";
import { environment } from "src/environments/environment";
import { ENTER, COMMA } from '@angular/cdk/keycodes';
import { FormControl, NgForm } from "@angular/forms";
import { Observable, map, startWith } from "rxjs";
import { MatChipInputEvent } from "@angular/material/chips";

@Component({
  selector: 'app-upsert-candidate',
  templateUrl: './upsert-candidate.component.html',
})
export class UpsertCandidateComponent implements OnInit {
  @ViewChild('f') form: NgForm;
  @BlockUI('upsert-candidate-blockui') blockUI: NgBlockUI;

  @Output() closeDrawer: EventEmitter<void> = new EventEmitter<void>();
  skillCtrl = new FormControl();
  model = new CandidateModel();
  qualifications = new Array<SelectListItemModel>();
  courseTypes = new Array<SelectListItemModel>();
  selectedSkills: Array<SelectListItemModel> = [];
  separatorKeysCodes: number[] = [ENTER, COMMA];
  selectedSkillStatus: boolean[] = [];
  selectedOptions: string[] = [];
  skills: Array<SelectListItemModel> = [];
  filteredSkills: Observable<Array<SelectListItemModel>>;
  isModelLoaded: boolean;
  selectedDocumentFile: any;
  years = AppUtils.getYears();
  currentYear = new Date().getFullYear();
  genderDropDown = AppUtils.getGenderForDropDown();
  isSameFileSelected: boolean = false;
  deletedFiles = new Array<string>();

  constructor(
    private baseService: BaseService,
    private service: CandidateService,
    private skillService: SkillService,
    private qualificationService: QualificationTypeService,
    private courseService: CourseTypeService,
    public appUtils: AppUtils,
    private storageService: StorageService) {
    this.getSkillLists();
    this.model.documentDetails.id = 0;
  }

  ngOnInit(): void {
    this.getQualificationList();
    this.getCourseList();
  }

  getSkillLists(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.skillService.getSkillList().subscribe({
      next: (response) => {
        this.skills = response;
        this.filteredSkills = this.skillCtrl.valueChanges.pipe(
          startWith(''),
          map(value => value ? this._filterSkills(value) : this.skills.slice()) // Show all skills when value is empty
        );
        this.blockUI.stop();
        this.isModelLoaded = true;
      },
      error: (error) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    })
  }

  getRecords(id: number): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.service.getCandidate(id).subscribe({
      next: (response) => {
        this.model = response;
        this.selectedSkills = this.model.skillNames.map(skillName => ({
          key: '', // Add an empty key
          value: skillName,
          description: '' // Add an empty description
        }));;
        if (
          this.model.marketingChannel !== "LinkedIn" &&
          this.model.marketingChannel !== "FaceBook" &&
          this.model.marketingChannel !== "Instagram"
        ) {
          this.model.marketingChannelType = "Other";
          this.model.other = this.model.marketingChannel;
        } else {
          this.model.marketingChannelType = this.model.marketingChannel;
        }
        this.selectedOptions = this.model.skillNames.slice();
        this.blockUI.stop();
        this.isModelLoaded = true;
      },
      error: (error) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  togglePursuing() {
    if (this.model.isPursuing) {
      this.model.passingYear = this.currentYear + 1;
    }
    if (this.model.isPursuing === false) {
      this.model.passingYear = null;
    }
  }


  onFileSelected(event: any): void {
    this.blockUI.start();
    const inputElement = event.target as HTMLInputElement;
    if (this.isSameFileSelected) {
      this.isSameFileSelected = false;
      return;
    }
    if (!event.target.files || event.target.files.length === 0) {
      this.removeDocument();
      return;
    }
    const selectedFile = event.target.files.item(0);
    this.model.documentDetails.id = 1;
    this.model.documentDetails.name = selectedFile.name;
    this.model.documentDetails.key = `${Guid.create()}.${event.target.files
      .item(0)
      .name.split('.')
      .pop()}`;
    this.storageService.uploadSingleFile(selectedFile, this.model.documentDetails.key).subscribe({
      next: () => {
        this.blockUI.stop();
        this.baseService.successNotification("Document has been uploaded successfully.");

      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });

    inputElement.value = '';
    this.isSameFileSelected = true;
    inputElement.dispatchEvent(new Event('change'));
    this.blockUI.stop();
  }

  removeDocument(): void {
    this.model.document.file = null;
    this.model.document.name = null;
    this.model.document.key = null;
  }

  initiateDelete(key: any) {
    this.deletedFiles.push(key);
    this.model.documentDetails.id = 0;
    this.model.documentDetails.name = null;
    this.model.documentDetails.key = null;
    this.model.documentFile = undefined;
    this.baseService.successNotification("Document has been deleted successfully");

  }

  deleteFile(key: string): void {
    this.blockUI.start();
    this.storageService.deleteSingleFile(key).subscribe({
      next: () => {
        this.blockUI.stop();
        this.baseService.successNotification("Document has been deleted successfully.");
        this.model.documentDetails.id = 0;
        this.model.documentDetails.name = null;
        this.model.documentDetails.key = null;
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  getQualificationList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.qualificationService.getSelectListItem().subscribe({
      next: (response) => {
        Object.assign(this.qualifications, response);
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

  getCourseList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.courseService.getSelectListItem().subscribe({
      next: (response) => {
        Object.assign(this.courseTypes, response);
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

  viewDocument(fileKey: string) {
    window.open(environment.apiBaseUrl + '/documents/' + fileKey, '_blank');
  }

  submit(): void {

    if (this.model.id) {
      this.blockUI.start();
      if (this.model.marketingChannelType == "Other") {
        this.model.marketingChannel = this.model.other;
      }
      else {
        this.model.marketingChannel = this.model.marketingChannelType;
      }
      this.service.update(this.model).subscribe({
        next: () => {
          this.blockUI.stop();
          this.baseService.successNotification("Candidate details has been updated successfully.");
          this.deletedFiles.forEach(key => {
            this.deleteFile(key);
          });
          this.cancel();

        },
        error: (error: any) => {
          this.blockUI.stop();
          this.baseService.processErrorResponse(error);
        }
      });
    }
    else {
      this.blockUI.start();
      if (this.model.marketingChannelType == "Other") {
        this.model.marketingChannel = this.model.other;
      }
      else {
        this.model.marketingChannel = this.model.marketingChannelType;
      }
      this.service.addCandidate(this.model).subscribe({
        next: () => {
          this.blockUI.stop();
          this.baseService.successNotification("Candidate has been added successfully");
          this.cancel();
        },
        error: (error: any) => {
          this.blockUI.stop();
          this.baseService.processErrorResponse(error);
        }
      });
    }
  }

  cancel(): void {
    this.model = new CandidateModel();
    this.selectedSkills = new Array<SelectListItemModel>();
    this.selectedOptions = new Array<string>();
    this.form.resetForm();
    this.closeDrawer.emit();
  }

  private _filterSkills(value: string): Array<SelectListItemModel> {
    const filterValue = value.toLowerCase();
    return this.skills.filter(skill => skill.value.toLowerCase().includes(filterValue));
  }

  addSkill(event: MatChipInputEvent): void {
    const value = event.value?.trim();
    if (value) {
      const isExist = this.model.skillNames.find((x) => x.toLowerCase() == value.toLowerCase());
      if (isExist === undefined) {
        this.selectedSkills.push({
          key: value, value: value,
          description: ""
        });
        this.model.skillNames.push(value);
      }
      else {
        this.baseService.errorNotification("Skill name already exists.");
      }
    }
    if (event.chipInput) {
      event.chipInput.inputElement.value = '';
    }
    this.skillCtrl.setValue(null);
  }

  removeSkill(skill: SelectListItemModel): void {
    const index = this.selectedSkills.indexOf(skill);
    if (index >= 0) {
      this.selectedSkills.splice(index, 1);
      const skillName = skill.value;
      const skillIndex = this.selectedOptions.indexOf(skillName);
      if (skillIndex !== -1) {
        this.selectedOptions.splice(skillIndex, 1);
      }
      this.model.skillNames = this.model.skillNames.filter((name: string) => name !== skillName);
    }
  }

  selectSkill(selectedSkill: SelectListItemModel): void {

    const skillName = selectedSkill.value;
    const isExist = this.model.skillNames.find((x) => x.toLowerCase() == skillName.toLowerCase());
    if (isExist !== undefined) {
      this.baseService.errorNotification("Skill name already exists.");

    }
    else {
      if (!this.selectedOptions.includes(skillName)) {
        this.selectedOptions.push(skillName);
      }
      this.selectedSkills.push(selectedSkill);
      this.model.skillNames.push(skillName);
      this.skillCtrl.setValue(null);
    }


  }

  displaySkill(skill: SelectListItemModel): string {
    return skill ? skill.value : '';
  }

}