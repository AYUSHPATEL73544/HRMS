import { Component, AfterViewInit, ViewChild, } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSort } from "@angular/material/sort";
import { MatPaginator } from "@angular/material/paginator";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { BaseService } from "src/app/shared/services";
import { AssetService } from "src/app/admin/services";
import { AssetModel } from "src/app/admin/asset/model/asset.model";
import { FilterModel, MatTableResponseModel, } from "src/app/shared/models";
import { Constants } from "src/app/utilities";
import { UpsertAssetComponent } from "../upsert/upsert-asset.component";
import { AssetAssignComponent } from "../assign/asset-assign.component";
import { DeleteComponent } from "src/app/shared/dialog";
import { AssignHistoryComponent } from "../assign-history/assign-history.component";

@Component({
    selector: 'app-asset-manage',
    templateUrl: './asset-manage.component.html',
})
export class AssetManageComponent implements AfterViewInit {
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @BlockUI('asset-blockui') blockUI: NgBlockUI;

    model = new Array<AssetModel>();
    response: MatTableResponseModel;
    totalCount: number;
    isModelLoaded: boolean;
    filterModel = new FilterModel();

    get constants(): typeof Constants {
        return Constants;
    }

    displayedColumns = ['name', 'isInWarranty', 'warrantyPeriod', 'purchaseDate', 'serialNumber', 'vendorName', 'action'];

    constructor(private dialog: MatDialog,
        private assetService: AssetService,
        private baseService: BaseService
    ) { this.isModelLoaded = false }

    ngAfterViewInit(): void {
        this.sort.sortChange.subscribe(() => {
            this.paginator.pageIndex = 0;
            this.getList();
        });
        this.paginator.page.subscribe(() => {

            this.getList();
        });
        this.getList();
    }

    getList() {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.updateFilterModel();
        if (this.filterModel.filterKey) {
            this.paginator.firstPage();
        }
        this.assetService.getList(this.filterModel).subscribe({
            next: (response) => {
                this.model = response.items;
                this.totalCount = response.totalCount;
                this.blockUI.stop();
                this.isModelLoaded = true;
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        })
    }

    addAsset(): void {
        const dialRef = this.dialog.open(UpsertAssetComponent, {
            width: Constants.dialogSize.large,
            disableClose: true
        });
        dialRef.afterClosed().subscribe(() => {
            this.getList();
        });
    }

    editAsset(id: number): void {
        const dialRef = this.dialog.open(UpsertAssetComponent, {
            width: Constants.dialogSize.large,
            disableClose: true,
            data: { id }
        });
        dialRef.afterClosed().subscribe(() => {
            this.getList();
        });
    }

    assignAsset(id: number, purchaseDate: string): void {
        const dialRef = this.dialog.open(AssetAssignComponent, {
            width: Constants.dialogSize.medium,
            disableClose: true,
            data: {
                id, 
                purchaseDate
            }
        });
        dialRef.afterClosed().subscribe(() => {
            this.getList();
        });
    }

    assignHistory(id: number): void {
        const dialRef = this.dialog.open(AssignHistoryComponent, {
            width: Constants.dialogSize.large,
            disableClose: true,
            data: { id }
        });
        dialRef.afterClosed().subscribe(() => {
            this.getList();
        });
    }

    deleteAsset(id: number): void {
        const dialRef = this.dialog.open(DeleteComponent, {
            data: {
                title: 'Delete',
                message: 'Are you sure you want to delete selected asset?',
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialRef.afterClosed().subscribe(
            (confirm) => {
                if (confirm) {
                    this.blockUI.start();
                    this.isModelLoaded = false;

                    this.assetService.deleteAsset(id).subscribe({
                        next: () => {
                            this.baseService.successNotification('Asset has been deleted successfully.');
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
                this.getList();
            }
        );
    }

    resetFilterKey(): void {
        this.filterModel.filterKey = null;
        this.paginator.firstPage();
        this.getList();
    }

    resetFilters(): void {
        this.filterModel = new FilterModel();
        this.paginator.firstPage();
    }

    updateFilterModel(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;
    }

}

