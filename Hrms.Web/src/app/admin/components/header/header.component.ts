import { Component, Output, EventEmitter } from '@angular/core';
import * as moment from 'moment';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',

})

export class HeaderComponent {
    @Output() toggleLayout = new EventEmitter();

    currentDate: any;
    currentTime: any;
    year: any;
    month: any;
    date: any;
    day: any;

    constructor() {
        this.currentDate = moment().add('month');
        this.year = this.currentDate.format('YYYY');
        this.month = this.currentDate.format('MMM');
        this.date = this.currentDate.format('DD');
        this.day = this.currentDate.format('ddd');
    }


    toggleSideBar(): void {
        this.toggleLayout.next('');
    }
}
