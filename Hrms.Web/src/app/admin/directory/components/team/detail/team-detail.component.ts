import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { EmployeeService, TeamService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { UserRoleService } from 'src/app/admin/services';
import { TeamModel, TeamReportessModel, UserRoleModel } from 'src/app/admin/directory/models';
import { SelectListItemModel } from 'src/app/shared/models';
import { DialogConfirmComponent } from 'src/app/shared/dialog';
import { UpsertTeamComponent } from '../upsert/upsert-team.component';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
    selector: 'app-team-detail',
    templateUrl: './team-detail.component.html',
})
export class TeamDetailComponent implements OnInit {
    @BlockUI('team-blockui') blockUI: NgBlockUI;

    isModelLoaded: boolean;
    displayedColumnsForReporting = [
        'name',
        'type',
        'department',
        'designation',
        'action',
    ];
    displayedColumnsForDirectReporting = [
        'name',
        'type',
        'department',
        'designation',
    ];

    model = new Array<TeamModel>();
    reporteesModel = new Array<TeamReportessModel>();
    userRoles = AppUtils.role();
    employees = new Array<SelectListItemModel>();
    userRoleModel = new UserRoleModel();
    typeDropDown = AppUtils.getEmployeeTypeDropDown();
    role: string;
    id = 0;
    isReportingEditable = false;
    isDirectReportingEditable = false;
    isRoleAdded = false;

    constructor(
        private dialog: MatDialog,
        private route: ActivatedRoute,
        private employeeService: EmployeeService,
        private service: TeamService,
        private userRoleService: UserRoleService,
        private baseService: BaseService
    ) {
        this.route.params.subscribe((params) => {
            this.id = params['id'];
        });
    }

    ngOnInit(): void {
        this.getEmployeeList();
        // this.getRoleSelectList();
        this.getReportessList(this.id);
        this.get(this.id);
    }

    get(id: number): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.service.getByEmployeeId(this.id).subscribe({
            next: (response) => {
                this.model = response;
                this.model.forEach((element) => {
                    element.typeName = this.typeDropDown.find(
                        (x) => x.key === element.type
                    ).value;
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

    getEmployeeList(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.employeeService.getSelectListItem().subscribe({
            next: (response) => {
                this.blockUI.stop();
                this.employees = response;
                this.getRoleList();
                this.blockUI.stop();
                this.isModelLoaded = true;
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            },
        });
    }

    // getRoleSelectList(): void {
    //     this.blockUI.start();
    //     this.isModelLoaded = false;
    //     this.userRoleService.getSelectListItems().subscribe({
    //         next: (response) => {
    //             this.userRoles = response;
    //             this.blockUI.stop();
    //             this.isModelLoaded = true;
    //         },
    //         error: (error: any) => {
    //             this.blockUI.stop();
    //             this.baseService.processErrorResponse(error);
    //         },
    //     });
    // }

    getRoleList(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.userRoleService.get(this.id).subscribe({
            next: (response) => {
                this.userRoleModel = response;
                if (this.userRoleModel.roleId > 0) {
                    this.role = this.userRoles.find(x => x.key == this.userRoleModel.roleId).value;
                }
                this.blockUI.stop();
                this.isModelLoaded = true;
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            },
        })
    }


    add(): void {
        const dailRef = this.dialog.open(UpsertTeamComponent, {
            width: Constants.dialogSize.medium,
            disableClose: true,
            data: { employeeId: this.id },
        });
        dailRef.afterClosed().subscribe(() => {
            this.get(this.id);
        });
    }

    editManager(id: number): void {
        const dailRef = this.dialog.open(UpsertTeamComponent, {
            width: Constants.dialogSize.medium,
            disableClose: true,
            data: { id: id },
        });
        dailRef.afterClosed().subscribe(() => {
            this.get(this.id);
        });
    }

    getReportessList(id: number): void {
        this.blockUI.start();
        this.service.getByManagerId(id).subscribe({
            next: (response) => {
                this.blockUI.stop();
                this.reporteesModel = response;
                this.reporteesModel.forEach((element) => {
                    element.typeName = this.typeDropDown.find(
                        (x) => x.key === element.type
                    ).value;
                });
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            },
        });
    }

    delete(id: number): void {
        const dailRef = this.dialog.open(DialogConfirmComponent, {
            data: {
                title: 'Delete',
                message:
                    'Are you sure you want to delete the selected manager details?',
            },
        });
        dailRef.afterClosed().subscribe((confirm) => {
            if (confirm) {
                this.service.delete(id).subscribe({
                    next: () => {
                        this.baseService.successNotification(
                            'Member has been delete successfully'
                        );
                        this.get(this.id);
                    },
                    error: (error: any) => {
                        this.baseService.processErrorResponse(error);
                    },
                });
            }
        });
    }

    reloadDetails(): void {
        this.get(this.id);
    }

    toggleAddRole(): void {
        this.isRoleAdded = true;
    }

    close(): void {
        this.isRoleAdded = false;
        this.getRoleList();
    }

    submit(): void {
        this.userRoleModel.userId = this.id;
        this.userRoleService.add(this.userRoleModel).subscribe({
            next: () => {
                this.blockUI.start();
                this.isModelLoaded = true;
                this.baseService.successNotification(
                    'User Role has been updated successfully'
                );
                this.isRoleAdded = false;
                this.blockUI.stop();
                this.getRoleList();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isRoleAdded = false;
                this.baseService.processErrorResponse(error);
            },
        });
    }
}