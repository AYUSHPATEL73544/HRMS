import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, Validator, FormControl } from '@angular/forms';

@Directive({
    // tslint:disable-next-line: directive-selector
    selector: '[strongPassword][ngModel]',
    providers: [{ provide: NG_VALIDATORS, useExisting: PasswordStrengthDirective, multi: true }]
})

export class PasswordStrengthDirective implements Validator {
    @Input() minLength: number;
    validate(c: FormControl): { [key: string]: any } {
        if (!this.minLength) { this.minLength = 6; }
        const v = c.value;
        const passwordRegex = new RegExp('^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{6,})');
        return v !== null
            && v !== undefined
            && v.length >= this.minLength
            && !passwordRegex.test(v)
            ? { strongPassword: true }
            : null;
    }
}
