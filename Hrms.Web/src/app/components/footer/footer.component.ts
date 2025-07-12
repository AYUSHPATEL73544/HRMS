import { Component } from '@angular/core';
import * as moment from 'moment';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss']
})

export class FooterComponent { 
    currentYear:any;
    year:any;

    constructor(){
        this.currentYear = moment();
        this.year = this.currentYear.format('YYYY');
    }
}
