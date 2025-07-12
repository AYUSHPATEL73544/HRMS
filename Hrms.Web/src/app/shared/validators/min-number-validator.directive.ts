import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, Validator, FormControl } from '@angular/forms';

@Directive({
    // tslint:disable-next-line: directive-selector
    selector: '[minNumber][ngModel]',
    providers: [{ provide: NG_VALIDATORS, useExisting: MinNumberDirective, multi: true }]
})

export class MinNumberDirective implements Validator {
    @Input() minNumber: number;

    validate(c: FormControl): { [key: string]: any } {
        const v = c.value;
        return (this.minNumber === null
            || this.minNumber === undefined
            || v < this.minNumber)
            ? { minNumber: true }
            : null;
    }
}
