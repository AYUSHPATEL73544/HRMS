import { Directive } from '@angular/core';
import { NG_VALIDATORS, Validator, FormControl } from '@angular/forms';

@Directive({
    // tslint:disable-next-line: directive-selector
    selector: '[emailAddress][ngModel]',
    providers: [{ provide: NG_VALIDATORS, useExisting: EmailAddressDirective, multi: true }]
})

export class EmailAddressDirective implements Validator {
    validate(c: FormControl): { [key: string]: any } {
        const v = c.value;
        const emailRegex = new RegExp('[a-z0-9._%+-]+@[a-z0-9.-]+.[a-z]{2,4}$');
        return v !== null
            && v !== undefined
            && !emailRegex.test(v)
            ? { emailAddress: true }
            : null;
    }
}
