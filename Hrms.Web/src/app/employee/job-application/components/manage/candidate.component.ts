import { Component, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatDrawer } from "@angular/material/sidenav";
import { MatSort } from "@angular/material/sort";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { Constants } from "src/app/utilities";
import { MatDialog } from "@angular/material/dialog";
import { environment } from "src/environments/environment";
import { CandidateChangeStatusModel, CandidateModel } from "../../model";
import { FilterModel } from "src/app/shared/models";
import { BaseService } from "src/app/shared/services";
import { CandidateService } from "src/app/employee/services";
import { DialogConfirmComponent } from "src/app/shared/dialog";
import { UpsertCandidateComponent } from "../upsert/upsert-candidate.component";
import { CandidateDetailComponent } from "../detail/candidate-detail.component";

@Component({
    selector: 'app-candidate',
    templateUrl: './candidate.component.html'
})
export class CandidateComponent {
    @BlockUI('candidate-blockui') blockUI: NgBlockUI;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild("drawer") drawer: MatDrawer;
    @ViewChild('candidateUpsert') candidateUpsert: UpsertCandidateComponent;
    
    displayedColumns = ['firstName', 'phone', 'email', 'createdDate', 'status', 'action'];
    model = new Array<CandidateModel>();
    filterModel = new FilterModel();
    changeStatusModel = new CandidateChangeStatusModel();
    isModelLoaded = false;
    totalCount = 0;

    constructor(private dialog: MatDialog,
        private baseService: BaseService,
        private service: CandidateService) {

    }

    get constants(): typeof Constants {
        return Constants;
    }

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
        this.service.getList(this.filterModel).subscribe({
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

    closeDrawer() {
        this.drawer.close();
        this.getList();
    }

    add(): void {
        const dialRef = this.dialog.open(UpsertCandidateComponent, {
            width: Constants.dialogSize.large,
            disableClose: true
        });
        dialRef.afterClosed().subscribe(() => {
            this.getList();
        });
    }

    edit(id: number): void {
        this.drawer.open();
        this.candidateUpsert.getRecords(id);
    }

    candidateDetail(id: number): void {
        const dialogRef = this.dialog.open(CandidateDetailComponent, {
            data: {
                title: 'Candidate Detail',
                id: id
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialogRef.afterClosed().subscribe(() => {
            this.getList();
        })
    }

    shortlist(id: number): void {
        const dialRef = this.dialog.open(DialogConfirmComponent, {
            data: {
                title: 'Shortlist Candidate',
                message: 'Are you sure you want to shortlist selected candidate?'
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialRef.afterClosed().subscribe(
            (confirm) => {
                if (confirm) {
                    this.blockUI.start();
                    this.service.shortlist(id).subscribe({
                        next: () => {
                            this.blockUI.stop();
                            this.isModelLoaded = true;
                            this.baseService.successNotification("Candidate has been shortlisted successfully.");
                            this.getList();
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


    viewDocument(fileKey: string) {
        window.open(environment.apiBaseUrl + '/documents/' + fileKey, '_blank');
    }


    resetFilterKey(): void {
        this.filterModel.filterKey = null;
        this.paginator.firstPage();
        this.getList();
    }

    resetFilters(): void {
        this.filterModel = new FilterModel();
        this.paginator.firstPage();
        this.getList();
    }

    updateFilterModel(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;
    }
}