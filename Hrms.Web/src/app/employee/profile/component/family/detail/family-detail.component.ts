import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSort } from "@angular/material/sort";
import { MatPaginator } from "@angular/material/paginator";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { FamilyServices } from "src/app/employee/services";
import { BaseService, RelationshipService } from "src/app/shared/services";
import { FilterModel, SelectListItemModel } from "src/app/shared/models";
import { FamilyModel } from "src/app/employee/profile/models/family.model";
import { UpsertFamilyComponent } from "../upsert/upsert-family.component";
import { DialogConfirmComponent } from "src/app/shared/dialog";
import { AppUtils, Constants } from "src/app/utilities";

@Component({
    selector: 'app-family-detail',
    templateUrl: './family-detail.component.html',
})

export class FamilyDetailComponent implements OnInit, AfterViewInit {
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @BlockUI('blockui-family-detail') blockUI: NgBlockUI;

    displayedColumns = ['firstName', 'relationshipId', 'dateOfBirth', 'email', 'phone', 'action'];
    model = new Array<FamilyModel>();
    filterModel = new FilterModel();
    relations = new Array<SelectListItemModel>();
    totalCount: number;
    isModelLoaded: boolean;
    get constants(): typeof Constants {
        return Constants;
    }

    constructor(private dialog: MatDialog,
        private baseService: BaseService,
        private relationshipService: RelationshipService,
        private services: FamilyServices) { this.isModelLoaded = false; }

    ngAfterViewInit(): void {
        this.sort.sortChange.subscribe(() => {
            this.getFamilyDetails();
        });
        this.paginator.page.subscribe(() => {
            this.getFamilyDetails();
        })
        this.getFamilyDetails();
    }

    ngOnInit(): void {
        this.getReltionShipList();
    }

    getFamilyDetails(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.updateFilterModel();
        if(this.filterModel.filterKey){
            this.paginator.firstPage();
        }
        this.services.getPageList(this.filterModel).subscribe({
            next: (response) => {
                this.model = response.items;
                this.totalCount = response.totalCount;
                this.model.forEach(element => {
                    const date = AppUtils.getLocalFormattedDate(element.dateOfBirth);
                    if (date) {
                        element.dateOfBirth = date;
                    }
                    const relationship = this.relations.find(x => x.key == element.relationshipId).value;
                    element.relatioName = relationship;
                });
                this.blockUI.stop();
                this.isModelLoaded = true;
            },
            error(error: any) {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseSerive.processErrorResponse(error);
            }
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
            }
        });
    }

    add(): void {
        const dailRef = this.dialog.open(UpsertFamilyComponent, {
            width: this.constants.dialogSize.large,
            disableClose: true
        });
        dailRef.afterClosed().subscribe(() => {
            this.getFamilyDetails();
        });
    }

    edit(id: number): void {
        const dailRef = this.dialog.open(UpsertFamilyComponent, {
            width: this.constants.dialogSize.large,
            disableClose: true,
            data: { id: id }
        });
        dailRef.afterClosed().subscribe(() => {
            this.getFamilyDetails();
        });
    }

    delete(id: number): void {
        const dailRef = this.dialog.open(DialogConfirmComponent, {
            data: {
                title: 'Delete',
                message: 'Are you sure you want to delete the selected member?'
            }
        });
        dailRef.afterClosed().subscribe(
            (confirm) => {
                if (confirm) {
                    this.blockUI.start()
                    this.services.delete(id).subscribe({
                        next: () => {
                            this.blockUI.stop();
                            this.baseService.successNotification('Member has been deleted successfully');
                            this.getFamilyDetails();
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
        this.getFamilyDetails();
    }

    resetFilter(): void {
        this.filterModel = new FilterModel();
        this.paginator.firstPage();
        this.getFamilyDetails();
    }

    updateFilterModel(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;
    }

}