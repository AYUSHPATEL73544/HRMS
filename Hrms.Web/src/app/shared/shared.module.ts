import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { MatStepperModule } from '@angular/material/stepper';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatBadgeModule } from '@angular/material/badge';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import {MatSliderModule} from '@angular/material/slider';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { BlockUIModule } from 'ng-block-ui';
import { ToastrModule } from 'ngx-toastr';

import { NaIfEmpty, SanitizeLocalFilePath, TooltipList } from './pipes';

import {
    AlphabetOnlyDirective, AlphabetWithSpaceOnlyDirective, AlphabetOnlyTypeOneDirective, AlphabetOnlyTypeTwoDirective,
    NumberOnlyDirective, DecimalNumberOnlyDirective, AlphaNumericOnlyDirective, AlphaNumericOnlyTypeOneDirective,
    AlphaNumericOnlyTypeTwoDirective, AlphaNumericOnlyTypeThreeDirective, AnythingButWhiteSpaceDirective,
    EmailAddressOnlyDirective, WebUrlOnlyDirective
} from './directives';

import {
    MinNumberDirective, MaxNumberDirective, PasswordStrengthDirective, EqualToDirective, EmailAddressDirective, GreaterToDirective
} from './validators';

import {
    BaseService,
    CalendarService,
    CandidateService,
    CityService,
    InterviewService,
    ListenerService,
    RelationshipService,
    StateService,
    StorageService,
    UserService
} from './services';

import { CalendarEventComponent, InterviewDetailComponent, SideNavComponent, UserDropdownComponent } from './components';
import { DeleteComponent } from './dialog/delete-dialog/delete.component';
import { FeedbackComponent } from './dialog';
import { DialogConfirmComponent } from './dialog/confirm/dialog-confirm.component';
import { RejectComponent } from './dialog/reject-with-log/reject.component';
import { CountryService } from './services/country.service';
import { NgxMatTimepickerModule } from 'ngx-mat-timepicker';
import { NgxMaskModule } from 'ngx-mask';
import { MatSortModule } from '@angular/material/sort';
import { FullCalendarModule } from '@fullcalendar/angular';
import { DocumentService } from './services/document.service';



@NgModule({
    providers: [
        MatDatepickerModule,
        MatNativeDateModule,
        UserService,
        BaseService,
        CountryService,
        StateService,
        CityService,
        RelationshipService,
        CalendarService,
        StorageService,
        InterviewService,
        CandidateService,
        DocumentService,
        ListenerService
    ],
    imports: [
        CommonModule,
        RouterModule,
        FormsModule,
        ReactiveFormsModule,
        MatSidenavModule,
        MatToolbarModule,
        MatIconModule,
        MatListModule,
        MatButtonModule,
        MatTabsModule,
        MatInputModule,
        MatSelectModule,
        MatRadioModule,
        MatCheckboxModule,
        MatProgressBarModule,
        MatDialogModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatTableModule,
        MatDividerModule,
        MatCardModule,
        MatStepperModule,
        MatMenuModule,
        MatPaginatorModule,
        MatChipsModule,
        MatTooltipModule,
        MatExpansionModule,
        MatGridListModule,
        MatBadgeModule,
        MatAutocompleteModule,
        MatSliderModule,
        MatSnackBarModule,
        NgxMatTimepickerModule,
        BlockUIModule.forRoot({ message: '' }),
        ToastrModule.forRoot(),
        NgxMaskModule.forRoot(),

        MatSortModule,
        FullCalendarModule

    ],
    declarations: [
        NaIfEmpty,
        SanitizeLocalFilePath,
        TooltipList,

        AlphabetOnlyDirective,
        AlphabetWithSpaceOnlyDirective,
        AlphabetOnlyTypeOneDirective,
        AlphabetOnlyTypeTwoDirective,
        NumberOnlyDirective,
        DecimalNumberOnlyDirective,
        AlphaNumericOnlyDirective,
        AlphaNumericOnlyTypeOneDirective,
        AlphaNumericOnlyTypeTwoDirective,
        AlphaNumericOnlyTypeThreeDirective,
        AnythingButWhiteSpaceDirective,
        EmailAddressOnlyDirective,
        WebUrlOnlyDirective,
        MinNumberDirective,
        MaxNumberDirective,
        PasswordStrengthDirective,
        EqualToDirective,
        EmailAddressDirective,
        GreaterToDirective,
        UserDropdownComponent,
        DeleteComponent,
        FeedbackComponent,
        DialogConfirmComponent,
        RejectComponent,
        SideNavComponent,
        CalendarEventComponent,
        InterviewDetailComponent

    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MatSidenavModule,
        MatToolbarModule,
        MatIconModule,
        MatListModule,
        MatButtonModule,
        MatTabsModule,
        MatInputModule,
        MatSelectModule,
        MatRadioModule,
        MatCheckboxModule,
        MatProgressBarModule,
        MatDialogModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatTableModule,
        MatDividerModule,
        MatCardModule,
        MatStepperModule,
        MatMenuModule,
        MatPaginatorModule,
        MatChipsModule,
        MatTooltipModule,
        MatExpansionModule,
        MatGridListModule,
        MatSliderModule,
        MatBadgeModule,
        MatAutocompleteModule,
        BlockUIModule,
        ToastrModule,
        MatSnackBarModule,

        NaIfEmpty,
        SanitizeLocalFilePath,
        TooltipList,

        AlphabetOnlyDirective,
        AlphabetWithSpaceOnlyDirective,
        AlphabetOnlyTypeOneDirective,
        AlphabetOnlyTypeTwoDirective,
        NumberOnlyDirective,
        DecimalNumberOnlyDirective,
        AlphaNumericOnlyDirective,
        AlphaNumericOnlyTypeOneDirective,
        AlphaNumericOnlyTypeTwoDirective,
        AlphaNumericOnlyTypeThreeDirective,
        AnythingButWhiteSpaceDirective,
        EmailAddressOnlyDirective,
        WebUrlOnlyDirective,
        MinNumberDirective,
        MaxNumberDirective,
        GreaterToDirective,
        PasswordStrengthDirective,
        EqualToDirective,
        EmailAddressDirective,

        UserDropdownComponent,
        SideNavComponent,
        NgxMatTimepickerModule,
        NgxMaskModule,

        MatSortModule,
        FullCalendarModule,

        CalendarEventComponent,
        InterviewDetailComponent
    ]
})

export class SharedModule { }
