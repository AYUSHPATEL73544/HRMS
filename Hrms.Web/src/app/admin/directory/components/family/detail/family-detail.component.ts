import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { MatSort } from '@angular/material/sort';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MatPaginator } from '@angular/material/paginator';
import { BaseService, RelationshipService } from 'src/app/shared/services';
import { FamilyService } from 'src/app/admin/services';
import { FamilyModel } from 'src/app/admin/directory/models';
import { FilterModel, SelectListItemModel } from 'src/app/shared/models';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
  selector: 'app-family-detail',
  templateUrl: './family-detail.component.html',
})
export class FamilyDetailComponent implements OnInit, AfterViewInit {
  @BlockUI('family-blockui') blockUI: NgBlockUI;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  displayedColumns = [
    'firstName',
    'relationshipId',
    'dateOfBirth',
    'email',
    'phone',
  ];

  model = new Array<FamilyModel>();
  relations = new Array<SelectListItemModel>();
  filterModel = new FilterModel();
  totalCount: number;
  isModelLoaded: boolean;

  id = 0;
  get constants(): typeof Constants {
    return Constants;
  }

  constructor(
    private dialog: MatDialog,
    private route: ActivatedRoute,
    private relationshipService: RelationshipService,
    private baseService: BaseService,
    private services: FamilyService
  ) {
    this.isModelLoaded = false;
    this.route.params.subscribe((params) => {
      this.id = params['id'];
    });
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => {
      this.getFamilyDetails();
    });
    this.paginator.page.subscribe(() => {
      this.getFamilyDetails();
    });
    this.getFamilyDetails();
  }

  ngOnInit(): void {
    this.getReltionShipList();
    this.getFamilyDetails();
  }

  getFamilyDetails(): void {
    this.blockUI.start();
    this.isModelLoaded = false;

    this.updateFilterModel();
    this.services.getPagedList(this.filterModel, this.id).subscribe({
      next: (response) => {
        this.model = response.items;
        this.totalCount = response.totalCount;
        this.model.forEach((element) => {
          const date = AppUtils.getLocalFormattedDate(element.dateOfBirth);
          if (date) {
            element.dateOfBirth = date;
          }
          const relationship = this.relations.find(
            (x) => x.key == element.relationshipId
          ).value;
          element.relationName = relationship;
        });
        this.blockUI.stop();
        this.isModelLoaded = true;
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.isModelLoaded = true;
        this.baseService.processErrorResponse(error);
      },
    });
  }

  getReltionShipList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;

    this.relationshipService.getSelectListItem().subscribe({
      next: (response) => {
        this.relations = response;
        this.blockUI.stop();
        this.isModelLoaded = true;
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.isModelLoaded = true;
        this.baseService.processErrorResponse(error);
      },
    });
  }

  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.getFamilyDetails();
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
    this.getFamilyDetails();
  }
}
