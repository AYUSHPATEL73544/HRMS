import { AfterViewInit, Component, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { MatDialog } from "@angular/material/dialog";
import { AppUtils, Constants } from "src/app/utilities";
import { environment } from "src/environments/environment";
import { FilterModel } from "src/app/shared/models";
import { InterviewModel } from "src/app/shared/models";
import { BaseService, InterviewService } from "src/app/shared/services";
import { InterviewDetailComponent } from "src/app/shared/components";


@Component({
    selector: 'app-interviewee',
    templateUrl: './interviewee.component.html',
})
export class IntervieweeComponent implements AfterViewInit {
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @BlockUI('interviewee-blockui') blockUI: NgBlockUI;

    displayedColumns = ['legalName', 'phone', 'email', 'dateTime', 'status', 'action'];

    filterModel = new FilterModel();
    isModelLoaded = false;
    totalCount = 0;
    model = new Array<InterviewModel>();
    constructor(private dialog: MatDialog,
        private service: InterviewService,
        private baseService: BaseService) { }


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

    get constants(): typeof Constants {
        return Constants;
    }


    getList(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.updateFilterModel();
        if(this.filterModel.filterKey){
            this.paginator.firstPage();
        }
        this.service.getList(this.filterModel).subscribe({
            next: (response) => {
                this.model = response.items;
                this.model.forEach(element => {
                    element.scheduleTime = AppUtils.getTime(element.scheduleTime);
                });
                this.totalCount = response.totalCount;
                this.isModelLoaded = true;
                this.blockUI.stop();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        })

    }

    viewDocument(fileKey: string) {
        window.open(environment.apiBaseUrl + '/documents/' + fileKey, '_blank');
    }

    interviewDetail(id:number): void {
        const dialogRef = this.dialog.open(InterviewDetailComponent, {
            data: {
                title: 'Interview Detail',
                id:id
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialogRef.afterClosed().subscribe((result) => {
            this.getList();
        });
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