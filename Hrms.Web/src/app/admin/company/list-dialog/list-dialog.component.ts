import { Component, Inject } from "@angular/core";
import { EmployeeModel } from "../../directory/models";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({
    selector:'app-list-dialog',
    templateUrl:'./list-dialog.component.html',
})

export class ListDialogComponent{

    model = new Array<EmployeeModel>();


    constructor(@Inject(MAT_DIALOG_DATA) data : any)
    {
        
    }

}