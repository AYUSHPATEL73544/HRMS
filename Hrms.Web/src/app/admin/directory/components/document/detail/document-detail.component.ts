import { Component } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { Constants } from "src/app/utilities";

@Component({
    selector: 'app-document-detail',
    templateUrl: './document-detail.component.html',
  })
  
  export class DocumentDetailComponent {
  
    get constants(): typeof Constants {
      return Constants;
    }
  
    constructor(private dialog: MatDialog) { }
  
    // addDocuments(): void {
    //   const dialRef = this.dialog.open(UpsertDocumentComponent, {
    //     width: this.constants.dialogSize.large,
    //     disableClose: true
    //   });
    // }
  }