import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MatSort } from '@angular/material/sort';
import { BaseService } from 'src/app/shared/services';
import { AttendanceRuleService } from 'src/app/admin/services';
import { AttendanceRuleModel } from 'src/app/admin/attendance/model';
import { FilterModel } from 'src/app/shared/models';
import { UpsertAttendanceRuleDetailComponent } from '../upsert/upsert-attendance-rule.component';
import { DeleteComponent } from 'src/app/shared/dialog';
import { AssignRuleComponent } from 'src/app/admin/attendance/components/assign-rule/assign-rule.component';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
    selector: 'app-attendance-rule-manage',
    templateUrl: './attendance-rule-manage.component.html',
})

export class AttendanceRuleManageComponent implements AfterViewInit {
    @BlockUI('attendance-blockui') blockUI: NgBlockUI;
    @ViewChild(MatSort, { static: false }) sort: MatSort;

    model = new Array<AttendanceRuleModel>();
    startDay: string;
    endDay: string;
    weekDays = AppUtils.getWeekDaysForDropDown();
    filterModel = new FilterModel();
    isModelLoaded: boolean;

    columns = ['title', 'StartDay', 'EndDay', 'peoples', 'action'];

    constructor(
        private dialog: MatDialog,
        private baseService: BaseService,
        private attendanceService: AttendanceRuleService
    ) {
        this.isModelLoaded = false;
    }

    ngAfterViewInit(): void {
        this.sort.sortChange.subscribe(() => {
            this.getAttendanceRuleList();
        });
        this.getAttendanceRuleList();
    }

    addRules(): void {
        const dialRef = this.dialog.open(UpsertAttendanceRuleDetailComponent, {
            width: Constants.dialogSize.large,
            disableClose: true
        });

        dialRef.afterClosed().subscribe(() => {
            this.getAttendanceRuleList();
        });
    }

    editRule(id: number): void {
        const dialRef = this.dialog.open(UpsertAttendanceRuleDetailComponent, {
            width: Constants.dialogSize.large,
            disableClose: true,
            data: { id }
        });

        dialRef.afterClosed().subscribe(() => {
            this.getAttendanceRuleList();
        });
    }

    getAttendanceRuleList(): void {
        this.blockUI.start();
        this.isModelLoaded = false;

        this.updateFilterModel();
        this.attendanceService.getPagedList(this.filterModel).subscribe({
            next: (response) => {
                this.model = response.items;
                this.model.forEach(element => {
                    element.weekStartDay = this.weekDays.find(x => x.key == element.startDay).value;
                    element.weekLastDay = this.weekDays.find(x => x.key == element.endDay).value;
                })
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

    assignRule(): void {
        this.dialog.open((AssignRuleComponent), {
            width: Constants.dialogSize.medium,
            disableClose: true,
        });
    }

    deleteRule(id: number): void {
        const dialRef = this.dialog.open(DeleteComponent, {
            data: {
                title: 'Delete',
                message: 'Are you sure you want to delete selected rule.',
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialRef.afterClosed().subscribe(
            (confirm) => {
                if (confirm) {
                    this.blockUI.start();
                    this.isModelLoaded = false;

                    this.attendanceService.deleteRule(id).subscribe({
                        next: () => {
                            this.baseService.successNotification('Rule has been deleted successfully.');
                            this.blockUI.stop();
                            this.isModelLoaded = true;
                            this.getAttendanceRuleList();
                        },
                        error: (error: any) => {
                            this.blockUI.stop();
                            this.isModelLoaded = true;
                            this.baseService.processErrorResponse(error);
                        }
                    });
                }
            }
        );
    }

    resetFilterKey(): void {
        this.filterModel.filterKey = null;
        this.getAttendanceRuleList();
    }

    resetFilter(): void {
        this.filterModel = new FilterModel();
        this.getAttendanceRuleList();
    }

    updateFilterModel(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
    }


}
