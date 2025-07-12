import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from "@angular/material/paginator";
import { MatDialog } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { CourseTypeService, QualificationTypeService } from 'src/app/employee/services';
import { EducationService } from 'src/app/employee/services/education.service';
import { BaseService } from 'src/app/shared/services';
import { FilterModel, SelectListItemModel } from 'src/app/shared/models';
import { EducationModel } from 'src/app/employee/profile/models/index';
import { UpsertEducationComponent } from '../upsert/upsert-education.component';
import { DialogConfirmComponent } from 'src/app/shared/dialog';
import { EducationViewDetailComponent } from '../view-detail/education-view-detail.component'
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
  selector: 'app-education-detail',
  templateUrl: './education-detail.component.html',
})

export class EducationDetailsComponent implements OnInit, AfterViewInit {
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @BlockUI('blockui-education') blockUI: NgBlockUI;

  displayedColumns = ['qualificationTypeId', 'collegeName', 'courseName', 'courseTypeId', 'action'];

  model = new Array<EducationModel>();
  filterModel = new FilterModel();
  qualifications = new Array<SelectListItemModel>();
  courseTypes = new Array<SelectListItemModel>();
  totalCount: number;
  isModelLoaded: boolean;

  get constants(): typeof Constants {
    return Constants;
  }

  constructor(private dialog: MatDialog,
    private service: EducationService,
    private qualificationService: QualificationTypeService,
    private courseService: CourseTypeService,
    private baseService: BaseService) {
    this.isModelLoaded = false;
    this.filterModel.sort = 'createdOn';
    this.filterModel.order = 'desc';

  }

  ngOnInit(): void {
    this.getQualificationList();
    this.getCourseList();
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => {
      this.getEducationDetail();
    });
    this.paginator.page.subscribe(() => {
      this.getEducationDetail();
    })
    this.getEducationDetail();
  }

  getEducationDetail(): void {
    this.updateFilterModel();
    this.blockUI.start();
    this.isModelLoaded = false;
    if(this.filterModel.filterKey){
      this.paginator.firstPage();
    }
    this.service.getPageList(this.filterModel).subscribe({
      next: (response) => {
        this.isModelLoaded = true;
        this.model = response.items;
        this.totalCount = response.totalCount;
        this.model.forEach(element => {
          const startDate = AppUtils.getLocalFormattedDate(element.start);
          if (startDate) {
            element.start = startDate;
          }
          const endDate = AppUtils.getLocalFormattedDate(element.end);
          if (endDate) {
            element.end = endDate;
          }
          if (element.qualificationTypeId > 0) {
            const qualification = this.qualifications.find(x => x.key == element.qualificationTypeId).value;
            element.qualificationTypeName = qualification;
          }
          if (element.courseTypeId > 0) {
            const courseType = this.courseTypes.find(x => x.key == element.courseTypeId).value;
            element.courseTypeName = courseType;
          }
        });
        this.blockUI.stop();
      },
      error(error: any) {
        this.blockUI.stop();
        this.baseSerive.processErrorResponse(error);
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

  add(): void {
    const dailRef = this.dialog.open(UpsertEducationComponent, {
      width: Constants.dialogSize.large,
      disableClose: true,
    });
    dailRef.afterClosed().subscribe(() => {
      this.getEducationDetail();
    });
  }

  edit(id: number): void {
    const dailRef = this.dialog.open(UpsertEducationComponent, {
      width: Constants.dialogSize.large,
      disableClose: true,
      data: {
        id: id
      }
    });
    dailRef.afterClosed().subscribe(() => {
      this.getEducationDetail();
    });
  }

  details(element: EducationModel): void {
    const dailRef = this.dialog.open(EducationViewDetailComponent, {

      data: {
        title: 'Educational Details',
        model: element
      },
      width: Constants.dialogSize.medium,
      disableClose: true,
    });
    dailRef.afterClosed().subscribe(() => {
      this.getEducationDetail();
    });

  }


  delete(id: number): void {
    const dailRef = this.dialog.open(DialogConfirmComponent, {
      data: {
        title: 'Delete',
        message: 'Are you sure you want to delete the selected education details?'
      }
    });
    dailRef.afterClosed().subscribe(
      (confirm) => {
        if (confirm) {
          this.blockUI.start();
          this.service.delete(id).subscribe({
            next: () => {
              this.blockUI.stop();
              this.baseService.successNotification('Education detail has been deleted successfully');
              this.getEducationDetail();
            },
            error: (error: any) => {
              this.blockUI.stop();
              this.baseService.processErrorResponse(error);
            }
          });
        }
      });
  }

  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.paginator.firstPage();
    this.getEducationDetail();
  }

  resetFilter(): void {
    this.filterModel = new FilterModel();
    this.paginator.firstPage();
    this.getEducationDetail();
  }

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
    this.filterModel.pageIndex = this.paginator.pageIndex,
      this.filterModel.pageSize = this.paginator.pageSize
  }

}