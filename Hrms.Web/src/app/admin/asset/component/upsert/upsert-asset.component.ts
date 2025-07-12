import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { AssetTypeService, VariantService, ManufacturerService, AssetService } from "src/app/admin/services";
import { BaseService } from "src/app/shared/services";
import { SelectListItemModel } from "src/app/shared/models";
import { AssetModel } from "src/app/admin/asset/model/asset.model";
import { AppUtils } from 'src/app/utilities';


@Component({
    selector: 'app-upsert-asset',
    templateUrl: './upsert-asset.component.html',
})

export class UpsertAssetComponent implements OnInit {
    @BlockUI('asset-blockui') blockUI: NgBlockUI;

    assetTypes = new Array<SelectListItemModel>();
    manafacturers = new Array<SelectListItemModel>();
    variants = new Array<SelectListItemModel>();
    model = new AssetModel();

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<UpsertAssetComponent>,
        private assetService: AssetService,
        private assetTypeService: AssetTypeService,
        private mafacturerService: ManufacturerService,
        private variantService: VariantService,
        private baseService: BaseService
    ) {
        if (data) {
            this.model.id = data.id;
        }
    }
    ngOnInit(): void {
        if (this.model.id) {
            this.getAsset();
        }
        else {
            this.getAssetTypeList();
        }

    }

    getAssetTypeList() {
        this.blockUI.start();
        this.assetTypeService.getSelectListItem().subscribe({
            next: (response) => {
                Object.assign(this.assetTypes, response);
                this.blockUI.stop();

            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    loadManufacturers() {
        const selectedAssetType = this.assetTypes.find(x => x.key === this.model.assetTypeId);
        if (selectedAssetType) {
            this.model.assetTypeName = selectedAssetType.value;
            this.blockUI.start();
            this.mafacturerService.getSelectListItem(this.model.assetTypeId).subscribe({
                next: (response) => {
                    Object.assign(this.manafacturers, response);
                    this.blockUI.stop();
                },
                error: (error: any) => {
                    this.blockUI.stop();
                    this.baseService.processErrorResponse(error);
                }
            });
        }
        else {
            this.mafacturerService.getSelectListItem(this.model.assetTypeId).subscribe({
                next: (response) => {
                    this.blockUI.start();
                    Object.assign(this.manafacturers, response);
                    this.blockUI.stop();
                    this.loadVariants();
                },
                error: (error: any) => {
                    this.blockUI.stop();
                    this.baseService.processErrorResponse(error);
                }
            });
        }
    }

    loadVariants(): void {
        const selectedManufacturer = this.manafacturers.find(x => x.key === this.model.manufacturerId);
        if (selectedManufacturer) {
            this.model.manufacturerName = selectedManufacturer.value;
            this.blockUI.start();
            this.variantService.getSelectListItem(this.model.manufacturerId).subscribe({
                next: (response) => {
                    Object.assign(this.variants, response);
                    this.blockUI.stop();
                },
                error: (error: any) => {
                    this.blockUI.stop();
                    this.baseService.processErrorResponse(error);
                }
            });
        }
    }

    submit(): void {
        this.model.purchaseDate = AppUtils.getFormattedDate(this.model.purchaseDate, null);
        this.model.purchaseDate = AppUtils.getDate(this.model.purchaseDate);

        if (this.model.id) {
            this.assetService.update(this.model).subscribe({
                next: () => {
                    this.baseService.successNotification("Asset has been updated successfully.");
                    this.dialogRef.close();
                },
                error: (error: any) => {
                    this.baseService.processErrorResponse(error);
                }
            });
        }
        else {
            this.assetService.add(this.model).subscribe({
                next: () => {
                    this.baseService.successNotification("Asset has been added successfully.");
                    this.dialogRef.close();
                },
                error: (error: any) => {
                    this.baseService.processErrorResponse(error);
                }
            });
        }
    }

    getAsset(): void {
        this.blockUI.start();
        this.assetService.getAsset(this.model.id).subscribe({
            next: (response) => {
                this.model = response;
                this.model.purchaseDate = AppUtils.getFormattedDate(this.model.purchaseDate, null);
                this.model.purchaseDate = AppUtils.getDate(this.model.purchaseDate);
                this.blockUI.stop();
                this.getAssetTypeList();
                this.loadManufacturers();

            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }
    onCheckboxChange(): void {
        if (!this.model.isInWarranty) {
            this.model.warrantyPeriod = 0;
        }
        else {
            this.model.warrantyPeriod = null;
        }
    }

    cancel(): void {
        this.dialogRef.close();
    }

}