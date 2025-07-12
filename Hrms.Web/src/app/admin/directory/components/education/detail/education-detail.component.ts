import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { ActivatedRoute } from "@angular/router";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { MatSort } from "@angular/material/sort";
import { EducationService } from "src/app/admin/services/education.service";
import { CourseTypeService, QualificationTypeService } from "src/app/admin/services";
import { BaseService } from "src/app/shared/services";
import { FilterModel, SelectListItemModel } from "src/app/shared/models";
import { EducationModel } from "src/app/admin/directory/models";
import { MatPaginator } from "@angular/material/paginator";
import { EducationViewDetailComponent } from "./detail/education-view-detail.component";
import { AppUtils, Constants } from "src/app/utilities";

@Component({
  selector: 'app-education-detail',
  templateUrl: './education-detail.component.html',
})

export class EducationDetailComponent implements OnInit, AfterViewInit {
  @BlockUI('education-blockui') blockUI: NgBlockUI;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

  displayedColumns = ['qualificationTypeId', 'collegeName', 'courseName', 'courseTypeId', 'action'];
  model = new Array<EducationModel>();
  qualifications = new Array<SelectListItemModel>();
  courseTypes = new Array<SelectListItemModel>();
  filterModel = new FilterModel();
  totalCount: number;
  isModelLoaded: boolean;

  id = 0;
  get constants(): typeof Constants {
    return Constants;
  }

  constructor(private dialog: MatDialog,
    private route: ActivatedRoute,
    private service: EducationService,
    private qualificationService: QualificationTypeService,
    private courseService: CourseTypeService,
    private baseService: BaseService
  ) {
    this.isModelLoaded = false;
    this.filterModel.sort = 'createdOn';
    this.filterModel.order = 'desc';
    this.route.params.subscribe((params) => {
      this.id = params['id'];
    });
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
    this.service.getPagedList(this.filterModel, this.id).subscribe({
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
        this.baseService.processErrorResponse(error);
      }
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

  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.getEducationDetail();
  }

  resetFilters(): void {
    this.filterModel = new FilterModel();
  }

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
    this.filterModel.pageIndex = this.paginator.pageIndex;
    this.filterModel.pageSize = this.paginator.pageSize;
  }

  reloadDetails(): void {
    this.getEducationDetail();
  }
}